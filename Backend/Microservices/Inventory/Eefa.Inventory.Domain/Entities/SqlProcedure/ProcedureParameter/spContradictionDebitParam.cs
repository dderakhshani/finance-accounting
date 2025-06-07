using System;

namespace Eefa.Inventory.Domain
{

    public class spContradictionDebitParam
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? CommodityId { get; set; }
        public string AccountHeadId { get; set; }
        public int? UserId { get; set; }
        public int? YearId { get; set; }
        public int PageNumber { get; set; }
        public int PageRow { get; set; }

    }

}
