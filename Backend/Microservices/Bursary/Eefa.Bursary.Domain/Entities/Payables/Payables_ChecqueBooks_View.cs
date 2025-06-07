using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_ChequeBooks_View", Schema = "bursary")]
    public class Payables_ChecqueBooks_View : BaseEntity
    {

        public int BankAccountId { get; set; }
        public string? BankAccountTitle { get; set; }
        public int? AccountHeadId { get; set; }
        public string? AccountHeadName { get; set; }
        public int? AccountReferencesGroupId { get; set; }
        public string? AccountReferencesGroupName { get; set; }
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
        public string? Descp { get; set; }
    }
}
