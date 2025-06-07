using System;
using System.Collections.Generic;
using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{

    public record ReceiptQueryModel : IMapFrom<ReceiptView>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReceiptView, ReceiptQueryModel>();
            profile.CreateMap<ReceiptItemsView, ReceiptQueryModel>();
            profile.CreateMap<ReceiptGroupbyInvoiceView, ReceiptQueryModel>();
            profile.CreateMap<DocumentHeadGetIdView, ReceiptQueryModel>();
            profile.CreateMap<ReceiptWithCommodityView, ReceiptQueryModel>();
            
        }
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? DocumentId { get; set; } = default!;
        public int? SerialNumber { get; set; } = default!;
        
        public int? DebitAccountHeadId { get; set; } = default!;
        public int? DebitAccountReferenceId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; } = default!;
        public int? CreditAccountHeadId { get; set; } = default!;
        public int? CreditAccountReferenceId { get; set; } = default!;
        public int? CreditAccountReferenceGroupId { get; set; } = default!;

        public int? DefultCreditAccountHeadId { get; set; } = default!;

        public int? DefultDebitAccountHeadId { get; set; } = default!;
        public string CreditReferenceTitle { get; set; }
        public string DebitReferenceTitle { get; set; }

        public string DebitAccountReferencesGroupTitle { get; set; }

        public int? CodeVoucherGroupId { get; set; } = default!;
        
        public string CodeVoucherGroupTitle { get; set; } = default!;
       
        public string RolesLevelCode { get; set; } = default!;

        public string DocumentIds { get; set; } = default!;

        public string CommodityTitle { get; set; } = default!;
        public string CommodityCode { get; set; } = default!;
        public double? Quantity { get; set; } = default!;
        public double? RemainQuantity { get; set; } = default!;
        
        public int? ItemsCount { get; set; } = default!;
        public int? VoucherHeadId { get; set; } = default!;

        /// <description>
        /// کد سال
        ///</description>
        public string DocumentSerial { get; set; }

        public int? YearId { get; set; } = default!;
        /// <description>
        /// کد انبار
        ///</description>

        public int? WarehouseId { get; set; } = default!;

        public string WarehouseTitle { get; set; } = default!;
        public string PartNumber { get; set; }
        public string ReferenceNumber { get; set; }
        /// <description>
        /// شماره سند
        ///</description>

        public int? DocumentNo { get; set; }
        /// <description>
        /// تاریخ سند
        ///</description>

        public DateTime DocumentDate { get; set; } = default!;
        /// <summary>
        /// تاریخ درخواست
        /// </summary>
        public DateTime? RequestDate { get; set; } = default!;

        public DateTime? InvoiceDate { get; set; } = default!;
        /// <description>
        /// توضیحات سند
        ///</description>

        public string DocumentDescription { get; set; }

        /// <description>
        /// تاریخ انقضا
        ///</description>

        public DateTime? ExpireDate { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = default!;

        public int? CreatedById { get; set; } = default!;

        /// <description>
        /// کد وضعیت سند
        ///</description>

        public int? DocumentStateBaseId { get; set; } = default!;
        public string DocumentStateBaseTitle { get; set; } = default!;

        public string RequestNo { get; set; } = default!;

        public bool? IsPlacementComplete { get; set; } = default!;
        public string RequesterReferenceTitle { get; set; } = default!;

        public string FollowUpReferenceTitle { get; set; } = default!;

        public string CorroborantReferenceTitle { get; set; } = default!;


        public int? RequesterReferenceId { get; set; } = default!;

        public int? FollowUpReferenceId { get; set; } = default!;
        public int? CorroborantReferenceId { get; set; } = default!;
        public double? TotalItemPrice { get; set; } = default!;
        public double? TotalProductionCost { get; set; } = default!;
        public long? VatDutiesTax { get; set; } = default!;
        public int? VatPercentage { get; set; } = default!;

        public bool? IsManual { get; set; } = default!;
        
        

        /// <description>
        /// شماره فاکتور فروشنده
        ///</description>
        public string InvoiceNo { get; set; }
        public string[] TagArray { get; set; } = default!;
        
        public string Tags { get; set; } = default!;
        public bool? IsImportPurchase { get; set; } = default!;
        public string  ImportPurchaseTitle { get { return IsImportPurchase == true ? "وارداتی" : "داخلی"; } }
        public Nullable<bool> IsFreightChargePaid { get; set; }
        public Nullable<bool> IsDocumentIssuance { get; set; }
        public Nullable<bool> IsAllowedInputOrOutputCommodity { get; set; }
        public string DescriptionItem { get; set; } = default!;
        public int? VoucherNo { get; set; } = default!;
        public int? ViewId { get; set; } = default!;
        
        public int? DocumentStauseBaseValue { get; set; } = default!;
        public bool? Selected { get; set; } = false;
        public int? AssetsCount { get; set; } = default!;
        public string FinancialOperationNumber { get; set; }
        public long? ExtraCost { get; set; } = default!;
        public string DocumentStauseBaseValueTitle { get; set; } = default!;
        public string Username { get; set; } = default!;
        public int? PrintCount { get; set; } = default!;
        public double? CurrencyRate { get; set; } = default!;
        public double? CurrencyPrice { get; set; } = default!;
        public int? CurrencyBaseId { get; set; } = default!;
        public string CurrencyBaseTitle { get; set; } = default!;
        public string DebitAccountHeadTitle { get; set; } = default!;
        public string CreditAccountHeadTitle { get; set; } = default!;
        public string DebitAccountReferenceGroupTitle { get; set; } = default!;
        public string CreditAccountReferenceGroupTitle { get; set; } = default!;
        
        public string YearName { get; set; } = default!;
        public Nullable<int> ExtraCostAccountHeadId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceGroupId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceId { get; set; }
        public virtual List<ReceiptItemModel> Items { get; set; } = default!;
        public virtual List<CorrectionRequestModel> CorrectionRequest { get; set; } = default!;
        public List<TagClass> TagClass { get; set; } = default!;
        public Nullable<double> ExtraCostCurrency { get; set; } = default!;

        public string ScaleBill { get; set; } = default!;
        public Nullable<double> UnitPriceWithExtraCost { get; set; } = default!;

        
        public string MeasureTitle { get; set; } = default!;

    }
    public class TagClass
    {
        public string Key { get; set; } = default!;
        public int Value { get; set; }
    }


}
