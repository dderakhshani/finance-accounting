using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    public partial class DocumentItem : BaseEntity
    {

        /// <summary>
        /// کد سرفصل سند
        /// </summary>
        public int DocumentHeadId { get; set; } = default!;

        /// <summary>
        /// کد سال
        /// </summary>
        public int YearId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;

        /// <summary>
        /// سریال کالا
        /// </summary>
        public string? CommoditySerial { get; set; }

        /// <summary>
        /// تعداد
        /// </summary>
        public double Quantity { get; set; } = default!;
        public double Weight { get; set; } = default!;

        /// <summary>
        /// قیمت واحد 
        /// </summary>
        public long UnitPrice { get; set; } = default!;

        /// <summary>
        /// قیمت پایه
        /// </summary>
        public long ProductionCost { get; set; } = default!;
        
        public int? CurrencyBaseId { get; set; }
        public int? CurrencyPrice { get; set; }
        

        /// <summary>
        /// تخفیف
        /// </summary>
        public long Discount { get; set; } = default!;


        public virtual User CreatedBy { get; set; } = default!;
        public virtual BaseValue CurrencyBase { get; set; }
        public virtual DocumentHead DocumentHead { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Year Year { get; set; } = default!;
    }
}
