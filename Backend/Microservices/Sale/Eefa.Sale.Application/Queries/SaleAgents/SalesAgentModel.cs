using AutoMapper;
using Eefa.Common;
using Eefa.Sale.Domain.Aggregates;

namespace Eefa.Sale.Application.Queries.SaleAgents
{
    public class SalesAgentModel : IMapFrom<SalesAgents>
    {
        public int Id { get; set; } = default!;
        public string Title { get; set; }
        public string Code { get; set; } = default!;
        public string? Description { get; set; }
        public bool? IsActive { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SalesAgents, SalesAgentModel>()
                .ForMember(x  => x.Title, opt => opt.MapFrom(x => x.Code + "  " + x.Person.FirstName + " " + x.Person.LastName));
        }
    }
}
