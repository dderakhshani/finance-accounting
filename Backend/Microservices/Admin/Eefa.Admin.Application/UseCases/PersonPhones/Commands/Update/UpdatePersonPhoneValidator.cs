using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.PersonPhones.Commands.Update
{
    public class UpdatePersonPhoneValidator : AbstractValidator<UpdatePersonPhoneCommand>
    {
        public UpdatePersonPhoneValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");

            RuleFor(x => x.PersonId)
                .NotEmpty().WithMessage("کد شخص نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("کد شخص نمی تواند برابر یا کمتر از 0 باشد.");

            RuleFor(x => x.PhoneTypeBaseId)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("عنوان نمی تواند برابر یا کمتر از 0 باشد.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("شماره نمی تواند خالی باشد.")
                .MaximumLength(25).WithMessage("شماره نمی تواند بیشتر از 25 کاراکتر باشد.");

            When(y => !string.IsNullOrEmpty(y.Description), () =>
            {
                RuleFor(x => x.Description)
                    .MaximumLength(500).WithMessage("توضیحات نمی تواند بیشتر از 500 کاراکتر باشد.");
            });
        }
    }
}