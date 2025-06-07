using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.CommandQueries.Year.Command.Create
{
    public class CreateYearValidator : AbstractValidator<CreateYearCommand>
    {
        public CreateYearValidator()
        {
            RuleFor(x => x.YearName)
                .NotEmpty().WithMessage("نام سال مالی جدید نمی تواند خالی باشد.")
                .GreaterThan(0).WithMessage("نام سال مالی جدید نمی تواند برابر یا کمتر از 0 باشد.");

            RuleFor(x => x.FirstDate)
                .NotEmpty().WithMessage("تاریخ شروع سال مالی نمی تواند خالی باشد.");
            //.MustAsync(ValidateFirstDate).WithMessage("");

            RuleFor(x => x.LastDate)
                .NotEmpty().WithMessage("تاریخ پایان سال مالی نمی تواند خالی باشد.");
                //.MustAsync(ValidateLastDate).WithMessage("");


            //When(y => y.LastEditableDate != null, () =>
            //{
            //    RuleFor(t => t.LastEditableDate)
            //        .MustAsync(ValidateLastEditableDate).WithMessage("");
            //});

            //RuleFor(x => x.CompanyId)
            //    .NotEmpty().WithMessage("کد شرکت نمی تواند خالی باشد.")
            //    .GreaterThan(0).WithMessage("کد شرکت نمی تواند برابر یا کمتر از 0 باشد.");
        }

        //private async Task<bool> ValidateLastEditableDate(DateTime? nullable, CancellationToken token)
        //{
        //    throw new NotImplementedException();
        //}

        //private async Task<bool> ValidateLastDate(DateTime time, CancellationToken token)
        //{
        //    throw new NotImplementedException();
        //}

        //private async Task<bool> ValidateFirstDate(DateTime time, CancellationToken token)
        //{
        //    throw new NotImplementedException();
        //}
    }
}