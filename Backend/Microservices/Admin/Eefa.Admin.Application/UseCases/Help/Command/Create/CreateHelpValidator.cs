using FluentValidation;
using Library.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.UseCases.Help.Command.Create
{
    public class CreateHelpValidator : AbstractValidator<CreateHelpCommand>
    {
        private readonly IRepository _repository;
        public CreateHelpValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.MenuItemId)
                .NotEmpty().WithMessage("عنوان صفحه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("عنوان صفحه نمی تواند برابر یا کمتر از 0 باشد.")
                .MustAsync(doesHelpExist)
                .WithMessage("راهنمایی که شما قصد افزودن آن را دارید در حال حاظر موجود است. لطفا آن را ویرایش نمایید.");

            RuleFor(x => x.Contents)
                .NotEmpty().WithMessage("محتوا نمی تواند خالی باشد.");
        }

        private async Task<bool> doesHelpExist(int menuItemId, CancellationToken token)
        {
            return !await _repository.Exist<Data.Databases.Entities.Help>
                (x => x.ConditionExpression(x => x.MenuId == menuItemId));
        }
    }
}