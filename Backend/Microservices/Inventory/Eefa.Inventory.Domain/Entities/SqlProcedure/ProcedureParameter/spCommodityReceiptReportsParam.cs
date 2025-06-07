using System;

namespace Eefa.Inventory.Domain
{

    public class spCommodityReceiptReportsParam
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? CommodityId { get; set; }
        public int? WarehouseId { get; set; }
        public int? UserId { get; set; }

        public int? YearId { get; set; }
        
        public int? DocumentNo { get; set; }
        public int? RequestNo { get; set; }

        public string CommodityTitle { get; set; }
        

        public int PageNumber { get; set; }
        public int PageRow { get; set; }

        


    }
    public class spCommodityReceiptReportsRialParam
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? CommodityId { get; set; }
        public int? AccountHeadId { get; set; }
        public int? WarehouseId { get; set; }
        public int? UserId { get; set; }
        public int? YearId { get; set; }
        public int? DocumentNo { get; set; }
        public int PageNumber { get; set; }
        public int PageRow { get; set; }
    }
    
    public class sUpdateStockQuantityParam
    {
        
        public int? CommodityId { get; set; }
        public int? WarehouseId { get; set; }
        

    }
    public class spGetWarehouseLayoutQuantitiesParam
    {

        public int? CommodityId { get; set; }
        public int? WarehouseId { get; set; }
        public int? UserId { get; set; }


    }
    public class spUpdateWarehouseLayoutQuantitiesParam
    {

        public int? CommodityId { get; set; }
        public int? WarehouseLayoutId { get; set; }


    }
    public class GetCommodityParam
    {
        public int? warehouseId { get; set; }
        public string searchTerm { get; set; }
        public bool? IsConsumable { get; set; }
        public bool? IsAsset { get; set; }
       

    } 
}
