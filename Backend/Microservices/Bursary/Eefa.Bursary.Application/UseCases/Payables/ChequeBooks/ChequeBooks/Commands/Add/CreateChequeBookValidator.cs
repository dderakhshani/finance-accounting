using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Add
{
    public class CreateChequeBookValidator : AbstractValidator<CreateChequeBookCommand>
    {
        private IBursaryUnitOfWork _uow;

        public CreateChequeBookValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;


            RuleFor(x => x).MustAsync(noDuplicateChequeBook).WithMessage("اطلاعات ثبت شده تکراری میباشند")
                .MustAsync(noDuplicateSheet).WithMessage("شماره چکها با اطلاعات موجود تداخل دارند");

            RuleFor(x => x.BankAccountId).GreaterThan(0).WithMessage("حساب بانکی انتخاب نشده است")
                .MustAsync(doExistBankAccount).WithMessage("حساب بانکی ارسال شده موجود نبوده و یا حساب جاری نمیباشد");
            RuleFor(x => x.StartNumber).GreaterThan(0).WithMessage("اولین شماره چک باید عددی مثبت باشد");
            RuleFor(x => x.SheetsCount).GreaterThanOrEqualTo(10).WithMessage("تعداد برگه دسته چک نباید کمتر از 10 باشد");
            RuleFor(x => x.Serial).NotEmpty().WithMessage("سریال دسته چک ثبت نشده است");
        }

        private async Task<bool> doExistBankAccount(int bankAccountId, CancellationToken cancellationToken)
        {
            var res = await _uow.BankAccounts.AsNoTracking().AsQueryable().AnyAsync(w => w.Id == bankAccountId && w.AccountTypeBaseId == 28527);
            if (res == null) return false;
            return true;
        }
        private async Task<bool> noDuplicateChequeBook(CreateChequeBookCommand cmd, CancellationToken cancellationToken)
        {
            var dup = await _uow.Payables_ChequeBooks.AsNoTracking().AsQueryable().FirstOrDefaultAsync(w => w.BankAccountId == cmd.BankAccountId && w.Serial == cmd.Serial && w.IsDeleted == false);
            if (dup != null) return false;
            return true;
        }
        private async Task<bool> noDuplicateSheet(CreateChequeBookCommand cmd, CancellationToken cancellationToken)
        {
            var dup = await _uow.Payables_ChequeBooksSheets.AsNoTracking().AsQueryable().FirstOrDefaultAsync(w => w.ChequeBook.BankAccountId == cmd.BankAccountId && w.ChequeSheetNo >= cmd.StartNumber & w.ChequeSheetNo <= cmd.StartNumber + cmd.SheetsCount && w.IsDeleted == false);
            if (dup != null) return false;
            return true;
        }



    }
}
