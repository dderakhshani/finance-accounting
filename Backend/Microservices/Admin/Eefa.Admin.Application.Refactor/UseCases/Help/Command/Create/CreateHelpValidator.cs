using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

public class CreateHelpValidator : AbstractValidator<CreateHelpCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateHelpValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.MenuItemId)
            .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
            .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.")
            .MustAsync(doesHelpExist)
            .WithMessage("راهنمایی که شما قصد افزودن آن را دارید در حال حاظر موجود است. لطفا آن را ویرایش نمایید.");

        RuleFor(x => x.Contents)
            .NotEmpty().WithMessage("محتوا نمی تواند خالی باشد.");
    }

    private async Task<bool> doesHelpExist(int menuItemId, CancellationToken token)
    {
        return !await _unitOfWork.Helps.ExistsAsync(x => x.MenuId == menuItemId);
    }
}