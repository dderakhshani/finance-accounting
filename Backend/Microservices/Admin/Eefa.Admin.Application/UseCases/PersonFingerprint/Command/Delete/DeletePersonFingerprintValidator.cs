using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.PersonFingerprint.Command.Delete
{
    public class DeletePersonFingerprintValidator : AbstractValidator<DeletePersonFingerprintCommand>
    {
        public DeletePersonFingerprintValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");
        }
    }
}