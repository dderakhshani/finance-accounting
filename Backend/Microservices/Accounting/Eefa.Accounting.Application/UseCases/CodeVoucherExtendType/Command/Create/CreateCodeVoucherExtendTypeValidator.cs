using FluentValidation;

namespace Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Command.Create
{
    public class CreateCodeVoucherExtendTypeValidator: AbstractValidator<CreateCodeVoucherExtendTypeCommand>
    {
        public CreateCodeVoucherExtendTypeValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(100).WithMessage("طول عنوان بیشتر از حد تایین شده است.");
        }
    }
}
