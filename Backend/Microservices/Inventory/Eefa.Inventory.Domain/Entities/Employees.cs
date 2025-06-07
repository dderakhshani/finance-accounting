using System;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{

    public partial class Employees : DomainBaseEntity
    {
        /// <summary>
        /// کد شخص
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
        /// درحال جابه جایی
        /// </summary>
        public bool Floating { get; set; }

        /// <summary>
        /// تاریخ ترک کار
        /// </summary>
        public DateTime? LeaveDate { get; set; }
        
        
    }
}
