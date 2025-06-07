public partial class UserRole : AuditableEntity
{
    public int RoleId { get; set; } = default!;
    public int UserId { get; set; } = default!;
    public bool AllowedStatus { get; set; } = default!;

    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Role Role { get; set; } = default!;
    public virtual User User { get; set; } = default!;
}
