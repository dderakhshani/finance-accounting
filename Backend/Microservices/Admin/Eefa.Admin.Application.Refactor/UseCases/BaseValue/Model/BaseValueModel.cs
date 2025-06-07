using AutoMapper;

public class BaseValueModel : IMapFrom<BaseValue>
{
    public int Id { get; set; }
    public int BaseValueTypeId { get; set; } = default!;
    public int? ParentId { get; set; }
    public string LevelCode { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public string Value { get; set; } = default!;
    public int OrderIndex { get; set; } = default!;
    public bool IsReadOnly { get; set; } = default!;
    public string Code { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<BaseValue, BaseValueModel>()
            ;
    }
}