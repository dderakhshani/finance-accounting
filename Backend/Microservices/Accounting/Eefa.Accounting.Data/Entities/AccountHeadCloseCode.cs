using Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Data.Entities
{
    public partial class AccountHeadCloseCode : BaseEntity
    {
        public int? ParentAccountHeadId { get; set; }
        public int AccountHeadId { get; set; }
        public int AccountClosingCodeId { get; set; }
        public string Description { get; set; }
        public bool Debit_CreditStatus { get; set; }
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
