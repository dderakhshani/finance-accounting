using Eefa.Common;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Commodity.Data.Entities
{
    public partial class Commodity : BaseEntity, IHierarchical
    {
        public string CommodityNationalId { get; set; }
        public string CommodityNationalTitle { get; set; }

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
        /// <summary>
        ///کالا مصرفی
        /// </summary>
        public bool? IsConsumable { get; set; }
        /// <summary>
        ///داغی دارد
        /// </summary>
        public bool? IsHaveWast { get; set; }
        /// <summary>
        ///داغی اجباری
        /// </summary>
        /// 

        public bool? IsHaveForceWast { get; set; }


        /// <summary>
        ///اموال
        /// </summary>
        /// 

        public bool? IsAsset { get; set; }

        
        public bool? IsActive { get; set; }
        

        public int? SinaId { get; set; } = default!;

        public virtual CommodityCategory? CommodityCategory { get; set; } = default!;
        public virtual MeasureUnit? MeasureUnit { get; set; } = default!;
        public virtual Commodity? Parent { get; set; } = default!;
        public virtual BaseValue? PricingTypeBase { get; set; } = default!;
        public virtual ICollection<BomValueHeader> BomValueHeaders { get; set; } = default!;
        public virtual ICollection<BomValue> BomValues { get; set; } = default!;
        public virtual ICollection<CommodityPropertyValue> CommodityPropertyValues { get; set; } = default!;
        public virtual ICollection<Commodity> InverseParent { get; set; } = default!;
        public virtual ICollection<BomItem> BomItems { get; set; } = default!;

    }
}
