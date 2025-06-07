using FluentValidation;

public class CreateVoucherAttachmentValidator : AbstractValidator<CreateVoucherAttachmentCommand>
{
    public CreateVoucherAttachmentValidator()
    {
        RuleFor(x => x.VoucherHeadId)
            .NotEmpty().WithMessage("کد فایل راهنما نمی تواند خالی باشد.");
    }
}