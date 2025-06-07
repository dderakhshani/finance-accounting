using AutoMapper;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Models
{
    public class BankBranchResponseModel : IMapFrom<BankBranches_View>
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public int BankId { get; set; }
        public string? BankCode { get; set; }
        public string? BankTitle { get; set; }
        public int CountryDivisionId { get; set; }
        public string? CountryDivisionTitle { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ManagerFullName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BankBranches_View, BankBranchResponseModel>();
        }


    }
}
