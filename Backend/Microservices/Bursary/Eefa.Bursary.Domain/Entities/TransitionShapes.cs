using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class TransitionShapes : BaseEntity
    {
         
        public int TransitionId { get; set; } = default!;
        public int ShapeId { get; set; } = default!;

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
         

        public virtual Shapes Shape { get; set; } = default!;
        public virtual Transitions Transition { get; set; } = default!;
    }
}
