using FluentValidation;
using Library.Interfaces;
using System.Threading.Tasks;
using System.Threading;

namespace Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Command.Delete
{
    public class DeleteAutoVoucherFormulaValidator: AbstractValidator<DeleteAutoVoucherFormulaCommand>
    {
        public IRepository _repository { get; }
        public DeleteAutoVoucherFormulaValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .NotEqual(0).WithMessage("شناسه نمیتواند صفر باشد.")
                .MustAsync(idMustExist).WithMessage("شناسه مورد نظر یافت نشد");
        }
        public async Task<bool> idMustExist(int id, CancellationToken cancellationToken)
        {
            return await _repository.Exist<Data.Entities.AutoVoucherFormula>(x => x.ObjectId(id));
        }
    }
}
