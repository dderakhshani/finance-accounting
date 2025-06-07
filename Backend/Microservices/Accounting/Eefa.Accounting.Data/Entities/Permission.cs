using System.Collections.Generic;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{

    public partial class Permission : BaseEntity
    {
     

        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام یکتا
        /// </summary>
        public string UniqueName { get; set; } = default!;
        public bool IsDataRowLimiter { get; set; } = default!;

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
        public virtual Permission? Parent { get; set; } = default!;
        public virtual ICollection<Permission> InverseParent { get; set; } = default!;
        public virtual ICollection<MenuItem> MenuItems { get; set; } = default!;
        public virtual ICollection<RequiredPermission> RequiredPermissionParentPermissions { get; set; } = default!;
        public virtual ICollection<RequiredPermission> RequiredPermissionPermissions { get; set; } = default!;
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = default!;
    }
}
