using System;
using System.Collections.Generic;

public partial class CodeVoucherGroup : AuditableEntity
{

    public string Code { get; set; } = default!;
    public int CompanyId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; }
    public DateTime? LastEditableDate { get; set; }
    public int OrderIndex { get; set; } = default!;
    public bool IsAuto { get; set; } = default!;
    public bool IsEditable { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
    public bool AutoVoucherEnterGroup { get; set; } = default!;
    public string? BlankDateFormula { get; set; }
    public int? ViewId { get; set; }
    public int? ExtendTypeId { get; set; }
    public string? ExtendTypeName { get; set; }
    public string? TableName { get; set; }
    public string? SchemaName { get; set; }
    public int? MenuId { get; set; }


    public virtual CompanyInformation Company { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulas { get; set; } = default!;
    public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulas1 { get; set; } = default!;
    public virtual ICollection<VouchersHead> VouchersHeads { get; set; } = default!;
}

