using Library.Common;

namespace Eefa.Audit.Data.Databases.Entities
{
    public partial class VoucherAttachment : BaseEntity
    {

        public int VoucherHeadId { get; set; } = default!;

        /// <summary>
        /// کد پیوست
        /// </summary>
        public int AttachmentId { get; set; } = default!;

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
        

        public virtual Attachment Attachment { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual VouchersHead VoucherHead { get; set; } = default!;
    }
}
