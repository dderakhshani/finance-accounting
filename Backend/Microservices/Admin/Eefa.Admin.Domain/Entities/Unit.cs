using System.Collections.Generic;

[HasUniqueIndex]
public partial class Unit : HierarchicalAuditableEntity
{
    [UniqueIndex]
    public string Title { get; set; } = default!;
    public int BranchId { get; set; } = default!;


    public virtual Branch Branch { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Unit? Parent { get; set; } = default!;
    public virtual ICollection<Unit> InverseParent { get; set; } = default!;
    public virtual ICollection<UnitPosition> UnitPositions { get; set; } = default!;
}