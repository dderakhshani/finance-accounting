using FluentValidation;
using Library.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Command.Create
{
    public class CreateAccountReferencesGroupValidator : AbstractValidator<CreateAccountReferencesGroupCommand>
    {
        public IRepository _repository { get; }
        public CreateAccountReferencesGroupValidator(IRepository repository)
        {
            _repository = repository;

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
                .MaximumLength(50).WithMessage("طول کد بیشتر از حد تایین شده است.")
                .MustAsync(isUniqueCode).WithMessage("کد تکراری است.");

        }
        public async Task<bool> isUniqueCode(string code, CancellationToken token)
        {
            return !await _repository.GetQuery<Data.Entities.AccountReference>()
                                 .AnyAsync(x => x.Code == code);
        }

        public async Task<bool> validateParentId(int? parentId, CancellationToken token)
        {
            return await _repository.Exist<Data.Entities.AccountReferencesGroup>(x => x.ObjectId(parentId));
        }
    }
}
