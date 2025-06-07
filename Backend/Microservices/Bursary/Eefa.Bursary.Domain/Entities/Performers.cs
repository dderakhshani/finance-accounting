using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Performers : BaseEntity
    {
        public Performers()
        {
            PerformerConditions = new HashSet<PerformerCondition>();
        }

         
        public int ActivityId { get; set; } = default!;
        public string? Title { get; set; }

 
        public int AssignationMethod { get; set; } = default!;

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
        public virtual ICollection<PerformerCondition> PerformerConditions { get; set; } = default!;
    }
}
