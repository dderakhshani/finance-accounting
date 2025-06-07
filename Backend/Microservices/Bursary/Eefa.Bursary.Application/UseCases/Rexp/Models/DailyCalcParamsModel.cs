namespace Eefa.Bursary.Application.UseCases.Rexp.Models
{
    public class DailyCalcParamsModel
    {
        public int UserId { get; set; }
        public string MonthlyForecastIds { get; set; }
        public int YM { get; set; }
        public int? ResultId { get; set; }
        public string? ResultMessage { get; set; }
    }
}
