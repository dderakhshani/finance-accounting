using System;

namespace Eefa.Inventory.Domain
{

    public class spGetConsumptionCommodityByRequesterReferenceId
    {
        public Nullable<double> Quantity { get; set; }
        public Nullable<int> CommodityQuota { get; set; }
        public Nullable<int> QuotaDays { get; set; }
        public Nullable<int> DocumentHeadsId { get; set; }
        public Nullable<int> DocumentNo { get; set; }
        public Nullable<DateTime> DocumentDate { get; set; }
    }
    
}
