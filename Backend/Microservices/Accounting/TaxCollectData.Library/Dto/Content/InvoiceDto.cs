namespace TaxCollectData.Library.Dto.Content
{
    public record InvoiceDto
    {
        public InvoiceHeaderDto Header { get; init; }

        public List<InvoiceBodyDto> Body { get; init; }

        public List<PaymentDto> Payments { get; init; }

        public List<InvoiceExtension> Extension { get; init; }
    }
}