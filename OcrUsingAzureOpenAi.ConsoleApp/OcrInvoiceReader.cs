using System.ClientModel;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace OcrUsingAzureOpenAi.ConsoleApp;

public sealed class OcrInvoiceReader
{
    public async Task<ClientResult<ChatCompletion>> RunAsync(string filePath)
    {
        var endpoint = new Uri("https://<your-resource-name>.openai.azure.com/");
        const string deploymentName = "<your-model-deployment-name>";
        const string apiKey = "<your-key>";

        AzureOpenAIClient azureClient = new(
            endpoint,
            new AzureKeyCredential(apiKey));

        var chatClient = azureClient.GetChatClient(deploymentName);

        var imgBytes = await EmbeddedResources.GetImageBytesAsync(filePath);

        var imgContentPart = ChatMessageContentPart.CreateImagePart(
            BinaryData.FromBytes(imgBytes),
            "image/png");

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("""
                                    # Task:
                                    As an AI specialized in reading and extracting information from invoice images, 
                                    your goal is to analyze the uploaded invoice and return its details in a structured JSON format. 
                                    
                                    # Input:
                                    A high-quality image of an invoice in common formats (JPEG, PNG).
                                    
                                    # Output:
                                    A JSON response containing the extracted information.
                                    
                                    # Instructions:
                                    1. Analyze the uploaded image to identify and extract relevant information.
                                    2. Ensure accuracy in the detected text and numbers.
                                    3. If any information is not present or illegible, indicate this in the JSON response using null or appropriate placeholders.
                                    4. Validate the final output for proper JSON formatting before returning it.
                                  """),

            new UserChatMessage(imgContentPart),
        };

        return await chatClient
            .CompleteChatAsync(
                messages,
                new ChatCompletionOptions
                {
                    MaxOutputTokenCount = 4096,
                    Temperature = 1.0f,
                    TopP = 1.0f,
                    ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                        "InvoiceProperties",
                        GetJsonSchemaForResponseFormat(),
                        "Invoice properties founded in the provided image",
                        false)
                });
    }

    private static BinaryData GetJsonSchemaForResponseFormat()
        => BinaryData.FromString("""
                                 {
                                   "$schema": "http://json-schema.org/draft-07/schema#",
                                   "type": "object",
                                   "properties": {
                                     "invoiceDate": {
                                       "type": "string"
                                     },
                                     "invoiceNumber": {
                                       "type": "string"
                                     },
                                     "lineItems": {
                                       "type": "array",
                                       "items": {
                                         "type": "object",
                                         "properties": {
                                           "description": {
                                             "type": "string"
                                           },
                                           "quantity": {
                                             "type": "integer"
                                           },
                                           "unitPrice": {
                                             "type": "number"
                                           },
                                           "gst": {
                                             "type": "number"
                                           },
                                           "amount": {
                                             "type": "number"
                                           }
                                         }
                                       }
                                     },
                                     "subtotal": {
                                       "type": "number"
                                     },
                                     "tax": {
                                       "type": "number"
                                     },
                                     "total": {
                                       "type": "number"
                                     }
                                   }
                                 }
                                 """);
}