using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Requests : BaseEntity
    {
         
        public int? ChainpreviousChainId { get; set; }
        public string? ExtraPayload { get; set; }
        public string? JsonData { get; set; }
        public string? Url { get; set; }
        public int HttpProtocol { get; set; } = default!;
        public int RequestStatus { get; set; } = default!;
        public int? ApiStatus { get; set; }
        public DateTime RegistrationDateTime { get; set; } = default!;
        public DateTime? ResponseDateTime { get; set; }
        public int ApplicantUserId { get; set; } = default!;
        public string? ResponderComment { get; set; }
        public string? ApplicantComment { get; set; }
        public string? QueueToSubscribe { get; set; }
    }
}
