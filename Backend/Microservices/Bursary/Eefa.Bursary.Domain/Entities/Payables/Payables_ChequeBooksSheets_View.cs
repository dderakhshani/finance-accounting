using Eefa.Common.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_ChequeBooksSheets_View", Schema = "bursary")]
    public class Payables_ChequeBooksSheets_View : BaseEntity
    {
        public int ChequeBookId { get; set; }
        public long ChequeSheetNo { get; set; }
        public string SayyadNo { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime? GetDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public string CancelDescp { get; set; }
        public int? DocumentId { get; set; }
        public int? DocumentLastOpId { get; set; }
        public int? StatusId { get; set; }
        public int? ReferenceId { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusName { get; set; }
        public long? Amount { get; set; }
        public int? AccountHeadId { get; set; }
        public string? AccountHeadName { get; set; }
        public int? AccountReferencesGroupId { get; set; }
        public string? AccountReferencesGroupName { get; set; }
        public int BankAccountId { get; set; }
        public string? BankAccountTitle { get; set; }
        public int BankId { get; set; }
        public string? BankTitle { get; set; }
        public int BankBranchId { get; set; }
        public string? BankBranchTitle { get; set; }
        public string? AccountNumber { get; set; }
    }
}
