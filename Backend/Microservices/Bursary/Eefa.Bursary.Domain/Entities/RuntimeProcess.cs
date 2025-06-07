using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class RuntimeProcess : BaseEntity
    {
        public RuntimeProcess()
        {
            RuntimeActivities = new HashSet<RuntimeActivities>();
        }

         
        public int RuntimeWorkflowId { get; set; } = default!;
        public int? ParentRuntimeProcessId { get; set; }
        public int? Type { get; set; }
        public int? CreatorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ProcessId { get; set; } = default!;

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
         

        public virtual RuntimeWorkflow RuntimeWorkflow { get; set; } = default!;
        public virtual ICollection<RuntimeActivities> RuntimeActivities { get; set; } = default!;
    }
}
