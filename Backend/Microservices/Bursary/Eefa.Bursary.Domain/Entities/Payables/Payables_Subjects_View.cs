using Eefa.Common.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_Subjects_View", Schema ="bursary")]
    public class Payables_Subjects_View : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
    }
}
