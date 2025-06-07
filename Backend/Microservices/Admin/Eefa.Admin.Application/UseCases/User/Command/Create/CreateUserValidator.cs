using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.CommandQueries.User.Command.Create
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.PersonId)
                .NotEmpty().WithMessage("شخص نمی تواند خالی باشد.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("نام کاربری نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("نام کاربری نمی تواند بیشتر از 50 کاراکتر باشد.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("رمز نمی تواند خالی باشد.")
                .MaximumLength(128).WithMessage("رمز نمی تواند بیشتر از 128 کاراکتر باشد.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("تایید رمز نمی تواند خالی باشد.")
                .MaximumLength(128).WithMessage("تایید رمز نمی تواند بیشتر از 128 کاراکتر باشد.");

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