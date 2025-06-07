using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class PerformerCondition : BaseEntity
    {
         
        public int? ParentId { get; set; }
        public int PerformerId { get; set; } = default!;
        public string? StaticValue { get; set; }

        /// <summary>
//1.Condition 2.And 3.Or 4....
        /// </summary>
        public int NodeType { get; set; } = default!;
        public int PerformerType { get; set; } = default!;

        /// <summary>
//"Current" value for to be equal to user correspond attribute or any static primary key value of correspond table
        /// </summary>
        public int ValueSourceType { get; set; } = default!;
        public bool? IsEqual { get; set; }

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
         

        public virtual Performers Performer { get; set; } = default!;
    }
}
