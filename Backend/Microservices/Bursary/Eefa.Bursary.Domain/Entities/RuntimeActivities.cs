using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class RuntimeActivities : BaseEntity
    {
        public RuntimeActivities()
        {
            Tasks = new HashSet<Tasks>();
        }

         
        public int RuntimeProcessId { get; set; } = default!;
        public int? CreatorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ActivityId { get; set; } = default!;
        public bool? IsFinished { get; set; }
        public short? ProgressStatus { get; set; }
        public short? Status { get; set; }

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
         

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        public virtual Activities Activity { get; set; } = default!;
        public virtual RuntimeProcess RuntimeProcess { get; set; } = default!;
        public virtual ICollection<Tasks> Tasks { get; set; } = default!;
    }
}
