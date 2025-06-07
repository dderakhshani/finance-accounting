using AutoMapper;
using Eefa.Common;

namespace Eefa.Bursary.Application
{
    public class AccountingDocumentModel :IMapFrom<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>
    {
   public int DocumentNo { get; set; }
   public int DocumentId { get; set; }
   public int DocumentDate { get; set; }
   public int CodeVoucherGroupId { get; set; }
   public int DebitAccountHeadId { get; set; }
   public int DebitAccountReferenceGroupId { get; set; }
   public int DebitAccountReferenceId { get; set; }
   public int CreditAccountHeadId { get; set; }
   public int CreditAccountReferenceGroupId { get; set; }
   public int CreditAccountReferenceId { get; set; }
   public int Amount { get; set; }
   public int DocumentTypeBaseId { get; set; }

   public void Mapping(Profile profile)
   {
       profile.CreateMap<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest ,AccountingDocumentModel> ()
           .ForMember(src=>src.DocumentId , opt=>opt.MapFrom(x=>x.Id)).IgnoreAllNonExisting();
   }

    }
}
