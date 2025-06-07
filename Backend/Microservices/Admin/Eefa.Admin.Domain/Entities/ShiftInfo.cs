using System;

[HasUniqueIndex]
public partial class ShiftInfo : AuditableEntity
{
    [UniqueIndex]
    public string Title { get; set; } = default!;
    public DateTime StartTime { get; set; } = default!;
    public DateTime? EndTime { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
}