using System.Collections.Generic;

[HasUniqueIndex]
public partial class Permission : HierarchicalAuditableEntity
{
    public string Title { get; set; } = default!;
    [UniqueIndex]
    public string UniqueName { get; set; } = default!;
    public bool IsDataRowLimiter { get; set; } = default!;
    public string SubSystem { get; set; }
    public bool? AccessToAll { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Permission? Parent { get; set; } = default!;
    public virtual ICollection<Permission> InverseParent { get; set; } = default!;
    public virtual ICollection<MenuItem> MenuItems { get; set; } = default!;
    public virtual ICollection<RequiredPermission> RequiredPermissionParentPermissions { get; set; } = default!;
    public virtual ICollection<RequiredPermission> RequiredPermissionPermissions { get; set; } = default!;
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = default!;
    public List<UserPermission> UserPermissions { get; set; }
    public List<PermissionCondition> PermissionConditions { get; set; }
}