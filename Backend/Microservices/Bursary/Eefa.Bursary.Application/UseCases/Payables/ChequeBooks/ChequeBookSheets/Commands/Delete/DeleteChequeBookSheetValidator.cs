using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Update;
using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.Delete
{
    internal class DeleteChequeBookSheetValidator:AbstractValidator<DeleteChequeBookSheetCommand>
    {
        private IBursaryUnitOfWork _uow;
        public DeleteChequeBookSheetValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("شناسه برگه چک ثبت نشده است");
            RuleFor(x => x).MustAsync(notUsed).WithMessage("تین برگه چک استفاده شده و قابل حذف نیست");
        }

        private async Task<bool> notUsed(DeleteChequeBookSheetCommand cmd, CancellationToken cancellationToken)
        {
            var usd = await _uow.Payables_Documents.FirstOrDefaultAsync(w => w.ChequeBookSheetId == cmd.Id);
            if (usd != null) return false;
            return true;
        }

    }
}
