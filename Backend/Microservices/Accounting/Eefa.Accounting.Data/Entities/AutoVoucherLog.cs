using System;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{

    public partial class AutoVoucherLog : BaseEntity
    {

        public DateTime? ActionDate { get; set; }

        /// <summary>
        /// کد نوع سند
        /// </summary>
        public int? VoucherTypeId { get; set; }

        /// <summary>
        /// تاریخ سند
        /// </summary>
        public DateTime? VoucherDate { get; set; }

        /// <summary>
        /// توضیحات سطر
        /// </summary>
        public string? RowDescription { get; set; }

        /// <summary>
        /// کد نهایی
        /// </summary>
        public byte? ResultId { get; set; }

        /// <summary>
        /// نام نهایی
        /// </summary>
        public string? ResultName { get; set; }


        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual CodeVoucherGroup? VoucherType { get; set; } = default!;
    }
}
