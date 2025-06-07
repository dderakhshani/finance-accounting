using FluentValidation;
using Library.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ServiceStack;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.AccountHead.Command.Delete
{
    public class DeleteAccountHeadValidator: AbstractValidator<DeleteAccountHeadCommand>
    {
        public IRepository _repository { get; }
        public DeleteAccountHeadValidator(IRepository repository)
        {
            _repository = repository;

            When(x => x.ForceDeactive == false, () =>
            {
                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                    .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                    .MustAsync(IsAccountHeadInUse).WithMessage("شناسه موردنظر استفاده شده است.")
                    .MustAsync(IsAccountHeadInUseInCurrentYear).WithMessage("شناسه موردنظر در سال جاری استفاده شده است.");
            }).Otherwise(() =>
            {
                RuleFor(x => x.Id)
                        .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                        .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                        .MustAsync(IsAccountHeadInUseInCurrentYear).WithMessage("شناسه موردنظر در سال جاری استفاده شده است.");
            });
        }
        public async Task<bool> IsAccountHeadInUseInCurrentYear(int id, CancellationToken token)
        {
            var currentYear = await _repository.Find<Data.Entities.Year>
                (x => x.ConditionExpression(x => x.IsCurrentYear)).FirstOrDefaultAsync();

            if (await _repository.GetQuery<Data.Entities.VouchersDetail>()
                    .AnyAsync(x => x.VoucherDate >= currentYear.FirstDate &&
                                   x.VoucherDate <= currentYear.LastDate &&
                                   x.AccountHeadId == id))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> IsAccountHeadInUse(int id, CancellationToken cancellationToken)
        {
            var currentYear = await _repository.Find<Data.Entities.Year>
                (x => x.ConditionExpression(x => x.IsCurrentYear)).FirstOrDefaultAsync();

            if (await _repository.GetQuery<Data.Entities.VouchersDetail>()
                        .AnyAsync(x => x.AccountHeadId == id &&
                                 (x.VoucherDate < currentYear.FirstDate ||
                                  x.VoucherDate > currentYear.LastDate)))
            {
                return true;
            }
            return false;
        }
    }
}
