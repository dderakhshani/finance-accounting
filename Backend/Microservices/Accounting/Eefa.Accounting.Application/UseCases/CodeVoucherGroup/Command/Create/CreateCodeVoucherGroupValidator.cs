﻿using FluentValidation;
using System.Threading.Tasks;
using System.Threading;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Command.Create
{
    public class CreateCodeVoucherGroupValidator : AbstractValidator<CreateCodeVoucherGroupCommand>
    {
        public IRepository _repository { get; }

        public CreateCodeVoucherGroupValidator(IRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("کد نمی تواند خالی باشد.")
                .MaximumLength(50).WithMessage("تعداد کاراکتر های کد بیش از اندازه است.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نمی تواند خالی باشد.")
                .MaximumLength(100).WithMessage("تعداد کاراکتر های عنوان بیش از اندازه است.");

            RuleFor(x => x.UniqueName)
                .NotEmpty().WithMessage("نام یکتا نمی تواند خالی باشد.")
                .MustAsync(beUniqueName).WithMessage("نام یکتا تکراری است");


            When(y => !string.IsNullOrEmpty(y.BlankDateFormula), () => 
            {
                RuleFor(x => x.BlankDateFormula)
                    .MaximumLength(300).WithMessage("تعداد کاراکتر های فرمول جایگزین خالی بودن بیش اندازه است.");
            });

            When(y => y.ViewId != null, () =>
            {
                RuleFor(x => x.ViewId)
                    .GreaterThan(0).WithMessage("کد گزارش وارد شده معتبر نیست.");
            });

            When(y => y.ExtendTypeId != null, () =>
            {
                RuleFor(x => x.ExtendTypeId)
                    .GreaterThan(0).WithMessage("نوع افزونه وارد شده معتبر نیست.");
            });
        }

        public async Task<bool> beUniqueName(string uniqueName, CancellationToken cancellationToken)
        {
            return !await _repository.GetQuery<Data.Entities.CodeVoucherGroup>()
                                  .AnyAsync(x => x.UniqueName == uniqueName);
        }
    }
}
