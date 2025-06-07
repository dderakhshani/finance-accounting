using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.CountryDivision.Model
{
    public class CountryDivisionModel : IMapFrom<Data.Databases.Entities.CountryDivision>
    {
        public int Id { get; set; }
        public string Ostan { get; set; }
        public string OstanTitle { get; set; }
        public string Shahrestan { get; set; }
        public string ShahrestanTitle { get; set; }
        public string Bakhsh { get; set; }
        public string BakhshTitle { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.CountryDivision, CountryDivisionModel>().IgnoreAllNonExisting();
        }
    }

}
