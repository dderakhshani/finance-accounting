using System.Collections.Generic;
using Eefa.Common.Data;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;

namespace Eefa.Sale.Domain.Aggregates
{
    public partial class SalesAgents : BaseEntity
    {
        public SalesAgents()
        {
            Customers = new HashSet<Customer>();
        }


       

       
        public int PersonId { get; set; } = default!;

       
        public string Code { get; set; } = default!;

       
        public string? Description { get; set; }

       
        public bool? IsActive { get; set; } = default!;

       
      

        public virtual Person Person { get; set; } = default!;
        public virtual ICollection<Customer> Customers { get; set; } = default!;
    }
}
