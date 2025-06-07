using FluentValidation;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Command.Update
{
    public class UpdateAccountReferencesGroupValidator : AbstractValidator<UpdateAccountReferencesGroupCommand>
    {
        public IRepository _repository {  get; }
        public UpdateAccountReferencesGroupValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x)
                .MustAsync(isUniqueCode).WithMessage("کد تکراری است.");

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.");

            When(y => y.ParentId != null && y.ParentId != 0, () =>
            {
                RuleFor(y => y.ParentId)
                .NotEqual(0).WithMessage("کد والد نمیتواند صفر باشد.")
                .MustAsync(validateParentId).WithMessage("کد والد وارد شده معتبر نمی باشد.");
            });

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(100).WithMessage("طول عنوان بیشتر از حد تایین شده است.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("کد نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("طول کد بیشتر از حد تایین شده است.");
        }

        public async Task<bool> isUniqueCode(UpdateAccountReferencesGroupCommand accountReferenceGroup, CancellationToken token)
        {
            return !await _repository.GetQuery<Data.Entities.AccountReferencesGroup>()
                                 .AnyAsync(x => x.Code == accountReferenceGroup.Code && x.Id != accountReferenceGroup.Id);
        }
        public async Task<bool> validateParentId(int? parentId, CancellationToken token)
        {
            return await _repository.Exist<Data.Entities.AccountReferencesGroup>(x => x.ObjectId(parentId));
        }
    }
}
