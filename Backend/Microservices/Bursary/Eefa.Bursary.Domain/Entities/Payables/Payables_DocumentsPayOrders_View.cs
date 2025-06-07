using Eefa.Common.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_DocumentsPayOrders_View", Schema ="bursary")]
    public class Payables_DocumentsPayOrders_View : BaseEntity
    {
        public int DocumentId { get; set; }
        public int PayOrderId { get; set; }
        public DateTime PayOrderDate { get; set; }
        public string? PayOrderNo { get; set; }
        public int? BankAccountId { get; set; }
        public string? BankAccountName { get; set; }
        public long? PayOrderAmount { get; set; }

    }
}
