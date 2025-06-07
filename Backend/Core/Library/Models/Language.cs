using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;

namespace Library.Models
{
    [Table("Languages",Schema = "admin")]
    public partial class LanguageRemove : BaseEntity
    {
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

        public virtual ICollection<ValidationMessage> ValidationMessages { get; set; } = default!;
    }
}
