using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.PersonAddress.Model;
using Eefa.Admin.Application.CommandQueries.PersonFingerprint.Model;
using Eefa.Admin.Application.CommandQueries.PersonPhones.Models;
using Eefa.Admin.Data.Databases.Entities;
using Library.Mappings;
using Library.Models;

namespace Eefa.Admin.Application.CommandQueries.Person.Model
{
    public class PersonModel : IMapFrom<Data.Databases.Entities.Person>
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        /// <summary>
        /// نام
        /// </summary>
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; } = default!;

        /// <summary>
        /// نام پدر
        /// </summary>
        public string? FatherName { get; set; } = default!;

        /// <summary>
        /// شماره ملی
        /// </summary>
        public string NationalNumber { get; set; } = default!;
        public string EconomicCode { get; set; } = default!;

        /// <summary>
        /// شماره شناسنامه
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// شماره بیمه
        /// </summary>
        public string? InsuranceNumber { get; set; }
        public string? MobileJson { get; set; }
        public string? Email { get; set; }
        public int? AccountReferenceId { get; set; }
        public string? AccountReferenceCode { get; set; }

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// محل تولد
        /// </summary>
        public int? BirthPlaceCountryDivisionId { get; set; }

        /// <summary>
        /// جنسیت
        /// </summary>
        public int GenderBaseId { get; set; } = default!;

        /// <summary>
        /// حقیقی/ حقوقی
        /// </summary>
        public int LegalBaseId { get; set; }

        /// <summary>
        /// دولتی/ غیر دولتی
        /// </summary>
        public int GovernmentalBaseId { get; set; }

        public string LegalBaseTitle { get; set; }
        public string GovernmentalBaseTitle { get; set; }
        public string GenderBaseTitle { get; set; }
        public int? AccountReferenceGroupId { get; set; }
        public string? AccountReferenceGroupTitle { get; set; }
        public string? WorkshopCode { get; set; } = default!;

        public string? DepositId { get; set; }

        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public ICollection<PersonAddressModel> PersonAddressList { get; set; }
        public ICollection<PersonBankAccount> PersonBankAccountsList { get; set; }
        public ICollection<PersonPhoneModel> PersonPhonesList { get; set; }
        public ICollection<PersonFingerprintModel> PersonFingerprintsList { get; set; }
        //public PersonCustomerModel PersonCustomer { get; set; }
        public IList<PhoneNumber> PhoneNumbers { get; set; }
        public string PhotoURL { get; set; }
        public string SignatureURL { get; set; }
        public bool TaxIncluded { get; set; } = default!;

        public string? EmployeeCode  { get; set; }
        public string? CreatedBy  { get; set; }
        public string? ModifiedBy  { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.Person, PersonModel>()
                .ForMember(src => src.FullName, opt => opt.MapFrom(t => t.FirstName + " " + t.LastName))
                .ForMember(src => src.GenderBaseTitle, opt => opt.MapFrom(dest => dest.GenderBase.Title))
                .ForMember(src => src.GovernmentalBaseTitle, opt => opt.MapFrom(dest => dest.GovernmentalBase.Title))
                .ForMember(src => src.LegalBaseTitle, opt => opt.MapFrom(dest => dest.LegalBase.Title))
                .ForMember(src => src.AccountReferenceCode, opt => opt.MapFrom(dest => dest.AccountReference.Code))
                .ForMember(src => src.DepositId, opt => opt.MapFrom(dest => dest.AccountReference.DepositId))
                .ForMember(src => src.PersonAddressList, opt => opt.MapFrom(t => t.PersonAddresses))
                .ForMember(src => src.PersonPhonesList, opt => opt.MapFrom(t => t.PersonPhones))
                .ForMember(src => src.PersonBankAccountsList, opt => opt.MapFrom(t => t.PersonBankAccounts))
                .ForMember(src => src.PersonFingerprintsList, opt => opt.MapFrom(t => t.PersonFingerprints))
                .ForMember(src => src.EmployeeCode, opt => opt.MapFrom(t => t.Employee.EmployeeCode))
                .ForMember(src => src.CreatedBy, opt => opt.MapFrom(t => t.CreatedBy.Person.FirstName + " " + t.CreatedBy.Person.LastName))
                .ForMember(src => src.ModifiedBy, opt => opt.MapFrom(t => t.ModifiedBy.Person.FirstName + " " + t.ModifiedBy.Person.LastName))
                .ForMember(src => src.CreatedAt, opt => opt.MapFrom(t => t.CreatedAt))
                .ForMember(src => src.ModifiedAt, opt => opt.MapFrom(t => t.ModifiedAt))
                .ForMember(src => src.AccountReferenceGroupTitle, opt => opt.MapFrom(t => t.AccountReference.AccountReferencesRelReferencesGroups.FirstOrDefault().ReferenceGroup.Title))
                .ForMember(src => src.AccountReferenceGroupId, opt => opt.MapFrom(t => t.AccountReference.AccountReferencesRelReferencesGroups.FirstOrDefault().ReferenceGroupId))
                .ForMember(src => src.PostalCode, opt => opt.MapFrom(t => t.PersonAddresses.FirstOrDefault().PostalCode))
                .ForMember(src => src.Address, opt => opt.MapFrom(t => t.PersonAddresses.FirstOrDefault().Address))
                .ForMember(src => src.PhoneNumber, opt => opt.MapFrom(t => t.PersonPhones.FirstOrDefault().PhoneNumber))
                ;
        }
    }
}
