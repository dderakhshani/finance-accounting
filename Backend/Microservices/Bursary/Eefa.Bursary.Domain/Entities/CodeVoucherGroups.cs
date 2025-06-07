using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1711;&#1585;&#1608;&#1607;&#1607;&#1575;&#1740;  &#1575;&#1587;&#1606;&#1575;&#1583; &#1581;&#1587;&#1575;&#1576;&#1583;&#1575;&#1585;&#1740;
    /// </summary>
    public partial class CodeVoucherGroups : BaseEntity
    {
        public CodeVoucherGroups()
        {
            AutoVoucherFormulas = new HashSet<AutoVoucherFormula>();
            AutoVoucherLogs = new HashSet<AutoVoucherLog>();
            AutoVoucherRowsLinks = new HashSet<AutoVoucherRowsLink>();
            DocumentHeads = new HashSet<DocumentHeads>();
            FinancialRequests = new HashSet<FinancialRequest>();
            VouchersHeads = new HashSet<VouchersHead>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
//کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;
        public int? DefultDebitAccountHeadId { get; set; }
        public int? DefultCreditAccountHeadId { get; set; }

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
//تاریخ قفل شدن اطلاعات
        /// </summary>
        public DateTime? LastEditableDate { get; set; }

        /// <summary>
//اتوماتیک است؟
        /// </summary>
        public bool IsAuto { get; set; } = default!;

        /// <summary>
//آیا قابل ویرایش است؟
        /// </summary>
        public bool IsEditable { get; set; } = default!;

        /// <summary>
//فعال 
        /// </summary>
        public bool? IsActive { get; set; } = default!;

        /// <summary>
//گروه سند مکانیزه
        /// </summary>
        public bool AutoVoucherEnterGroup { get; set; } = default!;

        /// <summary>
//فرمول جایگزین خالی بودن
        /// </summary>
        public string? BlankDateFormula { get; set; }

        /// <summary>
//کد گزارش
        /// </summary>
        public int? ViewId { get; set; }

        /// <summary>
//نوع سند خاص 
        /// </summary>
        public int? ExtendTypeId { get; set; }

        /// <summary>
//نام سند خاص
        /// </summary>
        public string? ExtendTypeName { get; set; }

        /// <summary>
//ترتیب آرتیکل سند
        /// </summary>
        public int OrderIndex { get; set; } = default!;
        public string? TableName { get; set; }
        public string? SchemaName { get; set; }
        public int? MenuId { get; set; }

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
//اصلاح کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
//آیا حذف شده است؟
        /// </summary>
         

        public virtual CompanyInformations Company { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual AutoVoucherIncompleteVouchers AutoVoucherIncompleteVouchers { get; set; } = default!;
        public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulas { get; set; } = default!;
        public virtual ICollection<AutoVoucherLog> AutoVoucherLogs { get; set; } = default!;
        public virtual ICollection<AutoVoucherRowsLink> AutoVoucherRowsLinks { get; set; } = default!;
        public virtual ICollection<DocumentHeads> DocumentHeads { get; set; } = default!;
        public virtual ICollection<FinancialRequest> FinancialRequests { get; set; } = default!;
        public virtual ICollection<VouchersHead> VouchersHeads { get; set; } = default!;
    }
}
