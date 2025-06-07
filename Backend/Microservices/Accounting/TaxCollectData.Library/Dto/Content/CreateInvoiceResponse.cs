namespace TaxCollectData.Library.Dto.Content;

public record CreateInvoiceResponse
{
    public string ConfirmationReferenceId { get; set; }
    public List<InvoiceErrorModel> Error { get; set; }
    public List<InvoiceErrorModel> Warning { get; set; }
    public bool Success { get; set; } = true;
}