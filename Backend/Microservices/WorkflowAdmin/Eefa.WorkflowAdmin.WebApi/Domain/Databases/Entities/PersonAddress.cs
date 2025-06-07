using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
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

        public virtual CountryDivision? CountryDivision { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
        public virtual BaseValue TypeBase { get; set; } = default!;
    }
}
