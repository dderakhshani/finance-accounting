using AutoMapper;
using Eefa.Bursary.Application.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;
using System;

namespace Eefa.Bursary.Application.Queries.FinancialRequest
{
    public class FinancialRequestDetailModel :IMapFrom<FinancialRequestDetails>
    {
        

        public int Id { get; set; }
        public int DetailId { get; set; }
        public int? FinancialRequestId { get; set; }
        public int DocumentTypeBaseId { get; set; } = default!;
        public string DocumentTypeBaseTitle { get; set; } = default!;
        public string? CreateName { get; set; }
        public string  ModifyName { get; set; }
        public int  CreditAccountHeadId { get; set; } = default!;
        public int? CreditAccountReferenceGroupId { get; set; }
        public int? CreditAccountReferenceId { get; set; }
        public int  DocumentNo { get; set; } = default!;
        public string CreditAccountHeadTitle { get; set; } = default!;
        public string? CreditAccountReferenceGroupTitle { get; set; }
        public string? CreditAccountReferenceTitle { get; set; }
        public string? CreditAccountReferenceCode { get; set; }


        public int DebitAccountHeadId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; }
        public int? DebitAccountReferenceId { get; set; }


        public string DebitAccountHeadTitle { get; set; } = default!;
        public string? DebitAccountReferenceGroupTitle { get; set; }
        public string? DebitAccountReferenceTitle { get; set; }
        public string? DebitAccountReferenceCode { get; set; }
        public int? CurrencyTypeBaseId { get; set; }
        public int? ChequeSheetId { get; set; }
        public decimal Amount { get; set; } = default!;
        public string? SowiftCode { get; set; }
        public string? DeliveryOrderCode { get; set; }
        public string? Description { get; set; }
        public int FinancialReferenceTypeBaseId { get; set; } = default!;
        public string FinancialReferenceTypeBaseTitle { get; set; } = default!;
        public string? RegistrationCode { get; set; }
        public string? PaymentCode { get; set; }
        public bool? IsRial { get; set; }
        public int? NonRialStatus { get; set; }
        public decimal? CurrencyFee { get; set; }
        public decimal? CurrencyAmount { get; set; }
        public string? SheetUniqueNumber { get; set; }
        public virtual ChequeSheetModel? ChequeSheet { get; set; }
        public string DebitAccountReferenceGroupCode { get;   set; }
        public string CreditAccountReferenceGroupCode { get;   set; }
        public string SheetSeqNumber { get;  set; }
        public int CodeVoucherGroupId { get;  set; }
        public DateTime? IssueDate { get; set; }
        public int? PaymentTypeBaseId { get;   set; }
        public DateTime DocumentDate { get;   set; }
        public int? VoucherHeadId { get;   set; }
        public string? DetailDescription { get;  set; }
        public int? VoucherHeadCode { get; set; }
        public int? VoucherStateId { get;   set; }
        public string? PaymentTypeBaseTitle { get;   set; }
        public short? AutomateState { get; set; }
        public bool? BesCurrencyStatus { get; set; }
        public bool? BedCurrencyStatus { get; set; }



        public void Mapping(Profile profile)
        {
            profile.CreateMap<FinancialRequestDetails, FinancialRequestDetailModel>()
                .ForMember(item => item.CreditAccountHeadTitle,
                    opt => opt.MapFrom(x => x.CreditAccountHead.Title))
                .ForMember(item => item.DebitAccountHeadTitle,
                    opt => opt.MapFrom(x => x.DebitAccountHead.Title))
                .ForMember(item => item.CreditAccountReferenceGroupTitle,
                    opt => opt.MapFrom(x => x.CreditAccountReferenceGroup.Title))
                .ForMember(item => item.DebitAccountReferenceGroupTitle,
                    opt => opt.MapFrom(x => x.DebitAccountReferenceGroup.Title))
                .ForMember(item => item.CreditAccountReferenceTitle,
                    opt => opt.MapFrom(x => x.CreditAccountReference.Title))
                .ForMember(item => item.DebitAccountReferenceTitle,
                    opt => opt.MapFrom(x => x.DebitAccountReference.Title));

        }

    }
}
