using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.VouchersHead.Utility;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.OldOperations.End
{
    public class EndVoucherCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public bool ReplaceEndVoucherFlag { get; set; } 
    }

    public class EndVoucherCommandHandler : IRequestHandler<EndVoucherCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public IUnitOfWork _accountingUnitOfWork { get; }
        public EndVoucherCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor, IUnitOfWork accountingUnitOfWork)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
            _accountingUnitOfWork = accountingUnitOfWork;
        }

        public async Task<ServiceResult> Handle(EndVoucherCommand request, CancellationToken cancellationToken)
        {

            await ExecuteStpUpdateTotalDebitCreditOnVoucherDetail();

            var previousEndVoucher =await GetExistclosingDocuments();

            if (previousEndVoucher != null && request.ReplaceEndVoucherFlag)
                await RemoveExistClosingDocuments(previousEndVoucher);

            var endVoucherHead = new Data.Entities.VouchersHead()
            {
                VoucherNo = await VoucherNo.GetNewVoucherNo(_repository, _currentUserAccessor, DateTime.UtcNow, null),
                VoucherDailyId = 1,
                VoucherDate = DateTime.Parse("2025-03-19 12:00:00.0000000"),
                CodeVoucherGroupId = 2243,
                CompanyId = _currentUserAccessor.GetCompanyId(),
                VoucherStateId = 1,
                YearId = _currentUserAccessor.GetYearId(),
                VoucherDescription = "سند اختتامیه",
            };

            _repository.Insert(endVoucherHead);


            var permanentAccountHeads = await _repository.GetQuery<Data.Entities.AccountHead>()
                .Where(x => x.LastLevel && x.TransferId == 2)
                .ToListAsync(cancellationToken);

            foreach (var permanentAccountHead in permanentAccountHeads)
            {
                var vouchersDetails = await _repository.GetQuery<Data.Entities.VouchersDetail>()
                    .Include(x => x.Voucher)
                    .Where(vd =>
                        vd.AccountHeadId == permanentAccountHead.Id &&
                        !vd.IsDeleted &&
                        vd.Voucher.YearId == _currentUserAccessor.GetYearId() &&
                        vd.Voucher.CompanyId == _currentUserAccessor.GetCompanyId() )
                    .Select(vd => new
                    {
                        GroupKey = vd.ReferenceId1 ?? vd.AccountHeadId,
                        vd.AccountHeadId,
                        vd.Debit,
                        vd.Credit,
                        vd.AccountReferencesGroupId,
                        vd.ReferenceId1,
                        vd.Level2,
                        vd.CurrencyTypeBaseId,
                        CurrencyKey = vd.AccountReferencesGroupId != 29 ? vd.CurrencyBaseTypeBaseValue : null,
                        CurrencyAmount = vd.AccountReferencesGroupId != 29 ? vd.CurrencyAmount : null
                    }).ToListAsync(cancellationToken);

                if (vouchersDetails.Count > 0)
                {
                    var groupedRecords = vouchersDetails
                        .GroupBy(v => new
                        {
                            v.AccountReferencesGroupId,
                            GroupKey = v.ReferenceId1 ?? v.AccountHeadId,
                            v.CurrencyKey 
                        })
                        .Select(group => new
                        {
                            AccountReferencesGroupId = group.Key.AccountReferencesGroupId,
                            GroupKey = group.Key.GroupKey,
                            CurrencyBaseTypeBaseValue = group.Key.CurrencyKey,
                            AccountHeadId = group.First().AccountHeadId,
                            TotalDebit = group.Where(vd => vd.Debit > 0).Sum(vd => vd.Debit),
                            TotalCredit = group.Where(vd => vd.Credit > 0).Sum(vd => vd.Credit),
                            ReferenceId = group.FirstOrDefault()?.ReferenceId1,
                            TotalCurrencyAmount = group.Where(vd => vd.Debit > 0).Sum(vd => vd.CurrencyAmount ?? 0)
                                                     - group.Where(vd => vd.Credit > 0).Sum(vd => vd.CurrencyAmount ?? 0)
                        }).ToList();

                    var rowIndex = 0;

                    foreach (var group in groupedRecords)
                    {
                        if (group.TotalCredit - group.TotalDebit != 0)
                        {
                            var newVoucherDetail = new Data.Entities.VouchersDetail()
                            {
                                Voucher = endVoucherHead,
                                AccountHeadId = group.AccountHeadId,
                                Debit = group.TotalCredit > group.TotalDebit ? group.TotalCredit - group.TotalDebit : 0,
                                Credit = group.TotalDebit > group.TotalCredit ? group.TotalDebit - group.TotalCredit : 0,
                                VoucherDate = DateTime.UtcNow,
                                IsDeleted = false,
                                AccountReferencesGroupId = group.AccountReferencesGroupId,
                                ReferenceId1 = group.ReferenceId,
                                CurrencyBaseTypeBaseValue = group.CurrencyBaseTypeBaseValue,
                                CurrencyAmount = Math.Abs(group.TotalCurrencyAmount),
                                CurrencyFee =( group.CurrencyBaseTypeBaseValue == null || group.CurrencyBaseTypeBaseValue.Id == 28306) ? 0 :4000,
                                VoucherRowDescription = "بابت سند اختتامیه",
                                RowIndex = rowIndex++,
                                TaxpayerFlag = false,
                            };
                            _repository.Insert(newVoucherDetail);
                        }
                    }
                }
            }

            //foreach (var permanentAccountHead in permanentAccountHeads)
            //{

            //    var vouchersDetails = await _repository.GetQuery<Data.Entities.VouchersDetail>()
            //            .Include(x => x.Voucher)
            //            .Where(vd => vd.AccountHeadId == permanentAccountHead.Id
            //     && vd.IsDeleted == false
            //     && vd.Voucher.YearId == _currentUserAccessor.GetYearId()
            //     && vd.Voucher.CompanyId == _currentUserAccessor.GetCompanyId()).Select(vd => new
            //     {
            //         GroupKey = vd.ReferenceId1 ?? vd.AccountHeadId,
            //         vd.AccountHeadId,
            //         vd.Debit,
            //         vd.Credit,
            //         vd.AccountReferencesGroupId,
            //         vd.ReferenceId1,
            //         vd.Level2,
            //         vd.CurrencyBaseTypeBaseValue,
            //         vd.CurrencyAmount

            //     }).ToListAsync(cancellationToken);


            // //   if (vouchersDetails.Count > 0)
            //  //  {
            //        //var groupedRecords = vouchersDetails
            //        //    .GroupBy(v => new
            //        //    {
            //        //        v.AccountReferencesGroupId,
            //        //        GroupKey = v.ReferenceId1 ?? v.AccountHeadId
            //        //    })
            //        //    .Select(group => new
            //        //    {
            //        //        AccountReferencesGroupId = group.Key.AccountReferencesGroupId,
            //        //        GroupKey = group.Key.GroupKey,
            //        //        AccountHeadId = group.FirstOrDefault().AccountHeadId,
            //        //        TotalDebit = group.Where(vd => vd.Debit > 0).Sum(vd => vd.Debit),
            //        //        TotalCredit = group.Where(vd => vd.Credit > 0).Sum(vd => vd.Credit),
            //        //        ReferenceId = group.FirstOrDefault()?.ReferenceId1
            //        //    }).ToList();


            //        if (vouchersDetails.Count > 0)
            //        {
            //            var groupedRecords = vouchersDetails
            //                .GroupBy(v => new
            //                {
            //                    v.AccountReferencesGroupId,
            //                    GroupKey = v.ReferenceId1 ?? v.AccountHeadId,
            //                    v.CurrencyBaseTypeBaseValue
            //                })
            //                .Select(group => new
            //                {
            //                    AccountReferencesGroupId = group.Key.AccountReferencesGroupId,
            //                    GroupKey = group.Key.GroupKey,
            //                    CurrencyBaseTypeBaseValue = group.Key.CurrencyBaseTypeBaseValue,
            //                    AccountHeadId = group.First().AccountHeadId,
            //                    TotalDebit = group.Where(vd => vd.Debit > 0).Sum(vd => vd.Debit),
            //                    TotalCredit = group.Where(vd => vd.Credit > 0).Sum(vd => vd.Credit),
            //                    ReferenceId = group.FirstOrDefault()?.ReferenceId1,
            //                    TotalCurrencyAmount = group.Where(vd => vd.Debit > 0).Sum(vd => vd.CurrencyAmount ?? 0)  - group.Where(vd => vd.Credit > 0).Sum(vd => vd.CurrencyAmount ?? 0)
            //                }).ToList();


            //            var rowIndex = 0;

            //            foreach (var group in groupedRecords)
            //            {
            //            //var cf = 0;
            //            //if (group.TotalCurrencyAmount != 0)
            //            //{
            //            //    cf = Math.Abs(
            //            //        Convert.ToInt32(
            //            //            group.TotalCredit > group.TotalDebit
            //            //                ? (group.TotalCredit / group.TotalCurrencyAmount)
            //            //                : (group.TotalDebit / group.TotalCurrencyAmount)
            //            //        )
            //            //    );
            //            //}
            //            //else
            //            //{
            //            //    cf = 0; 
            //            //}

            //            if (group.TotalCredit - group.TotalDebit != 0)
            //                {
            //                    var newVoucherDetail = new Data.Entities.VouchersDetail()
            //                    {
            //                        Voucher = endVoucherHead,
            //                        AccountHeadId = group.AccountHeadId,
            //                        Debit = group.TotalCredit > group.TotalDebit ? group.TotalCredit - group.TotalDebit : 0,
            //                        Credit = group.TotalDebit > group.TotalCredit ? group.TotalDebit - group.TotalCredit : 0,
            //                        VoucherDate = DateTime.UtcNow,
            //                        IsDeleted = false,
            //                        AccountReferencesGroupId = group.AccountReferencesGroupId,
            //                        ReferenceId1 = group.ReferenceId,
            //                        CurrencyBaseTypeBaseValue = group.CurrencyBaseTypeBaseValue,
            //                        CurrencyAmount = Math.Abs(group.TotalCurrencyAmount),
            //                        CurrencyFee = 40000,
            //                        VoucherRowDescription = "بابت سند اختتامیه",
            //                        RowIndex = rowIndex++,
            //                    };
            //                    _repository.Insert(newVoucherDetail);
            //                }
            //            }

            //        }
            //}

            if (await _repository.SaveChangesAsync(request.MenueId, cancellationToken) > 0)
            {

                await ExecuteStpUpdateTotalDebitCreditOnVoucherDetail();
                await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[Stp_ENABLE_TRIGGER]", null, new CancellationToken());
                return ServiceResult.Success();
            }
            return ServiceResult.Failure();
        }


        private async Task<Data.Entities.VouchersHead> GetExistclosingDocuments()
        {
            var previousEndVoucher = await _repository.GetQuery<Data.Entities.VouchersHead>().Include(x => x.VouchersDetails)
                .FirstOrDefaultAsync(x => x.CodeVoucherGroupId == 2243, new CancellationToken());

            return previousEndVoucher;
        }


        private async Task ExecuteStpUpdateTotalDebitCreditOnVoucherDetail()
        {
             await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[Stp_DISABLE_TRIGGER]", null, new CancellationToken());
            await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[StpUpdateTotalDebitCreditOnVoucherDetail]", null, new CancellationToken());

        }


        private async Task<bool> RemoveExistClosingDocuments(Data.Entities.VouchersHead previousEndVoucher) {
           
                if (previousEndVoucher.VouchersDetails != null)
                        foreach (var vouchersDetail in previousEndVoucher.VouchersDetails)
                            _repository.Delete(vouchersDetail);

                    _repository.Delete(previousEndVoucher);


            return await _repository.SaveChangesAsync(1, new CancellationToken()) > 0;

        }  


    }
}