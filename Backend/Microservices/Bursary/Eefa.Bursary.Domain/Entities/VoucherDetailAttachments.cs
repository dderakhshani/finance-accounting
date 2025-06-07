using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class VoucherDetailAttachments : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد آرتیکل سند حسابداری 
        /// </summary>
        public int VoucherDetailId { get; set; } = default!;

        /// <summary>
//کد پیوست
        /// </summary>
        public int AttachmentId { get; set; } = default!;

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
         

        public virtual Attachment Attachment { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual VouchersDetail VoucherDetail { get; set; } = default!;
    }
}
