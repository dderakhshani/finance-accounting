using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.StartVoucher
{
    public class CreateStartVoucherHeadCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateStartVoucherHeadCommand>, ICommand
    {
        public int VoucherNo { get; set; }
        public int YearId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateStartVoucherHeadCommand, Data.Entities.Year>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateStartVoucherHeadCommandHandler : IRequestHandler<CreateStartVoucherHeadCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public IUnitOfWork _accountingUnitOfWork { get; }
        public CreateStartVoucherHeadCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor, IUnitOfWork accountingUnitOfWork)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
            _accountingUnitOfWork = accountingUnitOfWork;
        }


        public async Task<ServiceResult> Handle(CreateStartVoucherHeadCommand request, CancellationToken cancellationToken)
        {

            if (request.YearId == _currentUserAccessor.GetYearId())
                return ServiceResult.Failure(null, "سال مالی جاری سیستم شما با سال مالی انتخابی در سند افتتاحیه یکسان است");


            var startCodeVocuerGroup = await _repository.GetQuery<Data.Entities.CodeVoucherGroup>()
                .Where(x => x.Code == "2200").FirstOrDefaultAsync();

            var startVoucher = await _repository.GetQuery<Data.Entities.VouchersHead>().Where(x => x.YearId == _currentUserAccessor.GetYearId() && x.CodeVoucherGroupId == startCodeVocuerGroup.Id).FirstOrDefaultAsync();

            if (startVoucher != null)
            {
                var isRemove = await RemoveStartVoucherDocument(startVoucher);
                if (!isRemove)
                    return ServiceResult.Failure(null, "در زمان حذف سند افتتاحیه جاری مشکلی پیش امد");
                
            }

            var endCodeVocuerGroup = await _repository.GetQuery<Data.Entities.CodeVoucherGroup>()
                .FirstOrDefaultAsync(x => x.Code == "2205" , cancellationToken: cancellationToken);


            var lastYearEndVoucherHead = await _accountingUnitOfWork.Set<Data.Entities.VouchersHead>()
                .Include(x => x.VouchersDetails).AsNoTracking().Where(x => x.YearId == request.YearId && x.CodeVoucherGroupId == endCodeVocuerGroup.Id && x.VoucherNo == request.VoucherNo)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (lastYearEndVoucherHead == null)
                return ServiceResult.Failure(null, "سند اختتامیه با شماره سند مربوطه در سال جاری انتخابی یافت نشد");


            //var inputeYear = _mapper.Map<Data.Entities.Year>(request);
            //inputeYear.CompanyId = _currentUserAccessor.GetCompanyId();
            // var year = _repository.Insert(inputeYear);

            var startVoucherHead = new Data.Entities.VouchersHead()
            {
                VoucherNo = 1,
                VoucherDailyId = 1,
                VoucherDate = DateTime.Parse("2025-03-22 12:00:00.0000000"),
                CodeVoucherGroupId = startCodeVocuerGroup.Id,
                CompanyId = _currentUserAccessor.GetCompanyId(),
                VoucherStateId = 1, // دائم
                YearId = _currentUserAccessor.GetYearId(),
                VoucherDescription = "سند افتتاحیه سال 1404",
                TotalCredit = lastYearEndVoucherHead.VouchersDetails.Aggregate(new double(), (c, n) => c + n.Debit),
                TotalDebit = lastYearEndVoucherHead.VouchersDetails.Aggregate(new double(), (c, n) => c + n.Credit),

            };

            _repository.Insert(startVoucherHead);

            int rowIndex = 1;
            foreach (var lastYearEndVouchersDetail in lastYearEndVoucherHead.VouchersDetails)
            {
                var voucherId = 0;
                var voucher = await _repository.GetQuery<Data.Entities.AccountHead>().Where(x => x.Id == lastYearEndVouchersDetail.AccountHeadId).FirstOrDefaultAsync();
                if (voucher != null)
                {

                if (voucher.Code.StartsWith("6") || voucher.Code.StartsWith("7") || voucher.Code.StartsWith("8"))
                    voucherId = 1958;
                }
                var startVouchersDetail = new Data.Entities.VouchersDetail()
                {
                    Voucher = startVoucherHead,
                    AccountHeadId = voucherId == 0 ? lastYearEndVouchersDetail.AccountHeadId : voucherId ,
                    Debit = lastYearEndVouchersDetail.Credit,
                    Credit = lastYearEndVouchersDetail.Debit,
                    VoucherDate = DateTime.UtcNow,
                    AccountReferencesGroupId = lastYearEndVouchersDetail.AccountReferencesGroupId,
                    ReferenceId1 = lastYearEndVouchersDetail.ReferenceId1,
                    VoucherRowDescription = "بابت سند افتتاحیه",
                    RowIndex = rowIndex++,
                    CurrencyAmount = lastYearEndVouchersDetail.CurrencyAmount,
                    CurrencyTypeBaseId = lastYearEndVouchersDetail.CurrencyTypeBaseId,
                    CurrencyFee = lastYearEndVouchersDetail.CurrencyFee,
                    TaxpayerFlag = false,
                };
                 
                _repository.Insert(startVouchersDetail);
            }

          
                if (await _repository.SaveChangesAsync(request.MenueId, cancellationToken) > 0)
                {
                await ExecuteStpUpdateTotalDebitCreditOnVoucherDetail();
                await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[Stp_ENABLE_TRIGGER]", null, new CancellationToken());

                return ServiceResult.Success();
                }
          

            return ServiceResult.Failure();
        }



        private async Task<bool> RemoveStartVoucherDocument(Data.Entities.VouchersHead startVoucher)
        {
            _repository.Delete(startVoucher);
            return await _repository.SaveChangesAsync(1, new CancellationToken()) > 0;
        }


        private async Task ExecuteStpUpdateTotalDebitCreditOnVoucherDetail()
        {
            await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[Stp_DISABLE_TRIGGER]", null, new CancellationToken());
            await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[StpUpdateTotalDebitCreditOnVoucherDetail]", null, new CancellationToken());

        }

    }
}
