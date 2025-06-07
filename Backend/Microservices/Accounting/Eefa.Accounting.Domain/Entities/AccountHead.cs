using System.Collections.Generic;

public partial class AccountHead : AuditableEntity
{
    public int CompanyId { get; set; } = default!;
    public int CodeLevel { get; set; } = default!;
    public int ParentId { get; set; } = default!;
    public string LevelCode { get; set; } = default!;
    public string Code { get; set; } = default!;
    public int? CodeLength { get; set; }
    public bool LastLevel { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int? BalanceId { get; set; }
    public int BalanceBaseId { get; set; } = default!;
    public string? BalanceName { get; set; }
    public int TransferId { get; set; } = default!;
    public string? TransferName { get; set; }
    public int? GroupId { get; set; }
    public int CurrencyBaseTypeId { get; set; } = default!;
    public bool CurrencyFlag { get; set; } = default!;
    public bool ExchengeFlag { get; set; } = default!;
    public bool TraceFlag { get; set; } = default!;
    public bool QuantityFlag { get; set; } = default!;
    public bool? IsActive { get; set; } = default!;

    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual AccountHead? Parent { get; set; } = default!;
    public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; } = default!;
    public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulas { get; set; } = default!;
    public virtual ICollection<AccountHead> InverseParent { get; set; } = default!;
    public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
}

