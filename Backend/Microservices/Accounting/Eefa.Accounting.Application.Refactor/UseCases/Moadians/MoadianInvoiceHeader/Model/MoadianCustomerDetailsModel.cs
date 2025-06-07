using AutoMapper;
using System.Linq;
// TODO check EconomicCode in Person
public class MoadianCustomerDetailsModel : IMapFrom<MoadianCustomerDetailsModel>
{
    public int PersonId { get; set; }
    public int? CustomerId { get; set; }
    public int AccountReferenceId { get; set; }
    public string AccountReferenceCode { get; set; }
    public string CustomerCode { get; set; }
    public string FullName { get; set; }
    public string NationalNumber { get; set; }
    public string? EconomicCode { get; set; }
    public int LegalType { get; set; }
    public string PostalCode { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Person, MoadianCustomerDetailsModel>()
            .ForMember(x => x.PersonId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.AccountReferenceId, opt => opt.MapFrom(x => x.AccountReferenceId))
            .ForMember(x => x.AccountReferenceCode, opt => opt.MapFrom(x => x.AccountReference.Code))
            .ForMember(x => x.NationalNumber, opt => opt.MapFrom(x => x.NationalNumber))
            .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FirstName + " " + x.LastName))
            .ForMember(x => x.CustomerId, opt => opt.MapFrom(x => x.Customers.FirstOrDefault().Id))
            .ForMember(x => x.CustomerCode, opt => opt.MapFrom(x => x.Customers.FirstOrDefault().CustomerCode))
            //.ForMember(x => x.EconomicCode, opt => opt.MapFrom(x => x.EconomicCode ?? x.Customers.FirstOrDefault().EconomicCode))
            .ForMember(x => x.PostalCode, opt => opt.MapFrom(x => x.PersonAddresses.FirstOrDefault(y => y.PostalCode != null).PostalCode))
            .ForMember(x => x.LegalType, opt => opt.MapFrom(x => int.Parse(x.LegalBase.Value)));
    }
}