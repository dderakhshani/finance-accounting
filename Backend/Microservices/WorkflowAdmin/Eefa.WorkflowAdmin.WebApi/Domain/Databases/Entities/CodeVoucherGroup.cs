using System;
using System.Collections.Generic;
using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    public partial class CodeVoucherGroup : BaseEntity
    {


        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// شناسه
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// تاریخ قفل شدن اطلاعات
        /// </summary>
        public DateTime? LastEditableDate { get; set; }

        /// <summary>
        /// اتوماتیک است؟
        /// </summary>
        public bool IsAuto { get; set; } = default!;

        /// <summary>
        /// آیا قابل ویرایش است؟
        /// </summary>
        public bool IsEditable { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;

        /// <summary>
        /// گروه سند مکانیزه
        /// </summary>
        public bool AutoVoucherEnterGroup { get; set; } = default!;

        /// <summary>
        /// فرمول جایگزین خالی بودن
        /// </summary>
        public string? BlankDateFormula { get; set; }

        /// <summary>
        /// کد گزارش
        /// </summary>
        public int? ViewId { get; set; }
        public int? ExtendTypeId { get; set; }
        public string? ExtendTypeName { get; set; }

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
        

        public virtual CompanyInformation Company { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual AutoVoucherIncompleteVoucher? AutoVoucherIncompleteVoucher { get; set; } = default!;
        public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulas { get; set; } = default!;
        public virtual ICollection<AutoVoucherLog> AutoVoucherLogs { get; set; } = default!;
        public virtual ICollection<AutoVoucherRowsLink> AutoVoucherRowsLinks { get; set; } = default!;
        public virtual ICollection<VouchersHead> VouchersHeads { get; set; } = default!;
    }
}
