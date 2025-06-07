public partial class CommodityProperty : AuditableEntity
{
    public int CommodityId { get; set; } = default!;
    public int CategoryPropertyId { get; set; } = default!;
    public int? ValueBaseId { get; set; }
    public string? Value { get; set; }


    public virtual CommodityCategoryProperty CategoryProperty { get; set; } = default!;
    public virtual Commodity Commodity { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual BaseValue? ValueBase { get; set; } = default!;
}