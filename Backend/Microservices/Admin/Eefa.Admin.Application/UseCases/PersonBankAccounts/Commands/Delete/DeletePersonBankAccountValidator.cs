using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Commands.Delete
{
    public class DeletePersonBankAccountValidator : AbstractValidator<DeletePersonBankAccountCommand>
    {
        public DeletePersonBankAccountValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
        }
    }
}