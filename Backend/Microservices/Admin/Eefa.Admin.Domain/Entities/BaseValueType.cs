using System.Collections.Generic;

[HasUniqueIndex]
public partial class BaseValueType : HierarchicalAuditableEntity
{
    public string Title { get; set; } = default!;
    [UniqueIndex]
    public string UniqueName { get; set; } = default!;
    public string? GroupName { get; set; }
    public bool IsReadOnly { get; set; } = default!;
    public string? SubSystem { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual BaseValueType? Parent { get; set; } = default!;
    public virtual ICollection<BaseValue> BaseValues { get; set; } = default!;
    public virtual ICollection<BaseValueType> InverseParent { get; set; } = default!;
}