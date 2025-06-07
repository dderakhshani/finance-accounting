public partial class AutoVoucherFormula : AuditableEntity
{
    public int VoucherTypeId { get; set; } = default!;
    public int OrderIndex { get; set; } = default!;
    public byte DebitCreditStatus { get; set; } = default!;
    public int AccountHeadId { get; set; } = default!;
    public string? RowDescription { get; set; }
    public string? DestinationFields { get; set; }
    public string? SourceFields { get; set; }
    public string? ConditionPart { get; set; }
    public string? GroupBy { get; set; }
    public string? OrderBy { get; set; }


    public virtual AccountHead AccountHead { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual CodeVoucherGroup VoucherType { get; set; } = default!;
}