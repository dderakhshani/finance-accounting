using System;

namespace Eefa.Inventory.Domain
{

    public class spCommodityReportsSumAllParam
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? CommodityId { get; set; }

        public string CommodityTitle { get; set; }
        public string WarehouseId { get; set; }
        public int? UserId { get; set; }
        public int? YearId { get; set; }
        

    }

}
