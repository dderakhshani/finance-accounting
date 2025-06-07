using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Domain.OutboxEntities
{
    public class OutboxMessages
    {
        public int Id { get; set; }  
        public string EventType { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsProcessed { get; set; } = false;
        public DateTime? ProcessedAt { get; set; }
        public int? VoucherHeadId { get; set; }
        public bool IsBursaryUpdated { get; set; } = false;
    }
}
