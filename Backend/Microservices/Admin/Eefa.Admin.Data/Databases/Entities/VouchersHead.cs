using System;
using System.Collections.Generic;
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class VouchersHead : BaseEntity
    {


        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// کد سند
        /// </summary>

        /// <summary>
        /// کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

        /// <summary>
        /// شماره سند
        /// </summary>
        public int VoucherNo { get; set; } = default!;

        /// <summary>
        /// تاریخ سند
        /// </summary>
        public DateTime VoucherDate { get; set; } = default!;

        /// <summary>
        /// شرح سند
        /// </summary>
        public string VoucherDescription { get; set; } = default!;

        /// <summary>
        /// کد گروه سند
        /// </summary>
        public int CodeVoucherGroupId { get; set; } = default!;

        /// <summary>
        /// کد وضعیت سند
        /// </summary>
        public byte VoucherStateId { get; set; } = default!;

        /// <summary>
        /// نام وضعیت سند
        /// </summary>
        public string? VoucherStateName { get; set; }

        /// <summary>
        /// گروه سند مکانیزه
        /// </summary>
        public int? AutoVoucherEnterGroup { get; set; }

        /// <summary>
        /// جمع بدهی
        /// </summary>
        public long? TotalDebit { get; set; }

        /// <summary>
        /// جمع بستانکاری
        /// </summary>
        public long? TotalCredit { get; set; }

        /// <summary>
        /// اختلاف
        /// </summary>
        public long? Difference { get; set; }

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
        

        public virtual CodeVoucherGroup CodeVoucherGroup { get; set; } = default!;
        public virtual CompanyInformation Company { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<VoucherAttachment> VoucherAttachments { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
    }
}
