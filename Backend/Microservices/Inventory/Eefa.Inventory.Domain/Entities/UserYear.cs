using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// سال هایی که کاربر به آنها دسترسی دارد
    /// </summary>
    public partial class UserYear: DomainBaseEntity
    {
    
        public int UserId { get; set; } = default!;
    /// <description>
            /// کد سال
    ///</description>
    
        public int YearId { get; set; } = default!;
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual Year Year { get; set; } = default!;
    }
}
