using Eefa.Bursary.Application;
using Eefa.Bursary.Application.Commands.CustomerReceipt.Create;
using Eefa.Bursary.Application.Interfaces;
using Eefa.Bursary.Application.Models;
using Eefa.Bursary.Application.Queries.FinancialRequest;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common.Common.Abstraction;
using Eefa.Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Infrastructure.BackgroundTasks
{
    public class TransactionBankServices : ITransactionBankServices 
    {

        private readonly ITejaratBankServices _tejaratBankServices;
        private readonly IRepository<BankTransactions> _bankTransactions;
        private readonly IRepository<BankTransactionHead> _bankTransactionHead;
        private readonly IRepository<AccountReferences> _accountReferences;
        private readonly IMediator _mediator;
        private readonly ILogger<TransactionBankServices> _logger;

        public TransactionBankServices(ITejaratBankServices tejaratBankServices, IRepository<BankTransactions> bankTransactions, IRepository<BankTransactionHead> bankTransactionHead, IMediator mediator, IRepository<AccountReferences> accountReferences, ILogger<TransactionBankServices> logger)
        {

            _tejaratBankServices = tejaratBankServices;
            _bankTransactions = bankTransactions;
            _bankTransactionHead = bankTransactionHead;
            _mediator = mediator;
            _accountReferences = accountReferences;
            _logger = logger;
        }

        public async Task SyncArdakanBankTransactions()
        {

            try
            {
                //  var lastRequestTime = _bankTransactionHead.GetAll().Where(x => x.AccountNumber == "2093801264").FirstOrDefault()?.Time;

                var lastRequestTime = _bankTransactionHead.GetAll()
.Where(x => x.AccountNumber == "2093801264")
.Select(x => x.Time)
.FirstOrDefault();

                if (lastRequestTime == null || lastRequestTime.Year ==  1)
                    lastRequestTime = DateTime.UtcNow;

                //var fromDate = lastRequestTime.ToString("yyyy/MM/dd", new CultureInfo("fa-IR")).Trim().Replace("/", "");
                //var toDate = DateTime.UtcNow.ToString("yyyy/MM/dd", new CultureInfo("fa-IR")).Trim().Replace("/", "");

                TimeZoneInfo iranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
                DateTime iranTimeFrom = TimeZoneInfo.ConvertTimeFromUtc(lastRequestTime, iranTimeZone);

                DateTime iranTimeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, iranTimeZone);

                var fromDate = iranTimeFrom.ToString("yyyy/MM/dd", new CultureInfo("fa-IR")).Trim().Replace("/", "");
                var toDate = iranTimeNow.ToString("yyyy/MM/dd", new CultureInfo("fa-IR")).Trim().Replace("/", "");



                var response = await _tejaratBankServices.CallGetAccountBalance(fromDate, toDate, "2093801264");

                if (response.Succeed && response?.ObjResult?.DetailItems != null)
                {
                    var newTransactions = new List<BankTransactions>();

                    using var transaction = await _bankTransactions.BeginTransactionAsync();

                    try
                    {

                        foreach (var item in response.ObjResult.DetailItems)
                        {
                            var exists = _bankTransactions.GetAll().Where(t => t.OrigKey == item.OrigKey && t.IsDeleted != true).Any();

                            if (!exists)
                            {
                                var req = new BankTransactions
                                {
                                    TransactionUniqueId = item.TransactionUniqueId,
                                    AccountNumber = item.BankName,
                                    TransactionAmount = (long)item.TransactionAmount,
                                    TransactionType = item.TransactionType,
                                    TransactionDate = item.TransactionDate,
                                    Balance = item.balance,
                                    TransactionAmountCredit = (decimal)item.TransactionAmountCredit,
                                    TransactionAmountDebit = (decimal)item.TransactionAmountDebit,
                                    OrigKey = item.OrigKey,
                                    OperationCode = item.OperationCode,
                                    TransactionTime = item.TransactionTime,
                                    DocNumber = item.DocNumber,
                                    Description = item.Description,
                                    PayId1 = item.PayId1,
                                    BranchCode = item.BranchCode,
                                    ModifiedAt = DateTime.UtcNow,
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedById = 1,
                                    IsDeleted = false,
                                    OwnerRoleId = 1
                                };

                                newTransactions.Add(req);
                                _bankTransactions.InsertBackgroundTransaction(req);
                            }
                        }



                        var lastRequest = _bankTransactionHead.GetAll().Where(x=>x.AccountNumber == "2093801264").FirstOrDefault();
                        if (lastRequest == null)
                        {
                            _bankTransactionHead.InsertBackgroundTransaction(new BankTransactionHead
                            {
                                Time = DateTime.UtcNow,
                                AccountNumber = "2093801264",
                                ModifiedAt = DateTime.UtcNow,
                                CreatedAt = DateTime.UtcNow,
                                CreatedById = 1,
                                IsDeleted = false,
                                OwnerRoleId = 1

                            });
                        }
                        else
                        {
                            lastRequest.Time = DateTime.UtcNow;
                            _bankTransactionHead.Update(lastRequest);
                        }

                        await _bankTransactionHead.SaveAsync(default);
                        _bankTransactions.CommitTransaction(transaction);



                        if (newTransactions.Any())
                            await RegisterTreasuryDocument(newTransactions, "2093801264");


                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, " خطا در سرویس های بک گراند " + DateTime.Now);
                        _bankTransactions.RollbackTransaction(transaction);
                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " خطا در همگام‌سازی تراکنش‌های بانک اردکان " + DateTime.Now);
                throw;
            }


        }

        public async Task SyncZafarBankTransactions()
        {
            try
            {
                //  var lastRequestTime = _bankTransactionHead.GetAll().Where(x => x.AccountNumber == "132924007").FirstOrDefault()?.Time;
                var lastRequestTime = _bankTransactionHead.GetAll()
                .Where(x => x.AccountNumber == "132924007")
                .Select(x => x.Time)
                .FirstOrDefault();


                if (lastRequestTime == null || lastRequestTime.Year == 1)
                    lastRequestTime = DateTime.UtcNow;


                //var fromDate = lastRequestTime.ToString("yyyy/MM/dd", new CultureInfo("fa-IR")).Trim().Replace("/", "");
                //var toDate = DateTime.UtcNow.ToString("yyyy/MM/dd", new CultureInfo("fa-IR")).Trim().Replace("/", "");

                TimeZoneInfo iranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
                DateTime iranTimeFrom = TimeZoneInfo.ConvertTimeFromUtc(lastRequestTime, iranTimeZone);

                DateTime iranTimeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, iranTimeZone);

                var fromDate = iranTimeFrom.ToString("yyyy/MM/dd", new CultureInfo("fa-IR")).Trim().Replace("/", "");
                var toDate = iranTimeNow.ToString("yyyy/MM/dd", new CultureInfo("fa-IR")).Trim().Replace("/", "");


                var response = await _tejaratBankServices.CallGetAccountBalance(fromDate, toDate, "132924007");

                if (response.Succeed && response.ObjResult.DetailItems != null)
                {
                    var newTransactions = new List<BankTransactions>();

                    using var transaction = await _bankTransactions.BeginTransactionAsync();

                    try
                    {
                        foreach (var item in response.ObjResult.DetailItems)
                        {

                            var exists = _bankTransactions.GetAll()
    .Where(t => t.OrigKey == item.OrigKey && t.IsDeleted != true)
    .Any();

                            if (!exists)
                            {
                                var req = new BankTransactions
                                {
                                    TransactionUniqueId = item.TransactionUniqueId,
                                    AccountNumber = item.BankName,
                                    TransactionAmount = (long)item.TransactionAmount,
                                    TransactionType = item.TransactionType,
                                    TransactionDate = item.TransactionDate,
                                    Balance = item.balance,
                                    TransactionAmountCredit = (decimal)item.TransactionAmountCredit,
                                    TransactionAmountDebit = (decimal)item.TransactionAmountDebit,
                                    OrigKey = item.OrigKey,
                                    OperationCode = item.OperationCode,
                                    TransactionTime = item.TransactionTime,
                                    DocNumber = item.DocNumber,
                                    Description = item.Description,
                                    PayId1 = item.PayId1,
                                    BranchCode = item.BranchCode,
                                    ModifiedAt = DateTime.UtcNow,
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedById = 1,
                                    IsDeleted = false,
                                    OwnerRoleId = 1
                                };

                                newTransactions.Add(req);
                                _bankTransactions.InsertBackgroundTransaction(req);
                            }
                        }


                        var lastRequest = _bankTransactionHead.GetAll().Where(x=>x.AccountNumber == "132924007").FirstOrDefault();
                        if (lastRequest == null)
                        {
                            _bankTransactionHead.InsertBackgroundTransaction(new BankTransactionHead
                            {
                                Time = DateTime.UtcNow,
                                AccountNumber = "132924007",
                                ModifiedAt = DateTime.UtcNow,
                                CreatedAt = DateTime.UtcNow,
                                CreatedById = 1,
                                IsDeleted = false,
                                OwnerRoleId = 1

                            });
                        }
                        else
                        {
                            lastRequest.Time = DateTime.UtcNow;
                            _bankTransactionHead.Update(lastRequest);
                        }

                        await _bankTransactionHead.SaveAsync(default);
                        _bankTransactions.CommitTransaction(transaction);

                        if (newTransactions.Any())
                            await RegisterTreasuryDocument(newTransactions, "132924007");

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, " خطا در سرویس های بک گراند " + DateTime.Now);
                        _bankTransactions.RollbackTransaction(transaction);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " خطا در همگام‌سازی تراکنش‌های بانک ظفر " + DateTime.Now);
                throw;
            }
        }

        private async Task RegisterTreasuryDocument(List<BankTransactions> transactions, string bankAccountNumber)
        {
            try
            {
                var bankReceipts = new List<BankReceiptsModel>();
               // .Where(x => x.PayId1 != null)
                foreach (var transaction in transactions.ToList())
                {

                    int debitReferenceId = bankAccountNumber == "132924007" ? 27855 : 27854;
                    int creditReferenceId = 0;
                    if (transaction.PayId1 != null)
                        creditReferenceId = _accountReferences.GetAll().Where(x => x.DepositId == transaction.PayId1).Select(x => x.Id).FirstOrDefault();
                    else
                        creditReferenceId = 21728;

                    creditReferenceId = creditReferenceId > 0 ? creditReferenceId : 21728;

                   // if (creditReferenceId > 0)
                   // {

                        var date = transaction.TransactionDate.ToString().Insert(4, "/").Insert(7, "/");



                        string[] parts = date.Split('/');
                        int year = int.Parse(parts[0]);
                        int month = int.Parse(parts[1]);
                        int day = int.Parse(parts[2]);


                        PersianCalendar persianCalendar = new PersianCalendar();
                        DateTime gregorianDate = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);

                        var model = new BankReceiptsModel();

                        model.DocumentDate = gregorianDate;
                        model.Amount = transaction.TransactionAmount;
                        model.TotalAmount = transaction.TransactionAmount;
                        model.Description = "بابت دریافت شماره ردیف |: و به شماره پیگیری  " + transaction.DocNumber;
                        model.FinancialRequestDetails = new List<ReceiptModel>
                    {
                        new ReceiptModel
                            {
                                Amount = transaction.TransactionAmount,
                                Description = transaction.DocNumber,
                                DocumentTypeBaseId = 28509,
                                FinancialReferenceTypeBaseId = 28516,
                                DebitAccountHeadId = 1900,
                                DebitAccountReferenceGroupId = 94,
                                DebitAccountReferenceId = debitReferenceId,
                                CurrencyTypeBaseId = 28306,
                                CreditAccountHeadId = 1901,
                                CreditAccountReferenceGroupId = 28,
                                CreditAccountReferenceId =creditReferenceId,
                                PaymentCode = transaction.DocNumber,
                                IsRial = true,
                                NonRialStatus = 0,
                            }
                    };

                        bankReceipts.Add(model);
                   // }
                }

                var command = new CreateBankReceiptCommand();
                command.bankReceiptsModels = new List<BankReceiptsModel>();
                command.bankReceiptsModels.AddRange(bankReceipts);

                if(command.bankReceiptsModels.Count > 0)
                { 
                var result = await _mediator.Send(command);

                if (result.Succeed)
                {
                    using var transaction = await _bankTransactions.BeginTransactionAsync();
                    try { 

                    foreach (var item in transactions.ToList())
                    {

                        var id = result.ObjResult.Where(x => x.FinancialRequestDetails[0].PaymentCode == item.DocNumber).Select(x => x.Id).OrderByDescending(x=>x).FirstOrDefault();
                        if (id > 0)
                        {
                            item.FinancialRequestId = id;
                            _bankTransactions.Update(item);
                        }

                    }

                    await _bankTransactions.SaveAsync(default);
                    _bankTransactions.CommitTransaction(transaction);
                    }
                    catch (Exception ex)
                    {
                        _bankTransactions.RollbackTransaction(transaction);
                        _logger.LogError(ex, " خطا " + " bankAccountNumber " + DateTime.Now);

                    }
                }
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, " خطا در ثبت خزانه داری  " + " bankAccountNumber " + DateTime.Now);
                throw;


            }
        }

    }
}
