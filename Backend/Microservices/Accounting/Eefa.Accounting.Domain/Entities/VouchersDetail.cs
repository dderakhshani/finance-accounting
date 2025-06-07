using System;

public partial class VouchersDetail : AuditableEntity
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


    public virtual AccountHead AccountHead { get; set; } = default!;
    public virtual AccountReferencesGroup AccountReferencesGroup { get; set; } = default!;
    public virtual BaseValue CurrencyBaseTypeBaseValue { get; set; }
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual AccountReference? ReferenceId1Navigation { get; set; } = default!;
    public virtual AccountReference? ReferenceId2Navigation { get; set; } = default!;
    public virtual AccountReference? ReferenceId3Navigation { get; set; } = default!;
    public virtual VouchersHead Voucher { get; set; } = default!;
}

