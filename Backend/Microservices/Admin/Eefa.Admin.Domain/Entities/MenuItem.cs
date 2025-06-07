using System.Collections.Generic;

public partial class MenuItem : AuditableEntity
{
    public int? ParentId { get; set; }
    public int OrderIndex { get; set; }
    public int? PermissionId { get; set; }
    public string Title { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string? FormUrl { get; set; }
    public string? QueryParameterMappings { get; set; }
    public string? HelpUrl { get; set; }
    public string? PageCaption { get; set; }
    public bool IsActive { get; set; } = default!;


    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Permission? Permission { get; set; } = default!;
    public virtual Help Help { get; set; } = default;
    public List<PermissionCondition> PermissionConditions { get; set; }
}