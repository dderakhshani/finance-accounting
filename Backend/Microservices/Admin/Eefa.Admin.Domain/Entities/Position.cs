using System.Collections.Generic;

[HasUniqueIndex]
public partial class Position : HierarchicalAuditableEntity
{
    [UniqueIndex]
    public string Title { get; set; } = default!;


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual ICollection<UnitPosition> UnitPositions { get; set; } = default!;
}