using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.EditRequest.Commands.Add
{
    public class CreateEditRequestValidator : AbstractValidator<CreateEditRequestCommand>
    {
        private IBursaryUnitOfWork _uow;
        public CreateEditRequestValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(x => x).CustomAsync(validateCommand);
            RuleFor(x => x.CodeVoucherGroupId).GreaterThan(0).WithMessage("کد نوع سند ثبت نشده است");
            RuleFor(x => x.OldData).NotEmpty().NotNull().WithMessage("اطلاعات قبلی ثبت نشده است");
            RuleFor(x => x.PayLoad).NotEmpty().NotNull().WithMessage("اطلاعات جدید ثبت نشده است");
            RuleFor(x => x.DocumentId).GreaterThan(0).WithMessage("شناسه سند ثبت نشده است");
            RuleFor(x => x.ApiUrl).NotEmpty().NotNull().WithMessage("مسیر API ثبت نشده است");
            RuleFor(x => x.YearId).GreaterThan(0).WithMessage("سال مالی ثبت نشده است");
        }

        private async Task validateCommand(CreateEditRequestCommand cmd, ValidationContext<CreateEditRequestCommand> context, CancellationToken token)
        {
            var r = await _uow.CorrectionRequests.FirstOrDefaultAsync(x => x.CodeVoucherGroupId == cmd.CodeVoucherGroupId && x.DocumentId == cmd.DocumentId && x.Status == 1 && x.IsDeleted == false);
            if (r != null)
            {
                context.AddFailure("برای این اطلاعات یک مورد درخواست تغییر باز قبلی وجود دارد");
                return;
            }
        }

    }
}
