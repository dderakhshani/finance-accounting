using AutoMapper;
using Eefa.Common;
using System.Collections.Generic;

namespace Eefa.Commodity.Application.Queries.Commodity
{
    public record CommodityCategoryPropertyModel : IMapFrom<Eefa.Commodity.Data.Entities.CommodityCategoryProperty>
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
        public string MeasureTitle { get; set; } = default!;
        /// <summary>
        /// قوانین حاکم بر مولفه
        /// </summary>
        public int? PropertyTypeBaseId { get; set; }
        public string PropertyTypeBaseTitle { get; set; } = default!;


        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

       

        

        public List<CommodityCategoryPropertyItemModel> Items { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.CommodityCategoryProperty, CommodityCategoryPropertyModel>()
                .ForMember(x => x.Items, opt => opt.MapFrom(x => x.CommodityCategoryPropertyItems));
        }

    }


}
