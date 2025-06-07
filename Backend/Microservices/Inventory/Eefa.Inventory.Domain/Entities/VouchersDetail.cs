using System;
using Eefa.Common.Domain;


namespace Eefa.Inventory.Domain
{

    public partial class VouchersDetail : DomainBaseEntity
    {

        public int VoucherId { get; set; } = default!;
        public DateTime VoucherDate { get; set; }
        public int AccountHeadId { get; set; } = default!;
        public int? AccountReferencesGroupId { get; set; } = default!;
        public string VoucherRowDescription { get; set; } = default!;
        public double Credit { get; set; } = default!;
        public double Debit { get; set; } = default!;
        public int? RowIndex { get; set; }
        public int? DocumentId { get; set; }
        public string? DocumentNo { get; set; }
        public string? DocumentIds { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string? FinancialOperationNumber { get; set; }
        public string? RequestNo { get; set; }
        public string? InvoiceNo { get; set; }
        public string? Tag { get; set; }

        public double? Weight { get; set; }

        public int? ReferenceId1 { get; set; }
        public int? ReferenceId2 { get; set; }
        public int? ReferenceId3 { get; set; }

        public int? ChequeSheetId { get; set; }
        public int? Level1 { get; set; }
        public int? Level2 { get; set; }
        public int? Level3 { get; set; }
        public byte? DebitCreditStatus { get; set; }
        public int? CurrencyTypeBaseId { get; set; }
        public double? CurrencyFee { get; set; }
        public double? CurrencyAmount { get; set; }
        public int? TraceNumber { get; set; }
        public double? Quantity { get; set; }
        public double? Remain { get; set; }


        
    }
}
