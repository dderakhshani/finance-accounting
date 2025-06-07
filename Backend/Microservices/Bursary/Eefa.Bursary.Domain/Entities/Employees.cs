using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1705;&#1575;&#1585;&#1605;&#1606;&#1583;&#1575;&#1606;
    /// </summary>
    public partial class Employees : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد شخص
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
//کد موقعیت واحد
        /// </summary>
        public int UnitPositionId { get; set; } = default!;

        /// <summary>
//کد پرسنلی
        /// </summary>
        public string EmployeeCode { get; set; } = default!;

        /// <summary>
//تاریخ استخدام
        /// </summary>
        public DateTime EmploymentDate { get; set; } = default!;

        /// <summary>
//درحال جابه جایی 
        /// </summary>
        public bool Floating { get; set; } = default!;

        /// <summary>
//تاریخ ترک کار
        /// </summary>
        public DateTime? LeaveDate { get; set; }

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
         

        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual UnitPositions UnitPosition { get; set; } = default!;
    }
}
