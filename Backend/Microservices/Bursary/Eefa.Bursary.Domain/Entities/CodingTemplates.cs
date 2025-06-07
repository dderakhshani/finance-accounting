using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class CodingTemplates : BaseEntity
    {
         
        public string Title { get; set; } = default!;
        public int CategoryId { get; set; } = default!;

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
         

        public virtual CodingTemplateProperties CodingTemplateProperties { get; set; } = default!;
    }
}
