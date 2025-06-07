using AutoMapper;

public class PersonAddressModel : IMapFrom<PersonAddress>
{
    public int Id { get; set; }
    public int PersonId { get; set; } = default!;
    public int TypeBaseId { get; set; }
    public string? Address { get; set; }
    public int? CountryDivisionId { get; set; }
    public string? TelephoneJson { get; set; }
    public string? PostalCode { get; set; }
    public string TypeBaseTitle { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<PersonAddress, PersonAddressModel>()
            .ForMember(x => x.TypeBaseTitle, opt => opt.MapFrom(x => x.TypeBase.Title));
    }
}