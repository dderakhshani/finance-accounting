using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.PersonAddress.Model
{
    public class PersonAddressModel : IMapFrom<Data.Databases.Entities.PersonAddress>
    {
        public int Id { get; set; }

        /// <summary>
        /// کد والد
        /// </summary>
        public int PersonId { get; set; } = default!;
        public int TypeBaseId { get; set; }

        /// <summary>
        /// آدرس
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// کد شهرستان
        /// </summary>
        public int? CountryDivisionId { get; set; }

        /// <summary>
        /// تلفن
        /// </summary>
        public string? TelephoneJson { get; set; }

        /// <summary>
        /// کد پستی
        /// </summary>
        public string? PostalCode { get; set; }
        public string TypeBaseTitle { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.PersonAddress, PersonAddressModel>()
                .ForMember(x=>x.TypeBaseTitle,opt=>opt.MapFrom(x=>x.TypeBase.Title));
        }
    }
}
