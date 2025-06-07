using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1583;&#1587;&#1578;&#1585;&#1587;&#1740; &#1607;&#1575;
    /// </summary>
    public partial class Permissions : BaseEntity
    {
        public Permissions()
        {
            InverseParent = new HashSet<Permissions>();
            MenuItems = new HashSet<MenuItems>();
            RequiredPermissionParentPermissions = new HashSet<RequiredPermission>();
            RequiredPermissionPermissions = new HashSet<RequiredPermission>();
            RolePermissions = new HashSet<RolePermissions>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;
        public bool IsDataRowLimiter { get; set; } = default!;

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;
        public string SubSystem { get; set; } = default!;

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
        public virtual Permissions Parent { get; set; } = default!;
        public virtual ICollection<Permissions> InverseParent { get; set; } = default!;
        public virtual ICollection<MenuItems> MenuItems { get; set; } = default!;
        public virtual ICollection<RequiredPermission> RequiredPermissionParentPermissions { get; set; } = default!;
        public virtual ICollection<RequiredPermission> RequiredPermissionPermissions { get; set; } = default!;
        public virtual ICollection<RolePermissions> RolePermissions { get; set; } = default!;
    }
}
