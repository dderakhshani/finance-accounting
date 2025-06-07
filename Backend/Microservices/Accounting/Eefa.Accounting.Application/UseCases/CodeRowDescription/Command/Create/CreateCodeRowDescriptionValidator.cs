using FluentValidation;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.CodeRowDescription.Command.Create
{
    public class CreateCodeRowDescriptionValidator: AbstractValidator<CreateCodeRowDescriptionCommand>
    {
        public IRepository _repository {  get; }
        public CreateCodeRowDescriptionValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(200).WithMessage("طول عنوان بیشتر از حد تایین شده است.")
                .MustAsync(beUniqueTitle).WithMessage("عنوان تکراری است.");
        }
        public async Task<bool> beUniqueTitle(string title, CancellationToken cancellationToken)
        {
            if (await _repository.GetQuery<Data.Entities.CodeRowDescription>()
                                 .AnyAsync(x => x.Title == title))
            {
                return false;
            }
            return true;
        }
    }
}