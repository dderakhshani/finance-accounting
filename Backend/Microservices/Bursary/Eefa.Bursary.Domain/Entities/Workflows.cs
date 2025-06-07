using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Workflows : BaseEntity
    {
        public Workflows()
        {
            FinancialRequests = new HashSet<FinancialRequest>();
            Pools = new HashSet<Pools>();
            Processes = new HashSet<Processes>();
            RuntimeWorkflows = new HashSet<RuntimeWorkflow>();
        }

         
        public Guid Guid { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int? CreatedBy { get; set; }
        public int? DocumentId { get; set; }
        public DateTime CreateTime { get; set; } = default!;
        public string? Description { get; set; }
        public string? UniqueName { get; set; }
        public string? WorkflowName { get; set; }
        public bool IsActive { get; set; } = default!;
        public int Version { get; set; } = default!;

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
         

        public virtual ICollection<FinancialRequest> FinancialRequests { get; set; } = default!;
        public virtual ICollection<Pools> Pools { get; set; } = default!;
        public virtual ICollection<Processes> Processes { get; set; } = default!;
        public virtual ICollection<RuntimeWorkflow> RuntimeWorkflows { get; set; } = default!;
    }
}
