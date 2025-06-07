using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Update;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.Delete;
using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Delete
{
    public class DeleteChequeBookValidator:AbstractValidator<DeleteChequeBookSheetCommand>
    {
        private readonly IBursaryUnitOfWork _uow;
        public DeleteChequeBookValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("شناسه دسته چک ثبت نشده است");
            RuleFor(x => x).MustAsync(notUsed).WithMessage("این دسته چک استفاده شده و قابل حذف نیست");
        }

        private async Task<bool> notUsed(DeleteChequeBookSheetCommand cmd, CancellationToken cancellationToken)
        {
            var usd = await _uow.Payables_Documents.FirstOrDefaultAsync(w => w.ChequeBookSheet.ChequeBook.Id == cmd.Id && w.IsDeleted == false);
            if (usd != null) return false;
            return true;
        }

    }
}
