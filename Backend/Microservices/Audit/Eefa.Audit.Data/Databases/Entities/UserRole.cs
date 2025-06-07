using Library.Common;

namespace Eefa.Audit.Data.Databases.Entities
{

    public partial class UserRole : BaseEntity
    {

        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد نقش
        /// </summary>
        public int RoleId { get; set; } = default!;

        /// <summary>
        /// کد کاربر
        /// </summary>
        public int UserId { get; set; } = default!;

        /// <summary>
        /// وضعیت دسترسی
        /// </summary>
        public bool AllowedStatus { get; set; } = default!;

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
        public virtual Role Role { get; set; } = default!;
        public virtual User User { get; set; } = default!;
    }
}
