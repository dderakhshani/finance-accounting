using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1580;&#1586;&#1574;&#1740;&#1575;&#1578; &#1583;&#1585;&#1582;&#1608;&#1575;&#1587;&#1578; &#1605;&#1575;&#1604;&#1740;
    /// </summary>
    public partial class FinancialRequestPartial : BaseEntity
    {
         

        /// <summary>
//شماره فرم عملیات مالی
        /// </summary>
        public int FinancialRequestId { get; set; } = default!;
        public int CreditAccountHeadId { get; set; } = default!;

        /// <summary>
//گروه بستانکار
        /// </summary>
        public int? CreditAccountReferenceGroupId { get; set; }

        /// <summary>
//کد بستانکار
        /// </summary>
        public int? CreditAccountReferenceId { get; set; }
        public int DebitAccountHeadId { get; set; } = default!;

        /// <summary>
//گروه بدهکار
        /// </summary>
        public int? DebitAccountReferenceGroupId { get; set; }

        /// <summary>
//کد بدهکار
        /// </summary>
        public int? DebitAccountReferenceId { get; set; }
        public decimal Amount { get; set; } = default!;

        /// <summary>
//تاریخ درخواست 
        /// </summary>
        public DateTime? RequestDate { get; set; }

        /// <summary>
//تاریخ صدور
        /// </summary>
        public DateTime? IssueDate { get; set; }

        /// <summary>
//تاریخ دریافت/پرداخت 
        /// </summary>
        public DateTime? RealDate { get; set; }
         
        public int? ChequeSheetId { get; set; }

        /// <summary>
//شناسه پرداخت 
        /// </summary>
        public string? TrackingCode { get; set; }

 
        public int IsAccumulativeSelectStatus { get; set; } = default!;
        public int OrderIndex { get; set; } = default!;

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
         

        public virtual ChequeSheets ChequeSheet { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual AccountHead CreditAccountHead { get; set; } = default!;
        public virtual AccountReferencesGroups CreditAccountReferencesGroup { get; set; } = default!;
        public virtual AccountReferences CreditReference { get; set; } = default!;
        public virtual AccountHead DebitAccountHead { get; set; } = default!;
        public virtual AccountReferencesGroups DebitAccountReferencesGroup { get; set; } = default!;
        public virtual AccountReferences DebitReference { get; set; } = default!;
        public virtual FinancialRequest FinancialRequest { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
