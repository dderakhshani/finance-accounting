using System.Collections.Generic;

public partial class Attachment : AuditableEntity
{
    public int LanguageId { get; set; } = default!;
    public int TypeBaseId { get; set; } = default!;
    public string Extention { get; set; } = default!;
    public bool IsUsed { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? KeyWords { get; set; }
    public string Url { get; set; } = default!;


    public virtual User CreatedBy { get; set; } = default!;
    public virtual Language Language { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual BaseValue TypeBase { get; set; } = default!;
    public virtual ICollection<HelpAttachment> HelpAttachments { get; set; } = default!;
    public virtual ICollection<VoucherAttachment> VoucherAttachments { get; set; } = default!;
}