using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;

namespace Library.Models
{
    [Table("ValidationMessages",Schema = "admin")]
    public partial class ValidationMessage : BaseEntity
    {
        /// <summary>
        /// کلمه کلیدی
        /// </summary>
        public string Keyword { get; set; } = default!;

        /// <summary>
        /// پیام
        /// </summary>
        public string Message { get; set; } = default!;

        /// <summary>
        /// کد زبان
        /// </summary>
        public int LanguageId { get; set; } = default!;


        //public virtual Language Language { get; set; } = default!;
    }
}
