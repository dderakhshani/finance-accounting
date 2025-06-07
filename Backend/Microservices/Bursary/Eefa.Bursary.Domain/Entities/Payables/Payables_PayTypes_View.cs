using Eefa.Common.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_PayTypes_View", Schema = "bursary")]
    public class Payables_PayTypes_View : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
    }
}
