using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1605;&#1588;&#1575;&#1594;&#1604; &#1740;&#1705; &#1608;&#1575;&#1581;&#1583; 
    /// </summary>
    public partial class UnitPositions : BaseEntity
    {
        public UnitPositions()
        {
            Employees = new HashSet<Employees>();
        }


        /// <summary>
//کد
        /// </summary>
         

        /// <summary>
//کد موقعیت شغلی
        /// </summary>
        public int PositionId { get; set; } = default!;

        /// <summary>
//کد واحد
        /// </summary>
        public int UnitId { get; set; } = default!;

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
        public virtual Positions Position { get; set; } = default!;
        public virtual Units Unit { get; set; } = default!;
        public virtual ICollection<Employees> Employees { get; set; } = default!;
    }
}
