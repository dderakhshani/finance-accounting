using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.Employee.Command.Create
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.PersonId)
                .NotEmpty().WithMessage("کد شخص نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("کد شخص نمی تواند برابر یا کمتر از 0 باشد.");

            //RuleFor(x => x.UnitPositionId)
            //    .NotEmpty().WithMessage("جایگاه شغلی نمی تواند خالی باشد.")
            //    .GreaterThan(0).WithMessage("جایگاه شغلی نمی تواند برابر یا کمتر از 0 باشد.");

            RuleFor(x => x.EmployeeCode)
                .NotEmpty().WithMessage("کد پرسنلی نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("کد پرسنلی نمی تواند بیشتر از 50 کاراکتر باشد.");

            //RuleFor(x => x.EmploymentDate)
            //    .NotEmpty().WithMessage("تاریخ استخدام نمی تواند خالی باشد.");
        }
    }
}