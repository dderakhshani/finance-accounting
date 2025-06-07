public partial class RolePermission : AuditableEntity
{
    public int RoleId { get; set; } = default!;
    public int PermissionId { get; set; } = default!;

    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Permission Permission { get; set; } = default!;
    public virtual Role Role { get; set; } = default!;
}

