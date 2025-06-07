using Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Add;
using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Update
{
    public class UpdateBankBranchValidator:AbstractValidator<UpdateBankBranchCommand>
    {
        private IBursaryUnitOfWork _uow;

        public UpdateBankBranchValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.BankId).GreaterThan(0).WithMessage("بانک ثبت نشده است");
            RuleFor(x => x.Code).NotEmpty().WithMessage("کد شعبه ثبت نشده است");
            RuleFor(x => x.Title).NotEmpty().WithMessage("نام شعبه ثبت نشده است");
            RuleFor(x => x).CustomAsync(validateCommand);
        }

        private async Task validateCommand(UpdateBankBranchCommand cmd, ValidationContext<UpdateBankBranchCommand> context, CancellationToken token)
        {
            var r = await _uow.BankBranches.FirstOrDefaultAsync(x => x.Code == cmd.Code && x.BankId == cmd.BankId && x.IsDeleted == false);
            if (r != null)
            {
                context.AddFailure("کد شعبه بانک ثبت شده تکراری است");
                return;
            }
            var r1 = await _uow.BankBranches.FirstOrDefaultAsync(x => x.Title == cmd.Title && x.BankId == cmd.BankId && x.IsDeleted == false);
            if (r != null)
            {
                context.AddFailure("نام شعبه بانک ثبت شده تکراری است");
                return;
            }
        }

    }
}
