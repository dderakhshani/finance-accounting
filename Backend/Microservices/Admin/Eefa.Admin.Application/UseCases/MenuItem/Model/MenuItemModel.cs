using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.MenuItem.Model
{
    public class MenuItemModel : IMapFrom<Data.Databases.Entities.MenuItem>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }
        public int OrderIndex { get; set; }

        /// <summary>
        /// کد دسترسی
        /// </summary>
        public int? PermissionId { get; set; }

        public string? QueryParameterMappings { get; set; }
        public string? HelpUrl { get; set; }
        /// <summary>
        /// عنوان منو
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// لینک تصویر
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// لینک فرم
        /// </summary>
        public string? FormUrl { get; set; }

        /// <summary>
        /// عنوان صفحه
        /// </summary>
        public string? PageCaption { get; set; }

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.MenuItem, MenuItemModel>();
        }
    }

}
