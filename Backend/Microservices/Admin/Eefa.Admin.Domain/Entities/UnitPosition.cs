using System.Collections.Generic;

public partial class UnitPosition : AuditableEntity
{
    public int PositionId { get; set; } = default!;
    public int UnitId { get; set; } = default!;


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Position Position { get; set; } = default!;
    public virtual Unit Unit { get; set; } = default!;
    public virtual ICollection<Employee> Employees { get; set; } = default!;
}