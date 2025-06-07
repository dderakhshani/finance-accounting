using System;

namespace Eefa.Inventory.Domain
{
    public class spCommodityReportsWithWarehouseParam
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string WarehouseId { get; set; }
        public string CodeVoucherGroupId { get; set; }
        public int DocumentStauseBaseValue { get; set; }
       
        public int? UserId { get; set; }
        public int? yearId { get; set; }

    }
   
}
