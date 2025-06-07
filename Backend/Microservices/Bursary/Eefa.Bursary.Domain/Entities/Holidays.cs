using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Holidays : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//تاریخ
        /// </summary>
        public DateTime Date { get; set; } = default!;

        /// <summary>
//توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

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
         

        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
