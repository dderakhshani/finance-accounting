using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Commands.Add
{

    public class CreateDocumentValidator : AbstractValidator<CreateDocumentCommand>
    {
        private IBursaryUnitOfWork _uow;
        public CreateDocumentValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.PayTypeId).GreaterThan(0).WithMessage("نوع پرداخت ثبت نشده است");
            RuleFor(x => x).Must(NotBankAccountMissing).WithMessage("حساب بانکی و یا برگه چک انتخاب گردد")
                .MustAsync(noDuplicateChequeSheet).WithMessage("برگه چک وارد شده قبلا استفاده شده است")
                .MustAsync(noCanceledChequeSheet).WithMessage("برگه چک وارد شده ابطال شده است")
                .Must(noOverTotal).WithMessage("مبلغ چک با جمع مبالغ تخصیصی به طرفهای حساب یکسان نمیباشد ")
                .Must(hasAccount).WithMessage("طرف حساب انتخاب نشده است")
                .Must(noDuplicateAccount).WithMessage("در لیست طرفهای حساب رکورد تکراری وجود دارد")
                .Must(validPayType).WithMessage("انتخاب برگه و نوع چک برای چک پرداختی الزامی و برای سایر اسناد پرداختی غیر مجاز است");

            RuleFor(x => x.DocumentDate.Date).LessThanOrEqualTo(DateTime.Now.Date).WithMessage("تاریخ صدور نمیتواند مربوط به آینده باشد");
            RuleFor(x => x.SubjectId).GreaterThan(0).WithMessage("موضوع ثبت نشده است");
            RuleForEach(x => x.Accounts).ChildRules(account =>
            {
                account.RuleFor(x => x.Amount).GreaterThan(0);
            }).WithMessage("مبلغ ثبت نشده است");
            RuleForEach(x => x.Accounts).ChildRules(account =>
            {
                account.RuleFor(x => x.ReferenceId).GreaterThan(0);
            }).WithMessage("طرف حساب انتخاب نشده است");

            RuleFor(x => x).CustomAsync(validateCommand);

        }

        private bool NotBankAccountMissing(CreateDocumentCommand command)
        {
            if (command.BankAccountId == null && command.ChequeBookSheetId == null) return false;
            return true;
        }
        private async Task<bool> noDuplicateChequeSheet(CreateDocumentCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.ChequeBookSheetId == null) return true;
            var dup = await _uow.Payables_Documents.FirstOrDefaultAsync(w => w.ChequeBookSheetId == cmd.ChequeBookSheetId && w.IsDeleted == false);
            if (dup != null) return false;
            return true;
        }
        private async Task<bool> noCanceledChequeSheet(CreateDocumentCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.ChequeBookSheetId == null) return true;
            var cnl = await _uow.Payables_ChequeBooksSheets.FirstOrDefaultAsync(w => w.Id == cmd.ChequeBookSheetId && w.IsCanceled);
            if (cnl != null) return false;
            return true;
        }
        private bool noOverTotal(CreateDocumentCommand cmd)
        {
            var t = cmd.Accounts.Sum(x => x.Amount);
            if (t != cmd.Amount) return false;
            return true;
        }
        private bool hasAccount(CreateDocumentCommand cmd)
        {
            if (cmd.Accounts == null || cmd.Accounts.Count == 0) return false;
            return true;
        }
        private bool noDuplicateAccount(CreateDocumentCommand cmd)
        {
            var dups = cmd.Accounts.GroupBy(x => x.ReferenceId).Where(g => g.Count() > 1);
            if (dups.Count() > 0) return false;
            return true;
        }
        private bool validPayType(CreateDocumentCommand cmd)
        {
            if (cmd.PayTypeId == 28498 && cmd.ChequeBookSheetId == null) return false;
            if (cmd.PayTypeId == 28498 && cmd.ChequeTypeId == null) return false;
            if (cmd.PayTypeId != 28498 && cmd.ChequeBookSheetId != null) return false;
            return true;
        }
        private async Task validateCommand(CreateDocumentCommand cmd, ValidationContext<CreateDocumentCommand> context, CancellationToken token)
        {
            if (cmd.MonetarySystemId == 29341 && (cmd.CurrencyAmount <= 0 || cmd.CurrencyRate <= 0))
            {
                context.AddFailure("برای پرداخت ارزی نرخ تبدیل ارز و مبلغ ارزی الزامی است");
                return;
            }
            if (cmd.MonetarySystemId != 29341 && (cmd.CurrencyAmount > 0 || cmd.CurrencyRate > 0 || cmd.CurrencyTypeBaseId != 28306))
            {
                context.AddFailure("برای پرداخت غیر ارزی انتخاب نوع و نرخ و مبلغ ارزی غیر مجاز است");
                return;
            }
            if (cmd.MonetarySystemId != 29341 && cmd.Amount <= 0)
            {
                context.AddFailure("مبلغ سند پرداخت وارد نشده است");
                return;
            }
            if (!string.IsNullOrEmpty(cmd.Descp) && cmd.Descp.Length > 1000)
            {
                context.AddFailure("حداکثر طول توضیحات 1000 کاراکتر است");
                return;
            }
            if (!string.IsNullOrEmpty(cmd.TrackingNumber) && cmd.TrackingNumber.Length > 50)
            {
                context.AddFailure("حداکثر طول شماره پیگیری 50 کاراکتر است");
                return;
            }
            if (!string.IsNullOrEmpty(cmd.ReferenceNumber) && cmd.ReferenceNumber.Length > 50)
            {
                context.AddFailure("حداکثر طول شماره ارجاء 50 کاراکتر است");
                return;
            }
            if (cmd.PayTypeId == 28497 && (cmd.CreditReferenceId == null || cmd.CreditReferenceId == 0))
            {
                context.AddFailure("برای پرداخت نقدی باید تفضیل بستانکار انتخاب گردد");
                return;
            }
            if (cmd.CreditAccountHeadId == null || cmd.CreditAccountHeadId == 0)
            {
                context.AddFailure("سرفصل حساب بستانکار انتخاب نشده است");
                return;
            }
            if (cmd.CreditReferenceGroupId == null || cmd.CreditReferenceGroupId == 0)
            {
                var r = await _uow.AccountHeadRelReferenceGroup.AnyAsync(r => r.AccountHeadId == cmd.CreditAccountHeadId);
                if (r)
                {
                    context.AddFailure("گروه تفضیل بستانکار انتخاب نشده است");
                    return;
                }
            }
            if (cmd.PayTypeId == 28498 && cmd.DueDate == null)
            {
                context.AddFailure("ثبت تاریخ سررسید برای چک پرداختی الزامی است");
                return;
            }


        }


    }
}
