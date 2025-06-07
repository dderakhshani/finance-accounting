using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.Position.Command.Delete
{
    public class DeletePositionValidator : AbstractValidator<DeletePositionCommand>
    {
        public DeletePositionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
        }
    }
}