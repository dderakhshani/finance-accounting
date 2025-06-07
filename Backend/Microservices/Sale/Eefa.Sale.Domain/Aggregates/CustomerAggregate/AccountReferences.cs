using System.Collections.Generic;
using Eefa.Common.Data;

namespace Eefa.Sale.Domain.Aggregates.CustomerAggregate
{

    public partial class AccountReferences : BaseEntity
    {
        public AccountReferences()
        {
            AccountReferencesRelReferencesGroups = new HashSet<AccountReferencesRelReferencesGroups>();
            Persons = new HashSet<Person>();
        }
         
        public string Code { get; set; } = default!;

        
        public string Title { get; set; } = default!;

        
        public bool? IsActive { get; set; } = default!;

        public string? DepositId { get; set; } = default!;

        public virtual ICollection<AccountReferencesRelReferencesGroups> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual ICollection<Person> Persons { get; set; } = default!;
    }
}
