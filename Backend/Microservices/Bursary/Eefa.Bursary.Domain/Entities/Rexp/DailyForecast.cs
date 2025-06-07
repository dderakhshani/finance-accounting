using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities.Rexp
{
    public class DailyForecast : BaseEntity
    {
        public int YearId { get; set; }
        public int MonthlyForecastId { get; set; }
        public DateTime ActDate { get; set; }
        public long Amount { get; set; }
        public virtual MonthlyForecast MonthlyForecast { get; set; }
    }
}
