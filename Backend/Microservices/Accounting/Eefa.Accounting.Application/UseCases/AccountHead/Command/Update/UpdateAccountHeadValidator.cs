using FluentValidation;
using Library.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Eefa.Accounting.Application.UseCases.AccountReference.Command.Update;


namespace Eefa.Accounting.Application.UseCases.AccountHead.Command.Update
{
    public class UpdateAccountHeadValidator: AbstractValidator<UpdateAccountHeadCommand>
    {
        public IRepository _repository { get; }
        public UpdateAccountHeadValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x)
                .MustAsync(beUniqueCode).WithMessage("کد تکراری است.");
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.");

            RuleFor(q => q.Code)
                .NotEmpty().WithMessage("کد نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("طول کد بیش از حد تایین شده است.");

            When(y => y.ParentId != null && y.ParentId != 0, () =>
            {
                RuleFor(y => y.ParentId)
                    .NotEqual(0).WithMessage("کد والد نمیتواند صفر باشد.")
                    .MustAsync(validateParentId).WithMessage("کد والد معتبر نمی باشد.");
            });

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(100).WithMessage("عنوان طول بیشتر از حد تایین شده است.");

            When(y => y.BalanceId != null && y.BalanceId != 0, () =>
            {
                RuleFor(x => x.BalanceId)
                    .Must(x => x.Equals(0) || x.Equals(1) || x.Equals(2)).WithMessage("ماهیت حساب صحیح نمیباشد.");
            });

            RuleFor(x => x.BalanceBaseId)
                .NotEmpty().WithMessage("کنترل ماهیت حساب نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("کنترل ماهیت حساب نمی تواند صفر باشد.");

            RuleFor(x => x.TransferId)
                .NotEmpty().WithMessage("نوع حساب نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("نوع حساب نمی تواند صفر باشد.");


            RuleFor(x => x.CurrencyBaseTypeId)
                .NotEmpty().WithMessage("نوع ارز نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("نوع ارز نمی تواند صفر باشد.");

            When(y => !string.IsNullOrEmpty(y.Description), () =>
            {
                RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("طول شرح بیشتر از حد تایین شده است.");
            });
        }
        public async Task<bool> beUniqueCode(UpdateAccountHeadCommand accountHead, CancellationToken cancellationToken)
        {
            return !await _repository.GetQuery<Data.Entities.AccountHead>().AnyAsync(x => x.Code == accountHead.Code && x.Id != accountHead.Id);
        }
        public async Task<bool> validateParentId(int? parentId, CancellationToken token)
        {
            return await _repository.Exist<Data.Entities.AccountHead>(x => x.ObjectId(parentId));
        }
    }
}
