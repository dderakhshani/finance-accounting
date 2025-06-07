using AutoMapper;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;

namespace Eefa.Bursary.Application.UseCases.Definitions.Bank.Models
{
    public class BankResponseModel : IMapFrom<Banks_View>
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Banks_View, BankResponseModel>();
        }

    }
}
