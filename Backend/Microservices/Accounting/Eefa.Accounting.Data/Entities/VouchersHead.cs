using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Drawing;
using Eefa.Accounting.Data.Events.VoucherHead;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
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
        public int VoucherDailyId { get; set; } = default!;
        public int? TraceNumber { get; set; } = null;
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
        public DateTime VoucherDate { get; set; }

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
        public int VoucherStateId { get; set; } = default!;

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
        public double? TotalDebit { get; set; }

        /// <summary>
        /// جمع بستانکاری
        /// </summary>
        public double? TotalCredit { get; set; }

        /// <summary>
        /// اختلاف
        /// </summary>
        public double? Difference { get; set; }

        public virtual CodeVoucherGroup CodeVoucherGroup { get; set; } = default!;
        public virtual CompanyInformation Company { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Year Year { get; set; }
        public virtual ICollection<VoucherAttachment> VoucherAttachments { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = new List<VouchersDetail>();
        public virtual ICollection<DocumentHead> DocumentHeads { get; set; } = default!;



        public void Apply(CreateVoucherHeadEvent @event)
        {
            VoucherDate = @event.VoucherDate;
            YearId = @event.YearId;
            CompanyId = @event.CompanyId;
            VoucherNo = @event.VoucherNo;
            VoucherDailyId = @event.VoucherDailyId;
            TotalCredit = @event.TotalCredit;
            TotalDebit = @event.TotalDebit;
            VoucherDescription = @event.VoucherDescription;
            CodeVoucherGroupId = @event.CodeVoucherGroupId;
            VoucherStateId = @event.VoucherStateId;
        }
        public VouchersDetail Apply(CreateVoucherDetailEvent @event)
        {
            var voucherDetail = new VouchersDetail
            {
                VoucherDate = @event.VoucherDate,
                AccountHeadId = @event.AccountHeadId,
                AccountReferencesGroupId = @event.AccountReferencesGroupId,
                ReferenceId1 = @event.ReferenceId1,
                VoucherRowDescription = @event.VoucherRowDescription,
                Credit = @event.Credit,
                Debit = @event.Debit,
                RowIndex = @event.RowIndex,
                Level1 = @event.Level1,
                Level2 = @event.Level2,
                Level3 = @event.Level3,
                CurrencyTypeBaseId = @event.CurrencyTypeBaseId,
                CurrencyFee = @event.CurrencyFee,
                CurrencyAmount = @event.CurrencyAmount,
            };

            this.VouchersDetails.Add(voucherDetail);

            return voucherDetail;
        }
    }
}
