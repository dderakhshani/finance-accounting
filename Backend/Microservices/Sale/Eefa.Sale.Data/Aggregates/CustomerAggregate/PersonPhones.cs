using Eefa.Common.Data;

namespace Eefa.Sale.Domain.Aggregates.CustomerAggregate
{
    public partial class PersonPhones : BaseEntity
    {

        

        
        public int PersonId { get; set; } = default!;

        
        public int PhoneTypeBaseId { get; set; } = default!;

        
        public string PhoneNumber { get; set; } = default!;

        
        public string? Description { get; set; }

        
        public bool IsDefault { get; set; } = default!;

        
        

        public virtual Person Person { get; set; } = default!;
        public virtual BaseValues PhoneTypeBase { get; set; } = default!;
    }
}
