using System;

namespace Eefa.Inventory.Domain
{

    public partial class AssetsView
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string WarehousesTitle { get; set; }
        public string AssetSerial { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public double Price { get; set; }
        public Nullable<int> DepreciationTypeBaseId { get; set; }
        public int AssetGroupId { get; set; }
        public Nullable<double> DepreciatedPrice { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int CommodityId { get; set; }
        public int MeasureId { get; set; }
        public int DocumentHeadId { get; set; }
        public int? DocumentItemId { get; set; } = default!;
        public string CommoditySerial { get; set; }
        public string CategoryTitle { get; set; }
        public string MeasureTitle { get; set; }
        public string SearchTerm { get; set; }
        public string CategoryLevelCode { get; set; }
        public string CommodityTitle { get; set; }


        public string UnitsTitle { get; set; }
        public string DepreciationTitle { get; set; }
        public string AssetGroupTitle { get; set; }
       
        public string CommodityCode { get; set; }
        public bool? IsHaveWast { get; set; } = default!;
        public bool? IsAsset { get; set; } = default!;
        public bool? IsConsumable { get; set; } = default!;
        public int? DocumentNo { get; set; } = default!;
        public string Description { get; set; } = default!;

        public string DocumentItemsDescription { get; set; } = default!;
        public string DocumentDescription { get; set; } = default!;

        public string TotalDescription { get; set; } = default!;

    }

 
}
