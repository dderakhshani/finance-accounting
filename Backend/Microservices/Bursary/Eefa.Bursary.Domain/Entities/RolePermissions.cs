using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1581;&#1602;&#1608;&#1602; &#1583;&#1587;&#1578;&#1585;&#1587;&#1740; &#1606;&#1602;&#1588; &#1607;&#1575;
    /// </summary>
    public partial class RolePermissions : BaseEntity
    {

        /// <summary>
//کد
        /// </summary>
         

        /// <summary>
//کد نقش
        /// </summary>
        public int RoleId { get; set; } = default!;

        /// <summary>
//کد دسترسی
        /// </summary>
        public int PermissionId { get; set; } = default!;

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
        public virtual Permissions Permission { get; set; } = default!;
        public virtual Roles Role { get; set; } = default!;
    }
}
