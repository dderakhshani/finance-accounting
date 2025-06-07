using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1575;&#1587;&#1606;&#1575;&#1583;
    /// </summary>
    public partial class DocumentHeads : BaseEntity
    {
        public DocumentHeads()
        {
            DocumentHeadExtends = new HashSet<DocumentHeadExtend>();
            DocumentItems = new HashSet<DocumentItems>();
            DocumentPayments = new HashSet<DocumentPayments>();
            FinancialRequestDocuments = new HashSet<FinancialRequestDocuments>();
            InverseParent = new HashSet<DocumentHeads>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد گروه سند 
        /// </summary>
        public int CodeVoucherGroupId { get; set; } = default!;

        /// <summary>
//کد انبار
        /// </summary>
        public int WarehouseId { get; set; } = default!;

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//آیدی سرفصل حساب بستانکار
        /// </summary>
        public int? CreditAccountHeadId { get; set; }

        /// <summary>
//آیدی گروه حساب بستانکار
        /// </summary>
        public int? CreditAccountReferenceGroupId { get; set; }

        /// <summary>
// آیدی حساب بستانکار
        /// </summary>
        public int? CreditAccountReferenceId { get; set; }

        /// <summary>
//آیدی سرفصل حساب بدهکار
        /// </summary>
        public int? DebitAccountHeadId { get; set; }

        /// <summary>
//آیدی حساب بدهکار
        /// </summary>
        public int? DebitAccountReferenceId { get; set; }

        /// <summary>
//آیدی گروه حساب بدهکار
        /// </summary>
        public int? DebitAccountReferenceGroupId { get; set; }

        /// <summary>
//کد سند حسابداری
        /// </summary>
        public int? VoucherHeadId { get; set; }

        /// <summary>
//شماره فرم عملیات مالی 
        /// </summary>
        public int DocumentId { get; set; } = default!;

        /// <summary>
//شماره سند
        /// </summary>
        public int? DocumentNo { get; set; }

        /// <summary>
//شماره سریال سند - کد سال +کد شعبه +کد سیستم 
        /// </summary>
        public string? DocumentSerial { get; set; }

        /// <summary>
//شماره فاکتور فروشنده
        /// </summary>
        public string? InvoiceNo { get; set; }

        /// <summary>
//شماره درخواست 
        /// </summary>
        public string? RequestNo { get; set; }

        /// <summary>
//برچسبها
        /// </summary>
        public string? Tags { get; set; }

        /// <summary>
//تاریخ سند
        /// </summary>
        public DateTime DocumentDate { get; set; } = default!;

        /// <summary>
//تاریخ درخواست 
        /// </summary>
        public DateTime? RequestDate { get; set; }

        /// <summary>
//تاریخ انقضا
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
//توضیحات سند
        /// </summary>
        public string? DocumentDescription { get; set; }

        /// <summary>
//کد وضعیت کالا در انبار ، مثل نیاز به تست
        /// </summary>
        public int? DocumentStateBaseId { get; set; }

        /// <summary>
//وضعیت سند در فرآیند گردش ، مرحله سند
        /// </summary>
        public int? DocumentStauseBaseValue { get; set; }

        /// <summary>
//شماره بخش
        /// </summary>
        public string? PartNumber { get; set; }

        /// <summary>
//دستی
        /// </summary>
        public bool IsManual { get; set; } = default!;

        /// <summary>
//وزن اقلام سند
        /// </summary>
        public double TotalWeight { get; set; } = default!;

        /// <summary>
//جمع تعداد اقلام سند
        /// </summary>
        public double TotalQuantity { get; set; } = default!;

        /// <summary>
//جمع مبلغ قابل پرداخت
        /// </summary>
        public long TotalItemPrice { get; set; } = default!;

        /// <summary>
//عوارض ارزش افزوده
        /// </summary>
        public long VatTax { get; set; } = default!;

        /// <summary>
//مالیات ارزش افزوده
        /// </summary>
        public long VatDutiesTax { get; set; } = default!;

        /// <summary>
//درصد مالیات بر ارزش افزوده 
        /// </summary>
        public int? VatPercentage { get; set; }

        /// <summary>
//عوارض سلامت
        /// </summary>
        public long HealthTax { get; set; } = default!;

        /// <summary>
//کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

        /// <summary>
//جمع تخفیف
        /// </summary>
        public long TotalItemsDiscount { get; set; } = default!;

        /// <summary>
//جمع قیمت تمام شده
        /// </summary>
        public long? TotalProductionCost { get; set; }

        /// <summary>
//درصد تخفیف کل فاکتور
        /// </summary>
        public double? DiscountPercent { get; set; }

        /// <summary>
//تخفیف کل سند
        /// </summary>
        public double? DocumentDiscount { get; set; }

        /// <summary>
//قیمت بعد از کسر تخفیف
        /// </summary>
        public double? PriceMinusDiscount { get; set; }

        /// <summary>
//قیمت با مالیات بعد از کسر تخفیف -مبلغ قابل پرداخت 
        /// </summary>
        public double? PriceMinusDiscountPlusTax { get; set; }

        /// <summary>
//نوع پرداخت
        /// </summary>
        public int PaymentTypeBaseId { get; set; } = default!;

        /// <summary>
//آیا جایگذاری تکمیل شده است 
        /// </summary>
        public bool? IsPlacementComplete { get; set; }

        /// <summary>
//صورتحساب داخلی یا خارجی 
        /// </summary>
        public bool? IsImportPurchase { get; set; }

        /// <summary>
//توضیحات برنامه نویس 
        /// </summary>
        public string? CommandDescription { get; set; }

        /// <summary>
//لیست انواع سند 
        /// </summary>
        public int? ViewId { get; set; }

        /// <summary>
//شماره فرم عملیات مالی
        /// </summary>
        public string? FinancialOperationNumber { get; set; }

        /// <summary>
//هزینه اضافه 
        /// </summary>
        public long? ExtraCost { get; set; }

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
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual AccountHead CreditAccountHead { get; set; } = default!;
        public virtual AccountReferences CreditAccountReference { get; set; } = default!;
        public virtual AccountReferencesGroups CreditAccountReferenceGroup { get; set; } = default!;
        public virtual AccountHead DebitAccountHead { get; set; } = default!;
        public virtual AccountReferences DebitAccountReference { get; set; } = default!;
        public virtual AccountReferencesGroups DebitAccountReferenceGroup { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual DocumentHeads Parent { get; set; } = default!;
        public virtual ICollection<DocumentHeadExtend> DocumentHeadExtends { get; set; } = default!;
        public virtual ICollection<DocumentItems> DocumentItems { get; set; } = default!;
        public virtual ICollection<DocumentPayments> DocumentPayments { get; set; } = default!;
        public virtual ICollection<FinancialRequestDocuments> FinancialRequestDocuments { get; set; } = default!;
        public virtual ICollection<DocumentHeads> InverseParent { get; set; } = default!;
    }
}
