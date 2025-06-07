using Eefa.Common.Data;
using System;
using System.Collections.Generic;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Definitions;

namespace Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate
{
 
    public partial class FinancialRequest : BaseEntity
    {
        public FinancialRequest()
        {
            FinancialRequestAttachments = new HashSet<FinancialRequestAttachments>();
            FinancialRequestDocuments = new HashSet<FinancialRequestDocuments>();
            FinancialRequestPartials = new HashSet<FinancialRequestPartial>();
            FinancialRequestVerifiers = new HashSet<FinancialRequestVerifiers>();
        }


        /// <summary>
//شناسه
        /// </summary>
         
        public int? ParentId { get; set; }
        public int CodeVoucherGroupId { get; set; } = default!;

        /// <summary>
//کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

        /// <summary>
//نوع پرداخت
        /// </summary>
        public int? PaymentTypeBaseId { get; set; }

        /// <summary>
//کد سند حسابداری
        /// </summary>
        public int? VoucherHeadId { get; set; }
        public int? WorkflowId { get; set; }

        /// <summary>
//آخرین وضعیت سند
        /// </summary>
        public int FinancialStatusBaseId { get; set; } = default!;

        /// <summary>
//شماره فرم عملیات مالی
        /// </summary>
        public int DocumentNo { get; set; } = default!;

        /// <summary>
//شماره سریال سند - کد سال +کد شعبه +کد سیستم 
        /// </summary>
        public string? DocumentSerial { get; set; }

        /// <summary>
//تاریخ سند
        /// </summary>
        public DateTime DocumentDate { get; set; } = default!;

        /// <summary>
//توضیحات
        /// </summary>
        public string? Description { get; set; }

     
        public int PaymentStatus { get; set; } = default!;

        /// <summary>
//مبلغ
        /// </summary>
        public decimal Amount { get; set; } = default!;

        /// <summary>
//تاریخ انقضا
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
//کل مبلغ قابل پرداخت 
        /// </summary>
        public decimal TotalAmount { get; set; } = default!;

        /// <summary>
//مقدار کسر شده از پرداخت
        /// </summary>
        public decimal? DeductAmount { get; set; }

        /// <summary>
//شرح علت کسراز پرداخت 
        /// </summary>
        public string? DeductionReason { get; set; }

        /// <summary>
//تاریخ صدور
        /// </summary>
        public DateTime? IssueDate { get; set; }
        public string? ExtraFieldJson { get; set; }

        /// <summary>
//جزئیات نواقص
        /// </summary>
        public string? MissedDocumentJson { get; set; }

        /// <summary>
//وضعیت جریان کاری 
        /// </summary>
        public string? WorkflowState { get; set; }

        /// <summary>
//پرداخت فوری
        /// </summary>
        public bool IsEmergent { get; set; } = default!;

        /// <summary>
//پرداخت تجمیعی
        /// </summary>
        public bool IsAccumulativePayment { get; set; } = default!;
        public bool? IsPending { get; set; }

        public int? TransactionId { get; set; }

        public short? AutomateState { get; set; }

        public virtual CodeVoucherGroups CodeVoucherGroup { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual BaseValues FinancialStatusBase { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual BaseValues PaymentTypeBase { get; set; } = default!;
        public virtual VouchersHead VoucherHead { get; set; } = default!;
        public virtual Workflows Workflow { get; set; } = default!;
        public virtual Years Year { get; set; } = default!;
        public virtual ICollection<FinancialRequestAttachments> FinancialRequestAttachments { get; set; } = default!;
        public virtual List<FinancialRequestDetails> FinancialRequestDetails { get; set; } = default!;
        public virtual ICollection<FinancialRequestDocuments> FinancialRequestDocuments { get; set; } = default!;
        public virtual ICollection<FinancialRequestPartial> FinancialRequestPartials { get; set; } = default!;
        public virtual ICollection<FinancialRequestVerifiers> FinancialRequestVerifiers { get; set; } = default!;


        public void Delete(int number)
        {
            IsDeleted = true;
            Description =  Description + " --- " +  DocumentNo;
            DocumentNo = number;
            FinancialRequestDetails.ForEach(x => x.Delete());
        }


        public void AddFinancialDetail(FinancialRequestDetails financialDetail)
        {
            financialDetail.FinancialRequest = this;
            this.FinancialRequestDetails = new List<FinancialRequestDetails>();
            this.FinancialRequestDetails.Add(financialDetail);
        }


        public void AddFinancialAttachment(FinancialRequestAttachments attachment)
        {
            attachment.FinancialRequest = this;
        }

        public void SetAmount(decimal amount)
        {
            this.Amount = amount;
            this.TotalAmount = amount;
        }

        public void SetDescription(string description)
        {
            this.Description = description;
        }


        public void SetDocumentDate(DateTime date)
        {
            this.DocumentDate = date;
        }

        public void SetDocNumber(int number)
        {
            this.DocumentNo = number;
        }

    }
}
