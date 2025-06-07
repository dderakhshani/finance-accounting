using System;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{

    public partial class AutoVoucherIncompleteVoucher : BaseEntity
    {
        public int? VoucherTypeId { get; set; }

        /// <summary>
        /// تاریخ سند
        /// </summary>
        public DateTime? VoucherDate { get; set; }

        /// <summary>
        /// کد سطر
        /// </summary>
        public int? RowId { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        public virtual User CreatedBy { get; set; } = default!;
        public virtual CodeVoucherGroup IdNavigation { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
