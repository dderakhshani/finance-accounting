namespace TaxCollectData.Library.Dto.Content;

public record InvoiceDtoWrapper
{
    public InvoiceDto Invoice { get; set; }
    public string FiscalId { get; set; }
    public string Uid { get; set; }
}