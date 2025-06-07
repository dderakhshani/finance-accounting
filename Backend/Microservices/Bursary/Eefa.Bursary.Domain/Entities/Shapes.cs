using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Shapes : BaseEntity
    {
        public Shapes()
        {
            TransitionShapes = new HashSet<TransitionShapes>();
        }

         
        public int? Height { get; set; }
        public int? Width { get; set; }
        public int? BorderColor { get; set; }
        public int? FillColor { get; set; }
        public bool? Expanded { get; set; }
        public int? ExpandedWidth { get; set; }
        public int? ExpandedHeight { get; set; }
        public int? XCoordinate { get; set; }
        public int? YCoordinate { get; set; }

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
         

        public virtual Activities Activities { get; set; } = default!;
        public virtual Lanes Lanes { get; set; } = default!;
        public virtual Pools Pools { get; set; } = default!;
        public virtual Processes Processes { get; set; } = default!;
        public virtual ICollection<TransitionShapes> TransitionShapes { get; set; } = default!;
    }
}
