using Eefa.Bursary.Application.ExtensionMethods;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Add;
using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Commands.Add
{
    public class CreateBankAccountValidator : AbstractValidator<CreateBankAccountCommand>
    {
        private IBursaryUnitOfWork _uow;

        public CreateBankAccountValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.BankBranchId).GreaterThan(0).WithMessage("شعبه بانک مشخص نشده است");
            RuleFor(x => x.AccountNumber).NotNull().WithMessage("شماره حساب وارد نشده است");
            RuleFor(x => x.Sheba).NotNull().WithMessage("شماره شبا وارد نشده است");
            RuleFor(x => x.AccountTypeBaseId).GreaterThan(0).WithMessage("نوع حساب وارد نشده است");
            RuleFor(x => x.Sheba).NotNull().WithMessage("شماره شبا وارد نشده است");
            RuleFor(x => x.Sheba).NotNull().WithMessage("شماره شبا وارد نشده است");
            RuleFor(x => x).CustomAsync(validateCommand);
        }

        private async Task validateCommand(CreateBankAccountCommand cmd, ValidationContext<CreateBankAccountCommand> context, CancellationToken token)
        {
            var r = await _uow.BankBranches.FirstOrDefaultAsync(x => x.Id == cmd.BankBranchId && x.IsDeleted == false);
            if (r == null)
            {
                context.AddFailure("شعبه بانک وارد شده وجود ندارد");
                return;
            }
            if (!BursaryDataVakidations.ValidateShebaNo(cmd.Sheba))
            {
                context.AddFailure("شماره شبا وارد شده نامعتبر است");
                return;
            }
            var r1 = await _uow.BankAccounts.FirstOrDefaultAsync(x => x.Sheba == cmd.Sheba && x.IsDeleted == false);
            if (r1 != null)
            {
                context.AddFailure("شماره شبای ثبت شده تکراری است");
                return;
            }
            var r2 = await _uow.BankAccounts.FirstOrDefaultAsync(x => x.AccountNumber == cmd.AccountNumber && x.IsDeleted == false);
            if (r2 != null)
            {
                context.AddFailure("شماره حساب ثبت شده تکراری است");
                return;
            }
        }

    }
}
