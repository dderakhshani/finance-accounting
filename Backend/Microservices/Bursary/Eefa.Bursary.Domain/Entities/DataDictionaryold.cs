using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class DataDictionaryold : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//فهرست جدول
        /// </summary>
        public string? TABLE_CATALOG { get; set; }

        /// <summary>
//شکل جدول
        /// </summary>
        public string? TABLE_SCHEMA { get; set; }

        /// <summary>
//نام جدول
        /// </summary>
        public string TABLE_NAME { get; set; } = default!;

        /// <summary>
//نام ستون
        /// </summary>
        public string? COLUMN_NAME { get; set; }

        /// <summary>
//توضیحات
        /// </summary>
        public string? DESCRIPTION { get; set; }

        /// <summary>
//موقعیت ترتیبی
        /// </summary>
        public int? ORDINAL_POSITION { get; set; }

        /// <summary>
//ستون پیش فرض
        /// </summary>
        public string? COLUMN_DEFAULT { get; set; }

        /// <summary>
//اجباری نیست
        /// </summary>
        public string? IS_NULLABLE { get; set; }

        /// <summary>
//نوع داده
        /// </summary>
        public string? DATA_TYPE { get; set; }

        /// <summary>
//حداکثر تعداد کاراکترها
        /// </summary>
        public int? CHARACTER_MAXIMUM_LENGTH { get; set; }
        public int? CHARACTER_OCTET_LENGTH { get; set; }
    }
}
