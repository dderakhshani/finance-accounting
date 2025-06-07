using FluentValidation;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Commands.Delete
{
    public class DeleteBankAccountValidator : AbstractValidator<DeleteBankAccountCommand>
    {
        public DeleteBankAccountValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("شناسه حساب بانکی وارد نشده است");
        }
    }
}
