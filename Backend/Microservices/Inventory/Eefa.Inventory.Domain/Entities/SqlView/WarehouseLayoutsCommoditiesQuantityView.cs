using System.ComponentModel.DataAnnotations;

namespace Eefa.Inventory.Domain
{
   /// <summary>
   /// تعداد موجودی کالا در هر موقعیت انبار
   /// </summary>
    public partial class WarehouseLayoutsCommoditiesQuantityView
    {
        [Key]
        public int Id { get; set; }
        public int? WarehouseId { get; set; }
        public int? WarehouseLayoutId { get; set; }
        public int? CommodityId { get; set; }
        public double? Quantity { get; set; }
        public string WarehouseTitle { get; set; }
        public string WarehouseLayoutTitle { get; set; }
        public string CommodityTitle { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityTadbirCode { get; set; }
        public int? WarehouseLayoutCapacity { get; set; }
        

    }
}
