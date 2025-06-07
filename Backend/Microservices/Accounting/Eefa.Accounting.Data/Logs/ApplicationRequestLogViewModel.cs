using AutoMapper;
using Eefa.Accounting.Data.Events.Abstraction;
using Library.Mappings;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Data.Logs
{
    public class ApplicationRequestLogViewModel : IMapFrom<ApplicationRequestLog>
    {
        public Guid Id { get; set; }
        public string RequestJSON { get; set; }
        public string RequestType { get; set; }
        public string ResponseJSON { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public List<ApplicationEventViewModel> SubEvents { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApplicationRequestLog, ApplicationRequestLogViewModel>()
              .ForMember(src => src.CreatedBy, opt => opt.MapFrom(dest => (dest.CreatedBy.Person.FirstName ?? "") + " " + (dest.CreatedBy.Person.LastName ?? "")));
        }
    }
}
