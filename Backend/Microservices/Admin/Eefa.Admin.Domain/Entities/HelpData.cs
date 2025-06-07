using System.Collections.Generic;

public partial class HelpData : HierarchicalAuditableEntity
{
    public int? LanguageId { get; set; }
    [UniqueIndex]
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? KeyWords { get; set; }
    public string? Url { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual Language? Language { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual ICollection<HelpAttachment> HelpAttachments { get; set; } = default!;
}