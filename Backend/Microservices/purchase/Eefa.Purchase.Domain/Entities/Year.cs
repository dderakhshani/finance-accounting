using System;
using System.Collections.Generic;
using Eefa.Common.Domain;
using Eefa.Purchase.Domain.Aggregates.InvoiceAggregate;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// سال های مالی
    /// </summary>
    public partial class Year: DomainBaseEntity
    {
    
        public int CompanyId { get; set; } = default!;
    /// <description>
            /// نام سال
    ///</description>
    
        public int YearName { get; set; } = default!;
    /// <description>
            /// تاریخ شروع
    ///</description>
    
        public DateTime FirstDate { get; set; } = default!;
    /// <description>
            /// تاریخ پایان
    ///</description>
    
        public DateTime LastDate { get; set; } = default!;
    /// <description>
            /// آیا قابل ویرایش است؟
    ///</description>
    
        public bool? IsEditable { get; set; } = default!;
    /// <description>
            /// قابل شمارش است؟
    ///</description>
    
        public bool IsCalculable { get; set; } = default!;
    /// <description>
            /// آیا تاریخ در سال جاری است؟
    ///</description>
    
        public bool IsCurrentYear { get; set; } = default!;
    /// <description>
            /// ایجاد سال مالی بدون سند افتتاحیه
    ///</description>
    
        public bool CreateWithoutStartVoucher { get; set; } = default!;
    /// <description>
            /// تاریخ قفل شدن اطلاعات
    ///</description>
    
        public DateTime? LastEditableDate { get; set; }
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
                public virtual ICollection<Invoice>
    DocumentHeads { get; set; } = default!;
                public virtual ICollection<DocumentItem>
    DocumentItems { get; set; } = default!;
                public virtual ICollection<UserYear>
    UserYears { get; set; } = default!;
    }
}
