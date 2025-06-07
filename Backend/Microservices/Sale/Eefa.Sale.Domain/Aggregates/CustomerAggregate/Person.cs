using System;
using System.Collections.Generic;
using Eefa.Common.Data;

namespace Eefa.Sale.Domain.Aggregates.CustomerAggregate
{

    public partial class Person : BaseEntity
    {
        public Person()
        {
            PersonPhones = new HashSet<PersonPhones>();
            SalesAgents = new HashSet<SalesAgents>();
        }


        

        
        public string FirstName { get; set; } = default!;

        
        public string LastName { get; set; } = default!;

        
        public string? FatherName { get; set; }

        
        public string NationalNumber { get; set; } = default!;

        
        public string? IdentityNumber { get; set; }

        
        public string? InsuranceNumber { get; set; }

        
        public string? MobileJson { get; set; }

        
        public string? Email { get; set; }

        
        public string? PhotoURL { get; set; }

        
        public string? SignatureURL { get; set; }

        
        public int? AccountReferenceId { get; set; }

        
        public DateTime? BirthDate { get; set; }

        
        public int? BirthPlaceCountryDivisionId { get; set; }
        public bool TaxIncluded { get; set; } = default!;

        
        public int GenderBaseId { get; set; } = default!;

        
        public int? LegalBaseId { get; set; }

        
        public int? GovernmentalBaseId { get; set; }

        
      
        public virtual AccountReferences AccountReference { get; set; } = default!;
        public virtual BaseValues GenderBase { get; set; } = default!;
        public virtual BaseValues GovernmentalBase { get; set; } = default!;
        public virtual BaseValues LegalBase { get; set; } = default!;
        public virtual ICollection<Customer> Customers { get; set; } = default!;
        public virtual ICollection<PersonPhones> PersonPhones { get; set; } = default!;
        public virtual ICollection<SalesAgents> SalesAgents { get; set; } = default!;
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
        public virtual ICollection<User> Users { get; set; } = default!;

    }
}
