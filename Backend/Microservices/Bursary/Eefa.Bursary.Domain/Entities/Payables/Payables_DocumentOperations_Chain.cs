using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_DocumentOperations_Chain", Schema = "bursary")]
    public class Payables_DocumentOperations_Chain
    {
        public string SourceCode { get; set; }
        public string DestCode { get; set; }

    }
}
