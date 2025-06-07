using System;
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{

    public partial class AutoVoucherRowsLink : BaseEntity
    {
         
        public int? VoucherId { get; set; }
        public int? VoucherTypeId { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int? RowId { get; set; }

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
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual CodeVoucherGroup? VoucherType { get; set; } = default!;
    }
}
