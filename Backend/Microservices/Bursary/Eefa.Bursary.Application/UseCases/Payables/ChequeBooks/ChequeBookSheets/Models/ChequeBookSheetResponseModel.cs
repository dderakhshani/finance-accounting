using AutoMapper;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using System;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Models
{
    public class ChequeBookSheetResponseModel : IMapFrom<Payables_ChequeBooksSheets_View>
    {
        public int Id { get; set; }
        public int ChequeBookId { get; set; }
        public long ChequeSheetNo { get; set; }
        public string SayyadNo { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime? CancelDate { get; set; }
        public string CancelDescp { get; set; }
        public int? DocumentId { get; set; }
        public int? DocumentLastOpId { get; set; }
        public int? StatusId { get; set; }
        public int? ReferenceId { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusName { get; set; }
        public long? Amount { get; set; }
        public int? BankAccountId { get; set; }
        public string? BankAccountTitle { get; set; }
        public int? AccountHeadId { get; set; }
        public string? AccountHeadName { get; set; }
        public int? AccountReferencesGroupId { get; set; }
        public string? AccountReferencesGroupName { get; set; }
        public int BankId { get; set; }
        public string? BankTitle { get; set; }
        public int BankBranchId { get; set; }
        public string? BankBranchTitle { get; set; }
        public string? AccountNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Payables_ChequeBooksSheets_View, ChequeBookSheetResponseModel>().IgnoreAllNonExisting();
        }

    }
}
