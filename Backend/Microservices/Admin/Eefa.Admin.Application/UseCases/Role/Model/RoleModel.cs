using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.Role.Model
{
    public class RoleModel : IMapFrom<Data.Databases.Entities.Role>
    {
        public int Id { get; set; }
        /// <summary>
        /// سطح دسترسی
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string? UniqueName { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }


        public virtual IList<int> PermissionsId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.Role, RoleModel>().IgnoreAllNonExisting()
            .ForMember(x=>x.PermissionsId, opt =>
            {
                opt.MapFrom(x =>
                    x.RolePermissionRoles.Select(x => x.PermissionId));
            });
        }
    }
}
