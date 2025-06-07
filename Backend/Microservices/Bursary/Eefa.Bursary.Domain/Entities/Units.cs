using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1608;&#1575;&#1581;&#1583; &#1607;&#1575; 
    /// </summary>
    public partial class Units : BaseEntity
    {
        public Units()
        {
            InverseParent = new HashSet<Units>();
            UnitPositions = new HashSet<UnitPositions>();
        }


        /// <summary>
//کد
        /// </summary>
         

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//کدهای سیستمهای خارجی 
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//کد شعبه
        /// </summary>
        public int BranchId { get; set; } = default!;

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
         

        public virtual Branches Branch { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual Units Parent { get; set; } = default!;
        public virtual ICollection<Units> InverseParent { get; set; } = default!;
        public virtual ICollection<UnitPositions> UnitPositions { get; set; } = default!;
    }
}
