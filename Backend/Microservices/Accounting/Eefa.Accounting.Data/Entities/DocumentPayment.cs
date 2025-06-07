using System;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    public partial class DocumentPayment : BaseEntity
    {

        /// <summary>
        /// کد سرفصل سند
        /// </summary>
        public int DocumentHeadId { get; set; } = default!;

        /// <summary>
        /// تاریخ سررسید
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public DateTime? PaiedDate { get; set; }

        /// <summary>
        /// پرداخت شده است
        /// </summary>
        public string? IsPaied { get; set; }

        /// <summary>
        /// نرخ شناور
        /// </summary>
        public long? LiquidationPrice { get; set; }

        /// <summary>
        /// موازنه
        /// </summary>
        public long? Balance { get; set; }

        /// <summary>
        /// شماره بارگیری
        /// </summary>
        public int? LadingNo { get; set; }

      
        public virtual User CreatedBy { get; set; } = default!;
        public virtual DocumentHead DocumentHead { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
