using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class UIGridColumns : BaseEntity
    {
         
        public int GridId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Field { get; set; } = default!;
        public short Width { get; set; } = default!;
        public string Template { get; set; } = default!;
        public bool Searchable { get; set; } = default!;

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
         

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        public virtual UIGrid Grid { get; set; } = default!;
    }
}
