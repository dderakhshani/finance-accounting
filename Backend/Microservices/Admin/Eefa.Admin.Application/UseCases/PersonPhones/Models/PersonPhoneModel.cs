using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.PersonPhones.Models
{
    public class PersonPhoneModel : IMapFrom<Data.Databases.Entities.PersonPhone>
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int PhoneTypeBaseId { get; set; }
        public string PhoneNumber { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.PersonPhone, PersonPhoneModel>().IgnoreAllNonExisting();
        }
    }
}
