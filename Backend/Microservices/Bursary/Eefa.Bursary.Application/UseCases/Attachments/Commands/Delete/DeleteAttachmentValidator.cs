using FluentValidation;

namespace Eefa.Bursary.Application.UseCases.Attachments.Commands.Delete
{
    public class DeleteAttachmentValidator:AbstractValidator<DeleteAttachmentCommand>
    {
        public DeleteAttachmentValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("شناسه فایل ثبت نشده است");
        }
    }
}
