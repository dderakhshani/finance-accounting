using System.Collections.Generic;
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class Attachment : BaseEntity
    {


        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد زبان
        /// </summary>
        public int LanguageId { get; set; } = default!;
        public int TypeBaseId { get; set; } = default!;
        public string Extention { get; set; } = default!;
        public bool IsUsed { get; set; }


        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// کلمات کلیدی
        /// </summary>
        public string? KeyWords { get; set; }

        /// <summary>
        /// لینک
        /// </summary>
        public string Url { get; set; } = default!;
        public string? FileNumber { get; set; }


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
        public virtual Language Language { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual BaseValue TypeBase { get; set; } = default!;
        public virtual ICollection<HelpAttachment> HelpAttachments { get; set; } = default!;
        public virtual ICollection<VoucherAttachment> VoucherAttachments { get; set; } = default!;
    }
}
