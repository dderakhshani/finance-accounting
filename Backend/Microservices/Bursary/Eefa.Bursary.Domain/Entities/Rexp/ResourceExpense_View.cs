using Eefa.Common.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Rexp
{
    [Table("ResourcesExpenses_View", Schema ="rexp")]
    public class ResourceExpense_View : BaseEntity
    {
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? TypeCode { get; set; }
    }
}
