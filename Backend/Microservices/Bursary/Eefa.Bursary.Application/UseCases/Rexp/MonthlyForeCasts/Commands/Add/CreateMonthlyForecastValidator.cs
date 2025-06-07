using Eefa.Bursary.Application.ExtensionMethods;
using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Commands.Add
{
    public class CreateMonthlyForecastValidator : AbstractValidator<CreateMonthlyForecastCommand>
    {
        private IBursaryUnitOfWork _uow;

        public CreateMonthlyForecastValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(x => x).CustomAsync(validateCommand);
            RuleFor(x => x.YM).GreaterThan(140001).WithMessage("ماه انتخاب شده غیر معتبر است");
        }

        private async Task validateCommand(CreateMonthlyForecastCommand cmd, ValidationContext<CreateMonthlyForecastCommand> context, CancellationToken token)
        {
            bool hasDuplicates = cmd.RexpId_Amount.GroupBy(x => x.RexpId).Any(g => g.Count() > 1);
            if(hasDuplicates)
            {
                context.AddFailure("در لیست منابع و مصارف ارسالی مورد تکراری وجود دارد");
                return;
            }

            foreach (var itm in cmd.RexpId_Amount)
            {
                if(itm.Amount < 0)
                {
                    context.AddFailure("مبلغ منفی " + itm.Amount.ToString("n0") + " غیر معتبر است ");
                    return;
                }
                var r = await _uow.ResourceExpense_View.FirstOrDefaultAsync(x => x.Id == itm.RexpId && x.IsDeleted == false);
                if (r == null)
                {
                    context.AddFailure("شناسه منابع و مصارف " + itm.RexpId.ToString() + " وجود ندارد "  );
                    return;
                }
            }
        }


    }


}
