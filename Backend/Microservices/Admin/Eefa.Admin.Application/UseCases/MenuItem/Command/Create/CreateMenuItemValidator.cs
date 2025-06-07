using FluentValidation;

namespace Eefa.Admin.Application.CommandQueries.MenuItem.Command.Create
{
    public class CreateMenuItemValidator : AbstractValidator<CreateMenuItemCommand>
    {
        public CreateMenuItemValidator()
        {
            When(y => (y.ParentId != null) && (y.ParentId > 0), () =>
            {
                RuleFor(t => t.ParentId)
                    .GreaterThan(0).WithMessage("کد والد نمی تواند برابر یا کمتر از 0 باشد.");
            });

            When(y => (y.PermissionId != null) && (y.PermissionId > 0), () =>
            {
                RuleFor(t => t.PermissionId)
                    .GreaterThan(0).WithMessage("کد دسترسی نمی تواند برابر یا کمتر از 0 باشد.");
            });

            When(y => (y.OrderIndex != null) && (y.OrderIndex > 0), () =>
            {
                RuleFor(t => t.OrderIndex)
                    .GreaterThan(0).WithMessage("ترتیب نمایش نمی تواند برابر یا کمتر از 0 باشد.");
            });

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان منو نمی تواند خالی باشد.")
                .MaximumLength(100).WithMessage("عنوان منو نمی تواند بیشتر از 100 کاراکتر باشد.");

            When(y => !string.IsNullOrEmpty(y.ImageUrl), () =>
            {
                RuleFor(t => t.ImageUrl)
                    .MaximumLength(50).WithMessage("لینک تصویر نمی تواند بیشتر از 50 کاراکتر باشد.");
            });

            When(y => !string.IsNullOrEmpty(y.HelpUrl), () =>
            {
                RuleFor(t => t.HelpUrl)
                    .MaximumLength(100).WithMessage("لینک صفحه راهنما نمی تواند بیشتر از 100 کاراکتر باشد.");
            });

            When(y => !string.IsNullOrEmpty(y.FormUrl), () =>
            {
                RuleFor(t => t.FormUrl)
                    .MaximumLength(100).WithMessage("لینک فرم نمی تواند بیشتر از 100 کاراکتر باشد.");
            });

            When(y => !string.IsNullOrEmpty(y.PageCaption), () =>
            {
                RuleFor(t => t.PageCaption)
                    .MaximumLength(100).WithMessage("عنوان صفحه نمی تواند بیشتر از 100 کاراکتر باشد.");
            });
        }
    }
}