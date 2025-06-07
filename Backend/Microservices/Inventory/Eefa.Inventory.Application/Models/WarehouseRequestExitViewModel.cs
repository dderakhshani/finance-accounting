using Eefa.Common;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Eefa.Inventory
{
    public partial class WarehouseRequestExitViewModel : IMapFrom<Domain.WarehouseRequestExitView>
    {
        public int ErpItemsId { get; set; } = default!;
        public int? Id { get; set; } = default!;
        public int? WarehouseId { get; set; } = default!;
        public string WarehouseTitle { get; set; } = default!;
        public string StatusTitle { get; set; } = default!;
        public int? DocumentNo { get; set; } = default!;

        public string RequestNo { get; set; } = default!;

        public DateTime? DocumentDate { get; set; } = default!;

        public double? ExitQuantity { get; set; } = default!;
        
        public double? RequestQuantity { get; set; } = default!;
        public string CommodityTitle { get; set; } = default!;
        public string CommodityCode { get; set; } = default!;
        public string WarehouseLayoutsTitle { get; set; } = default!;
        public double? RemainedQuantity { get; set; } = default!;


    }
}
