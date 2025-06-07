using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1705;&#1575;&#1604;&#1575; &#1607;&#1575;
    /// </summary>
    public partial class Commodities : BaseEntity
    {
        public Commodities()
        {
            BomItems = new HashSet<BomItems>();
            BomValueHeaders = new HashSet<BomValueHeaders>();
            BomValues = new HashSet<BomValues>();
            CommodityMeasures = new HashSet<CommodityMeasures>();
            CommodityPropertyValues = new HashSet<CommodityPropertyValues>();
            DocumentItemsBomCommodities = new HashSet<DocumentItemsBom>();
            DocumentItemsBomParentCommodities = new HashSet<DocumentItemsBom>();
            InverseParent = new HashSet<Commodities>();
            _CommoditiesCardex = new HashSet<_CommoditiesCardex>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//شناسه ملی کالا 
        /// </summary>
        public string? CommodityNationalId { get; set; }

        /// <summary>
//عنوان شناسه ملی کالا
        /// </summary>
        public string? CommodityNationalTitle { get; set; }

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//کد گروه کالا
        /// </summary>
        public int? CommodityCategoryId { get; set; }

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
//کد محصول
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
//کد محصول
        /// </summary>
        public string? TadbirCode { get; set; }
        public string? CompactCode { get; set; }

        /// <summary>
//عنوان
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
//توضیحات
        /// </summary>
        public string? Descriptions { get; set; }

        /// <summary>
//کد واحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; }

        /// <summary>
//کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

        /// <summary>
//حداقل تعداد
        /// </summary>
        public double? MinimumQuantity { get; set; }

        /// <summary>
//حداکثر تعداد
        /// </summary>
        public double? MaximumQuantity { get; set; }

        /// <summary>
//تعداد سفارش
        /// </summary>
        public double? OrderQuantity { get; set; }

        /// <summary>
//نوع محاسبه قیمت
        /// </summary>
        public int? PricingTypeBaseId { get; set; }
        public int? CommodityTypeBaseId { get; set; }

        /// <summary>
//مصرفی است  
        /// </summary>
        public bool? IsConsumable { get; set; }

        /// <summary>
//داغی دارد 
        /// </summary>
        public bool? IsHaveWast { get; set; }

        /// <summary>
//اموال است
        /// </summary>
        public bool? IsAsset { get; set; }

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
//اصلاح کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
//آیا حذف شده است؟
        /// </summary>
         

        public virtual CommodityCategories CommodityCategory { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual MeasureUnits Measure { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual Commodities Parent { get; set; } = default!;
        public virtual Years Year { get; set; } = default!;
        public virtual ICollection<BomItems> BomItems { get; set; } = default!;
        public virtual ICollection<BomValueHeaders> BomValueHeaders { get; set; } = default!;
        public virtual ICollection<BomValues> BomValues { get; set; } = default!;
        public virtual ICollection<CommodityMeasures> CommodityMeasures { get; set; } = default!;
        public virtual ICollection<CommodityPropertyValues> CommodityPropertyValues { get; set; } = default!;
        public virtual ICollection<DocumentItemsBom> DocumentItemsBomCommodities { get; set; } = default!;
        public virtual ICollection<DocumentItemsBom> DocumentItemsBomParentCommodities { get; set; } = default!;
        public virtual ICollection<Commodities> InverseParent { get; set; } = default!;
        public virtual ICollection<_CommoditiesCardex> _CommoditiesCardex { get; set; } = default!;
    }
}
