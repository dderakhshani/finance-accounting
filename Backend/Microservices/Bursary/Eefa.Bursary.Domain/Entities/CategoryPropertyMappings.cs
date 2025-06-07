using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1606;&#1711;&#1575;&#1588;&#1578; &#1576;&#1740;&#1606; &#1711;&#1585;&#1608;&#1607; &#1607;&#1575; &#1608; &#1605;&#1588;&#1582;&#1589;&#1575;&#1578; &#1705;&#1575;&#1604;&#1575;
    /// </summary>
    public partial class CategoryPropertyMappings : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//آیتم ویژگی گروه محصول1
        /// </summary>
        public int CommodityCategoryPropertyItems1 { get; set; } = default!;

        /// <summary>
//آیتم ویژگی گروه محصول2
        /// </summary>
        public int CommodityCategoryPropertyItems2 { get; set; } = default!;

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
         

        public virtual CommodityCategoryPropertyItems CommodityCategoryPropertyItems1Navigation { get; set; } = default!;
        public virtual CommodityCategoryPropertyItems CommodityCategoryPropertyItems2Navigation { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
