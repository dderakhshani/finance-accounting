using System;
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{

    public partial class AutoVoucherIncompleteVoucher : BaseEntity
    {

        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد نوع سند
        /// </summary>
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

        /// <summary>
        /// نقش صاحب سند
        /// </summary>
         

        /// <summary>
        /// ایجاد کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
        /// اصلاح کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
        /// آیا حذف شده است؟
        /// </summary>
        

        public virtual User CreatedBy { get; set; } = default!;
        public virtual CodeVoucherGroup IdNavigation { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
