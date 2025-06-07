using System;
using System.Collections.Generic;

public partial class VouchersHead : AuditableEntity
{
    public int CompanyId { get; set; } = default!;
    public int VoucherDailyId { get; set; } = default!;
    public int? TraceNumber { get; set; } = null;
    public int YearId { get; set; } = default!;
    public int VoucherNo { get; set; } = default!;
    public DateTime VoucherDate { get; set; } = default!;
    public string VoucherDescription { get; set; } = default!;
    public int CodeVoucherGroupId { get; set; } = default!;
    public int VoucherStateId { get; set; } = default!;
    public string? VoucherStateName { get; set; }
    public int? AutoVoucherEnterGroup { get; set; }
    public double? TotalDebit { get; set; }
    public double? TotalCredit { get; set; }
    public double? Difference { get; set; }

    public virtual CodeVoucherGroup CodeVoucherGroup { get; set; } = default!;
    public virtual CompanyInformation Company { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Year Year { get; set; }
    public virtual ICollection<VoucherAttachment> VoucherAttachments { get; set; } = default!;
    public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
}
