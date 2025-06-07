using System;

public partial class AutoVoucherIncompleteVoucher : AuditableEntity
{
    public int? VoucherTypeId { get; set; }
    public DateTime? VoucherDate { get; set; }
    public int? RowId { get; set; }
    public string? Description { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual CodeVoucherGroup IdNavigation { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
}