using FluentValidation;

namespace Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Commands.Delete
{
    public class DeleteMonthlyForecastValidator : AbstractValidator<DeleteMonthlyForecastCommand>
    {
        public DeleteMonthlyForecastValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("شناسه رکورد ارسال نشده است");
        }
    }

}