using AutoMapper;
using Eefa.Common;

namespace Eefa.Purchase.Application.Models.Warehouse
{

    public record WarehouseModel : IMapFrom<Domain.Entities.Warehouse>
    {
        public int Id { get; set; }
        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; }
        /// <description>
        /// کد سطح
        ///</description>

        public string? LevelCode { get; set; }
        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; } = default!;
        /// <description>
        /// فعال
        ///</description>

        public bool IsActive { get; set; } = default!;
        /// <description>
        /// کد گروه کالا
        ///</description>

        public int CommodityCategoryId { get; set; } = default!;
        //public string CommodityCategoryTitle { get; set; } = default!;
        public int? PermissionId { get; set; } 
        public string AccessPermissionTitle { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Warehouse, WarehouseModel>();
        }
    }
}
