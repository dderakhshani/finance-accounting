using System;
using System.Collections.Generic;

[HasUniqueIndex]
public partial class Person : AuditableEntity
{
    public string? FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? FatherName { get; set; } = default!;
    [UniqueIndex]
    public string NationalNumber { get; set; } = default!;
    public string EconomicCode { get; set; } = default!;
    public string? IdentityNumber { get; set; }
    public string? InsuranceNumber { get; set; }
    public string? MobileJson { get; set; } = default!;
    public string? Email { get; set; }
    public string? PhotoURL { get; set; }
    public string? SignatureURL { get; set; }
    public int? AccountReferenceId { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? BirthPlaceCountryDivisionId { get; set; }
    public int GenderBaseId { get; set; } = default!;
    public bool TaxIncluded { get; set; } = default!;
    public int? LegalBaseId { get; set; }
    public int? GovernmentalBaseId { get; set; }


    public virtual AccountReference? AccountReference { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual BaseValue GenderBase { get; set; } = default!;
    public virtual BaseValue? GovernmentalBase { get; set; } = default!;
    public virtual BaseValue? LegalBase { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual Employee? Employee { get; set; } = default!;
    public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
    public virtual ICollection<PersonBankAccount> PersonBankAccounts { get; set; } = default!;
    public virtual ICollection<PersonPhone> PersonPhones { get; set; } = default!;
    public virtual ICollection<PersonFingerprint> PersonFingerprints { get; set; } = default!;
    public virtual ICollection<Signer> Signers { get; set; } = default!;
    public virtual ICollection<User> Users { get; set; } = default!;
    public virtual Customer Customer { get; set; } = default!;
    public virtual ICollection<SalesAgents> SalesPersons { get; set; } = default!;
}