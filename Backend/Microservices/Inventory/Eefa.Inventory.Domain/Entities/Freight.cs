using System.Collections.Generic;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// جدول اطلاعات شرکت های حمل و نقل 
    /// </summary>
    public partial class Freight: DomainBaseEntity
    {
        public int PersonId { get; set; } = default!;
    /// <description>
            /// فعال
    ///</description>
    
        public bool IsActive { get; set; } = default!;
    /// <description>
            /// بلاک
    ///</description>
    
        public bool IsBlock { get; set; } = default!;
    /// <description>
            /// نام مدیر
    ///</description>
    
        public string? ManagerFullName { get; set; }
    /// <description>
            /// رتبه سیستمی
    ///</description>
    
        public int? Rate { get; set; }
    /// <description>
            /// توضیحات
    ///</description>
    
        public string? Descriptions { get; set; }
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
                public virtual ICollection<Turn>
    Turns { get; set; } = default!;
    }
}
