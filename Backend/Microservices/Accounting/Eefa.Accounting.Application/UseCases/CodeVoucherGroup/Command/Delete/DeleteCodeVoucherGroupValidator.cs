using FluentValidation;
using Library.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Command.Delete
{
    public class DeleteCodeVoucherGroupValidator: AbstractValidator<DeleteCodeVoucherGroupCommand>
    {
        public IRepository _repository { get; }
        public DeleteCodeVoucherGroupValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                .MustAsync(isInUs).WithMessage("شناسه مورد نظر یافت نشد");
        }
        public async Task<bool> isInUs(int id, CancellationToken cancellationToken)
        {
            if ((await _repository.Exist<Data.Entities.CodeVoucherGroup>(x => x.ObjectId(id))) &&
                (await _repository.GetQuery<Data.Entities.VouchersHead>()
                                  .AnyAsync(x => x.CodeVoucherGroupId == id)))
            {
                return true; 
            }
            return false;
        }
    }
}
