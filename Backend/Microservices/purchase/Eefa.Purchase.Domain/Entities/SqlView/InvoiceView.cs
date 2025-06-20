﻿using System;

namespace Eefa.Purchase.Domain.Entities.SqlView
{

    public partial class InvoiceView 
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int? CodeVoucherGroupId { get; set; } = default!;
        public string CodeVoucherGroupTitle { get; set; } = default!;
        public string InvoiceNo { get; set; } = default!;

        public string CommodityTitle { get; set; } = default!;
        public string CommodityCode { get; set; } = default!;
        public double? Quantity { get; set; } = default!;
        public double? RemainQuantity { get; set; } = default!;
        public int? ItemsCount { get; set; } = default!;
        public int? VoucherHeadId { get; set; } = default!;
        public string FinancialOperationNumber { get; set; } = default!;

        /// <description>
        /// کد سال
        ///</description>
        string DocumentSerial { get; set; }

        public int? YearId { get; set; } = default!;
        /// <description>
        /// کد انبار
        ///</description>

        public int? WarehouseId { get; set; } = default!;

        public string WarehouseTitle { get; set; } = default!;

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

        /// <description>
        /// توضیحات سند
        ///</description>

        public string DocumentDescription { get; set; }

        /// <description>
        /// تاریخ انقضا
        ///</description>

        public DateTime? ExpireDate { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = default!;

        /// <description>
        /// کد وضعیت سند
        ///</description>

        public int DocumentStateBaseId { get; set; } = default!;
        public string DocumentStateBaseTitle { get; set; } = default!;

        public string RequestNo { get; set; } = default!;

        public bool? IsPlacementComplete { get; set; } = default!;


        public string RequesterReferenceTitle { get; set; } = default!;

        public string FollowUpReferenceTitle { get; set; } = default!;

        public string CorroborantReferenceTitle { get; set; } = default!;


        public int? RequesterReferenceId { get; set; } = default!;

        public int? FollowUpReferenceId { get; set; } = default!;
        public int? CorroborantReferenceId { get; set; } = default!;
        public string Tags { get; set; } = default!;
        public double? TotalItemPrice { get; set; } = default!;
        public double? TotalProductionCost { get; set; } = default!;
        public long? VatDutiesTax { get; set; } = default!;
        public bool? IsImportPurchase { get; set; } = default!;
        public int? ViewId { get; set; } = default!;
        public int? DocumentStauseBaseValue { get; set; } = default!;
        //----------------تعداد شماره سریال اموال وارد شده
        public int? AssetsCount { get; set; } = default!;


    }



}

