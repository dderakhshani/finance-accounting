using System;
using System.Collections.Generic;
using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// رانندگان
    /// </summary>
    public partial class Driver: DomainBaseEntity
    {
    
        public int PersonId { get; set; } = default!;
    /// <description>
            /// شماره گواهینامه رانندگی
    ///</description>
    
        public string? DrivingLisenceNumber { get; set; }
    /// <description>
            /// تاریخ انقضاء گواهینامه رانندگی
    ///</description>
    
        public DateTime? DrivingLisenceExpiryDate { get; set; }
    /// <description>
            /// نوع گواهینامه رانندگی
    ///</description>
    
        public int? DrivingLisenceBaseTypeId { get; set; }
    /// <description>
            /// رتبه سیستمی
    ///</description>
    
        public int? Rate { get; set; }
    /// <description>
            /// شماره کارت سلامت
    ///</description>
    
        public string? HealthSmartCardNumber { get; set; }
    /// <description>
            /// تاریخ پایان اعتبار کارت سلامت
    ///</description>
    
        public DateTime? HealthSmartCardExpiryDate { get; set; }
    /// <description>
            /// بلاک
    ///</description>
    
        public bool IsBlock { get; set; } = default!;
    /// <description>
            /// علت بلاک
    ///</description>
    
        public int? BlockReasonBaseId { get; set; }
    /// <description>
            /// ماه های کاری راننده
    ///</description>
    
        public string? WorkingMonthJson { get; set; }
    /// <description>
            /// نقش صاحب سند
    ///</description>
    

    public virtual BaseValue? BlockReasonBase { get; set; }
    public virtual BaseValue? DrivingLisenceBaseType { get; set; }
                public virtual ICollection<Turn>
    Turns { get; set; } = default!;
    }
}
