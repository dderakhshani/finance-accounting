using AutoMapper;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Models
{
    public class BankAccountReferenceResponseModel : IMapFrom<BankAccountReferences_View>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int BankId { get; set; }
        public string BankCode { get; set; }
        public string BankTitle { get; set; }
        public string? Description { get; set; }
        public string? DepositId { get; set; }
        public bool IsActive { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BankAccountReferences_View, BankAccountReferenceResponseModel>();
        }

    }


}
