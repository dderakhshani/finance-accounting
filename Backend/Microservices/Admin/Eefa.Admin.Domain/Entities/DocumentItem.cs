using System;

public partial class DocumentItem : AuditableEntity
{
    public int DocumentHeadId { get; set; } = default!;
    public int? FirstDocumentHeadId { get; set; }
    public int? ParentId { get; set; }
    public int CommodityId { get; set; } = default!;
    public string? CommoditySerial { get; set; }
    public string? CommodityBachNumber { get; set; }
    public double Quantity { get; set; } = default!;
    public long UnitPrice { get; set; } = default!;
    public long BasePrice { get; set; } = default!;
    public DateTime? ExpireDate { get; set; }
    public string? PartNumber { get; set; }
    public int? CurrencyPrice { get; set; }
    public string? LadingBillNo { get; set; }
    public int? EquipmentId { get; set; }
    public string? EquipmentSerial { get; set; }
    public long Tax { get; set; } = default!;
    public long Discount { get; set; } = default!;


    public virtual Commodity Commodity { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual DocumentHead DocumentHead { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
}