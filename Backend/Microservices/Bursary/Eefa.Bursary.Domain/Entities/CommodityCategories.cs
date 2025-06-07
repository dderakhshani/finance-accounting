using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1711;&#1585;&#1608;&#1607; &#1607;&#1575;&#1740; &#1705;&#1575;&#1604;&#1575;
    /// </summary>
    public partial class CommodityCategories : BaseEntity
    {
        public CommodityCategories()
        {
            Commodities = new HashSet<Commodities>();
            CommodityCategoryMeasures = new HashSet<CommodityCategoryMeasures>();
            CommodityCategoryProperties = new HashSet<CommodityCategoryProperties>();
            InverseParent = new HashSet<CommodityCategories>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;
        public string Code { get; set; } = default!;
 
        public int CodingMode { get; set; } = default!;

        /// <summary>
//عنوان
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
//کدواحد اصلی کالا
        /// </summary>
        public int MeasureId { get; set; } = default!;

        /// <summary>
//ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
//this.Parent().Commodities
        /// </summary>
        public bool? RequireParentProduct { get; set; }

        /// <summary>
//آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;

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
         

        public virtual Users CreatedBy { get; set; } = default!;
        public virtual MeasureUnits Measure { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual CommodityCategories Parent { get; set; } = default!;
        public virtual ICollection<Commodities> Commodities { get; set; } = default!;
        public virtual ICollection<CommodityCategoryMeasures> CommodityCategoryMeasures { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperties> CommodityCategoryProperties { get; set; } = default!;
        public virtual ICollection<CommodityCategories> InverseParent { get; set; } = default!;
    }
}
