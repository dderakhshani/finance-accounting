using AutoMapper;
using Eefa.Common;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{

    public record WarehouseModel : IMapFrom<Domain.Warehouse>
    {
        public int Id { get; set; }
        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; }
        /// <description>
        /// کد سطح
        ///</description>

        public string LevelCode { get; set; }
        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; } = default!;
        /// <description>
        /// فعال
        ///</description>

        public bool IsActive { get; set; } = default!;
        public int? AccountRererenceGroupId { get; set; } = default!;
        public int? AccountReferenceId { get; set; } = default!;
        public int? AccountHeadId { get; set; } = default!;
        public int? Sort { get; set; } = default!;
        public bool Countable { get; set; } = default!;
        /// <description>
        /// کد گروه کالا
        ///</description>

        public int CommodityCategoryId { get; set; } = default!;
        //public string CommodityCategoryTitle { get; set; } = default!;
        public List<CommodityCategoryModel> CommodityCategories { get; set; } = default!;
        public List<ReceiptALLStatusModel> ReceiptAllStatus { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Warehouse, WarehouseModel>();
        }
    }
}
