using System.Collections.Generic;
using System.Linq;
using AutoMapper;

public class RoleModel : IMapFrom<Role>
{
    public int Id { get; set; }
    public string LevelCode { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? UniqueName { get; set; }
    public string? Description { get; set; }
    public int? ParentId { get; set; }
    public virtual IList<int> PermissionsId { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Role, RoleModel>().IgnoreAllNonExisting()
        .ForMember(x => x.PermissionsId, opt => opt.MapFrom(x => x.RolePermissionRoles
                                                            .Select(x => x.PermissionId)));
    }
}