using AutoMapper;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Models
{
    public class BankAccountsResponseModel:IMapFrom<BankAccounts_View>
    {
        public int Id {  get; set; } 
        public int? ParentId { get; set; }
        public int BankBranchId { get; set; }
        public string? Sheba { get; set; }
        public string? AccountNumber { get; set; }
        public int? SubsidiaryCodeId { get; set; }
        public int? RelatedBankAccountId { get; set; }
        public int? AccountTypeBaseId { get; set; }
        public int? AccountHeadId { get; set; }
        public string? AccountHeadName { get; set; }
        public int? AccountReferencesGroupId { get; set; }
        public string? AccountReferencesGroupName { get; set; }
        public int? ReferenceId { get; set; }
        public int? AccountStatus { get; set; }
        public decimal? WithdrawalLimit { get; set; }
        public bool HaveChekBook { get; set; }
        public int? CurrenceTypeBaseId { get; set; }
        public string? SignersJson { get; set; }
        public string? Title { get; set; }
        public string? BankBranchCode { get; set; }
        public string? BankBranchTitle { get; set; }
        public int BankId { get; set; }
        public string? BankCode { get; set; }
        public string? BankTitle { get; set; }
        public string? AccountTypeBaseTitle { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BankAccounts_View, BankAccountsResponseModel>();
        }

    }
}
