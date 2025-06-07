using System;

namespace Eefa.Inventory.Application
{


    public  class DocumentHeadsLog
    {
        public int CodeVoucherGroupId { get; set; } = default!;
        /// <description>
        /// کد سال
        ///</description>

        public int YearId { get; set; } = default!;
        /// <description>
        /// کد انبار
        ///</description>

        public int WarehouseId { get; set; } = default!;
        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; } = default!;
        /// <description>
        /// کد مرجع
        ///</description>
        public int? DebitAccountHeadId { get; set; } = default!;
        public int? DebitAccountReferenceId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; } = default!;

        public int? CreditAccountHeadId { get; set; } = default!;
        public int? CreditAccountReferenceId { get; set; } = default!;
        public int? CreditAccountReferenceGroupId { get; set; } = default!;


        /// <description>
        /// شماره سند
        ///</description>

        public int? DocumentNo { get; set; } = default!;
        /// <description>
        /// تاریخ سند
        ///</description>

        public DateTime DocumentDate { get; set; } = default!;

        /// <description>
        /// تاریخ درخواست
        ///</description>
        public DateTime? RequestDate { get; set; } = default!;

        /// <description>
        /// تاریخ انقضا
        ///</description>

        public DateTime? ExpireDate { get; set; } = default!;

        /// <description>
        /// توضیحات سند
        ///</description>

        public string DocumentDescription { get; set; }

        /// <description>
        /// شماره فاکتور فروشنده
        ///</description>
        public string InvoiceNo { get; set; }
        public string FinancialOperationNumber { get; set; }
        

        /// <description>
        /// کد وضعیت سند
        ///</description>

        public int DocumentStateBaseId { get; set; } = default!;
        
        /// <description>
        /// کد سند حسابداری
        ///</description>

        public int? VoucherHeadId { get; set; }
        
        /// <description>
        /// جمع مبلغ قابل پرداخت
        ///</description>

        public long TotalItemPrice { get; set; } = default!;
        /// <description>
        /// عوارض ارزش افزوده
        ///</description>

        public long VatTax { get; set; } = default!;
        /// <description>
        /// مالیات ارزش افزوده
        ///</description>

        public long VatDutiesTax { get; set; } = default!;
        

      
        /// جمع قیمت تمام شده
        ///</description>

        public long? TotalProductionCost { get; set; } = default!;
       
        /// <description>
        /// تخفیف کل سند
        ///</description>

        public double? DocumentDiscount { get; set; } = default!;
        /// <description>
        /// قیمت بعد از کسر تخفیف
        ///</description>

        public double? PriceMinusDiscount { get; set; } = default!;
        /// <description>
        /// قیمت با مالیات بعد از کسر تخفیف 
        ///</description>

       
        public long? ExtraCost { get; set; } = default!;
        
        /// <description>
        /// نوع پرداخت
        ///</description>

        public int PaymentTypeBaseId { get; set; } = default!;
        
        /// <description>
        /// شماره بخش
        ///</description>

        public string PartNumber { get; set; } = default!;
        public string RequestNo { get; set; } = default!;
        public string Tags { get; set; } = default!;

        public bool? IsPlacementComplete { get; set; } = default!;
        public bool? IsImportPurchase { get; set; } = default!;
        public string CommandDescription { get; set; } = default!;
        public int DocumentStauseBaseValue { get; set; } = default!;
        
        
    }


}
