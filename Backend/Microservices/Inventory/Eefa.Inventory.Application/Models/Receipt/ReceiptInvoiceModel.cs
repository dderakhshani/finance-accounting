using System;
using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{

    public partial class ReceiptInvoiceModel : IMapFrom<ReceiptInvoiceView>
    {

        public int Id { get; set; }
        public int? CreditAccountReferenceId { get; set; }
        public string CreditReferenceTitle { get; set; }
        public int? CodeVoucherGroupId { get; set; } = default!;
        public string CodeVoucherGroupTitle { get; set; } = default!;
        public string InvoiceNo { get; set; } = default!;
        public int? YearId { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = default!;
        public bool? IsImportPurchase { get; set; } = default!;
        public long? TotalItemPrice { get; set; } = default!;
    }



}
