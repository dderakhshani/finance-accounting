using System.Collections.Generic;
using Eefa.Common.Data;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;

namespace Eefa.Sale.Domain.Aggregates
{

    public partial class AccountReferencesGroups : BaseEntity
    {
        public AccountReferencesGroups()
        {
            AccountReferencesRelReferencesGroups = new HashSet<AccountReferencesRelReferencesGroups>();
            InverseParent = new HashSet<AccountReferencesGroups>();
        }


       

       
        public int CompanyId { get; set; } = default!;

       
        public int? ParentId { get; set; }
        public string Code { get; set; } = default!;

       
        public string LevelCode { get; set; } = default!;

       
        public string Title { get; set; } = default!;

       
        public bool? IsEditable { get; set; } = default!;

   

        public virtual AccountReferencesGroups Parent { get; set; } = default!;
        public virtual ICollection<AccountReferencesRelReferencesGroups> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual ICollection<AccountReferencesGroups> InverseParent { get; set; } = default!;
        public virtual ICollection<Customer> Customers { get; set; } = default!;
    }
}
