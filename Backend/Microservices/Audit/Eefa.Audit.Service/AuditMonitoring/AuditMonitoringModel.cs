using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Eefa.Audit.Service.AuditMonitoring
{
    public class AuditMonitoringModel
    {
        [BsonDateTimeOptions]
        public DateTime StartDate { get; set; }
        [BsonDateTimeOptions]
        public DateTime EndDate { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string _id { get; set; }
        public string _t { get; set; }
        public string Environment { get; set; }
        public string EventType { get; set; }
        public object Action { get; set; }
        public string HttpMethod { get; set; }
        public string ResponseStatusCode { get; set; }
        public string ResponseStatus { get; set; }
    }

    public class AuditMonitoringProjectionModel
    {
        public bool StartDate { get; set; } = true;
        public bool EndDate { get; set; } = true;
        public bool RoleId { get; set; } = true;
        public bool UserId { get; set; } = true;
        public bool RoleName { get; set; } = true;
        public bool Username { get; set; } = true;
        public bool _id { get; set; } = true;
        public bool _t { get; set; } = true;
        public bool Environment { get; set; } = true;
        public bool EventType { get; set; } = true;
        public bool Action { get; set; } = true;
        public bool HttpMethod { get; set; } = true;
        public bool ResponseStatusCode { get; set; } = true;
        public bool ResponseStatus { get; set; } = true;
    }
}