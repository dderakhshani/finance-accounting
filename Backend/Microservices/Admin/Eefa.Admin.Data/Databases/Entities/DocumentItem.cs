using System;
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class DocumentItem : BaseEntity
    {
         

        /// <summary>
        /// کد سرفصل سند
        /// </summary>
        public int DocumentHeadId { get; set; } = default!;

        /// <summary>
        /// اولین کد سرفصل سند
        /// </summary>
        public int? FirstDocumentHeadId { get; set; }

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد کالا
        /// </summary>
        public int CommodityId { get; set; } = default!;

        /// <summary>
        /// سریال
        /// </summary>
        public string? CommoditySerial { get; set; }

        /// <summary>
        /// شماره بچ کالا
        /// </summary>
        public string? CommodityBachNumber { get; set; }

        /// <summary>
        /// تعداد
        /// </summary>
        public double Quantity { get; set; } = default!;

        /// <summary>
        /// قیمت هر واحد
        /// </summary>
        public long UnitPrice { get; set; } = default!;

        /// <summary>
        /// قیمت پایه
        /// </summary>
        public long BasePrice { get; set; } = default!;

        /// <summary>
        /// تاریخ انقضا
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// شماره بخش
        /// </summary>
        public string? PartNumber { get; set; }

        /// <summary>
        /// نرخ ارز
        /// </summary>
        public int? CurrencyPrice { get; set; }

        /// <summary>
        /// شماره بارنامه
        /// </summary>
        public string? LadingBillNo { get; set; }

        /// <summary>
        /// کد تجهیزات
        /// </summary>
        public int? EquipmentId { get; set; }

        /// <summary>
        /// سریال تجهیزات
        /// </summary>
        public string? EquipmentSerial { get; set; }

        /// <summary>
        /// مالیات
        /// </summary>
        public long Tax { get; set; } = default!;

        /// <summary>
        /// تخفیف
        /// </summary>
        public long Discount { get; set; } = default!;

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
        

        public virtual Commodity Commodity { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual DocumentHead DocumentHead { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
