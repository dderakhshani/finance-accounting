using Eefa.Common.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class BankTransactions : BaseEntity
    {
        public string TransactionUniqueId { get; set; }

        public string AccountNumber { get; set; }
      
        public string OrigKey { get; set; }
 
        public decimal TransactionAmountDebit { get; set; }
     
        public decimal TransactionAmountCredit { get; set; }
 
        public string? OperationCode { get; set; }
 
        public long TransactionAmount { get; set; }
 
        public string TransactionType { get; set; }
 
        public long? TransactionDate { get; set; }
 
        public string TransactionTime { get; set; }
 
        public string DocNumber { get; set; }
 
        public string Description { get; set; }
 
        public string PayId1 { get; set; }
 
        public string BranchCode { get; set; }
 
        public string Balance { get; set; }

         public int? FinancialRequestId { get; set; }
        // public int? VoucherHeadId { get; set; }


        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;

    }
}
