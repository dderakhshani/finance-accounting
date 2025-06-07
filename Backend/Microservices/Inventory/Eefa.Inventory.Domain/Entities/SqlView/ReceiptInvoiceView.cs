using System;

namespace Eefa.Inventory.Domain
{

    public partial class ReceiptInvoiceView
    {

        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        /// <description>
        /// کد تامین کننده
        ///</description>

        public int? CreditAccountReferenceId { get; set; }
        public string CreditReferenceTitle { get; set; }
        public int? CodeVoucherGroupId { get; set; } = default!;
        public string CodeVoucherGroupTitle { get; set; } = default!;
        public string InvoiceNo { get; set; } = default!;

        /// <description>
        /// کد سال
        ///</description>
        
        public int? YearId { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = default!;
        
        /// <description>
        /// کد وضعیت سند
        ///</description>
        public bool? IsImportPurchase { get; set; } = default!;
        public double TotalItemPrice { get; set; } = default!;
    }



}
