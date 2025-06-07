using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.UseCases.Help.Model
{
    public class MinifiedHelpModel: IMapFrom<Data.Databases.Entities.Help>
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public string MenuTitle { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.Help, MinifiedHelpModel>()
                .ForMember(x => x.MenuTitle, opt => opt.MapFrom(x => x.MenuItem.Title))
                .ForMember(x => x.MenuItemId, opt => opt.MapFrom(x => x.MenuId));
        }
    }
}