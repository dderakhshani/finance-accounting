using System;

public partial class Signer : AuditableEntity
{
    public int PersonId { get; set; } = default!;
    public string SignerDescription { get; set; } = default!;
    public int SignerOrderIndex { get; set; } = default!;
    public bool? IsActive { get; set; } = default!;
    public DateTime ActiveDate { get; set; } = default!;
    public DateTime? ExpireDate { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Person Person { get; set; } = default!;
}