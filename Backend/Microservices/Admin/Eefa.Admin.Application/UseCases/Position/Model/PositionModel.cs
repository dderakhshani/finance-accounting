using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.Position.Model
{
    public class PositionModel : IMapFrom<Data.Databases.Entities.Position>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string? LevelCode { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string? Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.Position, PositionModel>();
        }
    }

}
