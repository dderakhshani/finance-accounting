using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application.Models.CommodityCategory
{
    public class CommodityCategoryPropertyItemModel : IMapFrom<CommodityCategoryPropertyItem>
    {
        public int Id { get; set; }
        /// <description>
        /// کد ویژگی گروه
        ///</description>

        public int CategoryPropertyId { get; set; } = default!;
        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; }
        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; } = default!;
        /// <description>
        /// نام اختصاصی
        ///</description>

        public string UniqueName { get; set; } = default!;
        /// <description>
        /// کد
        ///</description>

        public string? Code { get; set; }
        /// <description>
        /// ترتیب نمایش
        ///</description>

        public int OrderIndex { get; set; } = default!;
        /// <description>
        /// فعال است؟
        ///</description>

        public bool IsActive { get; set; } = default!;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CommodityCategoryPropertyItem, CommodityCategoryPropertyItemModel>();
        }

    }
}
