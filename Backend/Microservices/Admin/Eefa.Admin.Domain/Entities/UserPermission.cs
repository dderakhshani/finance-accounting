public class UserPermission : AuditableEntity
{
    public int UserId { get; set; }
    public int PermissionId { get; set; }


    public User CreatedBy { get; set; }
    public User ModifiedBy { get; set; }
    public Role OwnerRole { get; set; }
    public User User { get; set; }
    public Permission Permission { get; set; }
}