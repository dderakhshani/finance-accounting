using System;
using System.Collections.Generic;
using Library.Attributes;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    [HasUniqueIndex]
    public partial class DocumentHead : BaseEntity
    {

        public int CodeVoucherGroupId { get; set; } = default!;
        /// <summary>
        /// کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

        //public int? AccountReferencesGroupId { get; set; }

        /// <summary>
        /// کد انبار
        /// </summary>
        public int WarehouseId { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد مرجع
        /// </summary>
        public int? ReferenceId { get; set; }

        /// <summary>
        /// شماره سند
        /// </summary>
        public int? DocumentNo { get; set; }
        public string DocumentSerial { get; set; }

        /// <summary>
        /// تاریخ سند
        /// </summary>
        public DateTime DocumentDate { get; set; } = default!;

        /// <summary>
        /// توضیحات سند
        /// </summary>
        public string? DocumentDescription { get; set; }

        /// <summary>
        /// کد وضعیت سند
        /// </summary>
        public int DocumentStateBaseId { get; set; } = default!;

        /// <summary>
        /// دستی
        /// </summary>
        public bool IsManual { get; set; } = default!;

        /// <summary>
        /// کد سند حسابداری
        /// </summary>
        public int? VoucherHeadId { get; set; }

        /// <summary>
        /// جمع مبلغ سند
        /// </summary>
        public long TotalItemPrice { get; set; } = default!;

        /// <summary>
        /// جمع مالیات
        /// </summary>
        public long VatTax { get; set; } = default!;
        public long VatDutiesTax { get; set; } = default!;
        public long HealthTax { get; set; } = default!;

        /// <summary>
        /// جمع تخفیف
        /// </summary>
        public long TotalItemsDiscount { get; set; } = default!;


        /// <summary>
        /// درصد تخفیف
        /// </summary>
        public double? DiscountPercent { get; set; }

        /// <summary>
        /// تخفیف کل سند
        /// </summary>
        public double? DocumentDiscount { get; set; }
        public double TotalWeight { get; set; }
        public double TotalQuantity { get; set; }

        /// <summary>
        /// قیمت بعد از کسر تخفیف
        /// </summary>
        public long? PriceMinusDiscount { get; set; }

        /// <summary>
        /// قیمت با مالیات بعد از کسر تخفیف 
        /// </summary>
        public long? PriceMinusDiscountPlusTax { get; set; }
        public long? TotalProductionCost { get; set; }

        /// <summary>
        /// نوع پرداخت
        /// </summary>
        public int PaymentTypeBaseId { get; set; } = default!;
        public DateTime? ExpireDate { get; set; }
        public string? PartNumber { get; set; }

        public virtual User CreatedBy { get; set; } = default!;
        //public virtual AccountReferencesGroup AccountReferencesGroup { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual DocumentHead? Parent { get; set; } = default!;
        public virtual BaseValue PaymentTypeBase { get; set; } = default!;
        public virtual BaseValue DocumentStateBase { get; set; } = default!;
        public virtual AccountReference? Reference { get; set; } = default!;
        public virtual Year Year { get; set; } = default!;
        public virtual ICollection<DocumentItem> DocumentItems { get; set; } = default!;
        public virtual ICollection<DocumentPayment> DocumentPayments { get; set; } = default!;
        public virtual ICollection<DocumentHead> InverseParent { get; set; } = default!;

    }
}
