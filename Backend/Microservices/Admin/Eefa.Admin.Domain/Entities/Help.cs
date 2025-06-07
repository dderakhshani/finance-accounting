[HasUniqueIndex]
public partial class Help : AuditableEntity
{
    public string Contents { get; set; } = default!;
    public int MenuId { get; set; } = default!;


    public virtual MenuItem MenuItem { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;

}