
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class Customer : BaseEntity
    {
        public int PersonId { get; set; } = default!;

       
        public int CustomerTypeBaseId { get; set; } = default!;

       
        public int CurrentAgentId { get; set; } = default!;

       
        public string CustomerCode { get; set; } = default!;

       
        public string? EconomicCode { get; set; }

       
        public string? Description { get; set; }
        public bool? IsActive { get; set; } = default!;


        public virtual SalesAgents CurrentAgent { get; set; } = default!;
        public virtual BaseValue CustomerTypeBase { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
    }
}
