using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1575;&#1587;&#1606;&#1575;&#1583;
    /// </summary>
    public partial class FinancialRequestDetails : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
     

        /// <summary>
//شماره فرم عملیات مالی
        /// </summary>
        public int? FinancialRequestId { get; set; }

        /// <summary>
//نوع سند 
        /// </summary>
        public int DocumentTypeBaseId { get; set; } = default!;
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

        /// <summary>
//نوع ارز
        /// </summary>
        public int? CurrencyTypeBaseId { get; set; }

        /// <summary>
//نرخ ارز
        /// </summary>
        public decimal? CurrencyFee { get; set; }

        /// <summary>
//مقدار ارز
        /// </summary>
        public decimal? CurrencyAmount { get; set; }

        /// <summary>
//کد شیت چک 
        /// </summary>
        public int? ChequeSheetId { get; set; }

        /// <summary>
//مبلغ
        /// </summary>
        public decimal Amount { get; set; } = default!;

        /// <summary>
//کد سوئیفت 
        /// </summary>
        public string? SowiftCode { get; set; }

        /// <summary>
//شماره برگ سبز صادراتی
        /// </summary>
        public string? DeliveryOrderCode { get; set; }

        /// <summary>
//توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
//مبنای دریافت / پرداخت 
        /// </summary>
        public int FinancialReferenceTypeBaseId { get; set; } = default!;

        /// <summary>
//گد رهگیری فیش نقدی /پوز
        /// </summary>
        public string? RegistrationCode { get; set; }

        /// <summary>
//شناسه پرداخت / شماره حواله
        /// </summary>
        public string? PaymentCode { get; set; }

        /// <summary>
//0 : Rial , 2:Non-Rial
        /// </summary>
        public bool? IsRial { get; set; }

 
        public int? VocherDetailId { get; set; }

  
        public int? NonRialStatus { get; set; }

         public bool? BesCurrencyStatus { get; set; }
         public bool? BedCurrencyStatus { get; set; }


        public virtual ChequeSheets ChequeSheet { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual AccountHead CreditAccountHead { get; set; } = default!;
        public virtual AccountReferences CreditAccountReference { get; set; } = default!;


        public virtual AccountReferencesGroups CreditAccountReferenceGroup { get; set; } = default!;
        public virtual BaseValues CurrencyTypeBase { get; set; } = default!;
        public virtual AccountHead DebitAccountHead { get; set; } = default!;
        public virtual AccountReferences DebitAccountReference { get; set; } = default!;
        public virtual AccountReferencesGroups DebitAccountReferenceGroup { get; set; } = default!;
        public virtual BaseValues DocumentTypeBase { get; set; } = default!;
        public virtual BaseValues FinancialReferenceTypeBase { get; set; } = default!;
        public   FinancialRequest FinancialRequest { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual VouchersDetail VocherDetail { get; set; } = default!;

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
