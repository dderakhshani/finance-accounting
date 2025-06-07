using Eefa.Bursary.Application.Interfaces;
using Eefa.Bursary.Application.Models;
using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Common.Abstraction;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Eefa.Bursary.Application.Queries.TejaratBankAccount
{
    public class GetTejaratBalanceQuery : Pagination, IGetTejaratBalanceQuery,SearchableQuery
    {
        public DateTime? FromPersianDate { get; set; }
        public DateTime? ToPersianDate { get; set; }
        public string AccountNumber { get; set; }
        public List<QueryCondition> Conditions { get; set; }
        private readonly IRepository<Domain.Entities.AccountReferences> _accountReference;
        private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> _financialRequest;
        private readonly ITejaratBankServices _tejaratBankServices;
        private readonly ITransactionBankServices _syncServices;
        private readonly IRepository<BankTransactions> _bankTransaction;
        private readonly IRepository<VouchersHead> _voucherHead;
        private readonly ILogger<GetTejaratBalanceQuery> _logger;

        public GetTejaratBalanceQuery(IRepository<Domain.Entities.AccountReferences> accountReference, ITejaratBankServices tejaratBankServices, IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRequest, ITransactionBankServices syncServices, IRepository<BankTransactions> bankTransaction, IRepository<VouchersHead> voucherHead, ILogger<GetTejaratBalanceQuery> logger)
        {
            _accountReference = accountReference;
            _tejaratBankServices = tejaratBankServices;
            _financialRequest = financialRequest;
            _syncServices = syncServices;
            _bankTransaction = bankTransaction;
            _voucherHead = voucherHead;
            _logger = logger;
        }

        public async Task<ServiceResult<List<AccountBalanceModel>>> GetAll(GetTejaratBalanceQuery query)
        {

            try
            {
                var fromPersianDate = query.FromPersianDate.Value.AddHours(4).ToString("yyyy/MM/dd", new CultureInfo("fa-IR")).Trim().Replace("/", "");
                var toPersianDate = query.ToPersianDate.Value.AddHours(4).ToString("yyyy/MM/dd", new CultureInfo("fa-IR")).Trim().Replace("/", "");


                if (query.AccountNumber == "132924007")
                    await _syncServices.SyncZafarBankTransactions();
                else
                    await _syncServices.SyncArdakanBankTransactions();

                var fDate = long.Parse(fromPersianDate);
                var tDate = long.Parse(toPersianDate);
                var data = (from b in _bankTransaction.GetAll()
                            where (b.TransactionDate >= fDate && b.TransactionDate <= tDate && b.TransactionType == "C" && b.AccountNumber == query.AccountNumber)
                            select new AccountBalanceModel
                            {

                                TransactionType = b.TransactionType,
                                OperationCode = b.OperationCode,
                                balance = b.Balance,
                                TransactionUniqueId = b.TransactionUniqueId,
                                TransactionDate = b.TransactionDate,
                                TransactionTime = b.TransactionTime,
                                BranchCode = b.BranchCode,
                                TransactionAmountCredit = b.TransactionAmountCredit,
                                TransactionAmountDebit = b.TransactionAmountDebit,
                                DocNumber = b.DocNumber,
                                OrigKey = b.OrigKey,
                                PayId1 = b.PayId1,
                                Description = b.Description,
                                TransactionAmount = b.TransactionAmount,
                                financialRequestId = b.FinancialRequestId,


                            }).FilterQuery(query.Conditions); //.ToListAsync();

                var result = await data.ToListAsync();

                result.ForEach(x => x.init());

                // var result = await _tejaratBankServices.CallGetCreditAccountBalance(fromPersianDate, toPersianDate,query.AccountNumber,"C");
                var accountReferences = await (from ar in _accountReference.GetAll()
                                               where ar.DepositId != null
                                               select ar).ToListAsync();

                foreach (var item in result)
                {
                    item.BankName = query.AccountNumber == "132924007" ? "تجارت ظفر" : "تجارت اردکان";
                    if (!string.IsNullOrEmpty(item.PayId1))
                        item.accountReferenceTitle = accountReferences.Where(x => String.Equals(x.DepositId.Trim(), item.PayId1.Trim(), StringComparison.OrdinalIgnoreCase)).Select(x => x.Title).FirstOrDefault();
                    var req = await _financialRequest.GetAll().Where(x => x.Id == item.financialRequestId).FirstOrDefaultAsync();
                    if (req != null)
                    {
                        item.bursaryNo = req.DocumentNo;
                        item.voucherNo = await _voucherHead.GetAll().Where(x => x.Id == req.VoucherHeadId).Select(x => x.VoucherNo).FirstOrDefaultAsync();
                    }
                }

                return ServiceResult<List<AccountBalanceModel>>.Success(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " خطا در دریافت گزارش " + DateTime.Now);
                throw;
            }

        }

    }


}
