using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Events : BaseEntity
    {
         
        public string? Trigger { get; set; }
        public bool? IsAttached { get; set; }
        public Guid? Target { get; set; }

        /// <summary>
//Start = 1, Intermmediate = 2, End = 3
        /// </summary>
        public int Type { get; set; } = default!;

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
         

        public virtual Activities IdNavigation { get; set; } = default!;
    }
}
