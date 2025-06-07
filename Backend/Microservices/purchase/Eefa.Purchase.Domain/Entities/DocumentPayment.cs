using System;
using Eefa.Common.Domain;
using Eefa.Purchase.Domain.Aggregates.InvoiceAggregate;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// اسناد پرداختی
    /// </summary>
    public partial class DocumentPayment: DomainBaseEntity
    {
    
        public int DocumentHeadId { get; set; } = default!;
    /// <description>
            /// تاریخ سررسید
    ///</description>
    
        public DateTime? PaymentDate { get; set; }
    /// <description>
            /// تاریخ پرداخت
    ///</description>
    
        public DateTime? PaiedDate { get; set; }
    /// <description>
            /// پرداخت شده است
    ///</description>
    
        public string? IsPaied { get; set; }
    /// <description>
            /// نرخ شناور
    ///</description>
    
        public long? LiquidationPrice { get; set; }
    /// <description>
            /// موازنه
    ///</description>
    
        public long? Balance { get; set; }
    /// <description>
            /// شماره بارگیری
    ///</description>
    
        public int? LadingNo { get; set; }
    /// <description>
            /// نقش صاحب سند
    ///</description>
    

    public virtual Invoice DocumentHead { get; set; } = default!;
    }
}
