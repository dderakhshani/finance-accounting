using Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Update;
using Eefa.Bursary.Application.UseCases.Payables.DocumentOperations.Commands.Delete;
using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Update
{
    public class UpdateBankValidator : AbstractValidator<UpdateBankCommand>
    {
        private IBursaryUnitOfWork _uow;

        public UpdateBankValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x=>x.Code).NotEmpty().WithMessage("کد ثبت نشده است");
            RuleFor(x => x.Title).NotEmpty().WithMessage("نام بانک ثبت نشده است");
            RuleFor(x => x).CustomAsync(validateCommand);
        }

        private async Task validateCommand(UpdateBankCommand cmd, ValidationContext<UpdateBankCommand> context, CancellationToken token)
        {
            var r = await _uow.Banks.FirstOrDefaultAsync(x => x.Code == cmd.Code && x.Id != cmd.Id && x.IsDeleted == false);
            if (r != null)
            {
                context.AddFailure("کد بانک ثبت شده تکراری است");
                return;
            }
            r = await _uow.Banks.FirstOrDefaultAsync(x => x.Title == cmd.Title && x.Id != cmd.Id && x.IsDeleted == false);
            if (r != null)
            {
                context.AddFailure("نام بانک ثبت شده تکراری است");
                return;
            }
        }
    }
}
