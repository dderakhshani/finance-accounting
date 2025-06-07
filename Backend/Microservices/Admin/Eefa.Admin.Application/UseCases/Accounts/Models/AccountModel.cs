using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Person.Model;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.Accounts.Models
{
    public class AccountModel : IMapFrom<Data.Databases.Entities.AccountReference>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public PersonModel Person { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.AccountReference, AccountModel>();
        }
    }
}
