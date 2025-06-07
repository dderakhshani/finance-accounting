using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1588;&#1605;&#1575;&#1585;&#1607; &#1711;&#1584;&#1575;&#1585;&#1740; &#1575;&#1587;&#1606;&#1575;&#1583;
    /// </summary>
    public partial class DocumentNumberingFormats : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد نوع سند
        /// </summary>
        public int DocumentTypeBaseId { get; set; } = default!;

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//فرمت کد دهی
        /// </summary>
        public string CodingTemplate { get; set; } = default!;

        /// <summary>
//فعال
        /// </summary>
        public bool IsActive { get; set; } = default!;

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
        public virtual BaseValues DocumentTypeBase { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
