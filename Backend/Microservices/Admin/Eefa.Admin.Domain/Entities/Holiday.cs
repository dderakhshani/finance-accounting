using System;

public partial class Holiday : AuditableEntity
{
    public DateTime Date { get; set; } = default!;
    public string? Description { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
}