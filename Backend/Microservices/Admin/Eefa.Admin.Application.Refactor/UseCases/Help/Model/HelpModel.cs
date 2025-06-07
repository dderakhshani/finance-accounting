using AutoMapper;

public class HelpModel : IMapFrom<Help>
{
    public int Id { get; set; }
    public int MenuItemId { get; set; }
    public string Contents { get; set; }
    public string MenuTitle { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Help, HelpModel>()
            .ForMember(x => x.MenuTitle, opt => opt.MapFrom(x => x.MenuItem.Title))
            .ForMember(x => x.MenuItemId, opt => opt.MapFrom(x => x.MenuId));
    }
}