using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1575;&#1591;&#1604;&#1575;&#1593;&#1575;&#1578; &#1588;&#1740;&#1601;&#1578;
    /// </summary>
    public partial class ShiftInfo : BaseEntity
    {

        /// <summary>
//کد
        /// </summary>
         

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//شروع شیفت
        /// </summary>
        public DateTime StartTime { get; set; } = default!;

        /// <summary>
//پایان شیفت
        /// </summary>
        public DateTime? EndTime { get; set; }

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
    }
}
