using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.PersonCustomer.Models
{
    public class PersonCustomerModel : IMapFrom<Data.Databases.Entities.Customer>
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
            profile.CreateMap<Data.Databases.Entities.Customer, PersonCustomerModel>().IgnoreAllNonExisting();
        }
    }
}
