using Eefa.Accounting.Data.Entities;
using Library.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Data.Events.Abstraction
{
    public class ApplicationEvent
    {
        public Guid Id { get; set; }
        public Guid? OriginId { get; set; }
        public int EntityId { get; set; }
        [NotMapped]
        public BaseEntity Entity { get; set; }
        public string EntityType { get; set; }
        public string Descriptions { get; set; }
        public EventStates State { get; set; }
        [NotMapped]
        public object? Payload { get; set; }
        public string? PayloadJSON { get; set; }
        public string? PayloadType { get; set; }
        public EventTypes Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }


        public ICollection<ApplicationEvent> SubEvents { get; set; }
        public ApplicationEvent Origin { get; set; }
        public User CreatedBy { get; set; }
    }


    public enum EventStates
    {
        Initialized,
        Committed,
        Failed
    }

    public enum EventTypes
    {
        WebApi,
        UI,
        Database,
    }
}
