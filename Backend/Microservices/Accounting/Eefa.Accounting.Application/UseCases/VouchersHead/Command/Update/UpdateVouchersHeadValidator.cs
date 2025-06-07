using FluentValidation;
using System.Threading.Tasks;
using System.Threading;
using Library.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Create;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.Create;
using FluentValidation.Results;
using System.Collections.Generic;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Update;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Delete;
using System.Linq;
using Eefa.Accounting.Data.Entities;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.Update
{
    public class UpdateVouchersHeadValidator : AbstractValidator<UpdateVouchersHeadCommand>
    {
        private readonly IRepository repository;
        private readonly ICurrentUserAccessor currentUser;

        public UpdateVouchersHeadValidator(IRepository repository, ICurrentUserAccessor currentUser)
        {
            this.repository = repository;
            this.currentUser = currentUser;
            RuleFor(x => x.VoucherDate)
                .MustAsync(beWithinCurrentYearRange).WithMessage("تاریخ سند باید در سال جاری باشد. ")
                .MustAsync(beEditable).WithMessage("امکان ویرایش سند در سال مالی جاری وجود ندارد. ")
                .MustAsync(isAbleToAddVoucherInFutureDate).WithMessage("امکان ثبت سند به تاریخ آینده وجود ندارد. ");

            RuleFor(x => x.VoucherDescription)
                .NotEmpty().WithMessage("شرح سند نمی تواند خالی باشد.")
                .MaximumLength(1000).WithMessage(" تعداد کاراکتر های شرح سند  بیش از اندازه است.");

            RuleFor(x => x.CodeVoucherGroupId)
                 .GreaterThan(0).WithMessage(".کد گروه سند الزامی است");


            RuleFor(x => x.VoucherStateId)
                .Must(x => x.Equals(1) || x.Equals(2) || x.Equals(3)).WithMessage("وضعیت سند نامعتبر است.");

            RuleFor(x => x.VouchersDetailsCreatedList)
                .CustomAsync(voucherDetailsValidations);
            RuleFor(x => x.VouchersDetailsUpdatedList)
                .CustomAsync(voucherDetailsValidations);
            //RuleFor(x => x)
            // .MustAsync(checkIfVoucherIsGonnaBeEmptyAfterUpdate)
            // .WithMessage("امکان ثبت سند خالی وجود ندارد.");
        }


        private async Task<bool> beWithinCurrentYearRange(DateTime voucherDate, CancellationToken cancellationToken)
        {
            var userYear = await repository.GetQuery<Data.Entities.Year>().FirstOrDefaultAsync(x => x.Id == currentUser.GetYearId());
            return userYear.FirstDate <= voucherDate && userYear.LastDate >= voucherDate;
        }
        private async Task<bool> beEditable(DateTime voucherDate, CancellationToken cancellationToken)
        {
            var userYear = await repository.GetQuery<Data.Entities.Year>().FirstOrDefaultAsync(x => x.Id == currentUser.GetYearId());
            return userYear.IsEditable;
        }
        private async Task<bool> isAbleToAddVoucherInFutureDate(DateTime voucherDate, CancellationToken cancellationToken)
        {
            var voucherInFutureBaseValue = await repository.GetQuery<Data.Entities.BaseValue>().FirstOrDefaultAsync(x => x.UniqueName == "VoucherInFuture");
            return Boolean.Parse(voucherInFutureBaseValue.Value) == true ? true : voucherDate <= DateTime.UtcNow;
        }


        private async Task voucherDetailsValidations(List<UpdateVouchersDetailCommand> voucherDetails, ValidationContext<UpdateVouchersHeadCommand> context, CancellationToken token)
        {
            foreach (var voucherDetail in voucherDetails)
            {
                var defaultCurrnecyTypeId = await repository.GetQuery<BaseValue>().Where(x => x.UniqueName == "IRR").Select(x => x.Id).FirstOrDefaultAsync();

                var propertyName = "VoucherDetail" + voucherDetail.RowIndex.ToString();
                var defaultMessage = "آرتیکل شماره " + voucherDetail.RowIndex.ToString() + ": ";
                if (voucherDetail.AccountHeadId == default) context.AddFailure(new ValidationFailure(propertyName, defaultMessage + "کد حساب نمیتواند خالی باشد."));
                //if (voucherDetail.Level1 == default || voucherDetail.Level2 == default || voucherDetail.Level3 == default) context.AddFailure(new ValidationFailure(propertyName, defaultMessage + "سطوح حساب نامعتبر میباشد."));
                if (voucherDetail.AccountReferencesGroupId != default && voucherDetail.ReferenceId1 == default) context.AddFailure(new ValidationFailure(propertyName, defaultMessage + "در صورت انتخاب گروه تفصیل، انتخاب کد تفصیل شناور الزامی است."));

                if (voucherDetail.AccountHeadId != default && (voucherDetail.AccountReferencesGroupId == default || voucherDetail.ReferenceId1 == default))
                {
                    var hasAvailableGroups = await repository.GetQuery<Data.Entities.AccountHeadRelReferenceGroup>().AnyAsync(x => x.AccountHeadId == voucherDetail.AccountHeadId);
                    if (hasAvailableGroups) context.AddFailure(new ValidationFailure(propertyName, defaultMessage + "گروه تفصیل و تفصیل شناور نمیتواند خالی باشد"));
                }
                if (voucherDetail.AccountHeadId != default && voucherDetail.AccountReferencesGroupId != default)
                {
                    var isRelationValid = await repository.GetQuery<Data.Entities.AccountHeadRelReferenceGroup>().AnyAsync(x => x.AccountHeadId == voucherDetail.AccountHeadId && x.ReferenceGroupId == voucherDetail.AccountReferencesGroupId);
                    if (!isRelationValid) context.AddFailure(new ValidationFailure(propertyName, defaultMessage + "ارتباط کد سرفصل حساب و گروه تفصیل نا معتبر است."));
                }
                if (voucherDetail.ReferenceId1 != default && voucherDetail.AccountReferencesGroupId != default)
                {
                    var isRelationValid = await repository.GetQuery<Data.Entities.AccountReferencesRelReferencesGroup>().AnyAsync(x => x.ReferenceId == voucherDetail.ReferenceId1 && x.ReferenceGroupId == voucherDetail.AccountReferencesGroupId);
                    if (!isRelationValid) context.AddFailure(new ValidationFailure(propertyName, defaultMessage + "ارتباط کد تفصیل شناور و گروه تفصیل نامعتبر است."));
                }

                if (string.IsNullOrEmpty(voucherDetail.VoucherRowDescription)) context.AddFailure(new ValidationFailure(propertyName, defaultMessage + "شرح نمیتواند خالی باشد."));

                if (voucherDetail.Credit == 0 && voucherDetail.Debit == 0 || !(voucherDetail.Credit > 0 || voucherDetail.Debit > 0)) context.AddFailure(new ValidationFailure(propertyName, defaultMessage + "بستانکار یا بدهکار باید مقدار داشته باشد."));
                if (voucherDetail.CurrencyTypeBaseId == default)
                {
                    context.AddFailure(new ValidationFailure(propertyName, defaultMessage + "نوع ارز الزامی است."));
                }
                if ((!(voucherDetail.CurrencyFee > 0) || !(voucherDetail.CurrencyAmount > 0)) && voucherDetail.CurrencyTypeBaseId != defaultCurrnecyTypeId)
                {
                    context.AddFailure(new ValidationFailure(propertyName, defaultMessage + "برای ثبت آرتیکل ارزی وارد کردن نرخ و مقدار ارز الزامی است."));
                }
                if ((voucherDetail.CurrencyFee > 0 || voucherDetail.CurrencyAmount > 0) && voucherDetail.CurrencyTypeBaseId == defaultCurrnecyTypeId)
                {
                    context.AddFailure(new ValidationFailure(propertyName, defaultMessage + "امکان ثبت آرتیکل ریالی  با نرخ و مقدار ارز امکان پذیر نیست."));
                }


            }
        }

        private async Task<bool> checkIfVoucherIsGonnaBeEmptyAfterUpdate(UpdateVouchersHeadCommand voucherHead, CancellationToken token)
        {
            var voucherHeadDetailsCount = await repository.GetQuery<Data.Entities.VouchersHead>().Where(x => x.Id == voucherHead.Id).Select(x => x.VouchersDetails).CountAsync();
            return !(voucherHeadDetailsCount == voucherHead.VouchersDetailsDeletedList.Count && voucherHead.VouchersDetailsCreatedList.Count == 0);
        }

    }
}
