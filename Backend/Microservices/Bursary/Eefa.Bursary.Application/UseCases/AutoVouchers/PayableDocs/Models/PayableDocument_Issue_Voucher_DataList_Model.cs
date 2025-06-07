namespace Eefa.Bursary.Application.UseCases.AutoVouchers.PayableDocs.Models
{
    public class PayableDocument_Issue_Voucher_DataList_Model
    {
        public long DocumentId { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; }
        public long TotalAmount { get; set; }
        public int DebitAccountHeadId { get; set; }
        public int DebitAccountReferenceGroupId { get; set; }
        public int DebitAccountReferenceId { get; set; }
        public int? CreditAccountHeadId { get; set; }
        public int? CreditAccountReferenceGroupId { get; set; }
        public int? CreditAccountReferenceId { get; set; }
        public int? CurrencyTypeBaseId { get; set; }
        public long? CurrencyFee { get; set; }
        public long? CurrencyAmount { get; set; }
        public string DebitGroupDescription { get; set; }
        public string CreditGroupDescription { get; set; }
        public string DocumentIds { get; set; }
        public int? DebitGroupCode { get; set; }
        public int? CreditGroupCode { get; set; }
    }
}
