using System;
using System.Collections.Generic;

public partial class CodeVoucherGroup : AuditableEntity
{
    public string Code { get; set; } = default!;
    public int CompanyId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime? LastEditableDate { get; set; }
    public bool IsAuto { get; set; } = default!;
    public bool IsEditable { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
    public bool AutoVoucherEnterGroup { get; set; } = default!;
    public string? BlankDateFormula { get; set; }
    public int? ViewId { get; set; }
    public int? ExtendTypeId { get; set; }
    public string? ExtendTypeName { get; set; }


    public virtual CompanyInformation Company { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual AutoVoucherIncompleteVoucher? AutoVoucherIncompleteVoucher { get; set; } = default!;
    public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulas { get; set; } = default!;
    public virtual ICollection<AutoVoucherLog> AutoVoucherLogs { get; set; } = default!;
    public virtual ICollection<AutoVoucherRowsLink> AutoVoucherRowsLinks { get; set; } = default!;
    public virtual ICollection<VouchersHead> VouchersHeads { get; set; } = default!;
    public virtual ICollection<CorrectionRequest> CorrectionRequests { get; set; } = default!;
}