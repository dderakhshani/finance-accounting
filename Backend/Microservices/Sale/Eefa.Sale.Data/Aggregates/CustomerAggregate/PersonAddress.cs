

using Eefa.Common.Data;

namespace Eefa.Sale.Domain.Aggregates.CustomerAggregate

{
    public partial class PersonAddress : BaseEntity
    {
        public int PersonId { get; set; } = default!;

        public int TypeBaseId { get; set; } = default!;


        public string? Address { get; set; }


        public int? CountryDivisionId { get; set; }

        public string? TelephoneJson { get; set; }


        public string? PostalCode { get; set; }

        public bool IsDefault { get; set; }

        public virtual Person Person { get; set; } = default!;
        public virtual BaseValues TypeBase { get; set; } = default!;
    }
}
