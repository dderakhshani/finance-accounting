using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1705;&#1575;&#1585;&#1576;&#1585;&#1575;&#1606; &#1583;&#1575;&#1585;&#1575;&#1740; &#1606;&#1602;&#1588; &#1583;&#1587;&#1578;&#1585;&#1587;&#1740; &#1576;&#1607; &#1601;&#1575;&#1740;&#1604;
    /// </summary>
    public partial class _FileAccessRoleUsers : BaseEntity
    {
         
        public int UserId { get; set; } = default!;
        public int FileAccessRoleId { get; set; } = default!;
        public int CreateUserId { get; set; } = default!;
        public DateTime DateTime { get; set; } = default!;
        public string? Description { get; set; }
         

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
         

        public virtual _FileAccessRole FileAccessRole { get; set; } = default!;
        public virtual Users User { get; set; } = default!;
    }
}
