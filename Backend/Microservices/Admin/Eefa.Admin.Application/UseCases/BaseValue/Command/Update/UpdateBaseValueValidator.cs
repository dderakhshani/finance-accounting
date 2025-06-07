using Eefa.Admin.Application.CommandQueries.BaseValue.Command.Update;
using FluentValidation;

namespace Eefa.Admin.Application.UseCases.BaseValue.Command.Update
{
    public class UpdateBaseValueValidator: AbstractValidator<UpdateBaseValueCommand>
    {
        public UpdateBaseValueValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه وارد شده نمی تواند برابر یا کمتر از0 باشد.");

            RuleFor(x => x.BaseValueTypeId)
                .NotEmpty().WithMessage("نوع اطلاعات پایه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("لطفا یک نوع اطلاعات پایه انتخاب کنید.");

            When(y => (y.ParentId != null) && (y.ParentId > 0), () =>
            {
                RuleFor(t => t.ParentId)
                    .GreaterThan(0).WithMessage("کد والد وارد شده نمی تواند برابر یا کمتر از0 باشد.");
            });

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(250).WithMessage("عنوان وارد شده نمی تواند بیشتر از 250 کاراکتر باشد.");

            RuleFor(x => x.UniqueName)
                .NotEmpty().WithMessage("نام اختصاصی نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("نام اختصاصی وارد شده نمی تواند بیشتر از 50 کاراکتر باشد.");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("مقدار نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("مقدار وارد شده نمی تواند بیشتر از 50 کاراکتر باشد.");

            RuleFor(x => x.OrderIndex)
                .NotEmpty().WithMessage("ترتیب نمایش نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("ترتیب نمایش وارد شده نمی تواند برابر یا کمتر از0 باشد.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("کد نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("کد وارد شده نمی تواند بیشتر از 50 کاراکتر باشد.");
        }
    }
}
