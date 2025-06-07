using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1588;&#1593;&#1576;&#1607; &#1607;&#1575; 
    /// </summary>
    public partial class Branches : BaseEntity
    {
        public Branches()
        {
            InverseParent = new HashSet<Branches>();
            Units = new HashSet<Units>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }

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
        public virtual Branches Parent { get; set; } = default!;
        public virtual ICollection<Branches> InverseParent { get; set; } = default!;
        public virtual ICollection<Units> Units { get; set; } = default!;
    }
}
