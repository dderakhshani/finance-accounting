public partial class UserSetting : AuditableEntity
{
    public int UserId { get; set; } = default!;
    public string? Keyword { get; set; }
    public string? Value { get; set; }

    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual User User { get; set; } = default!;
}

