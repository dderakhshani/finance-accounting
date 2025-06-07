using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1606;&#1602;&#1588; &#1607;&#1575;&#1740; &#1705;&#1575;&#1585;&#1576;&#1585;&#1575;&#1606; 
    /// </summary>
    public partial class UserRoles : BaseEntity
    {

        /// <summary>
//کد
        /// </summary>
         

        /// <summary>
//کد نقش
        /// </summary>
        public int RoleId { get; set; } = default!;

        /// <summary>
//کد کاربر
        /// </summary>
        public int UserId { get; set; } = default!;

        /// <summary>
//وضعیت دسترسی
        /// </summary>
        public bool AllowedStatus { get; set; } = default!;

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
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual Roles Role { get; set; } = default!;
        public virtual Users User { get; set; } = default!;
    }
}
