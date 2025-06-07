using Eefa.Common.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Domain.Entities.Definitions
{
    [Table("BankAccounts_Reference_View", Schema = "bursary")]
    public class BankAccountReferences_View : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int BankId { get; set; }
        public string BankCode { get; set; }
        public string BankTitle { get; set; }
        public string? Description { get; set; }
        public string? DepositId { get; set; }
        public bool IsActive { get; set; }
    }
}
