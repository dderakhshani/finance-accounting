using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Tasks : BaseEntity
    {
        public Tasks()
        {
            TaskComments = new HashSet<TaskComments>();
            TaskWorkHistories = new HashSet<TaskWorkHistory>();
        }

         
        public string? Title { get; set; }
        public int? RelatedTaskId { get; set; }
        public int RuntimeWorkflowId { get; set; } = default!;
        public int RuntimeActivityId { get; set; } = default!;
        public int UserId { get; set; } = default!;
        public int CreatorUserId { get; set; } = default!;
        public DateTime CreateTime { get; set; } = default!;
        public DateTime? ReadTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        /// <summary>
//mintues
        /// </summary>
        public int? SpentTime { get; set; }
        public DateTime? DateDue { get; set; }
        public string? Description { get; set; }
        public short? TimeoutDays { get; set; }

        /// <summary>
//1=Low 2=Normal 3=Major 4=Critical
        /// </summary>
        public int Priority { get; set; } = default!;
        public short ProgressStatus { get; set; } = default!;

        /// <summary>
//1=Open 2=Read 3=Inprogress 4=Hold/Pause 5=Finished
        /// </summary>
        public short Status { get; set; } = default!;
        public int? RelationType { get; set; }
        public string? TempDesc { get; set; }

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
         

        public virtual RuntimeActivities RuntimeActivity { get; set; } = default!;
        public virtual RuntimeWorkflow RuntimeWorkflow { get; set; } = default!;
        public virtual ICollection<TaskComments> TaskComments { get; set; } = default!;
        public virtual ICollection<TaskWorkHistory> TaskWorkHistories { get; set; } = default!;
    }
}
