public partial class AccountReferencesRelReferencesGroup : AuditableEntity
{
    public int ReferenceId { get; set; } = default!;
    public int ReferenceGroupId { get; set; } = default!;

    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual AccountReference Reference { get; set; } = default!;
    public virtual AccountReferencesGroup ReferenceGroup { get; set; } = default!;
}

