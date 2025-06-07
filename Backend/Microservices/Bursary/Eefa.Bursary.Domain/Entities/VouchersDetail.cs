using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1570;&#1585;&#1578;&#1740;&#1705;&#1604; &#1607;&#1575;&#1740; &#1587;&#1606;&#1583; &#1581;&#1587;&#1575;&#1576;&#1583;&#1575;&#1585;&#1740;
    /// </summary>
    public partial class VouchersDetail : BaseEntity
    {
        public VouchersDetail()
        {
            FinancialRequestDetails = new HashSet<FinancialRequestDetails>();
            VoucherDetailAttachments = new HashSet<VoucherDetailAttachments>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد سند
        /// </summary>
        public int VoucherId { get; set; } = default!;

        /// <summary>
//تاریخ سند
        /// </summary>
        public DateTime? VoucherDate { get; set; }

        /// <summary>
//سرفصل حساب 
        /// </summary>
        public int AccountHeadId { get; set; } = default!;
        public int? AccountReferencesGroupId { get; set; }

        /// <summary>
//شرح آرتیکل  سند
        /// </summary>
        public string VoucherRowDescription { get; set; } = default!;

        /// <summary>
//اعتبار
        /// </summary>
        public double Credit { get; set; } = default!;

        /// <summary>
//بدهکار
        /// </summary>
        public double Debit { get; set; } = default!;

        /// <summary>
//ترتیب سطر
        /// </summary>
        public int? RowIndex { get; set; }

        /// <summary>
//شماره سند مرتبط 
        /// </summary>
        public int? DocumentId { get; set; }

        /// <summary>
//شماره سند مرتبط 
        /// </summary>
        public string? DocumentNo { get; set; }

        /// <summary>
//لیست اسناد مرتبط
        /// </summary>
        public string? DocumentIds { get; set; }

        /// <summary>
//تاریخ مرجع
        /// </summary>
        public DateTime? ReferenceDate { get; set; }

        /// <summary>
//شماره فرم عملیات مالی
        /// </summary>
        public string? FinancialOperationNumber { get; set; }

        /// <summary>
//شماره درخواست 
        /// </summary>
        public string? RequestNo { get; set; }

        /// <summary>
//شماره فاکتور مشتری
        /// </summary>
        public string? InvoiceNo { get; set; }

        /// <summary>
//تگ
        /// </summary>
        public string? Tag { get; set; }

        /// <summary>
//مقدار مرجع
        /// </summary>
        public double? Weight { get; set; }

        /// <summary>
//کد مرجع1
        /// </summary>
        public int? ReferenceId1 { get; set; }

        /// <summary>
//کد مرجع2
        /// </summary>
        public int? ReferenceId2 { get; set; }

        /// <summary>
//کد مرجع3
        /// </summary>
        public int? ReferenceId3 { get; set; }

        /// <summary>
//سطح 1
        /// </summary>
        public int? Level1 { get; set; }

        /// <summary>
//سطح 2
        /// </summary>
        public int? Level2 { get; set; }

        /// <summary>
//سطح 3
        /// </summary>
        public int? Level3 { get; set; }

        /// <summary>
//وضعیت مانده حساب
        /// </summary>
        public byte? DebitCreditStatus { get; set; }

        /// <summary>
//نوع ارز
        /// </summary>
        public int? CurrencyTypeBaseId { get; set; }

        /// <summary>
//نرخ ارزبه ریال
        /// </summary>
        public double? CurrencyFee { get; set; }

        /// <summary>
//مبلغ ارز
        /// </summary>
        public double? CurrencyAmount { get; set; }

        /// <summary>
//ویژگی پیگیری دارد  
        /// </summary>
        public int? TraceNumber { get; set; }

        /// <summary>
//مقدار ویژگی تعداد 
        /// </summary>
        public double? Quantity { get; set; }

        /// <summary>
//باقیمانده
        /// </summary>
        public double Remain { get; set; }

        public int? ChequeSheetId { get; set; }
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


        public virtual AccountHead AccountHead { get; set; } = default!;
        public virtual AccountReferencesGroups AccountReferencesGroup { get; set; } = default!;
        public virtual BaseValues CurrencyTypeBase { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual AccountReferences ReferenceId1Navigation { get; set; } = default!;
        public virtual AccountReferences ReferenceId2Navigation { get; set; } = default!;
        public virtual AccountReferences ReferenceId3Navigation { get; set; } = default!;
        public virtual VouchersHead Voucher { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetails { get; set; } = default!;
        public virtual ICollection<VoucherDetailAttachments> VoucherDetailAttachments { get; set; } = default!;
    }
}
