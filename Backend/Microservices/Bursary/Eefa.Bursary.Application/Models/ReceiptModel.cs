using Eefa.Common;
using AutoMapper;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Application.Models;

namespace Eefa.Bursary.Application
{
    public class ReceiptModel : IMapFrom<ReceiptModel>
    {
        public int Id { get; set; }
        public int? FinancialRequestId { get; set; }
        public int DocumentTypeBaseId { get; set; }
        public int FinancialReferenceTypeBaseId { get; set; }
        public string Description { get; set; }
        public int DebitAccountHeadId { get; set; }
        public int DebitAccountReferenceGroupId { get; set; }
        public int DebitAccountReferenceId { get; set; }
        public int CreditAccountHeadId { get; set; }
        public string CreditAccountReferenceCode { get; set; }
        public string CreditAccountReferenceTitle { get; set; }
        public int CurrencyTypeBaseId { get; set; }
        public int? ChequeSheetId { get; set; }
        public decimal? CurrencyFee { get; set; }
        public decimal? CurrencyAmount { get; set; }
        public int CreditAccountReferenceGroupId { get; set; }
        public int CreditAccountReferenceId { get; set; }
        public decimal Amount { get; set; }
        public int SowiftCode { get; set; }
        public int DeliveryOrderCode { get; set; }
        public int RegistrationCode { get; set; }
        public string PaymentCode { get; set; }
        public bool IsRial { get; set; }
        public int NonRialStatus { get; set; }
        public bool BedCurrencyStatus { get; set; }
         public bool BesCurrencyStatus { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ChequeSheetModel? ChequeSheet { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FinancialRequestDetails ,ReceiptModel>().IgnoreAllNonExisting();
        }
    }
}
