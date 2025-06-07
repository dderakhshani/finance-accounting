using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Models
{
    public class PersonBankAccountModel : IMapFrom<Data.Databases.Entities.PersonBankAccount>
    {
        public int Id { get; set; } = default!;
        public int PersonId { get; set; } = default!;
        public int? BankId { get; set; }
        public string? BankBranchName { get; set; }
        public int AccountTypeBaseId { get; set; } = default!;
        public string AccountNumber { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.PersonBankAccount, PersonBankAccountModel>().IgnoreAllNonExisting();
        }
    }
}
