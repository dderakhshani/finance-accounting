using System;

public partial class VouchersDetail : AuditableEntity
{
    public int VoucherId { get; set; } = default!;
    public DateTime? VoucherDate { get; set; }
    public int AccountHeadId { get; set; } = default!;
    public int AccountReferencesGroupId { get; set; } = default!;
    public string VoucherRowDescription { get; set; } = default!;
    public long Debit { get; set; } = default!;
    public long Credit { get; set; } = default!;
    public int? RowIndex { get; set; }
    public int? DocumentId { get; set; }
    public int? ReferenceId { get; set; }
    public DateTime? ReferenceDate { get; set; }
    public double? ReferenceQty { get; set; }
    public int? ReferenceId1 { get; set; }
    public int? ReferenceId2 { get; set; }
    public int? ReferenceId3 { get; set; }
    public int? Level1 { get; set; }
    public int? Level2 { get; set; }
    public int? Level3 { get; set; }
    public byte? DebitCreditStatus { get; set; }
    public long? Remain { get; set; }


    public virtual AccountHead AccountHead { get; set; } = default!;
    public virtual AccountReferencesGroup AccountReferencesGroup { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual DocumentHead? Document { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual AccountReference? ReferenceId1Navigation { get; set; } = default!;
    public virtual AccountReference? ReferenceId2Navigation { get; set; } = default!;
    public virtual AccountReference? ReferenceId3Navigation { get; set; } = default!;
    public virtual VouchersHead Voucher { get; set; } = default!;
}