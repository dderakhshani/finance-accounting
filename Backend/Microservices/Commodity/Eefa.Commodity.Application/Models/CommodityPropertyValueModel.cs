using AutoMapper;
using Eefa.Common;

namespace Eefa.Commodity.Application.Queries.Commodity
{

    public record CommodityPropertyValueModel : IMapFrom<Eefa.Commodity.Data.Entities.CommodityPropertyValue>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد کالا
        /// </summary>
        public int CommodityId { get; set; } = default!;
        public string CommodityTitle { get; set; }
        /// <summary>
        /// کد ویژگی گروه
        /// </summary>
        public int CategoryPropertyId { get; set; } = default!;
        public string CategoryPropertyTitle { get; set; }
        /// <summary>
        /// کد آیتم ویژگی مقدار 
        /// </summary>
        public int? ValuePropertyItemId { get; set; }
        public string? ValuePropertyItemTitle { get; set; }
        /// <summary>
        /// مقدار
        /// </summary>
        public string? Value { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Eefa.Commodity.Data.Entities.CommodityPropertyValue, CommodityPropertyValueModel>();
        }

    }


}
