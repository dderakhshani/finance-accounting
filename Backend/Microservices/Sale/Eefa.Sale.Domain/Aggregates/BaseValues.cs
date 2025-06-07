using System.Collections.Generic;
using Eefa.Common.Data;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;

namespace Eefa.Sale.Domain.Aggregates
{
    public partial class BaseValues : BaseEntity
    {
        public BaseValues()
        {
            Customers = new HashSet<Customer>();
            PersonPhones = new HashSet<PersonPhones>();
            PersonsGenderBases = new HashSet<Person>();
            PersonsGovernmentalBases = new HashSet<Person>();
            PersonsLegalBases = new HashSet<Person>();
        }





        public int BaseValueTypeId { get; set; } = default!;


        public int? ParentId { get; set; }


        public string Code { get; set; } = default!;


        public string LevelCode { get; set; } = default!;


        public string Title { get; set; } = default!;


        public string UniqueName { get; set; } = default!;


        public string Value { get; set; } = default!;


        public int OrderIndex { get; set; } = default!;


        public bool IsReadOnly { get; set; } = default!;




        public virtual ICollection<Customer> Customers { get; set; } = default!;
        public virtual ICollection<PersonPhones> PersonPhones { get; set; } = default!;
        public virtual ICollection<Person> PersonsGenderBases { get; set; } = default!;
        public virtual ICollection<Person> PersonsGovernmentalBases { get; set; } = default!;
        public virtual ICollection<Person> PersonsLegalBases { get; set; } = default!;
        public virtual ICollection<User> Users { get; set; } = default!;

    }
}
