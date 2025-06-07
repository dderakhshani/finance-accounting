using AutoMapper;
using Eefa.Common;
using System.Collections.Generic;

namespace Eefa.Commodity.Application.Queries.Commodity
{
    public record CommodityModel : IMapFrom<Eefa.Commodity.Data.Entities.Commodity>
    {
        public int Id { get; set; }
        public string? CommodityNationalId { get; set; }

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد گروه کالا
        /// </summary>
        public int? CommodityCategoryId { get; set; }
        public string? CommodityCategoryTitle { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// کد محصول
        /// </summary>
        public string? Code { get; set; }
        public string? TadbirCode { get; set; }
        public string? CompactCode { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Descriptions { get; set; }

        /// <summary>
        /// کد واحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; }

        public string MeasureTitle { get; set; }

        /// <summary>
        /// کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

        /// <summary>
        /// حداقل تعداد
        /// </summary>
        public double MinimumQuantity { get; set; } = default!;

        /// <summary>
        /// حداکثر تعداد
        /// </summary>
        public double? MaximumQuantity { get; set; }

        /// <summary>
        /// تعداد سفارش
        /// </summary>
        public double? OrderQuantity { get; set; }

        /// <summary>
        /// نوع محاسبه قیمت
        /// </summary>
        public int? PricingTypeBaseId { get; set; }
        /// <summary>
        /// عنوان کد ملی کالا
        /// </summary>
        public string CommodityNationalTitle { get; set; }
        public bool? IsConsumable { get; set; }
        /// <summary>
        ///داغی دارد
        /// </summary>
        public bool? IsHaveWast { get; set; }
        /// <summary>
        ///اموال
        /// </summary>
        /// 
        public bool? IsAsset { get; set; }

        public bool? IsWrongMeasure { get; set; }
        public int ? BomsCount { get; set; }
        public bool? IsHaveForceWast { get; set; }
        public bool? IsActive { get; set; }

        public List<CommodityPropertyValueModel> PropertyValues { get; set; }
        //public List<BomValueHeaderModel> BomValueHeaders { get; set; }
        //public List<BomValueModel> BomValues { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.Commodity, CommodityModel>()
                .ForMember(x => x.CommodityCategoryTitle, opt => opt.MapFrom(x => x.CommodityCategory.Title))
                .ForMember(x => x.PropertyValues, opt => opt.MapFrom(x => x.CommodityPropertyValues));

        }
    }


}
