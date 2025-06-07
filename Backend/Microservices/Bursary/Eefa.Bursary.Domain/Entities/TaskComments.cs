using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class TaskComments : BaseEntity
    {
         
        public int TaskId { get; set; } = default!;
        public string? Comment { get; set; }
        public DateTime? CreateDate { get; set; }

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
         

        public virtual Tasks Task { get; set; } = default!;
    }
}
