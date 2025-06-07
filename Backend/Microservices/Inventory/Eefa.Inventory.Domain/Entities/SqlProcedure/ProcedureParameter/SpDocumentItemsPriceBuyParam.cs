using System;

namespace Eefa.Inventory.Domain
{
    public class SpDocumentItemsPriceBuyParam
    {
        public int CommodityId { get; set; }
        public int? DocumentItemsId { get; set; }
        public int WarehouseId { get; set; }
       
    }
    public class spUpdateinventory_CaredxRepairParam
    {

        public Guid CaredxRepairId { get; set; }
        public int YearId { get; set; }


    }
    public class spUpdateWarehouseCardexParam
    {
        public int warehouseId { get; set; }
        public int YearId { get; set; }
    }
    public class spComputeAvgPriceParam
    {
        public int CommodityId { get; set; }
        public int ReceiptId { get; set; }
        public int YearId { get; set; }
        public Guid CaredxRepairId { get; set; }


    }
}
