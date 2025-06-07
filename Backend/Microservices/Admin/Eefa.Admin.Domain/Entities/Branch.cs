using System.Collections.Generic;

[HasUniqueIndex]
public partial class Branch : HierarchicalAuditableEntity
{
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    [UniqueIndex]
    public string Title { get; set; } = default!;


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Branch? Parent { get; set; } = default!;
    public virtual ICollection<Branch> InverseParent { get; set; } = default!;
    public virtual ICollection<Unit> Units { get; set; } = default!;
}