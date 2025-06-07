using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1662;&#1740;&#1608;&#1587;&#1578; &#1607;&#1575;&#1740; &#1585;&#1575;&#1607;&#1606;&#1605;&#1575;
    /// </summary>
    public partial class HelpAttachment : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد فایل راهنما
        /// </summary>
        public int HelpDataId { get; set; } = default!;

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
        public virtual HelpData HelpData { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
