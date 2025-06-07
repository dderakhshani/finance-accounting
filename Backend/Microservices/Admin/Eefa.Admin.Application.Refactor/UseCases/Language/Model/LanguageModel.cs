using AutoMapper;

public class LanguageModel : IMapFrom<Language>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Culture { get; set; } = default!;
    public string? SeoCode { get; set; }
    public string? FlagImageUrl { get; set; }
    public int DirectionBaseId { get; set; } = default!;
    public string DirectionBaseTitle { get; set; } = default!;
    public int DefaultCurrencyBaseId { get; set; } = default!;
    public string DefaultCurrencyBaseTitle { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Language, LanguageModel>()
            .ForMember(x => x.DirectionBaseTitle, opt => opt.MapFrom(x => x.DirectionBase.Title))
            .ForMember(x => x.DefaultCurrencyBaseTitle, opt => opt.MapFrom(x => x.DefaultCurrencyBase.Title));
    }
}