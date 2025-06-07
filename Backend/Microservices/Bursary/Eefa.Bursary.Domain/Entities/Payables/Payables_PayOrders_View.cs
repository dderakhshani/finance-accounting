using Eefa.Common.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_PayOrders_View", Schema = "bursary")]
    public class Payables_PayOrders_View : BaseEntity
    {
        public DateTime PayOrderDate { get; set; }
        public string PayOrderNo { get; set; }
        public int BankAccountId { get; set; }
        public string BankAccountName { get; set; }
        public long PayOrderAmount { get; set; }

    }
}
