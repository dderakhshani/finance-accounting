using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1587;&#1575;&#1604; &#1607;&#1575;&#1740; &#1605;&#1575;&#1604;&#1740;
    /// </summary>
    public partial class Years : BaseEntity
    {
        public Years()
        {
            Commodities = new HashSet<Commodities>();
            DocumentItems = new HashSet<DocumentItems>();
            FinancialRequests = new HashSet<FinancialRequest>();
            UserYears = new HashSet<UserYear>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
//نام سال
        /// </summary>
        public int YearName { get; set; } = default!;

        /// <summary>
//تاریخ شروع
        /// </summary>
        public DateTime FirstDate { get; set; } = default!;

        /// <summary>
//تاریخ پایان
        /// </summary>
        public DateTime LastDate { get; set; } = default!;

        /// <summary>
//آیا قابل ویرایش است؟
        /// </summary>
        public bool? IsEditable { get; set; } = default!;

        /// <summary>
//قابل شمارش است؟
        /// </summary>
        public bool IsCalculable { get; set; } = default!;

        /// <summary>
//آیا تاریخ در سال جاری است؟
        /// </summary>
        public bool IsCurrentYear { get; set; } = default!;

        /// <summary>
//ایجاد سال مالی بدون سند افتتاحیه
        /// </summary>
        public bool CreateWithoutStartVoucher { get; set; } = default!;

        /// <summary>
//تاریخ قفل شدن اطلاعات
        /// </summary>
        public DateTime? LastEditableDate { get; set; }

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
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual ICollection<Commodities> Commodities { get; set; } = default!;
        public virtual ICollection<DocumentItems> DocumentItems { get; set; } = default!;
        public virtual ICollection<FinancialRequest> FinancialRequests { get; set; } = default!;
        public virtual ICollection<UserYear> UserYears { get; set; } = default!;
    }
}
