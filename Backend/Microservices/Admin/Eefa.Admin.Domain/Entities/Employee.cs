using System;

[HasUniqueIndex]
public partial class Employee : AuditableEntity
{
    public int PersonId { get; set; } = default!;
    public int UnitPositionId { get; set; } = default!;
    [UniqueIndex]
    public string EmployeeCode { get; set; } = default!;
    public DateTime EmploymentDate { get; set; } = default!;
    public bool Floating { get; set; } = default!;
    public DateTime? LeaveDate { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Person Person { get; set; } = default!;
    public virtual UnitPosition UnitPosition { get; set; } = default!;
}