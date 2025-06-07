using Eefa.Common.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_PayRequests_View", Schema = "bursary")]
    public class Payables_PayRequests_View : BaseEntity
    {
        public int PayOrderId { get; set; }
        public DateTime PayRequestDate { get; set; }
        public string PayRequestNo { get; set; }
        public long PayRequestAmount { get; set; }
        public string PayRequestDescp { get; set; }
        public int PayRequestAccountId { get; set; }
        public string PayRequestAccountName { get; set; }
    }
}
