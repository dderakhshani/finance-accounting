using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.DocumentOperations.Commands.Delete
{
    public class DeleteDocumentOperationValidator : AbstractValidator<DeleteDocumentOperationCommand>
    {
        private readonly IBursaryUnitOfWork _uow;

        public DeleteDocumentOperationValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(x => x).CustomAsync(validateCommand);
        }

        private async Task validateCommand(DeleteDocumentOperationCommand cmd, ValidationContext<DeleteDocumentOperationCommand> context, CancellationToken token)
        {
            if (cmd == null || cmd.Id <= 0) context.AddFailure("اطلاعات عملیات مورد نظر ارسال نشده است");
            var r = await _uow.Payables_DocumentsOperations.FirstOrDefaultAsync(x => x.Id == cmd.Id && x.IsDeleted == false);
            if (r == null)
            {
                context.AddFailure("عمیات سند مورد نظر وجود ندارد");
                return;
            }
            if (r.OperationId == 28778) context.AddFailure("عملیات فوق توسط سیستم ثبت شده و قابل حذف نیست");
            var r1 = await _uow.Payables_DocumentsOperations.FirstOrDefaultAsync(x => x.DocumentId == r.DocumentId && x.IsDeleted == false && x.Id > cmd.Id);
            if (r1 != null) context.AddFailure("فقط آخرین عملیات سند پرداختی قابل حذف است");

        }
    }
}
