using Eefa.Common.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Definitions
{
    [Table("BankTypes_View", Schema ="bursary")]
    public class BankTypes_View : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
    }
}
