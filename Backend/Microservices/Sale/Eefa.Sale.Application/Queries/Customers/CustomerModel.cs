using AutoMapper;
using Eefa.Common;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;
using System;
using System.Linq;

namespace Eefa.Sale.Application.Queries.Customers
{
    public class CustomerModel : IMapFrom<Customer>
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string NationalNumber { get; set; }
        public int LegalBaseId { get; set; }
        public int GovernmentalBaseId { get; set; }
        public int GenderBaseId { get; set; }
        public string Email { get; set; }
        public int CustomerTypeBaseId { get; set; }
        public int CurrentAgentId { get; set; }
        public bool IsActive { get; set; }
        public string EconomicCode { get; set; }
        public string Description { get; set; }
        public bool TaxIncluded { get; set; }


        public int Id { get; set; }
        public bool Export { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerTypeUniqueName { get; set; }
        public int AccountReferenceId { get; set; }
        public int AccountReferenceGroupId { get; set; }
        public string AccountReferenceCode { get; set; }
        public string AccountReferenceGroupCode { get; set; }
        public string CustomerCode { get; set; }
        public string CurrentExpertCode { get; set; }
        public string Address { get; set; }
        public string PhoneNumbers { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string? DepositId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Customer, CustomerModel>()

                .ForMember(src => src.FullName, opt => opt.MapFrom(t => t.Person.FirstName + " " + t.Person.LastName ))
                .ForMember(src => src.Address, opt => opt.MapFrom(t => t.Person.PersonAddresses.FirstOrDefault().Address))
                .ForMember(src => src.PhoneNumbers, opt => opt.MapFrom(t => t.Person.PersonPhones.Count > 1 ?  string.Join(" - ", t.Person.PersonPhones.Select(x => x.PhoneNumber)) : t.Person.PersonPhones.FirstOrDefault().PhoneNumber))
                .ForMember(src => src.NationalNumber, opt => opt.MapFrom(t => t.Person.NationalNumber))
                .ForMember(src => src.LegalBaseId, opt => opt.MapFrom(t => t.Person.LegalBaseId))
                .ForMember(src => src.GovernmentalBaseId, opt => opt.MapFrom(t => t.Person.GovernmentalBaseId))
                .ForMember(src => src.Email, opt => opt.MapFrom(t => t.Person.Email))
                .ForMember(src => src.GenderBaseId, opt => opt.MapFrom(t => t.Person.GenderBaseId))
                .ForMember(src => src.FirstName, opt => opt.MapFrom(t => t.Person.FirstName))
                .ForMember(src => src.LastName, opt => opt.MapFrom(t => t.Person.FirstName + " " + t.Person.LastName ))
                .ForMember(src => src.AccountReferenceGroupCode, opt => opt.MapFrom(t => t.AccountReferencesGroup.Code))
                .ForMember(src => src.AccountReferenceCode, opt => opt.MapFrom(t => t.Person.AccountReference.Code))
                .ForMember(src => src.DepositId, opt => opt.MapFrom(t => t.Person.AccountReference.DepositId))
                .ForMember(src => src.AccountReferenceId, opt => opt.MapFrom(t => t.Person.AccountReferenceId))
                .ForMember(src => src.CurrentExpertCode, opt => opt.MapFrom(t => t.CurrentAgent.Code))
                .ForMember(src => src.Export, opt => opt.MapFrom(t => t.AccountReferencesGroup.Code == "32"))
                .ForMember(src => src.CustomerTypeUniqueName, opt => opt.MapFrom(t => t.CustomerTypeBase.UniqueName))
                .ForMember(src => src.CreatedBy, opt => opt.MapFrom(x => x.CreatedBy.Person.FirstName + " " + x.CreatedBy.Person.LastName))
                .ForMember(src => src.TaxIncluded, opt => opt.MapFrom(t => t.Person.TaxIncluded))
                .ForMember(src => src.IsActive, opt => opt.MapFrom(x => x.Person.AccountReference.IsActive));
        }
    }

}