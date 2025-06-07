using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1578;&#1606;&#1592;&#1740;&#1605;&#1575;&#1578; &#1705;&#1575;&#1585;&#1576;&#1585;&#1575;&#1606;
    /// </summary>
    public partial class UserSetting : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد کاربر
        /// </summary>
        public int UserId { get; set; } = default!;

        /// <summary>
//کلمه کلیدی
        /// </summary>
        public string? Keyword { get; set; }

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
         

        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual Users User { get; set; } = default!;
    }
}
