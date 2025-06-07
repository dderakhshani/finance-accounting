using System;

public partial class AutoVoucherLog : AuditableEntity
{
    public DateTime? ActionDate { get; set; }
    public int? VoucherTypeId { get; set; }
    public DateTime? VoucherDate { get; set; }
    public string? RowDescription { get; set; }
    public byte? ResultId { get; set; }
    public string? ResultName { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual CodeVoucherGroup? VoucherType { get; set; } = default!;
}