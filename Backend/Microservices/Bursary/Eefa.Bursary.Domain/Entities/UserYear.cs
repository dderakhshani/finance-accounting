using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1587;&#1575;&#1604; &#1607;&#1575;&#1740;&#1740; &#1705;&#1607; &#1705;&#1575;&#1585;&#1576;&#1585; &#1576;&#1607; &#1570;&#1606;&#1607;&#1575; &#1583;&#1587;&#1578;&#1585;&#1587;&#1740; &#1583;&#1575;&#1585;&#1583;
    /// </summary>
    public partial class UserYear : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد کاربر
        /// </summary>
        public int UserId { get; set; } = default!;

        /// <summary>
//کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

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
         

        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual Years Year { get; set; } = default!;
    }
}
