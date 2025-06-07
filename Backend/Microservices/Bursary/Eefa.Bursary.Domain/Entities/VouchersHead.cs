using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1587;&#1606;&#1583; &#1581;&#1587;&#1575;&#1576;&#1583;&#1575;&#1585;&#1740;
    /// </summary>
    public partial class VouchersHead : BaseEntity
    {
        public VouchersHead()
        {
            FinancialRequests = new HashSet<FinancialRequest>();
            VoucherAttachments = new HashSet<VoucherAttachments>();
            VouchersDetails = new HashSet<VouchersDetail>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
//کد سند
        /// </summary>
        public int VoucherDailyId { get; set; } = default!;

        /// <summary>
//شماره سند مرتبط
        /// </summary>
        public int? TraceNumber { get; set; }

        /// <summary>
//کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

        /// <summary>
//شماره سند
        /// </summary>
        public int VoucherNo { get; set; } = default!;

        /// <summary>
//تاریخ سند
        /// </summary>
        public DateTime VoucherDate { get; set; } = default!;

        /// <summary>
//شرح سند
        /// </summary>
        public string VoucherDescription { get; set; } = default!;

        /// <summary>
//کد گروه سند
        /// </summary>
        public int CodeVoucherGroupId { get; set; } = default!;

        /// <summary>
//کد وضعیت سند
        /// </summary>
        public int VoucherStateId { get; set; } = default!;

        /// <summary>
//نام وضعیت سند
        /// </summary>
        public string? VoucherStateName { get; set; }

        /// <summary>
//گروه سند مکانیزه
        /// </summary>
        public int? AutoVoucherEnterGroup { get; set; }

        /// <summary>
//جمع بدهی
        /// </summary>
        public double? TotalDebit { get; set; }

        /// <summary>
//جمع بستانکاری
        /// </summary>
        public double? TotalCredit { get; set; }

        /// <summary>
//اختلاف
        /// </summary>
        public double Difference { get; set; }

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
         

        public virtual CodeVoucherGroups CodeVoucherGroup { get; set; } = default!;
        public virtual CompanyInformations Company { get; set; } = default!;
        public virtual ICollection<FinancialRequest> FinancialRequests { get; set; } = default!;
        public virtual ICollection<VoucherAttachments> VoucherAttachments { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
    }
}
