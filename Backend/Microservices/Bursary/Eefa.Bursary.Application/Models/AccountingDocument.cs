using System;

namespace Eefa.Bursary.Application.Models
{
    public class AccountingDocument
    {
        public int DocumentNo { get; set; } 
        public int DocumentId { get; set; }
        public DateTime DocumentDate { get; set; }
        public int CodeVoucherGroupId { get; set; }
        public int DebitAccountHeadId { get; set; }
        public int DebitAccountReferenceGroupId { get; set; }
        public int DebitAccountReferenceId { get; set; }
        public int CreditAccountHeadId { get; set; }
        public int CreditAccountReferenceGroupId { get; set; }
        public int CreditAccountReferenceId { get; set; }
        public decimal Amount { get; set; }
        public int DocumentTypeBaseId { get; set; }
        public int SheetUniqueNumber { get; set; }
        public int CurrencyFee { get; set; }
        public decimal CurrencyAmount { get; set; }
        public int CurrencyTypeBaseId { get; set; }
        public int NonRialStatus { get; set; }
        public int ChequeSheetId { get; set; }
        public bool IsRial { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceCode { get; set; }
        public string Description { get; set; }
    }
}
