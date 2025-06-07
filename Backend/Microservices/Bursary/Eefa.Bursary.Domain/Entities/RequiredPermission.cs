using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1583;&#1587;&#1578;&#1585;&#1587;&#1740; &#1607;&#1575;&#1740; &#1605;&#1608;&#1585;&#1583; &#1606;&#1740;&#1575;&#1586;
    /// </summary>
    public partial class RequiredPermission : BaseEntity
    {

        /// <summary>
//کد
        /// </summary>
         

        /// <summary>
//کد والد
        /// </summary>
        public int ParentPermissionId { get; set; } = default!;

        /// <summary>
//کد دسترسی
        /// </summary>
        public int PermissionId { get; set; } = default!;

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
        public virtual Permissions ParentPermission { get; set; } = default!;
        public virtual Permissions Permission { get; set; } = default!;
    }
}
