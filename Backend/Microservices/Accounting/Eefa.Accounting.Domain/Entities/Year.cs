using System;
using System.Collections.Generic;

public partial class Year : AuditableEntity
{
    public int CompanyId { get; set; } = default!;
    public int YearName { get; set; } = default!;
    public DateTime FirstDate { get; set; } = default!;
    public DateTime LastDate { get; set; } = default!;
    public bool IsEditable { get; set; } = default!;
    public bool IsCalculable { get; set; } = default!;
    public bool IsCurrentYear { get; set; } = default!;
    public bool CreateWithOutStartVoucher { get; set; } = default!;
    public DateTime? LastEditableDate { get; set; }

    public virtual CompanyInformation Company { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual ICollection<VouchersHead> VouchersHeads { get; set; }
    public virtual ICollection<UserYear> UserYears { get; set; } = default!;
}
