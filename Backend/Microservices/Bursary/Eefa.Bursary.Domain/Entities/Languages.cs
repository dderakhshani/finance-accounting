using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1586;&#1576;&#1575;&#1606;&#1607;&#1575;
    /// </summary>
    public partial class Languages : BaseEntity
    {
        public Languages()
        {
            Attachments = new HashSet<Attachment>();
            HelpDatas = new HashSet<HelpData>();
            ValidationMessages = new HashSet<ValidationMessages>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//نماد
        /// </summary>
        public string Culture { get; set; } = default!;

        /// <summary>
//کد سئو
        /// </summary>
        public string? SeoCode { get; set; }

        /// <summary>
//نماد پرچم کشور
        /// </summary>
        public string? FlagImageUrl { get; set; }

        /// <summary>
//راست چین
        /// </summary>
        public int DirectionBaseId { get; set; } = default!;

        /// <summary>
//واحد پول پیش فرض
        /// </summary>
        public int DefaultCurrencyBaseId { get; set; } = default!;

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
         

        public virtual BaseValues DefaultCurrencyBase { get; set; } = default!;
        public virtual BaseValues DirectionBase { get; set; } = default!;
        public virtual ICollection<Attachment> Attachments { get; set; } = default!;
        public virtual ICollection<HelpData> HelpDatas { get; set; } = default!;
        public virtual ICollection<ValidationMessages> ValidationMessages { get; set; } = default!;
    }
}
