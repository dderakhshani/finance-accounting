public partial class HelpAttachment : AuditableEntity
{
    public int HelpDataId { get; set; } = default!;
    public int AttachmentId { get; set; } = default!;


    public virtual Attachment Attachment { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual HelpData HelpData { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
}