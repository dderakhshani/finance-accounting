using System.Collections.Generic;
using System.Linq;
using AutoMapper;

public class UnitModel : IMapFrom<Unit>
{
    public int Id { get; set; }
    public string LevelCode { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int? ParentId { get; set; }
    public int? BranchId { get; set; }
    public ICollection<int> PositionIds { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Unit, UnitModel>()
            .ForMember(x => x.PositionIds, opt => opt.MapFrom(x => x.UnitPositions.Select(t => t.PositionId)));
    }
}