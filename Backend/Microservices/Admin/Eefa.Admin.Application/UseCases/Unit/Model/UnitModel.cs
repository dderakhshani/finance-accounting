using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.Unit.Model
{
    public class UnitModel : IMapFrom<Data.Databases.Entities.Unit>
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

        /// <summary>
        /// کد شعبه
        /// </summary>
        public int? BranchId { get; set; }
        public ICollection<int> PositionIds { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.Unit, UnitModel>()
                .ForMember(x=>x.PositionIds, opt=>opt.MapFrom(x=>x.UnitPositions.Select(t=>t.PositionId)));
        }
    }

}
