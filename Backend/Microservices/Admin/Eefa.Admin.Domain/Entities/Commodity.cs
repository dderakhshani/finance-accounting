using System.Collections.Generic;

public partial class Commodity : AuditableEntity
{
    public int? ParentId { get; set; }
    public int? CategoryId { get; set; }
    public string LevelCode { get; set; } = default!;
    public string? ProductCode { get; set; }
    public string? Tite { get; set; }
    public string? Description { get; set; }
    public int? MeasureId { get; set; }
    public int CompanyId { get; set; } = default!;
    public double MinimumQuantity { get; set; } = default!;
    public double? MaximumQuantity { get; set; }
    public double? OrderQuantity { get; set; }
    public int? TypeId { get; set; }
    public int? ReferenceId { get; set; }
    public int? PricingType { get; set; }
    public virtual User CreatedBy { get; set; } = default!;
    public virtual BaseValue? Measure { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual BaseValue? PricingTypeNavigation { get; set; } = default!;
    public virtual BaseValue? Type { get; set; } = default!;
    public virtual ICollection<CommodityProperty> CommodityProperties { get; set; } = default!;
    public virtual ICollection<DocumentItem> DocumentItems { get; set; } = default!;
}