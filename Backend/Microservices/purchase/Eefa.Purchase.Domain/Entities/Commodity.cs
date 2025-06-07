using System.Collections.Generic;
using Eefa.Common.Data;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// کالا ها
    /// </summary>
    public partial class Commodity : BaseEntity
    {
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

        public string? Code { get; set; }
        public string? TadbirCode { get; set; }
        public string? CompactCode { get; set; }
        /// <description>
        /// عنوان
        ///</description>

        public string? Title { get; set; }
        /// <description>
        /// توضیحات
        ///</description>

        public string? Descriptions { get; set; }
        /// <description>
        /// کد واحد اندازه گیری
        ///</description>

        public int? MeasureId { get; set; }
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
        /// <description>
        /// نوع محاسبه قیمت
        ///</description>

        public int? PricingTypeBaseId { get; set; }
        /// <description>
        /// نقش صاحب سند
        ///</description>

        public virtual CommodityCategory? CommodityCategory { get; set; }
        public virtual MeasureUnit? Measure { get; set; }
        public virtual Commodity? Parent { get; set; }
        public virtual BaseValue? PricingTypeBase { get; set; }
        public virtual ICollection<BomValueHeader>
BomValueHeaders
        { get; set; } = default!;
        public virtual ICollection<BomValue>
BomValues
        { get; set; } = default!;
        public virtual ICollection<Commodity>
InverseParent
        { get; set; } = default!;
        public virtual ICollection<Stock>
Stocks
        { get; set; } = default!;
        public virtual ICollection<WarehouseHistory>
WarehouseHistories
        { get; set; } = default!;
        public virtual ICollection<WarehouseLayoutQuantity>
WarehouseLayoutQuantities
        { get; set; } = default!;
    }
}
