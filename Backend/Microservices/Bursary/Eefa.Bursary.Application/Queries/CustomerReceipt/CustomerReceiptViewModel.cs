using AutoMapper;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;

namespace Eefa.Bursary.Application.Queries.CustomerReceipt
{
    public record CustomerReceiptViewModel : IMapFrom<FinancialRequestDetails>
    {
        public int? FinancialRequestId { get; set; }

        /// <summary>
        /// نوع سند 
        /// </summary>
        public int DocumentTypeBaseId { get; set; } = default!;
        public int CreditAccountHeadId { get; set; } = default!;

        /// <summary>
        /// گروه بستانکار
        /// </summary>
        public int? CreditAccountReferenceGroupId { get; set; }

        /// <summary>
        /// کد بستانکار
        /// </summary>
        public int? CreditAccountReferenceId { get; set; }
        public int DebitAccountHeadId { get; set; } = default!;

        /// <summary>
        /// گروه بدهکار
        /// </summary>
        public int? DebitAccountReferenceGroupId { get; set; }

        /// <summary>
        /// کد بدهکار
        /// </summary>
        public int? DebitAccountReferenceId { get; set; }
        public int? CurrencyTypeBaseId { get; set; }

        /// <summary>
        /// کد شیت چک 
        /// </summary>
        public int? ChequeSheetId { get; set; }

        /// <summary>
        /// مبلغ
        /// </summary>
        public decimal Amount { get; set; } = default!;

        /// <summary>
        /// کد سوئیفت 
        /// </summary>
        public string? SowiftCode { get; set; }

        /// <summary>
        /// شماره برگ سبز صادراتی
        /// </summary>
        public string? DeliveryOrderCode { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// مبنای دریافت / پرداخت 
        /// </summary>
        public int FinancialReferenceTypeBaseId { get; set; } = default!;

        /// <summary>
        /// گد رهگیری فیش نقدی /پوز
        /// </summary>
        public string? RegistrationCode { get; set; }
        public string? PaymentCode { get; set; }

        public bool? IsRial { get; set; }

        public int? NonRialStatus { get; set; }
        public int? VoucherDetailId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FinancialRequestDetails, CustomerReceiptViewModel>();
         }
    }
}
