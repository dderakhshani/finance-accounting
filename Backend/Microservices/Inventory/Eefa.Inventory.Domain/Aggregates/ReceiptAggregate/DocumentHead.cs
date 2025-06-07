using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Eefa.Common.Data;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// DataBase Table: DocumentHead
    /// </summary>
    [Table("DocumentHeads", Schema = "common")]
    public partial class Receipt : DomainBaseEntity, IAggregateRoot
    {
        public int CodeVoucherGroupId { get; set; }
        public int WarehouseId { get; set; }
        public Nullable<int> SerialNumber { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<int> CreditAccountHeadId { get; set; }
        public Nullable<int> CreditAccountReferenceGroupId { get; set; }
        public Nullable<int> CreditAccountReferenceId { get; set; }
        public Nullable<int> DebitAccountHeadId { get; set; }
        public Nullable<int> DebitAccountReferenceId { get; set; }
        public Nullable<int> DebitAccountReferenceGroupId { get; set; }
        public Nullable<int> VoucherHeadId { get; set; }
        public Nullable<int> DocumentId { get; set; }
        public Nullable<int> DocumentNo { get; set; }
        public string DocumentSerial { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string RequestNo { get; set; }
        public string Tags { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public string DocumentDescription { get; set; }
        public Nullable<int> DocumentStateBaseId { get; set; }
        public Nullable<int> DocumentStauseBaseValue { get; set; }
        public string PartNumber { get; set; }
        public string PrintNumber { get; set; }
        public bool IsManual { get; set; }
        public double TotalWeight { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalItemPrice { get; set; }
        public long VatTax { get; set; }
        public long VatDutiesTax { get; set; }
        public Nullable<int> VatPercentage { get; set; }
        public long HealthTax { get; set; }
        public int YearId { get; set; }
        public long TotalItemsDiscount { get; set; }
        public Nullable<double> TotalProductionCost { get; set; }
        public Nullable<double> DiscountPercent { get; set; }
        public Nullable<double> DocumentDiscount { get; set; }
        public Nullable<double> PriceMinusDiscount { get; set; }
        public Nullable<double> PriceMinusDiscountPlusTax { get; set; }
        public int PaymentTypeBaseId { get; set; }
        public Nullable<bool> IsPlacementComplete { get; set; }
        public Nullable<bool> IsImportPurchase { get; set; }
        public Nullable<bool> IsFreightChargePaid { get; set; }
        public Nullable<bool> IsDocumentIssuance { get; set; }
 
        public string CommandDescription { get; set; }
        public Nullable<int> ViewId { get; set; }
        public string FinancialOperationNumber { get; set; }
        public Nullable<long> ExtraCost { get; set; }
        public Nullable<double> ExtraCostCurrency { get; set; }
        public string ScaleBill { get; set; }
        
        
        public Nullable<int> ExtraCostAccountHeadId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceGroupId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceId { get; set; }

        public virtual ICollection<DocumentItem> Items { get; set; } = default!;
        


        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }

        public DocumentItem AddItem(DocumentItem documentItem)
        {
            documentItem.YearId = YearId;
           
            Items ??= new List<DocumentItem>();

            this.Items.Add(documentItem);
            return documentItem;
        }
       
       
        

    }


}
