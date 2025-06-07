using FluentValidation;
using Library.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.AccountReference.Command.Update
{
    public class UpdateAccountReferenceValidator: AbstractValidator<UpdateAccountReferenceCommand>
    {
        public IRepository _repository { get; }
        public UpdateAccountReferenceValidator(IRepository repository)
        {
            _repository = repository;
            RuleFor(x => x)
                .MustAsync(beUniqueCode).WithMessage("کد تکراری است");
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                .MustAsync(idMustExist).WithMessage("شناسه مورد نظر یافت نشد.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(200).WithMessage("طول عنوان بیشتر از حد تایین شده است.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("کد نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("طول کد بیشتر از حد تایین شده است.");

            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description)
                    .MaximumLength(250).WithMessage("طول شرح بیشتر از حد تایین شده است.");
            });
        }

        public async Task<bool> idMustExist(int id, CancellationToken cancellationToken)
        {
            if (!await _repository.Exist<Data.Entities.AccountReference>(x => x.ObjectId(id)))
                return false;
            return true;
        }
        public async Task<bool> beUniqueCode(UpdateAccountReferenceCommand accountReference, CancellationToken cancellationToken)
        {
            return !await _repository.GetQuery<Data.Entities.AccountReference>().AnyAsync(x => x.Code == accountReference.Code && x.Id != accountReference.Id);
        }
    }
}
