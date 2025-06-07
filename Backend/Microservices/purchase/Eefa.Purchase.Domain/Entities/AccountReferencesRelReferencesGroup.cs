using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{

    public partial class AccountReferencesRelReferencesGroup : DomainBaseEntity
    {
        /// <summary>
        /// کد طرف حساب
        /// </summary>
        public int ReferenceId { get; set; } = default!;

        /// <summary>
        /// کد گروه طرف حساب
        /// </summary>
        public int ReferenceGroupId { get; set; } = default!;

        
    }
}
