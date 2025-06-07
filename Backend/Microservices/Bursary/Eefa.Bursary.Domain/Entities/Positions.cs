using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1605;&#1608;&#1602;&#1593;&#1740;&#1578; &#1607;&#1575;&#1740; &#1588;&#1594;&#1604;&#1740;
    /// </summary>
    public partial class Positions : BaseEntity
    {
        public Positions()
        {
            UnitPositions = new HashSet<UnitPositions>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

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
        public virtual ICollection<UnitPositions> UnitPositions { get; set; } = default!;
    }
}
