public partial class CodeAccountHeadGroup : AuditableEntity
{
    public int CompanyId { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;


    public virtual CompanyInformation Company { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
}