using System;

namespace Eefa.Inventory.Domain
{

    public class SpTejaratReportImportCommodity
    {
        public string WarehousePostalCode { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public string DocumentType { get; set; }
        public string CreditReferenceTitle { get; set; }
        public string CodeVoucherGroupTitle { get; set; }
        public double Quantity { get; set; }
        public Nullable<int> DocumentNo { get; set; }
        public string PhoneNumber { get; set; }
        public string EconomicCode { get; set; }
        public string RequestNo { get; set; }
        public long ItemUnitPrice { get; set; }
        public string cotajCode { get; set; }
        public string CommodityNationalId { get; set; }
        public string CurrencyStatus { get; set; }
    }
}
