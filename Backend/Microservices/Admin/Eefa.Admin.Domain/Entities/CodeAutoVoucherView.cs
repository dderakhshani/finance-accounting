public partial class CodeAutoVoucherView : AuditableEntity
{
    public int CompanyId { get; set; } = default!;
    public string ViewName { get; set; } = default!;
    public string ViewCaption { get; set; } = default!;


    public virtual CompanyInformation Company { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
}