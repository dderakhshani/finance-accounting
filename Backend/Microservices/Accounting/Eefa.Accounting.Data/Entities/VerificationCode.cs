using Library.Common;
using System;
using System.Collections.Generic;

namespace Eefa.Accounting.Data.Entities
{
    public partial class VerificationCode : BaseEntity
    {
        public VerificationCode()
        {
            MoadianInvoiceHeaders = new HashSet<MoadianInvoiceHeader>();
        }

        public string Code { get; set; } = default!;
        public DateTime ExpiryDate { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsUsed { get; set; } = default!;


        public virtual ICollection<MoadianInvoiceHeader> MoadianInvoiceHeaders { get; set; } = default!;
    }
}
