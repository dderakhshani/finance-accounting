using FluentValidation;
using Library.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.CodeRowDescription.Command.Delete
{
    public class DeleteCodeRowDescriptionValidator: AbstractValidator<DeleteCodeRowDescriptionCommand>
    {
        public IRepository _repository { get; }
        public DeleteCodeRowDescriptionValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                .MustAsync(idMustExist).WithMessage("شناسه مورد نظر یافت نشد");
        }

        public async Task<bool> idMustExist(int id, CancellationToken cancellationToken)
        {
            return await _repository.Exist<Data.Entities.CodeRowDescription>(x => x.ObjectId(id));
        }
    }
}
