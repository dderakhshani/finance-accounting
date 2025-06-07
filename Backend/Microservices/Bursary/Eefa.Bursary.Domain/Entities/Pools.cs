using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Pools : BaseEntity
    {
        public Pools()
        {
            Lanes = new HashSet<Lanes>();
        }

         
        public int WorkflowId { get; set; } = default!;
        public Guid Guid { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Type { get; set; }
        public bool? BoundaryVisible { get; set; }
        public int ShapeId { get; set; } = default!;

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
        public virtual ICollection<Lanes> Lanes { get; set; } = default!;
    }
}
