using Eefa.Common.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_Documents_View", Schema = "bursary")]
    public class Payables_Documents_View : BaseEntity
    {
        public int PayTypeId { get; set; }
        public string PayTypeName { get; set; }
        public int? ChequeBookSheetId { get; set; }
        public int? BankAccountId { get; set; }
        public long? DocumentNo { get; set; }
        public string? SayyadNo { get; set; }
        public int? BankId { get; set; }
        public string? BankTitle { get; set; }
        public string? AccountsName { get; set; }
        public int? BankBranchId { get; set; }
        public string? BankBranchCode { get; set; }
        public string? BankBranchTitle { get; set; }
        public string? AccountNumber { get; set; }
        public int? CurrencyTypeBaseId { get; set; }
        public string? CurrencyTypeName { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public int? ChequeTypeId { get; set; }
        public string? ChequeTypeName { get; set; }
        public long? CurrencyRate { get; set; }
        public long? CurrencyAmount { get; set; }
        public long Amount { get; set; }
        public string? Descp { get; set; }
        public int? LastOpId { get; set; }
        public string? LastOpCode { get; set; }
        public string LastOpName { get; set; }
        public DateTime LastOpDate { get; set; }
        public int? MonetarySystemId { get; set; }
        public int? CreditAccountHeadId { get; set; }
        public int? CreditReferenceId { get; set; }
        public int? CreditReferenceGroupId { get; set; }
        public string? MonetarySystemName { get; set; }
        public string? CreditAccountHeadName { get; set; }
        public string? CreditReferenceName { get; set; }
        public string? CreditReferenceGroupName { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public bool? ShowDebit { get; set; }
        public bool? ShowCredit { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? DraftDate { get; set; }


    }
}
