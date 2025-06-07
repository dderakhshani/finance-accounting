using AutoMapper;

public class MenuItemModel : IMapFrom<MenuItem>
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public int OrderIndex { get; set; }
    public int? PermissionId { get; set; }
    public string? QueryParameterMappings { get; set; }
    public string? HelpUrl { get; set; }
    public string Title { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string? FormUrl { get; set; }
    public string? PageCaption { get; set; }
    public bool IsActive { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<MenuItem, MenuItemModel>();
    }
}