using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.BaseValueType.Command.Delete
{
    public class DeleteBaseValueTypeValidator : AbstractValidator<DeleteBaseValueTypeCommand>
    {
        public DeleteBaseValueTypeValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
        }
    }
}