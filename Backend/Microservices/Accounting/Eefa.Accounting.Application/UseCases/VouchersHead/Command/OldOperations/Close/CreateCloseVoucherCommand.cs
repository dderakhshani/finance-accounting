using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Eefa.Accounting.Application.UseCases.AccountHead.Model;
using Eefa.Accounting.Application.UseCases.VouchersHead.Utility;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.Close
{
    public class CreateCloseVoucherCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {

        public bool ReplaceCloseVoucherFlag { get; set; }

    }

    public class CreateCloseVoucherCommandHandler : IRequestHandler<CreateCloseVoucherCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public IUnitOfWork _accountingUnitOfWork { get; }
        public CreateCloseVoucherCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor, IUnitOfWork accountingUnitOfWork)
        {
            _accountingUnitOfWork = accountingUnitOfWork;
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }
        public async Task<ServiceResult> Handle(CreateCloseVoucherCommand request, CancellationToken cancellationToken)
        {
            await ExecuteStpUpdateTotalDebitCreditOnVoucherDetail();

            var TempDocuments = await GetExistTempDocuments(cancellationToken);

            if (TempDocuments.Count > 0 && !request.ReplaceCloseVoucherFlag)
            {
                return ServiceResult.Failure(null, "ابتدا باید سند قبلی را حذف نمایید");
            }

            if (request.ReplaceCloseVoucherFlag && TempDocuments.Count > 0)
            {

                var isRemoveTempDocuments = await RemoveLastTempsDocuments(TempDocuments );
              
                if (isRemoveTempDocuments == false)
                    return ServiceResult.Failure(null, "مشکلی پیش آمد");
            }

            var voucherHead = await SetVoucherHead(cancellationToken);

            var parentAccountHeadIds = await _repository.GetQuery<Data.Entities.AccountHeadCloseCode>()
                .Where(x => x.IsDeleted == false)
                .Select(x => x.ParentAccountHeadId)
                .Distinct()
                .ToListAsync(cancellationToken);

            foreach (var parentAccountHeadId in parentAccountHeadIds)
            {
                var accountHeadCodes = await _repository.GetQuery<Data.Entities.AccountHeadCloseCode>()
                    .Where(x => x.ParentAccountHeadId == parentAccountHeadId && x.IsDeleted == false)
                    .ToListAsync(cancellationToken);

                var recoreds = new List<Data.Entities.VouchersDetail>();

                string description = "";
                int accountClosingCodeId = 0;
                bool debitCreditStatus = false;

                foreach (var accountHeadCode in accountHeadCodes)
                {
                    var accountHeadId = accountHeadCode.AccountHeadId;

                    accountClosingCodeId = accountHeadCode.AccountClosingCodeId;
                    debitCreditStatus = accountHeadCode.Debit_CreditStatus;
                    description = accountHeadCode.Description;

                    var vouchersDetails = await _repository.GetQuery<Data.Entities.VouchersDetail>()
                        .Include(x => x.Voucher)
                        .Where(vd => vd.AccountHeadId == accountHeadId
                                     && vd.IsDeleted == false
                                     && vd.Voucher.YearId == _currentUserAccessor.GetYearId()
                                     && vd.Voucher.CompanyId == _currentUserAccessor.GetCompanyId()
                                    //  && vd.AccountReferencesGroupId != 5 
                                     )
                                    .Select(vd => new
                                        {
                                            GroupKey = vd.ReferenceId1 ?? vd.AccountHeadId,
                                            vd.AccountHeadId,
                                            vd.Debit,
                                            vd.Credit,
                                            vd.AccountReferencesGroupId,
                                            vd.ReferenceId1,
                                            vd.Level2
                                         }).ToListAsync(cancellationToken);


                    if (vouchersDetails.Count > 0)
                    {
                        var groupedRecords = vouchersDetails
                            .GroupBy(v => new
                            {
                                v.AccountReferencesGroupId,
                                GroupKey = v.ReferenceId1 ?? v.AccountHeadId
                            })
                            .Select(group => new
                            {
                                AccountReferencesGroupId = group.Key.AccountReferencesGroupId,
                                GroupKey = group.Key.GroupKey,
                                AccountHeadId = group.FirstOrDefault().AccountHeadId,
                                TotalDebit = group.Where(vd => vd.Debit > 0).Sum(vd => vd.Debit),
                                TotalCredit = group.Where(vd => vd.Credit > 0).Sum(vd => vd.Credit),
                                ReferenceId = group.FirstOrDefault()?.ReferenceId1
                            }).ToList();
                    

                    var rowIndex = 0;

                        foreach (var group in groupedRecords)
                        {
                                 var newVoucherDetail = new Data.Entities.VouchersDetail()
                                {
                                    Voucher = voucherHead,
                                    AccountHeadId = group.AccountHeadId,
                                    Debit = group.TotalCredit > group.TotalDebit ? group.TotalCredit - group.TotalDebit : 0,
                                    Credit = group.TotalDebit > group.TotalCredit ? group.TotalDebit - group.TotalCredit : 0,
                                    VoucherDate = DateTime.UtcNow,
                                    IsDeleted = false,
                                    AccountReferencesGroupId = group.AccountReferencesGroupId,
                                    ReferenceId1 = group.ReferenceId,
                                    VoucherRowDescription = accountHeadCode.Description,
                                    RowIndex = rowIndex++,
                                    TaxpayerFlag = false,

                                };
                                _repository.Insert(newVoucherDetail);
                                recoreds.Add(newVoucherDetail);
                            }
                         
                    }
                }
                var totalDebit = recoreds.Sum(g => g.Debit);
                var totalCredit = recoreds.Sum(g => g.Credit);
                var difference = totalDebit - totalCredit;
                if (difference < 0)
                    difference = -1 * difference;

                var finalVoucherDetail = _repository.Insert(new Data.Entities.VouchersDetail()
                {
                    Voucher = voucherHead,
                    AccountHeadId = accountClosingCodeId,
                    Debit = !debitCreditStatus ? difference : 0,
                    Credit = debitCreditStatus ? difference : 0,
                    VoucherDate = DateTime.UtcNow,
                    VoucherRowDescription = description,
                    IsDeleted = false,
                    TaxpayerFlag = false,
                });
            }
            if (await _repository.SaveChangesAsync(request.MenueId, cancellationToken) > 0)
            {

                await ExecuteStpUpdateTotalDebitCreditOnVoucherDetail();
                await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[Stp_ENABLE_TRIGGER]", null, new CancellationToken());

                return ServiceResult.Success();
            }
            return ServiceResult.Failure();

        }


        private async Task<List<Data.Entities.VouchersHead>> GetExistTempDocuments(CancellationToken cancellationToken)
        {
            var previousCloseCodeVoucher = await _repository.GetQuery<Data.Entities.VouchersHead>()
                    .Include(x => x.VouchersDetails)
                    .Where(x => x.CodeVoucherGroupId == 2242 && x.YearId == _currentUserAccessor.GetYearId() && x.CompanyId == _currentUserAccessor.GetCompanyId())
                    .ToListAsync(cancellationToken: cancellationToken);


            return previousCloseCodeVoucher;
        }

        private async Task<bool> RemoveLastTempsDocuments(List<Data.Entities.VouchersHead> TempDocuments )
        {
            if (TempDocuments.Count > 0)
            {
                foreach (var vouchersDetails in TempDocuments.Select(x => x.VouchersDetails))
                {
                    foreach (var item in vouchersDetails)
                        _repository.Delete(item);
                }

                foreach (var item in TempDocuments)
                    _repository.Delete(item);
            }

            var isRemoveEndVoucher =await RemoveEndVoucherDocument();

            if(isRemoveEndVoucher)
            return await _repository.SaveChangesAsync(1, new CancellationToken()) > 0;
            else return false;
        }
        private async Task<bool> RemoveEndVoucherDocument()
        {
            var endVoucher = await _repository.GetQuery<Data.Entities.VouchersHead>().Where(x => x.CodeVoucherGroupId == 2243).FirstOrDefaultAsync();
            if (endVoucher != null)
                _repository.Delete(endVoucher);

            return true;
           // return await _repository.SaveChangesAsync(menuId, new CancellationToken()) > 0;
        }


        private async Task<Data.Entities.VouchersHead> SetVoucherHead(CancellationToken cancellationToken)
        {
            int voucherNo = await VoucherNo.GetNewVoucherNo(_repository, _currentUserAccessor, DateTime.UtcNow, null);
            var lastVoucherId = (await _repository.GetQuery<Data.Entities.VouchersHead>().Where(x =>
    x.VoucherDate.Date == DateTime.Now).OrderBy(x => x.Id).LastOrDefaultAsync(cancellationToken))?.VoucherDailyId ?? 0;


            voucherNo++;
            var getCurrentYear = _currentUserAccessor.GetYearId();
            DateTime getDate = await _repository.GetQuery<Data.Entities.Year>().Where(x=>x.Id == getCurrentYear).Select(x=>x.LastDate).FirstOrDefaultAsync();  

            var voucherHead = new Data.Entities.VouchersHead()
            {
                VoucherNo = voucherNo,
                VoucherDailyId = lastVoucherId + 1,
                VoucherDate = getDate,
                CodeVoucherGroupId = 2242,
                CompanyId = _currentUserAccessor.GetCompanyId(),
                VoucherStateId = 1, // دائم
                YearId = _currentUserAccessor.GetYearId(),
                VoucherDescription = "بابت بستن حساب های موقت",
                TotalCredit = 0,
                TotalDebit = 0
            };

            _repository.Insert(voucherHead);

            return voucherHead;
        }

        private async Task ExecuteStpUpdateTotalDebitCreditOnVoucherDetail()
        {
            await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[Stp_DISABLE_TRIGGER]", null, new CancellationToken());
             await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[StpUpdateTotalDebitCreditOnVoucherDetail]", null, new CancellationToken());

        }
        //public async Task<ServiceResult> Handle(CreateCloseVoucherCommand request, CancellationToken cancellationToken)
        //{
        //    //if (request.CloserAccountHeadId == 0)
        //    //{
        //    //    var settings =
        //    //        await new SystemSettings(_repository).Get(SubSystemType.AccountingSettings);

        //    //    foreach (var baseValue in settings)
        //    //    {
        //    //        if (baseValue.UniqueName == "CloseTemproryAccounts")
        //    //        {
        //    //            request.CloserAccountHeadId = int.Parse(baseValue.Value);
        //    //            break;
        //    //        }
        //    //    }
        //    //}


        //    var closeCodeVocuerGroup = await _repository.GetQuery<Data.Entities.CodeVoucherGroup>()
        //        .FirstOrDefaultAsync(x => x.UniqueName == "temproraryEnd", cancellationToken: cancellationToken);



        //    var previousCloseCodeVoucher = await _repository.GetQuery<Data.Entities.VouchersHead>()
        //        .FirstOrDefaultAsync(x => x.CodeVoucherGroupId == closeCodeVocuerGroup.Id, cancellationToken: cancellationToken);

        //    if (previousCloseCodeVoucher != null)
        //    {
        //        if (request.ReplaceCloseVoucherFlag)
        //        {
        //            if (previousCloseCodeVoucher.VouchersDetails != null)
        //                foreach (var vouchersDetail in previousCloseCodeVoucher.VouchersDetails)
        //                {
        //                    _repository.Delete(vouchersDetail);
        //                }

        //            _repository.Delete(previousCloseCodeVoucher);
        //        }
        //        else
        //        {
        //            throw new Exception("CloseVoucherIsAlreadyExists");
        //        }
        //    }



        //    //var temproraryAccoundHeads = await
        //    //    _repository.GetQuery<Data.Entities.AccountHead>()
        //    //        .Where(x => x.TransferId == 1)
        //    //        .ToListAsync(cancellationToken);

        //    var tempAccountHeads = await _repository.GetQuery<Data.Entities.AccountHeadCloseCode>().ToListAsync();



        //    foreach(var ah in tempAccountHeads)
        //    {

        //        var voucherDetails = await _repository.GetQuery<Data.Entities.VouchersDetail>().Where(x => x.IsDeleted != true).ToListAsync();


        //    }




        //    var sumCredit = tempAccountHeads.Sum(temproraryAccoundHead =>
        //           _repository.GetQuery<Data.Entities.VouchersDetail>()
        //        .Where(x => x.AccountHeadId == temproraryAccoundHead.Id)
        //        .Select(x => x.Credit)
        //        .ToList()
        //        .Aggregate(new double(), (c, n) => c + n));

        //    var sumDebit = tempAccountHeads.Sum(temproraryAccoundHead =>
        //        _repository.GetQuery<Data.Entities.VouchersDetail>()
        //            .Where(x => x.AccountHeadId == temproraryAccoundHead.Id)
        //            .Select(x => x.Debit)
        //            .ToList()
        //        .Aggregate(new double(), (c, n) => c + n));


        //    var lastVoucherId = (await _repository.GetQuery<Data.Entities.VouchersHead>().Where(x =>
        //        x.VoucherDate.Date == DateTime.Now).OrderBy(x => x.Id).LastOrDefaultAsync(cancellationToken))?.VoucherDailyId ?? 0;

        //    var voucherNo = await VoucherNo.GetNewVoucherNo(_repository, _currentUserAccessor, DateTime.Now, null);
        //    var closeVoucherHead = _repository.Insert(new Data.Entities.VouchersHead()
        //    {
        //        VoucherNo = voucherNo,
        //        VoucherDailyId = lastVoucherId + 1,
        //        VoucherDate = DateTime.Now,
        //        CodeVoucherGroupId = closeCodeVocuerGroup.Id,
        //        CompanyId = _currentUserAccessor.GetCompanyId(),
        //        VoucherStateId = 3, // دائم
        //        YearId = _currentUserAccessor.GetYearId(),
        //        VoucherDescription = "بابت بستن حسابهای موقت",
        //        TotalCredit = sumDebit,
        //        TotalDebit = sumCredit
        //    });

        //    _repository.Insert(new Data.Entities.VouchersDetail()
        //    {
        //        Voucher = closeVoucherHead.Entity,
        //        AccountHeadId = request.CloserAccountHeadId,
        //        Debit = sumCredit,
        //        Credit = sumDebit,
        //        VoucherRowDescription = "بابت بستن حسابهای موقت"
        //    });



        //    if (request.SaveChanges)
        //    {
        //        if (await _repository.SaveChangesAsync(request.MenueId, cancellationToken) > 0)
        //        {
        //            return ServiceResult.Success(closeVoucherHead.Entity);
        //        }
        //    }
        //    else
        //    {
        //        return ServiceResult.Success(closeVoucherHead.Entity);
        //    }

        //    return ServiceResult.Failure();
        //}
    }
}
