using FluentValidation;
using Library.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Command.Delete
{
    public class DeleteAccountReferencesGroupValidator: AbstractValidator<DeleteAccountReferencesGroupCommand>
    {
        public IRepository _repository { get; }
        public DeleteAccountReferencesGroupValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                .MustAsync(isReferenceGroupInUse).WithMessage("شناسه مورد نظر یافت نشد");           
        }
        public async Task<bool> isReferenceGroupInUse(int id, CancellationToken cancellationToken)
        {
            return !await _repository.GetQuery<Eefa.Accounting.Data.Entities.AccountReferencesRelReferencesGroup>()
                             .AnyAsync(x => x.ReferenceGroupId == id);
        }
    }
}
