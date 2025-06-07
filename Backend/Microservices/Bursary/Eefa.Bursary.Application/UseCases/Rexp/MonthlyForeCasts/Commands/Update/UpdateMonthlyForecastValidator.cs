using Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Commands.Add;
using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Commands.Update
{
    public class UpdateMonthlyForecastValidator : AbstractValidator<UpdateMonthlyForecastCommand>
    {
        private IBursaryUnitOfWork _uow;

        public UpdateMonthlyForecastValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(x => x).CustomAsync(validateCommand);
            RuleFor(x => x.YM).GreaterThan(140001).WithMessage("ماه انتخاب شده غیر معتبر است");
            RuleFor(x => x.RexpId).GreaterThan(0).WithMessage("شناسه منابع و مصارف ارسالی غیر معتبر است");
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("مبلغ ارسال شده غیر معتبر است");
        }

        private async Task validateCommand(UpdateMonthlyForecastCommand cmd, ValidationContext<UpdateMonthlyForecastCommand> context, CancellationToken token)
        {
                var r = await _uow.MonthlyForecast.FirstOrDefaultAsync(x => x.Id != cmd.Id && x.YM == cmd.YM && x.RexpId == cmd.RexpId && x.IsDeleted == false);
                if (r == null)
                {
                    context.AddFailure(" منابع و مصارف مورد نظر قبلا ثبت شده است");
                    return;
                }
            
        }

    }
}
