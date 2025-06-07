using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.CommandQueries.User.Command.Update
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("شناسه نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("شناسه نمی تواند برابر یا کمتر از 0 باشد.");

            RuleFor(x => x.PersonId)
                .NotEmpty().WithMessage("شخص نمی تواند خالی باشد.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("نام کاربری نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("نام کاربری نمی تواند بیشتر از 50 کاراکتر باشد.");

            When(y => (y.IsBlocked) && (y.BlockedReasonBaseId != null), () =>
            {
                RuleFor(t => t.BlockedReasonBaseId)
                    .GreaterThan(0).WithMessage("باید یک علت برای قفل کرد حساب کاربر انتخاب کنید.");
            });

            When(y => (y.OneTimePassword != string.Empty) && (y.OneTimePassword != null), () =>
            {
                RuleFor(t => t.OneTimePassword)
                    .MaximumLength(128).WithMessage("رمز موقت نمی تواند بیشتر از 128 کاراکتر باشد.");
            });

            When(y => !string.IsNullOrEmpty(y.Password), () =>
            {
                RuleFor(x => x.Password)
                .MaximumLength(128).WithMessage("رمز نمی تواند بیشتر از 128 کاراکتر باشد.");
            });

            When(y => !string.IsNullOrEmpty(y.Password), () =>
            {
                RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("تایید رمز نمی تواند خالی باشد.")
                .MaximumLength(128).WithMessage("تایید رمز نمی تواند بیشتر از 128 کاراکتر باشد.");
            });


            //RuleFor(x => x.RolesIdList)
            //    .MustAsync(ValidateRolesIdList).WithMessage("");

            //RuleFor(x => x.UserAllowedYears)
            //    .MustAsync(ValidateUserAllowedYears).WithMessage("");
        }

        //private async Task<bool> ValidateUserAllowedYears(ICollection<int> collection, CancellationToken token)
        //{
        //    throw new NotImplementedException();
        //}

        //private async Task<bool> ValidateRolesIdList(IList<int> list, CancellationToken token)
        //{
        //    throw new NotImplementedException();
        //}
    }
}