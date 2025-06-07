using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Activities : BaseEntity
    {
        public Activities()
        {
            Performers = new HashSet<Performers>();
            RuntimeActivities = new HashSet<RuntimeActivities>();
        }

         
        public Guid Guid { get; set; } = default!;
        public int? Type { get; set; }
        public string? Name { get; set; }
        public int? ProcessId { get; set; }
        public string? FormUrl { get; set; }

 
        public string? FinishedRule { get; set; }
        public string? StatusDescription { get; set; }
        public short? DefaultStatus { get; set; }
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
         

        public virtual Processes Process { get; set; } = default!;
        public virtual Shapes Shape { get; set; } = default!;
        public virtual Events Events { get; set; } = default!;
        public virtual ServiceActivities ServiceActivities { get; set; } = default!;
        public virtual ICollection<Performers> Performers { get; set; } = default!;
        public virtual ICollection<RuntimeActivities> RuntimeActivities { get; set; } = default!;
    }
}
