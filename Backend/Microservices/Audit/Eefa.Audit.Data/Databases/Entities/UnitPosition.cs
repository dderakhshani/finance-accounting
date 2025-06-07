using System.Collections.Generic;
using Library.Common;

namespace Eefa.Audit.Data.Databases.Entities
{

    public partial class UnitPosition : BaseEntity
    {


        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد موقعیت شغلی
        /// </summary>
        public int PositionId { get; set; } = default!;

        /// <summary>
        /// کد واحد
        /// </summary>
        public int UnitId { get; set; } = default!;

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
        public virtual Position Position { get; set; } = default!;
        public virtual Unit Unit { get; set; } = default!;
        public virtual ICollection<Employee> Employees { get; set; } = default!;
    }
}
