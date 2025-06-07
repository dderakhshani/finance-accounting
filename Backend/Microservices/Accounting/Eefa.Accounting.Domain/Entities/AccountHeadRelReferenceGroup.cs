public partial class AccountHeadRelReferenceGroup : AuditableEntity
{
    public int AccountHeadId { get; set; } = default!;
    public int ReferenceGroupId { get; set; } = default!;
    public int ReferenceNo { get; set; } = default!;
    public bool IsDebit { get; set; } = default!;
    public bool IsCredit { get; set; } = default!;


    public virtual AccountHead AccountHead { get; set; } = default!;
    public virtual AccountReferencesGroup AccountReferencesGroup { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
}

