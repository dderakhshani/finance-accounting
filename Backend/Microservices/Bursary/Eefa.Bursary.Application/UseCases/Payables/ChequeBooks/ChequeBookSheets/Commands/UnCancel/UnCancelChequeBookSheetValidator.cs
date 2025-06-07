using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.UnCancel
{
    public class UnCancelChequeBookSheetValidator : AbstractValidator<UnCancelChequeBookSheetCommand>
    {
        private IBursaryUnitOfWork _uow;

        public UnCancelChequeBookSheetValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(w => w.Id).GreaterThan(0).WithMessage("شناسه برگه چک ثبت نشده است");
            RuleFor(w => w).CustomAsync(validateCommand);
        }

        private async Task validateCommand(UnCancelChequeBookSheetCommand cmd, ValidationContext<UnCancelChequeBookSheetCommand> context, CancellationToken token)
        {
            var r = await _uow.Payables_Documents.AsQueryable().AsNoTracking().FirstOrDefaultAsync(w => w.ChequeBookSheetId == cmd.Id && w.IsDeleted == false);
            if (r != null)
            {
                context.AddFailure("این برگه چک استفاده شده و قابل ابطال نیست");
                return;
            }
            var r2 = await _uow.Payables_ChequeBooksSheets.AsQueryable().AsNoTracking().FirstOrDefaultAsync(w => w.Id == cmd.Id && (w.IsDeleted || w.IsCanceled == false));
            if (r2 != null)
            {
                context.AddFailure("این برگه قبلا حذف شده و یا ابطال نشده است");
                return;
            }
        }

    }
}
