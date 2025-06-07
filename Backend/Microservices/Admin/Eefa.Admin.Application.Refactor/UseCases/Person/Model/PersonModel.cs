using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

public class PersonModel : IMapFrom<Person>
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? FatherName { get; set; } = default!;
    public string NationalNumber { get; set; } = default!;
    public string EconomicCode { get; set; } = default!;
    public string? IdentityNumber { get; set; }
    public string? InsuranceNumber { get; set; }
    public string? MobileJson { get; set; }
    public string? Email { get; set; }
    public int? AccountReferenceId { get; set; }
    public string? AccountReferenceCode { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? BirthPlaceCountryDivisionId { get; set; }
    public int GenderBaseId { get; set; } = default!;
    public int LegalBaseId { get; set; }
    public int GovernmentalBaseId { get; set; }
    public string LegalBaseTitle { get; set; }
    public string GovernmentalBaseTitle { get; set; }
    public string GenderBaseTitle { get; set; }
    public int? AccountReferenceGroupId { get; set; }
    public string? AccountReferenceGroupTitle { get; set; }
    public ICollection<PersonAddressModel> PersonAddressList { get; set; }
    public ICollection<PersonBankAccount> PersonBankAccountsList { get; set; }
    public ICollection<PersonPhoneModel> PersonPhonesList { get; set; }
    public ICollection<PersonFingerprintModel> PersonFingerprintsList { get; set; }
    //public PersonCustomerModel PersonCustomer { get; set; }
    public IList<PhoneNumber> PhoneNumbers { get; set; }
    public string PhotoURL { get; set; }
    public string SignatureURL { get; set; }
    public bool TaxIncluded { get; set; } = default!;
    public string? EmployeeCode { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Person, PersonModel>()
            .ForMember(src => src.FullName, opt => opt.MapFrom(t => t.FirstName + " " + t.LastName))
            .ForMember(src => src.GenderBaseTitle, opt => opt.MapFrom(dest => dest.GenderBase.Title))
            .ForMember(src => src.GovernmentalBaseTitle, opt => opt.MapFrom(dest => dest.GovernmentalBase.Title))
            .ForMember(src => src.LegalBaseTitle, opt => opt.MapFrom(dest => dest.LegalBase.Title))
            .ForMember(src => src.AccountReferenceCode, opt => opt.MapFrom(dest => dest.AccountReference.Code))
            .ForMember(src => src.PersonAddressList, opt => opt.MapFrom(t => t.PersonAddresses))
            .ForMember(src => src.PersonPhonesList, opt => opt.MapFrom(t => t.PersonPhones))
            .ForMember(src => src.PersonBankAccountsList, opt => opt.MapFrom(t => t.PersonBankAccounts))
            .ForMember(src => src.PersonFingerprintsList, opt => opt.MapFrom(t => t.PersonFingerprints))
            .ForMember(src => src.EmployeeCode, opt => opt.MapFrom(t => t.Employee.EmployeeCode))
            .ForMember(src => src.CreatedBy, opt => opt.MapFrom(t => t.CreatedBy.Person.FirstName + " " + t.CreatedBy.Person.LastName))
            .ForMember(src => src.ModifiedBy, opt => opt.MapFrom(t => t.ModifiedBy.Person.FirstName + " " + t.ModifiedBy.Person.LastName))
            .ForMember(src => src.AccountReferenceGroupTitle, opt => opt.MapFrom(t => t.AccountReference.AccountReferencesRelReferencesGroups.FirstOrDefault().ReferenceGroup.Title))
            .ForMember(src => src.AccountReferenceGroupId, opt => opt.MapFrom(t => t.AccountReference.AccountReferencesRelReferencesGroups.FirstOrDefault().ReferenceGroupId));
    }
}