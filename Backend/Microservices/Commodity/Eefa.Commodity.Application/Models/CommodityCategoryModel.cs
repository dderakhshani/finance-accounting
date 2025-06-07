using AutoMapper;
using Eefa.Common;

namespace Eefa.Commodity.Application.Queries.Commodity
{
    public record CommodityCategoryModel : IMapFrom<Eefa.Commodity.Data.Entities.CommodityCategory>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// کد 
        /// </summary>
        public string Code { get; set; } = default!;
        public int CodingMode { get; set; } = default!;


        /// <summary>
        /// عنوان
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// کدواحد اندازه گیری
        /// </summary>
        public int MeasureId { get; set; } = default!;
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;
        public bool? RequireParentProduct { get; set; }
        /// <summary>
        /// آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;

        //public List<Data.Entities.Commodity> Commodities { get; set; }
        //public List<CommodityCategoryProperty> CommodityCategoryProperties { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.CommodityCategory, CommodityCategoryModel>()
                //.ForMember(dest => dest.MeasureUnitTitle, opt => opt.MapFrom(src => src.Measure.Title))
            ;
        }

    }


}
