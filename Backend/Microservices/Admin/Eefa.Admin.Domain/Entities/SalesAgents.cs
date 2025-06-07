using System.Collections.Generic;

public partial class SalesAgents : AuditableEntity
{
    public int PersonId { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Description { get; set; }
    public bool? IsActive { get; set; } = default!;


    public virtual Person Person { get; set; } = default!;
    public virtual ICollection<Customer> Customers { get; set; } = default!;
}