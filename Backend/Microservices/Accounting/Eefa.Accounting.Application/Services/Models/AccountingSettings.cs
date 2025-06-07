using System.Collections.Generic;

namespace Eefa.Accounting.Application.Services.Models
{
    public class AccountingSettings
    {
        public List<VoucherHeadBeingModified> VoucherHeadsBeingModified { get; set; } = new List<VoucherHeadBeingModified>();
    }
}
