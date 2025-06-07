using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Eefa.Inventory.Domain
{
    public partial class WarehouseRequestExitView
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int ErpItemsId { get; set; } = default!;
        public string RequestNo { get; set; } = default!;
        public string StatusTitle { get; set; } = default!;
        public int? Id { get; set; } = default!;
        public int? WarehouseId { get; set; } = default!;

        
        public int? DocumentNo { get; set; } = default!;

        
        public string WarehouseTitle { get; set; } = default!;

        public DateTime? DocumentDate { get; set; } = default!;

        public double? ExitQuantity { get; set; } = default!;
       
        public double? RequestQuantity { get; set; } = default!;
        public string CommodityTitle { get; set; } = default!;
        public string CommodityCode { get; set; } = default!;

        public double? RemainedQuantity { get; set; } = default!;
        
    }
}
