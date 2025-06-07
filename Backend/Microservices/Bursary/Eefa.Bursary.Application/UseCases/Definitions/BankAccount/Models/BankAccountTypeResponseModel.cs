using AutoMapper;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Models
{
    public class BankAccountTypeResponseModel : IMapFrom<BankAccountTypes_View>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BankAccountTypes_View, BankAccountTypeResponseModel>();
        }
    }
}
