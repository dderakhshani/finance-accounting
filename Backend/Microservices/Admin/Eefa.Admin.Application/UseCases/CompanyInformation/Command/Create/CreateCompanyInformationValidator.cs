using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.CompanyInformation.Command.Create
{
    public class CreateCompanyInformationValidator : AbstractValidator<CreateCompanyInformationCommand>
    {
        public CreateCompanyInformationValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(250).WithMessage("عنوان نمی تواند بیشتر از 250 کاراکتر باشد.");

            RuleFor(x => x.UniqueName)
                .NotEmpty().WithMessage("نام اختصاصی نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("نام اختصاصی نمی تواند بیشتر از 50 کاراکتر باشد.");

            When(y => y.ExpireDate != null, ()  => 
            {
                //RuleFor(t => t.ExpireDate);
            });

            RuleFor(x => x.MaxNumOfUsers)
                .NotEmpty().WithMessage("حداکثر تعداد کاربر نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("حداکثر تعداد کاربر نمی تواند برابر یا کمتر از 0 باشد.");

            When(y => !string.IsNullOrEmpty(y.Logo), () =>
            {
                RuleFor(t => t.Logo)
                    .MaximumLength(300).WithMessage("آدرس لوگو نمی تواند بیشتر از 300 کاراکتر باشد.");
            });
        }
    }
}