using AutoMapper;
using Library.ConfigurationAccessor.Mappings;

namespace Eefa.Admin.Application.UseCases.MenuItem.Model
{
    public class MenuItemRoleModel : IMapFrom<Data.Databases.Entities.Role>
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? UniqueName { get; set; }
        public string? Description { get; set; }

        public void Mapping(Profile profile) 
        {
            profile.CreateMap<Data.Databases.Entities.Role, MenuItemRoleModel>();
        }
    }
}