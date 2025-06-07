using Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Commands.Add;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Rexp.DailyForeCasts.Commands.Calcs
{
    public class CalcDailyForecastValidator : AbstractValidator<CalculateDailyForecast>
    {
        public CalcDailyForecastValidator()
        {
            RuleFor(x => x).CustomAsync(validateCommand);
        }

        private async Task validateCommand(CalculateDailyForecast cmd, ValidationContext<CalculateDailyForecast> context, CancellationToken token)
        {
            if ((cmd.MonthlyForecastIds == null || cmd.MonthlyForecastIds.Count == 0) && (cmd.YM == null || cmd.YM <= 0))
            {
                context.AddFailure("ماه و یا شناسه منابع و مصارف جهت محاسبه مشخص نشده است");
                return;
            }
            if (cmd.YM != null && cmd.YM > 0 && cmd.YM < 140301)
            {
                context.AddFailure("ماه وارد شده نامعتبر است");
                return;
            }
            if (cmd.YM != null && cmd.YM > 0 && cmd.MonthlyForecastIds.Count> 0)
            {
                context.AddFailure("ارسال همزمان ماه و شناسه منابع و مصارف مجاز نیست");
                return;
            }

            foreach (var item in cmd.MonthlyForecastIds)
            {
                if (item <= 0)
                {
                    context.AddFailure("شناسه منابع و مصارف " + item.ToString() + " ارسال شده نامعتبر است " );
                }
            }
        }
    }

}