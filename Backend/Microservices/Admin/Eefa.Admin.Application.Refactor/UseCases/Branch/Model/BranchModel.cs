using AutoMapper;

public class BranchModel : IMapFrom<Branch>
{
    public int Id { get; set; }
    public string LevelCode { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int? ParentId { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Branch, BranchModel>()

            ;
    }
}