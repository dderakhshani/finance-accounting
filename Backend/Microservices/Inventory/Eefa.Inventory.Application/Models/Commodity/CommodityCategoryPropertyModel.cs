using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application.Models.CommodityCategory
{
    public class CommodityCategoryPropertyModel : IMapFrom<CommodityCategoryProperty>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد گروه
        /// </summary>
        public int? CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// کد واحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; }
        public string? MeasureUnitTitle { get; set; }
        /// <summary>
        /// قوانین حاکم بر مولفه
        /// </summary>
        public int? PropertyTypeBaseId { get; set; }
        public string PropertyTypeBaseTitle { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CommodityCategoryProperty, CommodityCategoryPropertyModel>();
        }

    }
}
