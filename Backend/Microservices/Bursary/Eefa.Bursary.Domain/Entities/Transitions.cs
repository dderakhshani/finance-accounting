using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Transitions : BaseEntity
    {
        public Transitions()
        {
            TransitionShapes = new HashSet<TransitionShapes>();
        }

         
        public int ProcessId { get; set; } = default!;
        public Guid Guid { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Condition { get; set; }
        public string? Description { get; set; }
        public Guid? From { get; set; }
        public Guid? To { get; set; }
        public int? BusinessRuleId { get; set; }

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
         

        public virtual BusinessRules BusinessRule { get; set; } = default!;
        public virtual Processes Process { get; set; } = default!;
        public virtual ICollection<TransitionShapes> TransitionShapes { get; set; } = default!;
    }
}
