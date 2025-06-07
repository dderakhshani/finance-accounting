public partial class Position : AuditableEntity
{
    public int? ParentId { get; set; }
    public string LevelCode { get; set; } = default!;
    public string Title { get; set; } = default!;

    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
}

