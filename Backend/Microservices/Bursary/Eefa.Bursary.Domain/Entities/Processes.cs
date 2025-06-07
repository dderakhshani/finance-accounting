using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Processes : BaseEntity
    {
        public Processes()
        {
            Activities = new HashSet<Activities>();
            Transitions = new HashSet<Transitions>();
        }

         
        public Guid Guid { get; set; } = default!;
        public int WorkflowId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int? ParentProcessId { get; set; }
        public bool? IsActivitySet { get; set; }
        public int? ShapeId { get; set; }

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
         

        public virtual Shapes Shape { get; set; } = default!;
        public virtual Workflows Workflow { get; set; } = default!;
        public virtual ICollection<Activities> Activities { get; set; } = default!;
        public virtual ICollection<Transitions> Transitions { get; set; } = default!;
    }
}
