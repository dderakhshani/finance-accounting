using AutoMapper;
using Eefa.Admin.Data.Databases.Entities;
using Library.Mappings;
using System.Collections.Generic;

namespace Eefa.Admin.Application.CommandQueries.Permission.Model
{
    public class PermissionModel : IMapFrom<Data.Databases.Entities.Permission>
    {
        public int Id { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;
        public bool IsDataRowLimiter { get; set; } = default!;
        public string SubSystem { get; set; }
        public bool? AccessToAll { get; set; }
        public List<PermissionCondition> PermissionConditions { get; set; }
		public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.Permission, PermissionModel>();
        }
    }

}
