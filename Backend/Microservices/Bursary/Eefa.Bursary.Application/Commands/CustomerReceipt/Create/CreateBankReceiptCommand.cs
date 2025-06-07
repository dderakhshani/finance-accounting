using AutoMapper;
using Eefa.Bursary.Application.Interfaces;
using Eefa.Bursary.Application.Models;
using Eefa.Bursary.Application.Models.Enums;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.OutboxEntities;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Commands.CustomerReceipt.Create
{
    public class CreateBankReceiptCommand : Common.CommandQuery.CommandBase, IRequest<ServiceResult<List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>>, IMapFrom<CreateBankReceiptCommand>, ICommand
    {
        public List<BankReceiptsModel> bankReceiptsModels { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBankReceiptCommand, List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>().IgnoreAllNonExisting();
        }

        public class CreateBankReceiptCommandHandler : IRequestHandler<CreateBankReceiptCommand, ServiceResult<List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>>
        {
            private readonly IMapper _autoMapper;
            private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> _financialRepository;
            private readonly IRepository<AccountReferences> _accountReference;
            private readonly IRepository<AccountReferencesRelReferencesGroups> _accountReferenceRelReferenceGroup;

            private readonly IRepository<FinancialRequestDetails> _financialDetailRepository;
            private readonly IConfiguration _configuration;

            private readonly IApplicationLogs _applicationLogs;
            private readonly IOutboxRepository _outboxRepository;
            private readonly ILogger<CreateBankReceiptCommand> _log;

            public CreateBankReceiptCommandHandler(
                IMapper autoMapper,
                IConfiguration configuration,
                IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRepository,
                IRepository<FinancialRequestDetails> financialDetailRepository,

                IApplicationLogs applicationLogs,
                IOutboxRepository outboxRepository,
                IRepository<AccountReferencesRelReferencesGroups> accountReferenceRelReferenceGroup,
                IRepository<AccountReferences> accountReference,
                ILogger<CreateBankReceiptCommand> log)
            {
                _autoMapper = autoMapper;
                _configuration = configuration;
                _financialRepository = financialRepository;
                _financialDetailRepository = financialDetailRepository;

                _applicationLogs = applicationLogs;
                _outboxRepository = outboxRepository;
                _accountReferenceRelReferenceGroup = accountReferenceRelReferenceGroup;
                _accountReference = accountReference;
                _log = log;
            }

            public async Task<ServiceResult<List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>> Handle(CreateBankReceiptCommand command, CancellationToken cancellationToken)
            {
                var requests = new List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>();

                using var transaction = await _financialRepository.BeginTransactionAsync();
                using var Outboxtransaction = await _outboxRepository.BeginTransactionAsync();
                try
                {

                    foreach (var requestModel in command.bankReceiptsModels)
                    {
                      
                        var financialEntity = MapToFinancialRequest(requestModel);

                  
                        var docNo = await _financialRepository.GetAll()
                        .Where(x => x.YearId == financialEntity.YearId)
                        .Select(x => x.DocumentNo)
                        .MaxAsync(cancellationToken);

                        docNo = docNo == null ? 0 : docNo;


                        financialEntity.DocumentNo = financialEntity.DocumentNo != 0 ? financialEntity.DocumentNo : docNo + 1;
                        if (financialEntity.Description.Contains("|:"))
                            financialEntity.Description = financialEntity.Description.Replace("|:", financialEntity.DocumentNo.ToString());


                    
                        if (await _financialRepository.GetAll().AnyAsync(x => x.DocumentNo == financialEntity.DocumentNo && !x.IsDeleted && x.YearId == financialEntity.YearId))
                        {
                            throw new ValidationError("Duplicate document number detected. Please try again.");
                        }
                        _financialRepository.InsertBackgroundTransaction(financialEntity);
                        await _financialRepository.SaveAsync(cancellationToken);

                        var references = await (from r in _accountReference.GetAll()
                                                join rg in _accountReferenceRelReferenceGroup.GetAll() on r.Id equals rg.ReferenceId
                                                where rg.IsDeleted != true && r.IsDeleted != true && r.Id == requestModel.FinancialRequestDetails[0].CreditAccountReferenceId 
                                                select rg.ReferenceGroupId).ToArrayAsync();

                        bool IsCustomer = references.Length == 1 && references[0] == 28;

                        if (IsCustomer)
                        {
                            var outboxMessage = new OutboxMessages
                            {
                                EventType = "BankReceiptCreated",
                                Payload = JsonConvert.SerializeObject(MapToAccountingModel(financialEntity)),
                                CreatedAt = DateTime.UtcNow,
                                ProcessedAt = null,
                            };

                            _outboxRepository.Insert(outboxMessage);
                            await _outboxRepository.SaveAsync(cancellationToken);

                            financialEntity.AutomateState = (short?)AutomateEnum.PendingForInsertAutomateDocument;
                        }
                        else
                        {
                            financialEntity.AutomateState = (short?)AutomateEnum.Undecided;
                        }
                            requests.Add(financialEntity);
                    }



                    _outboxRepository.CommitTransaction(Outboxtransaction);
                    _financialRepository.CommitTransaction(transaction);

                    return ServiceResult<List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>.Success(requests);
                }
                catch (Exception ex)
                {
                    _log.LogError(ex.Message, "خطا در CreateBankReceiptCommand", DateTime.Now);
                    _financialRepository.RollbackTransaction(transaction);
                    _outboxRepository.RollbackTransaction(Outboxtransaction);
                    return ServiceResult<List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>.Failed();
                }
            }

            private Domain.Aggregates.FinancialRequestAggregate.FinancialRequest MapToFinancialRequest(BankReceiptsModel model)
            {
              
                var financialEntity = new Domain.Aggregates.FinancialRequestAggregate.FinancialRequest
                {
                    Amount = model.TotalAmount,
                    CodeVoucherGroupId = model.CodeVoucherGroupId,
                    DocumentNo = model.DocumentNo,
                    DocumentDate = model.DocumentDate,
                    Description = model.Description,
                    YearId = model.YearId,
                    FinancialStatusBaseId = 28574, // ثبت درخواست در چرخه جریان کار 
                    PaymentStatus = model.PaymentStatus,
                    IsEmergent = model.IsEmergent,
                    IsAccumulativePayment = model.IsAccumulativePayment,
                    TotalAmount = model.TotalAmount,
                    PaymentTypeBaseId = model.PaymentTypeBaseId,
                    AutomateState = 1,
                    ModifiedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = 1,
                    IsDeleted = false,
                    OwnerRoleId = 1
                };

                financialEntity.FinancialRequestDetails = model.FinancialRequestDetails.Select(detail => new FinancialRequestDetails
                {
                    DocumentTypeBaseId = detail.DocumentTypeBaseId,
                    FinancialReferenceTypeBaseId = detail.FinancialReferenceTypeBaseId,
                    Description = detail.Description,
                    DebitAccountHeadId = detail.DebitAccountHeadId,
                    DebitAccountReferenceGroupId = detail.DebitAccountReferenceGroupId == 0 ? null : detail.DebitAccountReferenceGroupId,
                    DebitAccountReferenceId = detail.DebitAccountReferenceId == 0 ? null : detail.DebitAccountReferenceId,
                    CreditAccountHeadId = detail.CreditAccountHeadId,
                    CurrencyTypeBaseId = detail.CurrencyTypeBaseId,
                    ChequeSheetId = detail.ChequeSheetId,
                    CurrencyFee = detail.CurrencyFee,
                    CurrencyAmount = detail.CurrencyAmount,
                    CreditAccountReferenceGroupId = detail.CreditAccountReferenceGroupId == 0 ? null : detail.CreditAccountReferenceGroupId,
                    CreditAccountReferenceId = detail.CreditAccountReferenceId == 0 ? null : detail.CreditAccountReferenceId,
                    Amount = detail.Amount,
                    PaymentCode = detail.PaymentCode,
                    IsRial = detail.IsRial,
                    NonRialStatus = detail.NonRialStatus,
                    ModifiedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = 1,
                    IsDeleted = false,
                    OwnerRoleId = 1,
                    BedCurrencyStatus = false,
                    BesCurrencyStatus = false,
                }).ToList();

                return financialEntity;
            }

            private SendDocument<DataListModel> MapToAccountingModel(Domain.Aggregates.FinancialRequestAggregate.FinancialRequest item)
            {
                var model = new DataListModel
                {
                    DocumentNo = item.DocumentNo.ToString(),
                    Amount = (long)item.FinancialRequestDetails.First().Amount,
                    DocumentId = item.Id,
                    DocumentDate = item.DocumentDate,
                    CodeVoucherGroupId = item.CodeVoucherGroupId,
                    DebitAccountHeadId = item.FinancialRequestDetails.First().DebitAccountHeadId.ToString(),
                    DebitAccountReferenceGroupId = item.FinancialRequestDetails.First().DebitAccountReferenceGroupId.ToString(),
                    DebitAccountReferenceId = item.FinancialRequestDetails.First().DebitAccountReferenceId.ToString(),
                    CreditAccountHeadId = item.FinancialRequestDetails.First().CreditAccountHeadId.ToString(),
                    CreditAccountReferenceGroupId = item.FinancialRequestDetails.First().CreditAccountReferenceGroupId.ToString(),
                    CreditAccountReferenceId = item.FinancialRequestDetails.First().CreditAccountReferenceId.ToString(),
                    DocumentTypeBaseId = item.FinancialRequestDetails.First().DocumentTypeBaseId,
                    CurrencyAmount = 0,
                    CurrencyTypeBaseId = (int)item.FinancialRequestDetails.First().CurrencyTypeBaseId,
                    IsRial = item.FinancialRequestDetails.First().IsRial ?? true,
                    CurrencyFee = 0,
                    NonRialStatus = 0,
                    Description = item.Description,
                    SheetUniqueNumber = "",
                    BedCurrencyStatus = false,
                    BesCurrencyStatus = false,

                };

                return new SendDocument<DataListModel> { voucherHeadId = 0, dataList = new List<DataListModel> { model } };
            }
        }

    }
}

//public class CreateBankReceiptCommandHandler : IRequestHandler<CreateBankReceiptCommand, ServiceResult<List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>>
//{
//    private readonly IMapper _autoMaper;
//    private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> _financialRepository;
//    private readonly IRepository<FinancialRequestDetails> _financialDetailRepository;
//    private readonly IConfiguration _configuration;
//    private readonly ICurrentUserAccessor _currentUserAccessor;
//    private readonly IApplicationLogs _applicationLogs;
//    private readonly IRequestClient<SendDocument<DataListModel>> _requestClient;

//    public CreateBankReceiptCommandHandler(IMapper autoMaper, IConfiguration configuration,
//        IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRepository,
//        IRepository<FinancialRequestDetails> financialDetailRepository,
//        ICurrentUserAccessor currentUserAccessor, IApplicationLogs applicationLogs, IRequestClient<SendDocument<DataListModel>> requestClient)
//    {
//        _configuration = configuration;
//        _autoMaper = autoMaper;
//        _financialRepository = financialRepository;
//        _financialDetailRepository = financialDetailRepository;
//        _currentUserAccessor = currentUserAccessor;
//        _applicationLogs = applicationLogs;
//        _requestClient = requestClient;
//    }

//    public async Task<ServiceResult<List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>> Handle(CreateBankReceiptCommand value, CancellationToken cancellationToken)
//    {

//        var requests = new List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>();

//        try
//        {

//            foreach (var request in value.bankReceiptsModels)
//            {
//                request.YearId = 4;
//                request.FinancialStatusBaseId = (int)FinancialStatus.Insert;
//                request.PaymentStatus = (int)Eefa.Bursary.Application.PaymentStatus.Settlement;

//                foreach (var item in request.FinancialRequestDetails)
//                {
//                    item.ChequeSheet = null;
//                    item.CurrencyTypeBaseId = item.CurrencyTypeBaseId == 0 ? (int)CurrencyTypes.IRR : item.CurrencyTypeBaseId;

//                }

//                request.Amount = request.TotalAmount;
//                request.TotalAmount = request.TotalAmount;

//                var docNO = await _financialRepository.GetAll().Where(x => x.YearId == 4).Select(x => x.DocumentNo).MaxAsync(cancellationToken);
//                if (docNO == null) docNO = 0;
//                // --- در صورتی که کاربر شماره عملیات مالی را وارد نکند سیستم اتوماتیک شماره را برای فرم می سازد ------
//                request.DocumentNo = request.DocumentNo is not (0) ? request.DocumentNo : docNO + 1;

//                var isDocumentNo = await _financialRepository.GetAll().AnyAsync(x => x.DocumentNo == request.DocumentNo && x.IsDeleted == false && x.YearId == 4);

//                if (isDocumentNo)
//                    throw new ValidationError("کد تکراری می باشد لطفا دوباره تلاش نمایید");


//                // --- به دلیل عدم دسترسی به شماره ردیف در قسمت فرانت از کاراکتر زیر استفاده می کنیم تا در سمت بک اند کد بتوانیم با شماره ردیف ان را جایگزین کنیم
//                if (request.Description.Contains("|:"))
//                    request.Description = request.Description.Replace("|:", request.DocumentNo.ToString());

//                var financialEntity = new Domain.Aggregates.FinancialRequestAggregate.FinancialRequest()
//                {
//                    Amount = request.Amount,
//                    CodeVoucherGroupId = request.CodeVoucherGroupId,
//                    DocumentNo = request.DocumentNo,
//                    DocumentDate = request.DocumentDate,
//                    Description = request.Description,
//                    YearId = request.YearId,
//                    FinancialStatusBaseId = request.FinancialStatusBaseId,
//                    PaymentStatus = request.PaymentStatus,
//                    IsEmergent = request.IsEmergent,
//                    IsAccumulativePayment = request.IsAccumulativePayment,
//                    TotalAmount = request.TotalAmount,
//                    PaymentTypeBaseId = request.PaymentTypeBaseId,
//                    ModifiedAt = DateTime.UtcNow,
//                    CreatedAt = DateTime.UtcNow,
//                    CreatedById = 1,
//                    IsDeleted = false,
//                    OwnerRoleId = 1
//                };

//                financialEntity.FinancialRequestDetails = new List<FinancialRequestDetails>();

//                foreach (var item in request.FinancialRequestDetails)
//                {
//                    var detail = new FinancialRequestDetails()
//                    {
//                        FinancialRequest = financialEntity,
//                        DocumentTypeBaseId = item.DocumentTypeBaseId,
//                        FinancialReferenceTypeBaseId = item.FinancialReferenceTypeBaseId,
//                        Description = item.Description,
//                        DebitAccountHeadId = item.DebitAccountHeadId,
//                        DebitAccountReferenceGroupId = item.DebitAccountReferenceGroupId == 0 ? null : item.DebitAccountReferenceGroupId,
//                        DebitAccountReferenceId = item.DebitAccountReferenceId == 0 ? null : item.DebitAccountReferenceId,
//                        CreditAccountHeadId = item.CreditAccountHeadId,
//                        CurrencyTypeBaseId = item.CurrencyTypeBaseId,
//                        ChequeSheetId = item.ChequeSheetId,
//                        CurrencyFee = item.CurrencyFee,
//                        CurrencyAmount = item.CurrencyAmount,
//                        CreditAccountReferenceGroupId = item.CreditAccountReferenceGroupId == 0 ? null : item.CreditAccountReferenceGroupId,
//                        CreditAccountReferenceId = item.CreditAccountReferenceId == 0 ? null : item.CreditAccountReferenceId,
//                        Amount = item.Amount,
//                        PaymentCode = item.PaymentCode,
//                        IsRial = item.IsRial,
//                        NonRialStatus = item.NonRialStatus,
//                        ModifiedAt = DateTime.UtcNow,
//                        CreatedAt = DateTime.UtcNow,
//                        CreatedById = 1,
//                        IsDeleted = false,
//                        OwnerRoleId = 1
//                    };

//                    financialEntity.AddFinancialDetail(detail);
//                }

//                var requestDocument = MapFinancialRequestToAccountingModel(financialEntity);

//                var response = await _requestClient.GetResponse<ServiceResultModel<List<VoucherResultModel>>>(requestDocument);

//                if (response.Message.succeed)
//                {
//                    financialEntity.VoucherHeadId = response.Message.objResult.First().voucherHeadId;
//                }
//                else
//                {
//                    var errors = "";
//                    foreach(var item in response.Message.exceptions)
//                        errors += item.ToString()+ " *** ";
//                    throw new Exception(errors);
//                }
//                _financialRepository.InsertBackgroundTransaction(financialEntity);

//                await _financialRepository.SaveAsync(cancellationToken);
//                requests.Add(financialEntity);

//            }

//            return ServiceResult<List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>.Success(requests);

//        }
//        catch (Exception ex)
//        {
//            return ServiceResult<List<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>.Failed();

//        }

//    }

//}


//private static SendDocument<DataListModel> MapFinancialRequestToAccountingModel( Domain.Aggregates.FinancialRequestAggregate.FinancialRequest item)
//{
//        var myDataList = new DataListModel();
//        myDataList.DocumentNo = item.DocumentNo.ToString();
//        myDataList.Amount = (long)item.FinancialRequestDetails[0].Amount;
//        myDataList.DocumentId = item.Id;
//        myDataList.DocumentDate = item.DocumentDate;
//        myDataList.CodeVoucherGroupId = item.CodeVoucherGroupId;
//        myDataList.DebitAccountHeadId = item.FinancialRequestDetails[0].DebitAccountHeadId.ToString();
//        myDataList.DebitAccountReferenceGroupId = item.FinancialRequestDetails[0].DebitAccountReferenceGroupId.ToString();
//        myDataList.DebitAccountReferenceId = item.FinancialRequestDetails[0].DebitAccountReferenceId.ToString();
//        myDataList.CreditAccountHeadId = item.FinancialRequestDetails[0].CreditAccountHeadId.ToString();
//        myDataList.CreditAccountReferenceGroupId = item.FinancialRequestDetails[0].CreditAccountReferenceGroupId.ToString();
//        myDataList.CreditAccountReferenceId = item.FinancialRequestDetails[0].CreditAccountReferenceId.ToString();
//        myDataList.DocumentTypeBaseId = item.FinancialRequestDetails[0].DocumentTypeBaseId;
//        myDataList.CurrencyAmount = 0;
//        myDataList.CurrencyTypeBaseId = (int)item.FinancialRequestDetails[0].CurrencyTypeBaseId;
//        myDataList.IsRial = item.FinancialRequestDetails[0].IsRial ?? true;
//        myDataList.CurrencyFee =0; //
//        myDataList.NonRialStatus =  0;
//        myDataList.Description = item.Description;
//        myDataList.SheetUniqueNumber = "";


//  var result = new SendDocument<DataListModel>();
//    result.dataList.Add(myDataList);
//    return result;  


//}



