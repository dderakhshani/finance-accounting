using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.DocumentOperations.Commands.Add
{
    public class CreateDocumentOperationValidator : AbstractValidator<CreateDocumentOperationCommand>
    {
        private readonly IBursaryUnitOfWork _uow;

        public CreateDocumentOperationValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.DocumentIds).NotEmpty().WithMessage("سند پرداختی انتخاب نشده است");
            RuleFor(x => x.OperationId).GreaterThan(0).WithMessage("نوع عملیات سند پرداختی ثبت نشده است");
            RuleFor(x => x).MustAsync(validateOp).WithMessage("عملیات مورد نظر غیر معتبر است")
                .MustAsync(isValidOpDate).WithMessage("تاریخ عملیات مورد درخواست نمیتواند قبل از تاریخ عملیات قبلی باشد");
        }

        private async Task<bool> validateOp(CreateDocumentOperationCommand cmd, CancellationToken token)
        {
            var newop = await _uow.BaseValues.FirstOrDefaultAsync(w => w.Id == cmd.OperationId);
            if (newop == null) return false;

            foreach (var i in cmd.DocumentIds)
            {
                var curop = await _uow.Payables_DocumentsOperations.Include(x => x.Operation).OrderByDescending(w => w.OperationDate).ThenByDescending(w => w.Id).FirstOrDefaultAsync(w => w.DocumentId == i);
                if (curop == null) return false;
                var val = await _uow.Payables_DocumentOperations_Chain.FirstOrDefaultAsync(w => w.SourceCode == curop.Operation.Code && w.DestCode == newop.Code);
                if (val == null) return false;
            }
            return true;
        }

        private async Task<bool> isValidOpDate(CreateDocumentOperationCommand cmd, CancellationToken token)
        {
            if (cmd.OperationDate == null) return true;
            foreach (var i in cmd.DocumentIds)
            {
                var curop = await _uow.Payables_DocumentsOperations.OrderByDescending(w => w.OperationDate).ThenByDescending(w => w.Id).FirstOrDefaultAsync(w => w.DocumentId == i);
                if (curop == null) return false;
                if (curop.OperationDate > cmd.OperationDate) return false;
            }
            return true;
        }

    }
}
