using System.Collections.Generic;


public partial class Permission : AuditableEntity
{
    public string LevelCode { get; set; } = default!;
    public int? ParentId { get; set; }
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public bool IsDataRowLimiter { get; set; } = default!;

    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Permission? Parent { get; set; } = default!;
    public virtual ICollection<Permission> InverseParent { get; set; } = default!;
    public virtual ICollection<RequiredPermission> RequiredPermissionParentPermissions { get; set; } = default!;
    public virtual ICollection<RequiredPermission> RequiredPermissionPermissions { get; set; } = default!;
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = default!;
}
