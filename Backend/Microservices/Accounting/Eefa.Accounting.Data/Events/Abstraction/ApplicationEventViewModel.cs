using AutoMapper;
using Library.Mappings;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Data.Events.Abstraction
{
    public class ApplicationEventViewModel : IMapFrom<ApplicationEvent>
    {
        public Guid Id { get; set; }
        public Guid? OriginId { get; set; }
        public int EntityId { get; set; }
        public string EntityType { get; set; }
        public string Descriptions { get; set; }
        public EventStates State { get; set; }
        public string? PayloadJSON { get; set; }
        public string? PayloadType { get; set; }
        public EventTypes Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public List<ApplicationEventViewModel> SubEvents { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApplicationEvent, ApplicationEventViewModel>()
              .ForMember(src => src.CreatedBy, opt => opt.MapFrom(dest => (dest.CreatedBy.Person.FirstName ?? "") + " " + (dest.CreatedBy.Person.LastName ?? "")))
              .ForMember(x => x.SubEvents, opt => opt.MapFrom(x => x.SubEvents));
        }
    }
}
