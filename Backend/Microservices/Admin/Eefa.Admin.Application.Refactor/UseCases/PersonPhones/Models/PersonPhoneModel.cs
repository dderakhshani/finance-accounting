using AutoMapper;

public class PersonPhoneModel : IMapFrom<PersonPhone>
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public int PhoneTypeBaseId { get; set; }
    public string PhoneNumber { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
 
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<PersonPhone, PersonPhoneModel>().IgnoreAllNonExisting();
    }
}