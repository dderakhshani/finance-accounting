
using Eefa.Common;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using System;
namespace Eefa.Inventory.Application
{


    public class WarehouseHistoriesDocumentViewModel : IMapFrom<WarehouseHistoriesDocumentView>
    {
        public int? Id { get; set; } = default!;
        public int? Commodityld { get; set; } = default!;
        public int? WarehouseId { get; set; } = default!;
        public int? WarehouseLayoutId { get; set; } = default!;
        public Nullable<int> SerialNumber { get; set; }
        public double? Quantity { get; set; } = default!;
        public int? Mode { get; set; } = default!;
        public Nullable<int> DocumentItemId { get; set; } = default!;
        public string RequestNo { get; set; } = default!;
        public string RequestNoPurchaseForExit { get; set; } = default!;
        public string CommodityCode { get; set; } = default!;
        public string MeasureTitle { get; set; } = default!;
        public string CommodityTitle { get; set; } = default!;

        public System.DateTime DocumentDate { get; set; } = default!;
        public System.DateTime CreatedDate { get; set; } = default!;
        public Nullable<System.DateTime> RequestDate { get; set; } = default!;
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string DocumentNo { get; set; } = default!;
        public string WarehouseLayoutTitle { get; set; } = default!;
        public string WarehousesTitle { get; set; } = default!;
        public string ModeTitle { get; set; } = default!;
        public string CreditReferenceTitle { get; set; } = default!;
        public string DebitReferenceTitle { get; set; } = default!;
        public string CodeVoucherGroupTitle { get; set; } = default!;
        public string RequesterReferenceTitle { get; set; } = default!;
        public int? DocumentHeadId { get; set; } = default!;
        public int? CreditAccountReferenceId { get; set; } = default!;
        public int? DebitAccountReferenceId { get; set; } = default!;
        public int? CreditAccountHeadId { get; set; }
        public int? DebitAccountHeadId { get; set; }
        public double? ItemUnitPrice { get; set; } = default!;
        public double? TotalProductionCost { get; set; } = default!;
        public long? ExtraCost { get; set; } = default!;
        public string InvoiceNo { get; set; } = default!;
        public double? TotalQuantity { get; set; }
        public string DocumentStateBaseTitle { get; set; }
        public int? DocumentStateBaseId { get; set; }
        public int? DocumentStauseBaseValue { get; set; }
        public bool? IsImportPurchase { get; set; }
        public bool? IsPlacementComplete { get; set; }
        public int? VoucherNo { get; set; }
        public int? VoucherHeadId { get; set; }

        public int? documentId { get; set; }

        public int? ViewId { get; set; }
        public int? CodeVoucherGroupId { get; set; }
        public string ScaleBill { get; set; }
        public string DocumentDescription { get; set; } = default!;
        public string Tags { get; set; }
        public bool IsRead { get; set;}
        public string PartNumber { get; set; }
        public string DescriptionItems { get; set; }

    }
}
