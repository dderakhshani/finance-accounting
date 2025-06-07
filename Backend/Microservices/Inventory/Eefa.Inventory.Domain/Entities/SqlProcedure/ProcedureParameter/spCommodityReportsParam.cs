using System;

namespace Eefa.Inventory.Domain
{

    public class spCommodityReportsParam
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? CommodityId { get; set; }
        public string WarehouseId { get; set; }
        public string CommodityTitle { get; set; }
        public int? UserId { get; set; }
        public int? YearId { get; set; }
        public int PageNumber { get; set; }
        public int PageRow { get; set; }

    }
    public class spCommodityReportsRialParam
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? CommodityId { get; set; }
        public int AccountHeadId { get; set; }
        public int? UserId { get; set; }
        public int? YearId { get; set; }
        public int PageNumber { get; set; }
        public int PageRow { get; set; }

    }

}
