using System.Collections.Generic;

public partial class AccountReferencesGroup : AuditableEntity
{
    public int CompanyId { get; set; } = default!;
    public string Code { get; set; }
    public string LevelCode { get; set; }
    public int ParentId { get; set; }
    public string Title { get; set; } = default!;
    public bool? IsEditable { get; set; } = default!;


    public virtual CompanyInformation Company { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual AccountReferencesGroup? Parent { get; set; } = default!;
    public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; } = default!;
    public virtual ICollection<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; } = default!;
    public virtual ICollection<AccountReferencesGroup> InverseParent { get; set; } = default!;
    public virtual ICollection<Customer> Customers { get; set; } = default!;
}

