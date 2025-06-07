
using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
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


        public virtual SalesAgent CurrentAgent { get; set; } = default!;
        public virtual BaseValue CustomerTypeBase { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
    }
}
