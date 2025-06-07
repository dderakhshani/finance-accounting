using Eefa.Common.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Rexp
{
    [Table("MonthlyForecast_View", Schema = "rexp")]

    public class MonthlyForecast_View : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int YM { get; set; }
        public int RexpId { get; set; }
        public long Amount { get; set; }
        public int BaseValueTypeId { get; set; }
    }
}
