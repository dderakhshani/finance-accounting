using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Lanes : BaseEntity
    {
         
        public int PoolId { get; set; } = default!;
        public Guid Guid { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int? PerentId { get; set; }
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
         

        public virtual Pools Pool { get; set; } = default!;
        public virtual Shapes Shape { get; set; } = default!;
    }
}
