using AutoMapper;
using Eefa.Common;
using Eefa.Purchase.Domain.Entities;

namespace Eefa.Purchase.Application.Models.Receipt
{
    public class InvoiceCommodityModel: IMapFrom<Commodity>
    {

        public int? Id { get; set; }
        public int? ParentId { get; set; }
        /// <description>
        /// کد گروه کالا
        ///</description>

        public int? CommodityCategoryId { get; set; }
        /// <description>
        /// کد سطح
        ///</description>

        public string LevelCode { get; set; } = default!;
        /// <description>
        /// کد محصول
        ///</description>

        public string Code { get; set; }
        public string TadbirCode { get; set; }
        public string CompactCode { get; set; }
        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; }
        /// <description>
        /// توضیحات
        ///</description>

        public string Descriptions { get; set; }
        /// <description>
        /// کد واحد اندازه گیری
        ///</description>

        public int? MeasureId { get; set; }
        public string MeasureTitle { get; set; }
        /// <description>
        /// کد سال
        ///</description>

        public int YearId { get; set; } = default!;
        /// <description>
        /// حداقل تعداد
        ///</description>

        public double MinimumQuantity { get; set; } = default!;
        /// <description>
        /// حداکثر تعداد
        ///</description>

        public double? MaximumQuantity { get; set; }
        /// <description>
        /// تعداد سفارش
        ///</description>

        public double? OrderQuantity { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Commodity, InvoiceCommodityModel>();
        }

    }
}
