using System;
using Library.Common;

namespace Eefa.Identity.Data.Databases.Entities
{

    public partial class Employee : BaseEntity
    {

        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// کد موقعیت واحد
        /// </summary>
        public int UnitPositionId { get; set; } = default!;

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public string EmployeeCode { get; set; } = default!;

        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        public DateTime EmploymentDate { get; set; } = default!;

        /// <summary>
        /// شناور
        /// </summary>
        public bool Floating { get; set; } = default!;

        /// <summary>
        /// تاریخ ترک کار
        /// </summary>
        public DateTime? LeaveDate { get; set; }

        /// <summary>
        /// نقش صاحب سند
        /// </summary>
         

        /// <summary>
        /// ایجاد کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
        /// اصلاح کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
        /// آیا حذف شده است؟
        /// </summary>
        

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
        public virtual UnitPosition UnitPosition { get; set; } = default!;
    }
}
