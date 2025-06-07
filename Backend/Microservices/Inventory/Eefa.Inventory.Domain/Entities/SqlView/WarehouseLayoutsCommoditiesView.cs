using System;
using System.ComponentModel.DataAnnotations;

namespace Eefa.Inventory.Domain
{
    
    /// <summary>
    /// محل و تعداد قرار گرفتن کالا در هر موقعیت انبار
    /// </summary>
    public partial class WarehouseLayoutsCommoditiesView 
    {
        [Key]
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int? WarehouseLayoutId { get; set; }
        public int? WarehousesParentId { get; set; }
        public int CommodityId { get; set; }
        public double? Quantity { get; set; }
        //public double? SixMonthsUse { get; set; }
        public int? WarehouseLayoutParentId { get; set; }
        public int? WarehouseLayoutCapacity { get; set; }
        public string WarehouseTitle { get; set; }
        public string WarehousesLevelCode { get; set; }
        public string WarehouseLayoutTitle { get; set; }
        public string WarehouseLayoutLevelCode { get; set; }
        public string CommodityCompactCode { get; set; }
        public string CommodityTadbirCode { get; set; }
        public string CommodityTitle { get; set; }
        public string CommodityCode { get; set; }
        public bool AllowInput { get; set; }
        public bool AllowOutput { get; set; }

        public int MeasureId { get; set; }
        public string MeasureTitle { get; set; }
        public DateTime ModifiedAt { get; set; }



    }
   
}
