
using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Update
{
    public class UpdateChequeBookValidator : AbstractValidator<UpdateChequeBookCommand>
    {
        private IBursaryUnitOfWork _uow;

        public UpdateChequeBookValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("شناسه دسته چک ارسال نشده است")
                .MustAsync(doExists).WithMessage("شناسه دسته چک ارسال شده در سیستم موجود نمیباشد");
            RuleFor(x => x).MustAsync(noDuplicateChequeBook).WithMessage("اطلاعات ثبت شده تکراری میباشند")
                .MustAsync(noDuplicateSheet).WithMessage("شماره چکها با اطلاعات موجود تداخل دارند")
                .MustAsync(notUsed).WithMessage("این دسته چک در اسناد خزانه استفاده شده و قابل تغییر نیست");
            RuleFor(x => x.StartNumber).GreaterThan(0).WithMessage("اولین شماره چک باید عددی مثبت باشد");
            RuleFor(x => x.SheetsCount).GreaterThanOrEqualTo(10).WithMessage("تعداد برگه دسته چک نباید کمتر از 10 باشد");
            RuleFor(x => x.Serial).NotEmpty().WithMessage("سریال دسته چک ثبت نشده است");
        }

        private async Task<bool> noDuplicateChequeBook(UpdateChequeBookCommand cmd, CancellationToken cancellationToken)
        {
            var chequeBook = await _uow.Payables_ChequeBooks.AsQueryable().AsNoTracking().FirstOrDefaultAsync(w => w.Id == cmd.Id);
            if (chequeBook == null) return false;
            var dup = await _uow.Payables_ChequeBooks.AsNoTracking().AsQueryable().FirstOrDefaultAsync(w => w.BankAccountId == chequeBook.BankAccountId && w.Serial == cmd.Serial && w.Id != cmd.Id && w.IsDeleted == false);
            if (dup != null) return false;
            return true;
        }
        private async Task<bool> noDuplicateSheet(UpdateChequeBookCommand cmd, CancellationToken cancellationToken)
        {
            var chequeBook = await _uow.Payables_ChequeBooks.AsQueryable().AsNoTracking().FirstOrDefaultAsync(w => w.Id == cmd.Id);
            if (chequeBook == null) return false;
            var dup = await _uow.Payables_ChequeBooksSheets.AsNoTracking().AsQueryable().FirstOrDefaultAsync(w => w.ChequeBook.BankAccountId == chequeBook.BankAccountId && w.ChequeBookId != cmd.Id && w.ChequeSheetNo >= cmd.StartNumber & w.ChequeSheetNo <= cmd.StartNumber + cmd.SheetsCount && w.IsDeleted == false);
            if (dup != null) return false;
            return true;
        }

        private async Task<bool> notUsed(UpdateChequeBookCommand cmd, CancellationToken cancellationToken)
        {
            var usd = await _uow.Payables_Documents.FirstOrDefaultAsync(w => w.ChequeBookSheet.ChequeBook.Id == cmd.Id && w.IsDeleted == false);
            if (usd != null) return false;
            return true;
        }
        private async Task<bool> doExists(int Id, CancellationToken cancellationToken)
        {
            var ext = await _uow.Payables_ChequeBooks.FirstOrDefaultAsync(w => w.Id == Id);
            if (ext == null) return false;
            return true;
        }

    }
}
