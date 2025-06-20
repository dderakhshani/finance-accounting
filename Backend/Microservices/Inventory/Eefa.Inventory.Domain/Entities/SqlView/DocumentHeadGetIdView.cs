﻿using System;

namespace Eefa.Inventory.Domain
{

    public partial class DocumentHeadGetIdView
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public Nullable<int> WarehouseId { get; set; }
        public Nullable<int> SerialNumber { get; set; }
        public string WarehouseTitle { get; set; }
        public Nullable<int> DebitAccountReferenceId { get; set; }
        public string DebitReferenceTitle { get; set; }
        public Nullable<int> CreditAccountReferenceId { get; set; }
        public string CreditReferenceTitle { get; set; }
        public Nullable<int> CodeVoucherGroupId { get; set; }
        public string CodeVoucherGroupTitle { get; set; }
        public Nullable<int> DefultCreditAccountHeadId { get; set; }
        public Nullable<int> DefultDebitAccountHeadId { get; set; }
        public Nullable<int> ViewId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string DocumentDescription { get; set; }
        public Nullable<int> DocumentNo { get; set; }
        public string DocumentSerial { get; set; }
        public Nullable<int> DocumentStateBaseId { get; set; }
        public string DocumentStateBaseTitle { get; set; }
        public bool IsPlacementComplete { get; set; }
        public string RequestNo { get; set; }
        public System.DateTime ModifiedAt { get; set; }
        public int CreatedById { get; set; }
        public int YearId { get; set; }
        public string Tags { get; set; }
        public string InvoiceNo { get; set; }
        public double TotalItemPrice { get; set; }
        public Nullable<double> TotalProductionCost { get; set; }
        public long VatDutiesTax { get; set; }
        public int VoucherHeadId { get; set; }
        public bool IsImportPurchase { get; set; }
        public string FinancialOperationNumber { get; set; }
        public Nullable<int> DocumentStauseBaseValue { get; set; }
        public Nullable<int> CreditAccountHeadId { get; set; }
        public Nullable<int> CreditAccountReferenceGroupId { get; set; }
        public Nullable<int> DebitAccountHeadId { get; set; }
        public Nullable<int> DebitAccountReferenceGroupId { get; set; }
        public Nullable<long> ExtraCost { get; set; }
        public string DocumentStauseBaseValueTitle { get; set; }
        public string RolesLevelCode { get; set; }
        public Nullable<int> VoucherNo { get; set; }
        public string CodeVoucherGroupsCode { get; set; }
        public string DebitAccountHeadTitle { get; set; }
        public string CreditAccountHeadTitle { get; set; }
        public string DebitAccountReferenceGroupTitle { get; set; }
        public string CreditAccountReferenceGroupTitle { get; set; }
        public string Username { get; set; }
        
        public Nullable<int> YearName { get; set; }
        public Nullable<int> DocumentId { get; set; }

        public Nullable<int> ExtraCostAccountHeadId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceGroupId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceId { get; set; }

        public Nullable<double> ExtraCostCurrency { get; set; }
        public string ScaleBill { get; set; } = default!;

        public Nullable<bool> IsFreightChargePaid { get; set; }

        public Nullable<bool> IsDocumentIssuance { get; set; }

    }



}
