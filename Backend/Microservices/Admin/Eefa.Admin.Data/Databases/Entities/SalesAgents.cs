using System.Collections.Generic;
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class SalesAgents : BaseEntity
    {
        public int PersonId { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string? Description { get; set; }
        public bool? IsActive { get; set; } = default!;

        public virtual Person Person { get; set; } = default!;
        public virtual ICollection<Customer> Customers { get; set; } = default!;
    }
}
