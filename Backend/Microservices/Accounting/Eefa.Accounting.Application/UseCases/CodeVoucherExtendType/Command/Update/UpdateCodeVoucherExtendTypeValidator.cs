using FluentValidation;
using Library.Interfaces;
using System.Threading.Tasks;
using System.Threading;

namespace Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Command.Update
{
    public class UpdateCodeVoucherExtendTypeValidator: AbstractValidator<UpdateCodeVoucherExtendTypeCommand>
    {
        public IRepository _repository {  get; }
        public UpdateCodeVoucherExtendTypeValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                .MustAsync(idMustExist).WithMessage("شناسه مورد نظر یافت نشد");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(100).WithMessage("طول عنوان بیشتر از حد تایین شده است.");
        }
        public async Task<bool> idMustExist(int id, CancellationToken cancellationToken)
        {
            return await _repository.Exist<Data.Entities.CodeVoucherExtendType>(x => x.ObjectId(id));
        }
    }
}
