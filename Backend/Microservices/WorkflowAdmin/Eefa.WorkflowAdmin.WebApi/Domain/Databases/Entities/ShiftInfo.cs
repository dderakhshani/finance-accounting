using System;
using Library.Attributes;
using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    [HasUniqueIndex]
    public partial class ShiftInfo : BaseEntity
    {
        [UniqueIndex]
        public string Title { get; set; } = default!;

        /// <summary>
        /// شروع شیفت
        /// </summary>
        public DateTime StartTime { get; set; } = default!;

        /// <summary>
        /// پایان شیفت
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// نقش صاحب سند
        /// </summary>
         

        /// <summary>
        /// ایجاد کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
        /// اصلاح کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
        /// آیا حذف شده است؟
        /// </summary>
        

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
