using System;
using Library.Attributes;
using Library.Common;

namespace Eefa.Audit.Data.Databases.Entities
{
    [HasUniqueIndex]
    public partial class Employee : BaseEntity
    {

        public int PersonId { get; set; } = default!;

        /// <summary>
        /// کد موقعیت واحد
        /// </summary>
        public int UnitPositionId { get; set; } = default!;

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        [UniqueIndex]
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

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
        public virtual UnitPosition UnitPosition { get; set; } = default!;
    }
}
