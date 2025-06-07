using System;

namespace Eefa.Inventory.Domain
{

    public class SpCalculateRemainQuantityRequest
    {


        public Nullable<int> Quantity { get; set; }
        public string RequestNo { get; set; }
        public Nullable<int> CommodityId { get; set; }


    }
    
}
