using System;

namespace Eefa.Accounting.Application.Services.Models
{
    public class VoucherHeadBeingModified
    {
        public int Id { get; set; }
        public int ModifierId { get; set; }
        public string ModifierName { get; set; }
        public DateTime LockDueDate { get; set; }
    }
}
