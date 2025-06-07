using System;

public partial class AutoVoucherRowsLink : AuditableEntity
{
    public int? VoucherId { get; set; }
    public int? VoucherTypeId { get; set; }
    public DateTime? VoucherDate { get; set; }
    public int? RowId { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual CodeVoucherGroup? VoucherType { get; set; } = default!;
}