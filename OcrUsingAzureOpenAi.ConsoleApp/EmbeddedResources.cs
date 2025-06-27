using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace OcrUsingAzureOpenAi.ConsoleApp;

public static class EmbeddedResources
{
    public static async Task<byte[]> GetImageBytesAsync(string filename)
    {
        var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        await using var stream = embeddedProvider.GetFileInfo(filename).CreateReadStream();

        var imgBytes = new byte[stream.Length];
        stream.ReadExactly(imgBytes);

        return imgBytes;
    }
}