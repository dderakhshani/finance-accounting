public partial class AutoVoucherFormula : AuditableEntity
{
    public int VoucherTypeId { get; set; } = default!;
    public int SourceVoucherTypeId { get; set; } = default!;
    public int OrderIndex { get; set; } = default!;
    public byte DebitCreditStatus { get; set; } = default!;
    public int AccountHeadId { get; set; } = default!;
    public string? RowDescription { get; set; }
    public string? Formula { get; set; }
    public string? Conditions { get; set; }
    public string? GroupBy { get; set; }

    public virtual AccountHead AccountHead { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual CodeVoucherGroup VoucherType { get; set; } = default!;
    public virtual CodeVoucherGroup SourceVoucherType { get; set; } = default!;
}

