using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class DataBaseMetadata : BaseEntity
    {
         
        public string? TableName { get; set; }
        public string? SchemaName { get; set; }
        public string? Command { get; set; }
        public string? CommandProperty { get; set; }
        public string? TableProperty { get; set; }
        public string? Property { get; set; }
        public int? LanguageId { get; set; }
        public string? Translated { get; set; }

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
         
    }
}
