using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1606;&#1602;&#1588; &#1583;&#1587;&#1578;&#1585;&#1587;&#1740; &#1576;&#1607; &#1601;&#1575;&#1740;&#1604;
    /// </summary>
    public partial class _FileAccessRole : BaseEntity
    {
        public _FileAccessRole()
        {
            _FileAccessRoleUsers = new HashSet<_FileAccessRoleUsers>();
        }

         
        public int RoleAccess { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime? DateTime { get; set; }
        public bool IsActive { get; set; } = default!;
         

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
         

        public virtual ICollection<_FileAccessRoleUsers> _FileAccessRoleUsers { get; set; } = default!;
    }
}
