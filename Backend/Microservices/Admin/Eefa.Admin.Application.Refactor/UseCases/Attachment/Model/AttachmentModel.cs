using AutoMapper;

public class AttachmentModel : IMapFrom<Attachment>
{
    public int LanguageId { get; set; } = default!;
    public int TypeBaseId { get; set; } = default!;
    public string Extention { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? KeyWords { get; set; }
    public bool IsUsed { get; set; }

    public string Url { get; set; } = default!;
    public string TypeBaseTitle { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Attachment, AttachmentModel>()
            .ForMember(x => x.TypeBaseTitle, opt => opt.MapFrom(x => x.TypeBase.Title));
    }
}