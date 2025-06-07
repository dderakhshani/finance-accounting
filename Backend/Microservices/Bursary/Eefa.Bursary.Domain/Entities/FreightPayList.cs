using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1604;&#1740;&#1587;&#1578; &#1662;&#1585;&#1583;&#1575;&#1582;&#1578; &#1705;&#1585;&#1575;&#1740;&#1607; &#1581;&#1605;&#1604; &#1576;&#1575;&#1585;
    /// </summary>
    public partial class FreightPayList : BaseEntity
    {
        public FreightPayList()
        {
            FreightPays = new HashSet<FreightPays>();
        }

         
        public decimal TotalAmount { get; set; } = default!;
        public string Title { get; set; } = default!;
        public DateTime DateTime { get; set; } = default!;
        public bool IsSend { get; set; } = default!;
        public bool IsPaid { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
        public string? Description { get; set; }

        /// <summary>
//نقش صاحب سند
        /// </summary>
         
        public int CreateUserId { get; set; } = default!;

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
         
         

        public virtual Users CreateUser { get; set; } = default!;
        public virtual ICollection<FreightPays> FreightPays { get; set; } = default!;
    }
}
