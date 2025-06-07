using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1662;&#1740;&#1608;&#1587;&#1578; &#1607;&#1575;&#1740; &#1575;&#1588;&#1582;&#1575;&#1589;
    /// </summary>
    public partial class PersonRelAttachments : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//ضمائم
        /// </summary>
        public int AttachmentId { get; set; } = default!;

        /// <summary>
//کد شخص
        /// </summary>
        public int PersonId { get; set; } = default!;

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
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
