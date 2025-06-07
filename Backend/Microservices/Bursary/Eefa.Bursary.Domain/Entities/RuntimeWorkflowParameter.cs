using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class RuntimeWorkflowParameter : BaseEntity
    {
         
        public int RuntimeWorkflowId { get; set; } = default!;
        public string? ParameterName { get; set; }
        public string? ParameterValue { get; set; }
        public string? ParameterDataType { get; set; }

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
         

        public virtual RuntimeWorkflow RuntimeWorkflow { get; set; } = default!;
    }
}
