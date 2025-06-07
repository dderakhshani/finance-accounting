using System;

namespace Eefa.Inventory.Domain
{

    public partial class AssetsNotAssignedView
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; } = default!;
        public int WarehouseId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public int MeasureId { get; set; } = default!;
        public int AssetGroupId { get; set; } = default!;
        public int? UnitId { get; set; } = default!;
        public DateTime DocumentDate { get; set; } = default!;
        public string CommoditySerial { get; set; } = default!;
        public string AssetSerial { get; set; } = default!;
        public int? DepreciationTypeBaseId { get; set; } = default!;
        public float Price { get; set; } = default!;
        public float? DepreciatedPrice { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
        public int? DocumentHeadId { get; set; } = default!;
        public int? DocumentItemId { get; set; } = default!;
        
        public string Code { get; set; }
        public string Title { get; set; } = default!;
    }
   

}
