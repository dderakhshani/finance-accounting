using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities.Rexp
{
    public class MonthlyForecast : BaseEntity
    {
        public int YM { get; set; }
        public int RexpId { get; set; }
        public long Amount { get; set; }
        public virtual ICollection<DailyForecast> DailyForecast { get; set; }
    }
}
