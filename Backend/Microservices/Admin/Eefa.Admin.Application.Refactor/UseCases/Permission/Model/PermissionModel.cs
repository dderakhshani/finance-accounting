using AutoMapper;
using System.Collections.Generic;

public class PermissionModel : IMapFrom<Permission>
{
    public int Id { get; set; }
    public string LevelCode { get; set; } = default!;
    public int? ParentId { get; set; }
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public bool IsDataRowLimiter { get; set; } = default!;
    public string SubSystem { get; set; }
    public bool? AccessToAll { get; set; }
    public List<PermissionCondition> PermissionConditions { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Permission, PermissionModel>();
    }
}