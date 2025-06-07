using AutoMapper;
using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Models;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;

namespace Eefa.Bursary.Application.UseCases.Definitions.Bank.Models
{
    public class BankTypeResponseModel:IMapFrom<BankTypes_View>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<BankTypes_View, BankTypeResponseModel>().IgnoreAllNonExisting();
        }

    }
}
