using FluentValidation;
using Library.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Eefa.Accounting.Application.UseCases.AccountReference.Command.Create
{
    public class CreateAccountReferenceValidator: AbstractValidator<CreateAccountReferenceCommand>
    {
        public IRepository _repository {  get; }

        public CreateAccountReferenceValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(200).WithMessage("طول عنوان بیشتر از حد تایین شده است.")
                .MustAsync(beUniqueTitle).WithMessage("عنوان تکراری است.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("کد نمی تواند خالی باشد.")
                .MaximumLength(200).WithMessage("طول کد بیشتر از حد تایین شده است.")
                .MustAsync(beUniqueCode).WithMessage("کد تکراری است.");

            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description)
                    .MaximumLength(250).WithMessage("طول شرح بیشتر از حد تایین شده است.");
            });
        }
        public async Task<bool> beUniqueTitle(string title, CancellationToken cancellationToken)
        {
            var data = _repository.GetAll<Data.Entities.AccountReference>()
                .Where(x => x.Title == title).ToList();
            if (data.Any()) return false;
            return true;
        }
        public async Task<bool> beUniqueCode(string code, CancellationToken cancellationToken)
        {
            var data = _repository.GetAll<Data.Entities.AccountReference>()
                .Where(x => x.Code == code).ToList();
            if (data.Any()) return false;
            return true;
        }
    }
}
