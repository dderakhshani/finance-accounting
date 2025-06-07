using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{

    public partial class AccountReferencesRelReferencesGroup : BaseEntity
    {
        /// <summary>
        /// کد طرف حساب
        /// </summary>
        public int ReferenceId { get; set; } = default!;

        /// <summary>
        /// کد گروه طرف حساب
        /// </summary>
        public int ReferenceGroupId { get; set; } = default!;

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
        public virtual AccountReference Reference { get; set; } = default!;
        public virtual AccountReferencesGroup ReferenceGroup { get; set; } = default!;
    }
}
