using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
   
   
    public partial class AccountReference : DomainBaseEntity
    {


        /// <summary>
        /// شناسه
        /// </summary>
       
        public string Code { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;



    }
}
