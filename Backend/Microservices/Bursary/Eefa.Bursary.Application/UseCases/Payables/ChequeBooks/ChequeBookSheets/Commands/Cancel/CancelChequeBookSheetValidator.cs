using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.Cancel
{
    public class CancelChequeBookSheetValidator : AbstractValidator<CancelChequeBookSheetCommand>
    {

        private IBursaryUnitOfWork _uow;

        public CancelChequeBookSheetValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(w => w.Id).GreaterThan(0).WithMessage("شناسه چک ثبت نشده است");
            RuleFor(w => w.CancelDescp).NotNull().NotEmpty().WithMessage("توضیحات ابطال وارد گردد");
            RuleFor(x => x).CustomAsync(validateCommand);
        }
        private async Task validateCommand(CancelChequeBookSheetCommand cmd, ValidationContext<CancelChequeBookSheetCommand> context, CancellationToken token)
        {
            var r = await _uow.Payables_Documents.AsQueryable().AsNoTracking().FirstOrDefaultAsync(w => w.ChequeBookSheetId == cmd.Id && w.IsDeleted == false);
            if (r != null)
            {
                context.AddFailure("این برگه چک استفاده شده و قابل ابطال نیست");
                return;
            }
            var r2 = await _uow.Payables_ChequeBooksSheets.AsQueryable().AsNoTracking().FirstOrDefaultAsync(w => w.Id == cmd.Id && (w.IsDeleted || w.IsCanceled));
            if (r2 != null)
            {
                context.AddFailure("این برگه قبلا حذف یا ابطال شده است");
                return;
            }

            if (cmd.CancelDate != null)
            {
                var r1 = await _uow.Payables_ChequeBooksSheets_View.FirstOrDefaultAsync(w => w.Id == cmd.Id);
                if (r1 != null)
                {
                    if (cmd.CancelDate < r1.GetDate)
                    {
                        context.AddFailure("تاریخ ابطال نمیتواند قبل از تاریخ دریافت دسته چک باشد");
                        return;
                    }
                }
            }
        }


    }
}
