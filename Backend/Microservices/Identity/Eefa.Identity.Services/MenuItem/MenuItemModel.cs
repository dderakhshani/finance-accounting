using AutoMapper;
using Library.Mappings;

namespace Eefa.Identity.Services.MenuItem
{
    public class MenuItemModel : IMapFrom<Data.Databases.Entities.MenuItem>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? PermissionId { get; set; }
        public string Title { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public string? HelpUrl { get; set; }
        public string? FormUrl { get; set; }
        public string? PageCaption { get; set; }
        public bool IsActive { get; set; } = default!;
        public int OrderIndex { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.MenuItem, MenuItemModel>();
        }
    }
}