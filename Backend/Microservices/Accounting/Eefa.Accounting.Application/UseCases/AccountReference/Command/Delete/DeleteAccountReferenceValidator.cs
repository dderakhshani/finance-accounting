using FluentValidation;
using Library.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.AccountReference.Command.Delete
{
    public class DeleteAccountReferenceValidator: AbstractValidator<DeleteAccountReferenceCommand>
    {
        public IRepository _repository { get; }
        public DeleteAccountReferenceValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                .MustAsync(IsAccountReferenceInUse).WithMessage("شناسه مورد نظر یافت نشد");
        }

        public async Task<bool> IsAccountReferenceInUse(int id, CancellationToken cancellationToken)
        {
            if (!await _repository.GetQuery<Data.Entities.VouchersDetail>()
                .AnyAsync(x => x.ReferenceId1 == id ||
                               x.ReferenceId2 == id ||
                               x.ReferenceId3 == id))
            {
                return false;
            }
            return true;
        }
    }
}
