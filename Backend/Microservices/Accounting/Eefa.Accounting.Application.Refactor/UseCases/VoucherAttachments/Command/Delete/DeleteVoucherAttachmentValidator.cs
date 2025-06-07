using FluentValidation;

public class DeleteVoucherAttachmentValidator : AbstractValidator<DeleteVoucherAttachmentCommand>
{
    public DeleteVoucherAttachmentValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.");
    }
}