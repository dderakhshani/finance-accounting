using Eefa.Common.Domain;
using Eefa.Purchase.Domain.Aggregates.InvoiceAggregate;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// تاریخچه انبار ها
    /// </summary>
    public partial class WarehouseHistory : DomainBaseEntity
    {
        
        public int Commodityld { get; set; } = default!;
    /// <description>
            /// کد چیدمان انبار
    ///</description>
    
        public int WarehouseLayoutId { get; set; } = default!;
    /// <description>
            /// تعداد
    ///</description>
    
        public double Quantity { get; set; } = default!;
    /// <description>
            /// نوع عملیات
    ///</description>
    
        public int Mode { get; set; } = default!;
    /// <description>
            /// شماره آیتم در سند
    ///</description>
    
        public int? DocumentItemId { get; set; }

        /// <description>
        ///شماره درخواست خروج
        ///</description>
        ///
        public int? RequestNo { get; set; }
        

    public virtual Commodity Commodity { get; set; } = default!;
    public virtual DocumentItem DocumentItem { get; set; }
    public virtual WarehouseLayout WarehouseLayout { get; set; } = default!;
   
    }
}
