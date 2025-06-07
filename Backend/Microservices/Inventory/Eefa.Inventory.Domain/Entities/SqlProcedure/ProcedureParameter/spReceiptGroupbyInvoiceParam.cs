using System;

namespace Eefa.Inventory.Domain
{
    public class spReceiptGroupbyInvoiceParam
    {
        public DateTime? FromInvoiceDate { get; set; }
        public DateTime? ToInvoiceDate { get; set; }
        public int? VoucherHeadId { get; set; }
        public string DocumentIds { get; set; }
        public int? CreditAccountReferenceId  { get; set; }
        public int? DebitAccountReferenceId  { get; set; }
        public int? CreditAccountHeadId  { get; set; }
        public int? CreditAccountReferenceGroupId  { get; set; }
        public int? DebitAccountReferenceGroupId  { get; set; }
        public int? DebitAccountHeadId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageRow { get; set; }
       
    }
    public class spReceiptInvoiceParam
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string InvoiceNo { get; set; }
        public int? CreditAccountReferenceId { get; set; }
        public int PageNumber { get; set; }
        public int PageRow { get; set; }

    }
    public class spCommodityCostParam
    {
        public DateTime? FromInvoiceDate { get; set; }
        public DateTime? ToInvoiceDate { get; set; }
        public string WarehouseId { get; set; }
    }
}
