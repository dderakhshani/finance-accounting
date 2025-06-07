using AutoMapper;

public class PositionModel : IMapFrom<Position>
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string? LevelCode { get; set; }
    public string? Title { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Position, PositionModel>();
    }
}