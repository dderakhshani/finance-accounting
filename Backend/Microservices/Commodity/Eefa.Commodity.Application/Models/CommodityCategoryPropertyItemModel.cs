using AutoMapper;
using Eefa.Common;

namespace Eefa.Commodity.Application.Queries.Commodity
{
    public record CommodityCategoryPropertyItemModel : IMapFrom<Eefa.Commodity.Data.Entities.CommodityCategoryPropertyItem>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد ویژگی گروه
        /// </summary>
        public int CategoryPropertyId { get; set; } = default!;
        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; } = default!;
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// کد
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;
        public bool IsDeleted { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Eefa.Commodity.Data.Entities.CommodityCategoryPropertyItem, CommodityCategoryPropertyItemModel>();
        }

    }


}
