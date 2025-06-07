using AutoMapper;
using Eefa.Inventory.Domain.Common;
using System;
using System.Text.Json.Serialization;

namespace Eefa.Inventory.Domain
{

    public partial class WarehouseHistoriesDocumentView
    {
        
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public int? Commodityld { get; set; } = default!;
        public int? WarehouseId { get; set; } = default!;
        public int? WarehouseLayoutId { get; set; } = default!;

        public Nullable<int> SerialNumber { get; set; }
        public double? Quantity { get; set; }
        public int? Mode { get; set; } = default!;
        public Nullable<int> DocumentItemId { get; set; }
        public string RequestNo { get; set; }
        public string RequestNoPurchaseForExit { get; set; } = default!;
        public string InvoiceNo { get; set; }
        public string CommodityCode { get; set; }
        public string MeasureTitle { get; set; }
        public string CommodityTitle { get; set; }

        public System.DateTime DocumentDate { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        
        public System.DateTime CreatedDate { get; set; }

        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<int> DocumentNo { get; set; }
        public string WarehouseLayoutTitle { get; set; }
        public string WarehousesTitle { get; set; }
        public string ModeTitle { get; set; }
        public string CreditReferenceTitle { get; set; }
        public string DebitReferenceTitle { get; set; }
        public string CodeVoucherGroupTitle { get; set; }
        public int? DocumentHeadId { get; set; }
        public int? CreditAccountReferenceId { get; set; }
        public int? DebitAccountReferenceId { get; set; }
        public int? CreditAccountHeadId { get; set; }
        public int? DebitAccountHeadId { get; set; }
        public double? ItemUnitPrice { get; set; }
        public double? TotalProductionCost { get; set; }

        public long? ExtraCost { get; set; }
        public string DocumentStateBaseTitle { get; set; }

        public string ScaleBill { get; set; }
        public int? DocumentStateBaseId { get; set; }
        public int? DocumentStauseBaseValue { get; set; }
        public bool? IsImportPurchase { get; set; }
        public bool? IsPlacementComplete { get; set; }
        public int? VoucherNo { get; set; }
        public int? VoucherHeadId { get; set; }
        public int? documentId { get; set; }
        public int? ViewId { get; set; }
        public string DocumentDescription { get; set; } = default!;
        public int? CodeVoucherGroupId { get; set; }
        public string Tags { get; set; }
        public bool IsRead { get; set; }
        public string PartNumber { get; set; }
        public string DescriptionItems { get; set; }
        public Nullable<bool> IsDocumentIssuance { get; set; }

    }



}
