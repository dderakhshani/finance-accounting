using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1601;&#1585;&#1605;&#1608;&#1604; &#1607;&#1575;&#1740; &#1587;&#1575;&#1582;&#1578;
    /// </summary>
    public partial class Boms : BaseEntity
    {
        public Boms()
        {
            BomItems = new HashSet<BomItems>();
            BomValueHeaders = new HashSet<BomValueHeaders>();
        }


        /// <summary>
//شناسه
        /// </summary>
         
        public int? RootId { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; } = default!;

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
//کد گروه کالا
        /// </summary>
        public int CommodityCategoryId { get; set; } = default!;

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
         

        public virtual ICollection<BomItems> BomItems { get; set; } = default!;
        public virtual ICollection<BomValueHeaders> BomValueHeaders { get; set; } = default!;
    }
}
