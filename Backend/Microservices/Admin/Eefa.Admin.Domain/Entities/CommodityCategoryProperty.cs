using System.Collections.Generic;

public partial class CommodityCategoryProperty : AuditableEntity
{
    public int? ParentId { get; set; }
    public int? CategoryId { get; set; }
    public string LevelCode { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int? MeasureId { get; set; }
    public string? PropertyRule { get; set; }
    public int OrderIndex { get; set; } = default!;


    public virtual User CreatedBy { get; set; } = default!;
    public virtual BaseValue? Measure { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual ICollection<CommodityProperty> CommodityProperties { get; set; } = default!;
}