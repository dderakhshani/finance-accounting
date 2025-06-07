using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class RuntimeWorkflow : BaseEntity
    {
        public RuntimeWorkflow()
        {
            RuntimeProcesses = new HashSet<RuntimeProcess>();
            RuntimeWorkflowParameters = new HashSet<RuntimeWorkflowParameter>();
            Tasks = new HashSet<Tasks>();
        }

         
        public int? CreatorId { get; set; }
        public int WorkflowId { get; set; } = default!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

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
         

        public virtual Workflows Workflow { get; set; } = default!;
        public virtual ICollection<RuntimeProcess> RuntimeProcesses { get; set; } = default!;
        public virtual ICollection<RuntimeWorkflowParameter> RuntimeWorkflowParameters { get; set; } = default!;
        public virtual ICollection<Tasks> Tasks { get; set; } = default!;
    }
}
