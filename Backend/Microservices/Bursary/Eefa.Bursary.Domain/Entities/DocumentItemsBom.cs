using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class DocumentItemsBom : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         
        public int DocumentItemsId { get; set; } = default!;

        /// <summary>
//شماره فرمول ساخت 
        /// </summary>
        public int BomValueHeaderId { get; set; } = default!;

        /// <summary>
//لوکیشن کالا در انبار 
        /// </summary>
        public int ParentCommodityId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public int WarehouseLayoutsId { get; set; } = default!;

        /// <summary>
//نوع ورود1 خروج -1
        /// </summary>
        public int? IOMode { get; set; }

        /// <summary>
//قیمت در سیستم  درخواست 
        /// </summary>
        public long UnitBasePrice { get; set; } = default!;

        /// <summary>
//قیمت واحد 
        /// </summary>
        public long UnitPrice { get; set; } = default!;

        /// <summary>
//قیمت پایه
        /// </summary>
        public long ProductionCost { get; set; } = default!;

        /// <summary>
//وزن کالا 
        /// </summary>
        public double Weight { get; set; } = default!;

        /// <summary>
//تعداد
        /// </summary>
        public double Quantity { get; set; } = default!;

        /// <summary>
//نوع ارز
        /// </summary>
        public int? CurrencyBaseId { get; set; }

        /// <summary>
//مبلغ ارز
        /// </summary>
        public long? CurrencyPrice { get; set; }

        /// <summary>
//شرح کالا
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
//واحد شمارش اصلی کالا
        /// </summary>
        public int MainMeasureId { get; set; } = default!;

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
         

        public virtual BomValueHeaders BomValueHeader { get; set; } = default!;
        public virtual Commodities Commodity { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual BaseValues CurrencyBase { get; set; } = default!;
        public virtual DocumentItems DocumentItems { get; set; } = default!;
        public virtual MeasureUnits MainMeasure { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual Commodities ParentCommodity { get; set; } = default!;
    }
}
