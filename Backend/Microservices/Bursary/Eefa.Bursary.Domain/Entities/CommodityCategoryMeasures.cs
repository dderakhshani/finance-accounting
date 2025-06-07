using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1608;&#1740;&#1688;&#1711;&#1740; &#1607;&#1575;&#1740; &#1711;&#1585;&#1608;&#1607; &#1705;&#1575;&#1604;&#1575;
    /// </summary>
    public partial class CommodityCategoryMeasures : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد گروه
        /// </summary>
        public int CategoryId { get; set; } = default!;

        /// <summary>
//کد واحد اندازه گیری
        /// </summary>
        public int MeasureId { get; set; } = default!;

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
    }
}
