using System;
using System.Collections.Generic;
using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// نوبت ها
    /// </summary>
    public partial class Turn: DomainBaseEntity
    {
    
        public int DriverId { get; set; } = default!;
    /// <description>
            /// کد وسیله نقلیه
    ///</description>
    
        public int VehicleId { get; set; } = default!;
    /// <description>
            /// مقصد اعلامی
    ///</description>
    
        public string AnnouncedDestinationAddress { get; set; } = default!;
    /// <description>
            /// نام مشتری اعلامی
    ///</description>
    
        public string? AnnouncedCustomerFullName { get; set; }
    /// <description>
            /// کد شرکت حمل و نقل
    ///</description>
    
        public int FreightId { get; set; } = default!;
    /// <description>
            /// تاریخ بارگیری
    ///</description>
    
        public DateTime? LoadedDate { get; set; }
    /// <description>
            /// وضعیت نوبت
    ///</description>
    
        public int StatusTypeBaseId { get; set; } = default!;
    /// <description>
            /// شماره نوبت
    ///</description>
    
        public int TurnNumber { get; set; } = default!;
    /// <description>
            /// کد انبار
    ///</description>
    
        public int? WarehouseId { get; set; }
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual Driver Driver { get; set; } = default!;
    public virtual Freight Freight { get; set; } = default!;
    public virtual BaseValue StatusTypeBase { get; set; } = default!;
                public virtual ICollection<TurnDocument>
    TurnDocuments { get; set; } = default!;
    }
}
