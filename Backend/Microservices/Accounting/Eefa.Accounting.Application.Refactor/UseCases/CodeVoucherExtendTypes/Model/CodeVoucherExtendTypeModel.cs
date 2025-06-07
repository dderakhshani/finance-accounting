using AutoMapper;

public class CodeVoucherExtendTypeModel : IMapFrom<CodeVoucherExtendType>
{
    public int Id { get; set; }
    public string Title { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeVoucherExtendType, CodeVoucherExtendTypeModel>()
            .IgnoreAllNonExisting();
    }
}