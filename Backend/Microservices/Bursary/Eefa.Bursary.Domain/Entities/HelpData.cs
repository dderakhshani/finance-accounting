using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class HelpData : BaseEntity
    {
        public HelpData()
        {
            HelpAttachments = new HashSet<HelpAttachment>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//کد زبان
        /// </summary>
        public int? LanguageId { get; set; }

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
//کلمه کلیدی
        /// </summary>
        public string? KeyWords { get; set; }

        /// <summary>
//لینک
        /// </summary>
        public string? Url { get; set; }

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
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual ICollection<HelpAttachment> HelpAttachments { get; set; } = default!;
    }
}
