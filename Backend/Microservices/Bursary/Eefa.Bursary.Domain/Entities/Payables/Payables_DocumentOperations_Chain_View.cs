using Eefa.Common.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_DocumentOperations_Chain_View", Schema = "bursary")]
    public class Payables_DocumentOperations_Chain_View : BaseEntity
    {
        public int SourceOpId { get; set; }
        public string SourceOpCode { get; set; }
        public string SourceOpTitle { get; set; }
        public int DestOpId { get; set; }
        public string DestOpCode { get; set; }
        public string DestOpTitle { get; set; }
    }
}
