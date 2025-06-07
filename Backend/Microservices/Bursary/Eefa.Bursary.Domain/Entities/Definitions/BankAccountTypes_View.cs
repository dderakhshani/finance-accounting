using Eefa.Common.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Definitions
{
    [Table("BankAccountTypes_View", Schema = "bursary")]
    public class BankAccountTypes_View : BaseEntity
    {
        public string? Code { get; set; }
        public string? Title { get; set; }
    }
}
