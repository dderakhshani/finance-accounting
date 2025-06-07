using Eefa.Common.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Definitions
{
    [Table("CountryDivisions_View", Schema = "common")]
    public class CountryDivisions_View : BaseEntity
    {
        public string Title { get; set; }
    }
}
