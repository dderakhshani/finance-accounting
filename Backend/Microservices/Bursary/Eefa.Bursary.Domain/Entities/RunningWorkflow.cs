using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1580;&#1585;&#1740;&#1575;&#1606; &#1607;&#1575;&#1740; &#1705;&#1575;&#1585;&#1740; &#1601;&#1593;&#1575;&#1604;
    /// </summary>
    public partial class RunningWorkflow : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//شناسه ی شخص یا نقش ایجاد کننده ی فعالیت بعدی
        /// </summary>
        public int? CreatorId { get; set; }

        /// <summary>
//کد جریان کاری
        /// </summary>
        public int WorkflowId { get; set; } = default!;

        /// <summary>
//تاریخ و زمان شروع
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
//تاریخ و زمان پایان
        /// </summary>
        public DateTime? EndDate { get; set; }

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
         
    }
}
