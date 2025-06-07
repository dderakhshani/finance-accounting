using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// شرکت هایی که کاربر به آنها دسترسی دارد
    /// </summary>
    public partial class UserCompany: DomainBaseEntity
    {
    
        public int UserId { get; set; } = default!;
    /// <description>
            /// کد اطلاعات شرکت 
    ///</description>
    
        public int CompanyInformationsId { get; set; } = default!;
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    }
}
