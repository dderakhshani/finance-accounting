using AutoMapper;

public class CodeRowDescriptionModel : IMapFrom<CodeRowDescription>
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Title { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeRowDescription, CodeRowDescriptionModel>();
    }
}