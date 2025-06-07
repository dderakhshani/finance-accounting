using System.Collections.Generic;
using Library.Attributes;
using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    [HasUniqueIndex]
    public partial class Language : BaseEntity
    {

        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نماد
        /// </summary>
        public string Culture { get; set; } = default!;

        /// <summary>
        /// کد سئو
        /// </summary>
        [UniqueIndex]
        public string? SeoCode { get; set; }

        /// <summary>
        /// نماد پرچم کشور
        /// </summary>
        public string? FlagImageUrl { get; set; }

        /// <summary>
        /// راست چین
        /// </summary>
        public int DirectionBaseId { get; set; } = default!;

        /// <summary>
        /// واحد پول پیش فرض
        /// </summary>
        public int DefaultCurrencyBaseId { get; set; } = default!;

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
        public virtual BaseValue DirectionBase { get; set; } = default!;
        public virtual BaseValue DefaultCurrencyBase { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<Attachment> Attachments { get; set; } = default!;
        public virtual ICollection<HelpData> HelpDatas { get; set; } = default!;
    }
}
