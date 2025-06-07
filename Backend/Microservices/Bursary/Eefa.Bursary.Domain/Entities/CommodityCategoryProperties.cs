using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1608;&#1740;&#1688;&#1711;&#1740; &#1607;&#1575;&#1740; &#1711;&#1585;&#1608;&#1607; &#1705;&#1575;&#1604;&#1575;
    /// </summary>
    public partial class CommodityCategoryProperties : BaseEntity
    {
        public CommodityCategoryProperties()
        {
            CommodityCategoryPropertyItems = new HashSet<CommodityCategoryPropertyItems>();
            CommodityPropertyValues = new HashSet<CommodityPropertyValues>();
            InverseParent = new HashSet<CommodityCategoryProperties>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//کد گروه
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
//نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//کد واحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; }

        /// <summary>
//قوانین حاکم بر مولفه
        /// </summary>
        public int? PropertyTypeBaseId { get; set; }

        /// <summary>
//ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

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
         

        public virtual CommodityCategories Category { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual MeasureUnits Measure { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual CommodityCategoryProperties Parent { get; set; } = default!;
        public virtual BaseValues PropertyTypeBase { get; set; } = default!;
        public virtual ICollection<CommodityCategoryPropertyItems> CommodityCategoryPropertyItems { get; set; } = default!;
        public virtual ICollection<CommodityPropertyValues> CommodityPropertyValues { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperties> InverseParent { get; set; } = default!;
    }
}
