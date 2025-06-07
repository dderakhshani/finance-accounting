using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public class BankTransactionHead: BaseEntity
    {
        public string AccountNumber { get; set; }
        public DateTime Time { get; set; }
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
