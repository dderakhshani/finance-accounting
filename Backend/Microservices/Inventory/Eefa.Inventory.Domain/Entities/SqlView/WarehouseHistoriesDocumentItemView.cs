using System;

namespace Eefa.Inventory.Domain
{

    public partial class WarehouseHistoriesDocumentItemView
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public Nullable<int> SerialNumber { get; set; }
        public int Commodityld { get; set; }
        public int WarehouseLayoutId { get; set; }
        public double Quantity { get; set; }
        public Nullable<double> TotalWeight { get; set; }

        public int Mode { get; set; }
        public Nullable<int> DocumentItemId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> WarehouseId { get; set; }
        public Nullable<double> AVGPrice { get; set; }

       

        
        public string CommodityCode { get; set; }
        public string CompactCode { get; set; }
        public string MeasureTitle { get; set; }
        public string CommodityTitle { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public Nullable<int> DocumentId { get; set; }
        public double ItemUnitPrice { get; set; }
        public Nullable<double> UnitPriceWithExtraCost { get; set; }
        public string CodeVoucherGroupTitle { get; set; }
        public int CodeVoucherGroupId { get; set; }
        public int YearId { get; set; }
        public Nullable<int> DocumentNo { get; set; }
        public Nullable<int> DocumentStauseBaseValue { get; set; }
        public System.DateTime CreatedAt { get; set; }

    }



}
