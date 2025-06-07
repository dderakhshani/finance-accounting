using System;
using System.Collections.Generic;

public partial class CompanyInformation : AuditableEntity
{
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public DateTime? ExpireDate { get; set; }
    public int MaxNumOfUsers { get; set; } = default!;
    public string? Logo { get; set; }


    public ICollection<UserCompany> UserCompanies { get; set; }
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual ICollection<AccountReferencesGroup> AccountReferencesGroups { get; set; } = default!;
    public virtual ICollection<CodeAccountHeadGroup> CodeAccountHeadGroups { get; set; } = default!;
    public virtual ICollection<CodeAutoVoucherView> CodeAutoVoucherViews { get; set; } = default!;
    public virtual ICollection<CodeRowDescription> CodeRowDescriptions { get; set; } = default!;
    public virtual ICollection<CodeVoucherGroup> CodeVoucherGroups { get; set; } = default!;
    public virtual ICollection<VouchersHead> VouchersHeads { get; set; } = default!;
    public virtual ICollection<Year> Years { get; set; } = default!;
}