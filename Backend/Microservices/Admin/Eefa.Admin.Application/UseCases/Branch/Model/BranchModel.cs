using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.Branch.Model
{
    public class BranchModel : IMapFrom<Data.Databases.Entities.Branch>
    {
        public int Id { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.Branch, BranchModel>()
              
                ;
        }
    }

}
