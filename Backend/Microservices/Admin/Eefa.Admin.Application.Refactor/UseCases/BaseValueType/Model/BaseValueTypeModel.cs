using AutoMapper;

public class BaseValueTypeModel : IMapFrom<BaseValueType>
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string LevelCode { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public string? GroupName { get; set; }
    public bool IsReadOnly { get; set; } = default!;
    public string? SubSystem { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<BaseValueType, BaseValueTypeModel>();
    }
}