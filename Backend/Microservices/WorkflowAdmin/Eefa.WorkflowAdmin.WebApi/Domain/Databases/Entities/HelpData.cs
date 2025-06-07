using System.Collections.Generic;
using Library.Attributes;
using Library.Interfaces;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    public partial class HelpData : HierarchicalBaseEntity
    {

        /// <summary>
        /// کد زبان
        /// </summary>
        public int? LanguageId { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        [UniqueIndex]
        public string Title { get; set; } = default!;

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// کلمه کلیدی
        /// </summary>
        public string? KeyWords { get; set; }

        /// <summary>
        /// لینک
        /// </summary>
        public string? Url { get; set; }

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
        public virtual Language? Language { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<HelpAttachment> HelpAttachments { get; set; } = default!;
    }
}
