using System.Collections.Generic;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    public partial class Commodity : BaseEntity
    {
        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد گروه کالا
        /// </summary>
        public int? CommodityCategoryId { get; set; }

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


        public virtual CommodityCategory? CommodityCategory { get; set; } = default!;
        public virtual Commodity? Parent { get; set; } = default!;
        public virtual BaseValue? PricingTypeBase { get; set; } = default!;
        public virtual ICollection<Commodity> InverseParent { get; set; } = default!;

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
