using AutoMapper;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Models
{
    public class CountryDivisionResponseModel:IMapFrom<CountryDivisions_View>
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CountryDivisions_View, CountryDivisionResponseModel>();
        }


    }
}
