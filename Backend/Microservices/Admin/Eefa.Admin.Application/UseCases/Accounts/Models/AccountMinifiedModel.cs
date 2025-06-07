using AutoMapper;
using Library.Mappings;
using System.Linq;

namespace Eefa.Admin.Application.CommandQueries.Accounts.Models
{
    public class AccountMinifiedModel : IMapFrom<Data.Databases.Entities.AccountReference>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string NationalNumber { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.AccountReference, AccountMinifiedModel>()
                .ForMember(src => src.FullName, opt => opt.MapFrom(t => t.Person.FirstName + " " + t.Person.LastName))
                .ForMember(src => src.Address, opt => opt.MapFrom(t => t.Person.PersonAddresses.FirstOrDefault().Address))
                .ForMember(src => src.PhoneNumber, opt => opt.MapFrom(t => t.Person.PersonPhones.FirstOrDefault().PhoneNumber))
                .ForMember(src => src.NationalNumber, opt => opt.MapFrom(t => t.Person.NationalNumber));
        }
    }
}
