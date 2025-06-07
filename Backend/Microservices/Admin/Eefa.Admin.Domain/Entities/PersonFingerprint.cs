public partial class PersonFingerprint : AuditableEntity
{
    public int PersonId { get; set; } = default!;
    public int FingerBaseId { get; set; } = default!;
    public string FingerPrintTemplate { get; set; } = default!;
    public string? FingerPrintPhotoURL { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual BaseValue FingerBase { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Person Person { get; set; } = default!;
}