using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1608;&#1575;&#1581;&#1583; &#1607;&#1575;&#1740; &#1575;&#1606;&#1583;&#1575;&#1586;&#1607; &#1711;&#1740;&#1585;&#1740;
    /// </summary>
    public partial class MeasureUnits : BaseEntity
    {
        public MeasureUnits()
        {
            Commodities = new HashSet<Commodities>();
            CommodityCategories = new HashSet<CommodityCategories>();
            CommodityCategoryMeasures = new HashSet<CommodityCategoryMeasures>();
            CommodityCategoryProperties = new HashSet<CommodityCategoryProperties>();
            CommodityMeasures = new HashSet<CommodityMeasures>();
            DocumentItems = new HashSet<DocumentItems>();
            DocumentItemsBoms = new HashSet<DocumentItemsBom>();
            InverseParent = new HashSet<MeasureUnits>();
            MeasureUnitConversionsDestinationMeasureUnits = new HashSet<MeasureUnitConversions>();
            MeasureUnitConversionsSourceMeasureUnits = new HashSet<MeasureUnitConversions>();
        }


        /// <summary>
//شناسه
        /// </summary>
         
        public int? ParentId { get; set; }

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//نام اختصاصی
        /// </summary>
        public string? UniqueName { get; set; }

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
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual MeasureUnits Parent { get; set; } = default!;
        public virtual ICollection<Commodities> Commodities { get; set; } = default!;
        public virtual ICollection<CommodityCategories> CommodityCategories { get; set; } = default!;
        public virtual ICollection<CommodityCategoryMeasures> CommodityCategoryMeasures { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperties> CommodityCategoryProperties { get; set; } = default!;
        public virtual ICollection<CommodityMeasures> CommodityMeasures { get; set; } = default!;
        public virtual ICollection<DocumentItems> DocumentItems { get; set; } = default!;
        public virtual ICollection<DocumentItemsBom> DocumentItemsBoms { get; set; } = default!;
        public virtual ICollection<MeasureUnits> InverseParent { get; set; } = default!;
        public virtual ICollection<MeasureUnitConversions> MeasureUnitConversionsDestinationMeasureUnits { get; set; } = default!;
        public virtual ICollection<MeasureUnitConversions> MeasureUnitConversionsSourceMeasureUnits { get; set; } = default!;
    }
}
