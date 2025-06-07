using AutoMapper;
using Eefa.Common;

namespace Eefa.Commodity.Application.Queries.Bom
{
    public record BomValueModel : IMapFrom<Eefa.Commodity.Data.Entities.BomValue>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد سند فرمول ساخت
        /// </summary>
        public int BomValueHeaderId { get; set; } = default!;
        public int BomWarehouseId { get; set; } = default!;

        /// <summary>
        /// کد کالای مصرفی
        /// </summary>
        public int UsedCommodityId { get; set; } = default!;
        public string CommodityCode { get; set; } = default!;
        public string CommodityTitle { get; set; } = default!;
        public string MainMeasureTitle { get; set; } = default!;

        /// <summary>
        /// مقدار
        /// </summary>
        public double Value { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Eefa.Commodity.Data.Entities.BomValue, BomValueModel>();
        }
    }

}
