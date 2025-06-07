using System.Collections.Generic;

[HasUniqueIndex]
public partial class Language : AuditableEntity
{
    public string Title { get; set; } = default!;
    public string Culture { get; set; } = default!;
    [UniqueIndex]
    public string? SeoCode { get; set; }
    public string? FlagImageUrl { get; set; }
    public int DirectionBaseId { get; set; } = default!;
    public int DefaultCurrencyBaseId { get; set; } = default!;


    public virtual User CreatedBy { get; set; } = default!;
    public virtual BaseValue DirectionBase { get; set; } = default!;
    public virtual BaseValue DefaultCurrencyBase { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual ICollection<Attachment> Attachments { get; set; } = default!;
    public virtual ICollection<HelpData> HelpDatas { get; set; } = default!;
}
