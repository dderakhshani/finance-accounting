public partial class RequiredPermission : AuditableEntity
{
    public int ParentPermissionId { get; set; } = default!;
    public int PermissionId { get; set; } = default!;


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Permission ParentPermission { get; set; } = default!;
    public virtual Permission Permission { get; set; } = default!;
}