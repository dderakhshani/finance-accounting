using Eefa.Common.Data;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;

namespace Eefa.Sale.Domain.Aggregates
{

    public partial class AccountReferencesRelReferencesGroups : BaseEntity
    {

        

        
        public int ReferenceId { get; set; } = default!;

        
        public int ReferenceGroupId { get; set; } = default!;

        
      
        public virtual AccountReferences Reference { get; set; } = default!;
        public virtual AccountReferencesGroups ReferenceGroup { get; set; } = default!;
    }
}
