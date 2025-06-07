using Eefa.Common;
using System;
using System.Collections.Generic;

namespace Eefa.Ticketing.Domain.Core.Entities.BaseInfo
{
    public class Person : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FatherName { get; set; }

        [UniqueIndex]
        public string NationalNumber { get; set; }
        public string EconomicCode { get; set; }

        public string IdentityNumber { get; set; }

        public string InsuranceNumber { get; set; }
        public string MobileJson { get; set; }

        public string Email { get; set; }

        public string PhotoURL { get; set; }

        public string SignatureURL { get; set; }

        public int? AccountReferenceId { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? BirthPlaceCountryDivisionId { get; set; }

        public int GenderBaseId { get; set; }
        public bool TaxIncluded { get; set; }

        public int? LegalBaseId { get; set; }

        public int? GovernmentalBaseId { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
