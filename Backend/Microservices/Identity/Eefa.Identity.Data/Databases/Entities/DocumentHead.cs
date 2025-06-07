using System;
using System.Collections.Generic;
using Library.Common;

namespace Eefa.Identity.Data.Databases.Entities
{
    public partial class DocumentHead : BaseEntity
    {


        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد نوع فرم
        /// </summary>
        public int FormTypeId { get; set; } = default!;

        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// کد فروشگاه
        /// </summary>
        public int StoreId { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد مرجع
        /// </summary>
        public int? ReferenceId { get; set; }

        /// <summary>
        /// شماره فرم
        /// </summary>
        public int? FormNo { get; set; }

        /// <summary>
        /// تاریخ فرم
        /// </summary>
        public DateTime FormDate { get; set; } = default!;

        /// <summary>
        /// توضیحات فرم
        /// </summary>
        public string? FormDescription { get; set; }

        /// <summary>
        /// کد وضعیت فرم
        /// </summary>
        public byte FormStateId { get; set; } = default!;

        /// <summary>
        /// آیا دستی است؟
        /// </summary>
        public bool IsManual { get; set; } = default!;

        /// <summary>
        /// کد سند
        /// </summary>
        public int? VocherId { get; set; }

        /// <summary>
        /// کد نوع حساب
        /// </summary>
        public int InvoiceTypeId { get; set; } = default!;

        /// <summary>
        /// نوع
        /// </summary>
        public int TypeId { get; set; } = default!;

        /// <summary>
        /// شرح پرداخت
        /// </summary>
        public string? PayDescription { get; set; }

        /// <summary>
        /// جمع کل سند
        /// </summary>
        public long TotalItemPrice { get; set; } = default!;

        /// <summary>
        /// جمع مالیات
        /// </summary>
        public long TotalTax { get; set; } = default!;

        /// <summary>
        /// جمع تخفیف
        /// </summary>
        public long TotalDiscount { get; set; } = default!;

        /// <summary>
        /// جمع ارزش افزوده
        /// </summary>
        public long? TotalVat { get; set; }

        /// <summary>
        /// قیمت بعد از کسر تخفیف
        /// </summary>
        public long? PriceMinusDiscount { get; set; }

        /// <summary>
        /// قیمت با مالیات بعد از کسر تخفیف 
        /// </summary>
        public long? PriceMinusDiscountPlusTax { get; set; }

        /// <summary>
        /// نوع پرداخت
        /// </summary>
        public int PaymentTypeId { get; set; } = default!;

        /// <summary>
        /// تاریخ سررسید
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public DateTime? PaiedDate { get; set; }

        /// <summary>
        /// آیا پرداخت شده است؟
        /// </summary>
        public int IsPaied { get; set; } = default!;

        /// <summary>
        /// نرخ شناور
        /// </summary>
        public long? LiquidationPrice { get; set; }

        /// <summary>
        /// موازنه
        /// </summary>
        public long? Balance { get; set; }

        /// <summary>
        /// شماره بارگذاری
        /// </summary>
        public int? LadingNo { get; set; }

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
        public virtual BaseValue FormType { get; set; } = default!;
        public virtual BaseValue InvoiceType { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual BaseValue PaymentType { get; set; } = default!;
        public virtual BaseValue Type { get; set; } = default!;
        public virtual ICollection<DocumentItem> DocumentItems { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
    }
}
