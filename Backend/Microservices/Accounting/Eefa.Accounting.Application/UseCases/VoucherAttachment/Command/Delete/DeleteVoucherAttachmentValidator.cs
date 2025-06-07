using FluentValidation;
using Library.Interfaces;

namespace Eefa.Accounting.Application.UseCases.VoucherAttachment.Command.Delete
{
    public class DeleteVoucherAttachmentValidator: AbstractValidator<DeleteVoucherAttachmentCommand>
    {
        public DeleteVoucherAttachmentValidator(IRepository repository)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.");
        }
    }
}
