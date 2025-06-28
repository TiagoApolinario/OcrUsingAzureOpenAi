namespace OcrUsingAzureOpenAi.ConsoleApp;

public sealed record InvoiceResponseDto
{
    public string InvoiceDate { get; set; }
    public string InvoiceNumber { get; set; }
    public List<InvoiceItemDto> Items { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
}

public sealed record InvoiceItemDto(
    string Description,
    int Quantity,
    decimal UnitPrice,
    decimal Gst,
    decimal Amount);