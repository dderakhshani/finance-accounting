using AutoMapper;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Models;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using System;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Models
{
    public class Payables_ChecqueBooks_View_ResponseModel : IMapFrom<Payables_ChecqueBooks_View>
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public DateTime GetDate { get; set; }
        public string Serial { get; set; }
        public int SheetsCount { get; set; }
        public long StartNumber { get; set; }
        public int BankId { get; set; }
        public string BankTitle { get; set; }
        public int BankBranchId { get; set; }
        public string BankBranchCode { get; set; }
        public string BankBranchTitle { get; set; }
        public string AccountNumber { get; set; }
        public bool Isdeleted { get; set; }
        public string? BankAccountTitle { get; set; }
        public int? AccountHeadId { get; set; }
        public string? AccountHeadName { get; set; }
        public int? AccountReferencesGroupId { get; set; }
        public string? AccountReferencesGroupName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Payables_ChecqueBooks_View, Payables_ChecqueBooks_View_ResponseModel>().IgnoreAllNonExisting();
        }

    }
}
