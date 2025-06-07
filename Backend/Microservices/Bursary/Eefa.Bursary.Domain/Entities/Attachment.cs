using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1662;&#1740;&#1608;&#1587;&#1578; &#1607;&#1575;
    /// </summary>
    public partial class Attachment : BaseEntity
    {
        public Attachment()
        {
            DocumentAttachments = new HashSet<DocumentAttachments>();
            FinancialRequestAttachments = new HashSet<FinancialRequestAttachments>();
            HelpAttachments = new HashSet<HelpAttachment>();
            PersonRelAttachments = new HashSet<PersonRelAttachments>();
            VoucherAttachments = new HashSet<VoucherAttachments>();
            VoucherDetailAttachments = new HashSet<VoucherDetailAttachments>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد زبان
        /// </summary>
        public int LanguageId { get; set; } = default!;

        /// <summary>
//کد نوع
        /// </summary>
        public int TypeBaseId { get; set; } = default!;

        /// <summary>
//نوع پسوند فایل
        /// </summary>
        public string Extention { get; set; } = default!;

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;
        public string FileNumber { get; set; } = default!;

        /// <summary>
//توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
//کلمات کلیدی
        /// </summary>
        public string? KeyWords { get; set; }

        /// <summary>
//لینک
        /// </summary>
        public string Url { get; set; } = default!;

        /// <summary>
//استفاد شده است 
        /// </summary>
        public bool IsUsed { get; set; } = default!;

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
         

        public virtual Languages Language { get; set; } = default!;
        public virtual BaseValues TypeBase { get; set; } = default!;
        public virtual ICollection<DocumentAttachments> DocumentAttachments { get; set; } = default!;
        public virtual ICollection<FinancialRequestAttachments> FinancialRequestAttachments { get; set; } = default!;
        public virtual ICollection<HelpAttachment> HelpAttachments { get; set; } = default!;
        public virtual ICollection<PersonRelAttachments> PersonRelAttachments { get; set; } = default!;
        public virtual ICollection<VoucherAttachments> VoucherAttachments { get; set; } = default!;
        public virtual ICollection<VoucherDetailAttachments> VoucherDetailAttachments { get; set; } = default!;
    }
}
