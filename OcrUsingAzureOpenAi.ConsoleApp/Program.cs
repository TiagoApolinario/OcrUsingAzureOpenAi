using OcrUsingAzureOpenAi.ConsoleApp;

var chatCompletionPoc = new OcrInvoiceReader();
var response = await chatCompletionPoc.RunAsync("Images/invoice-01.png");

Console.WriteLine(response.Value.Role);
Console.WriteLine(response.Value.Content[0].Text);