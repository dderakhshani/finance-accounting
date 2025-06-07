using Library.Common;

namespace Eefa.Accounting.Data.Entities
{

    public partial class RolePermission : BaseEntity
    {

        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد نقش
        /// </summary>
        public int RoleId { get; set; } = default!;

        /// <summary>
        /// کد دسترسی
        /// </summary>
        public int PermissionId { get; set; } = default!;

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
        public virtual Permission Permission { get; set; } = default!;
        public virtual Role Role { get; set; } = default!;
    }
}
