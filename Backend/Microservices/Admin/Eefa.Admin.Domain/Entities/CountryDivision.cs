using System.Collections.Generic;

public partial class CountryDivision : AuditableEntity
{
    public string? Ostan { get; set; }
    public string? OstanTitle { get; set; }
    public string? Shahrestan { get; set; }
    public string? ShahrestanTitle { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
}