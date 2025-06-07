using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1605;&#1602;&#1575;&#1583;&#1740;&#1585; &#1608;&#1740;&#1688;&#1711;&#1740; &#1607;&#1575;&#1740; &#1705;&#1575;&#1604;&#1575;
    /// </summary>
    public partial class CommodityPropertyValues : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد کالا
        /// </summary>
        public int CommodityId { get; set; } = default!;

        /// <summary>
//کد ویژگی گروه
        /// </summary>
        public int CategoryPropertyId { get; set; } = default!;

        /// <summary>
//کد آیتم ویژگی مقدار 
        /// </summary>
        public int? ValuePropertyItemId { get; set; }

        /// <summary>
//مقدار
        /// </summary>
        public string? Value { get; set; }

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
         

        public virtual CommodityCategoryProperties CategoryProperty { get; set; } = default!;
        public virtual Commodities Commodity { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual CommodityCategoryPropertyItems ValuePropertyItem { get; set; } = default!;
    }
}
