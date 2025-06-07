using AutoMapper;

public class PersonCustomerModel : IMapFrom<Customer>
{
    public int Id { get; set; } = default!;
    public int PersonId { get; set; } = default!;
    public int CustomerTypeBaseId { get; set; } = default!;
    public string CustomerCode { get; set; } = default!;
    public int CurentExpertId { get; set; } = default!;
    public string? EconomicCode { get; set; }
    public string? Description { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Customer, PersonCustomerModel>().IgnoreAllNonExisting();
    }
}