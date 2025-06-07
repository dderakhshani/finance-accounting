using FluentValidation;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.CodeRowDescription.Command.Update
{
    public class UpdateCodeRowDescriptionValidator: AbstractValidator<UpdateCodeRowDescriptionCommand>
    {
        public IRepository _repository { get; }
        public UpdateCodeRowDescriptionValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                .MustAsync(idMustExist).WithMessage("شناسه مورد نظر یافت نشد");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MustAsync(beUniqueTitle).WithMessage("عنوان تکراری است.")
                .MaximumLength(200).WithMessage("طول عنوان بیشتر از حد تایین شده است.");
        }

        public async Task<bool> idMustExist(int id, CancellationToken cancellationToken)
        {
            return await _repository.Exist<Data.Entities.CodeRowDescription>(x => x.ObjectId(id));
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
