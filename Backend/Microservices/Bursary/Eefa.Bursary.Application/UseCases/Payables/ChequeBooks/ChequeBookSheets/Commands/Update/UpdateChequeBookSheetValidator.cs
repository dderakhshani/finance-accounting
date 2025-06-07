using Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Add;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Update;
using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.Update
{
    public class UpdateChequeBookSheetValidator : AbstractValidator<UpdateChequeBookSheetCommand>
    {
        private IBursaryUnitOfWork _uow;

        public UpdateChequeBookSheetValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(x => x.SayyadNo).NotNull().NotEmpty().WithMessage("شماره صیادی وارد گردد");
            RuleFor(x => x).CustomAsync(validateCommand);

        }

        private async Task validateCommand(UpdateChequeBookSheetCommand cmd, ValidationContext<UpdateChequeBookSheetCommand> context, CancellationToken token)
        {
            var r = await _uow.Payables_ChequeBooksSheets.AsQueryable().AsNoTracking().FirstOrDefaultAsync(w => w.SayyadNo == cmd.SayyadNo && w.Id != cmd.Id && w.IsDeleted == false);
            if (r != null)
            {
                context.AddFailure("شماره صیادی ثبت شده تکراری است");
                return;
            }

            var r1 = await _uow.Payables_ChequeBooksSheets.AsQueryable().AsNoTracking().FirstOrDefaultAsync(w => w.Id == cmd.Id && w.IsCanceled == true);
            if (r != null)
            {
                context.AddFailure("این چک قبلا ابطال شده است");
                return;
            }
        }

    }

}
