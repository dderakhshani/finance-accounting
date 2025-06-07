using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Inventory.Domain;
using Newtonsoft.Json;


namespace Eefa.Inventory.Application
{
    public class ConvertToRailsReceipt
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("documentId")]
        public int DocumentId { get; set; }
        [JsonProperty("financialOperationNumber")]
        public string FinancialOperationNumber { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("debitAccountHeadId")]
        public int DebitAccountHeadId { get; set; } = default!;

        [JsonProperty("debitAccountReferenceId")]
        public int DebitAccountReferenceId { get; set; } = default!;

        [JsonProperty("debitAccountReferenceGroupId")]
        public int DebitAccountReferenceGroupId { get; set; } = default!;

        [JsonProperty("creditAccountHeadId")]
        public int CreditAccountHeadId { get; set; } = default!;

        [JsonProperty("creditAccountReferenceId")]
        public int CreditAccountReferenceId { get; set; } = default!;

        [JsonProperty("creditAccountReferenceGroupId")]
        public int CreditAccountReferenceGroupId { get; set; } = default!;

        [JsonProperty("vatDutiesTax")]
        public long? vatDutiesTax { get; set; } = default!;

        [JsonProperty("extraCost")]
        public long? ExtraCost { get; set; } = default!;

        [JsonProperty("extraCostCurrency")]
        public double? ExtraCostCurrency { get; set; } = default!;

        [JsonProperty("documentDescription")]
        public string documentDescription { get; set; }

        [JsonProperty("invoiceDate")]
        public DateTime? InvoiceDate { get; set; }

        [JsonProperty("id")]
        public Nullable<int> ExtraCostAccountHeadId { get; set; }

        [JsonProperty("extraCostAccountReferenceGroupId")]
        public Nullable<int> ExtraCostAccountReferenceGroupId { get; set; }

        [JsonProperty("extraCostAccountReferenceId")]
        public Nullable<int> ExtraCostAccountReferenceId { get; set; }
        
        [JsonProperty("receiptDocumentItems")]
        public ICollection<RialsReceiptDocumentItemCommand> ReceiptDocumentItems { get; set; }

        [JsonProperty("attachmentIds")]
        public List<int> AttachmentIds { get; set; } = default!;
        //--------------مورد نیاز در اصلاحیه سند-------------

        [JsonProperty("voucherHeadId")]
        public int? VoucherHeadId { get; set; }
     

    }
    public class RialsReceiptDocumentItemCommand : IMapFrom<DocumentItem>
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("commodityId")]
        public int CommodityId { get; set; } = default!;

        [JsonProperty("documentHeadId")]
        public int DocumentHeadId { get; set; } = default!;

        [JsonProperty("unitPrice")]
        public double UnitPrice { get; set; } = default!;

        [JsonProperty("currencyPrice")]
        public double CurrencyPrice { get; set; } = default!;
        [JsonProperty("currencyRate")]
        public double CurrencyRate { get; set; } = default!;

        [JsonProperty("currencyBaseId")]
        public int CurrencyBaseId { get; set; } = default!;

       

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("productionCost")]
        public double productionCost { get; set; } = default!;
        //--------------مورد نیاز در اصلاحیه سند-------------

        [JsonProperty("quantity")]
        public double Quantity { get; set; } = default!;
    }

}
