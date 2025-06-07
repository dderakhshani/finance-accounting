using System.Collections.Generic;

public partial class AccountReference : AuditableEntity
{
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public bool IsActive { get; set; } = default!;

    public virtual Person Person { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual ICollection<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; } = default!;
    public virtual ICollection<VouchersDetail> VouchersDetailReferenceId1Navigation { get; set; } = default!;
    public virtual ICollection<VouchersDetail> VouchersDetailReferenceId2Navigation { get; set; } = default!;
    public virtual ICollection<VouchersDetail> VouchersDetailReferenceId3Navigation { get; set; } = default!;
    public virtual ICollection<MoadianInvoiceHeader> MoadianInvoices { get; set; } = default!;


}
