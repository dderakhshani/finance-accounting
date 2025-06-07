using AutoMapper;

using System;
using System.Collections.Generic;
using Eefa.Common;

namespace Eefa.Bursary.Application.Queries.FinancialRequest
{
    public record FinancialRequestModel : IMapFrom<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>
    {

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int CodeVoucherGroupId { get; set; } = default!;
        public string DetailDescription { get; set; }
        public string DocumentTypeBaseTitle { get; set; }
        public decimal DetailAmount { get; set; }
        /// <summary>
        /// کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

        /// <summary>
        /// نوع پرداخت
        /// </summary>
        public int? PaymentTypeBaseId { get; set; } = default!;

        /// <summary>
        /// کد سند حسابداری
        /// </summary>
        public int? VoucherHeadId { get; set; }
        public string? VoucherHeadTitle { get; set; }
    

        public int? WorkflowId { get; set; }

        public int? CurrencyTypeBaseId { get; set; }
        public string CurrencyTypeBaseTitle { get; set; }

        /// <summary>   
        /// آخرین وضعیت سند
        /// </summary>
        public int FinancialStatusBaseId { get; set; } = default!;

        /// <summary>
        /// شماره فرم عملیات مالی
        /// </summary>
        public int DocumentNo { get; set; } = default!;

        /// <summary>
        /// شماره سریال سند - کد سال +کد شعبه +کد سیستم 
        /// </summary>
        public string? DocumentSerial { get; set; }

        /// <summary>
        /// تاریخ سند
        /// </summary>
        public DateTime DocumentDate { get; set; } = default!;

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 1= علی الحساب
        /// 2= پیش پرداخت
        /// 3= تسویه
        /// </summary>
        public int PaymentStatus { get; set; } = default!;

        /// <summary>
        /// مبلغ
        /// </summary>
        public decimal Amount { get; set; } = default!;

        /// <summary>
        /// تاریخ انقضا
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// کل مبلغ قابل پرداخت 
        /// </summary>
        public decimal TotalAmount { get; set; } = default!;

        /// <summary>
        /// مقدار کسر شده از پرداخت
        /// </summary>
        public decimal? DeductAmount { get; set; }

        /// <summary>
        /// شرح علت کسراز پرداخت 
        /// </summary>
        public string? DeductionReason { get; set; }

        /// <summary>
        /// تاریخ صدور
        /// </summary>
        public DateTime? IssueDate { get; set; }
        public string? ExtraFieldJson { get; set; }

        /// <summary>
        /// جزئیات نواقص
        /// </summary>
        public string? MissedDocumentJson { get; set; }

        /// <summary>
        /// وضعیت جریان کاری 
        /// </summary>
        public string? WorkflowState { get; set; }

        /// <summary>
        /// پرداخت فوری
        /// </summary>
        public bool IsEmergent { get; set; } = default!;

        /// <summary>
        /// پرداخت تجمیعی
        /// </summary>
        public bool IsAccumulativePayment { get; set; } = default!;

        public int? VoucherHeadCode { get; set; }

        public decimal? CurrencyFee { get; set; }
        public decimal? CurrencyAmount { get; set; }

        public List<FinancialRequestDetailModel> FinancialRequestDetails { get; set; }

        public List<FinancialAttachmentModel> FinancialRequestAttachments { get; set; }

        public string CreditAccountReferenceTitle { get; set; }
        public string DebitAccountReferenceTitle { get; set; }
        public bool IsDetail { get; set; }
        public object PaymentTypeBaseTitle { get; set; }
        public string CreditAccountReferenceCode { get;   set; }
        public string DebitAccountReferenceCode { get;   set; }
        public string CreditAccountReferenceGroupTitle { get; set; }
        public string CreateName { get; set; }
        public int? VoucherStateId { get;  set; }
        public string CreditAccountReferenceGroupCode { get; set; }
        public short? AutomateState { get; set; }
        public int? CreditAccountReferenceGroupId { get; internal set; }
        public int CreditAccountHeadId { get; internal set; }
        public int? CreditAccountReferenceId { get; internal set; }
        public int? DebitAccountReferenceId { get; internal set; }
        public int DebitAccountHeadId { get; internal set; }
        public int? DebitAccountReferenceGroupId { get; internal set; }
        public int DocumentTypeBaseId { get; internal set; }
               public bool? BesCurrencyStatus { get; set; }
        public bool? BedCurrencyStatus { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest, FinancialRequestModel>()
            
                .ForMember(src=>src.VoucherHeadCode , opt => opt.MapFrom(x=>x.VoucherHead.VoucherNo));
                     
        }

    }
}
