using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.PersonCustomer.Commands.Delete
{
    public class DeletePersonCustomerValidator : AbstractValidator<DeletePersonCustomerCommand>
    {
        public DeletePersonCustomerValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
        }
    }
}