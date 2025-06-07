using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class FinancialRequestDocuments : BaseEntity
    {
         

        /// <summary>
//شماره سند 
        /// </summary>
        public int DocumentHeadId { get; set; } = default!;

        /// <summary>
//شماره فرم عملیات مالی 
        /// </summary>
        public int FinancialRequestId { get; set; } = default!;

        /// <summary>
//وضعیت تسویه سند 
        /// </summary>
        public int? SettledBaseId { get; set; }

        /// <summary>
//مبلغ تسویه شده از این سند 
        /// </summary>
        public decimal? Amount { get; set; }

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
         

        public virtual Users CreatedBy { get; set; } = default!;
        public virtual DocumentHeads DocumentHead { get; set; } = default!;
        public virtual FinancialRequest FinancialRequest { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual BaseValues SettledBase { get; set; } = default!;
    }
}
