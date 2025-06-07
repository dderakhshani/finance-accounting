using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Eefa.Bursary.Application.Commands.CustomerReceipt.Create
{
    public class CreateCustomerReceiptCommand : Common.CommandQuery.CommandBase, IRequest<ServiceResult<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>, IMapFrom<CreateCustomerReceiptCommand>, ICommand
    {

        public int DocumentNo { get; set; }
        public int CodeVoucherGroupId { get; set; }
        public DateTime DocumentDate { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; } = 1;
        public int YearId { get; set; } = 1;
        public int FinancialStatusBaseId { get; set; }
        public int PaymentStatus { get; set; }
        public bool IsEmergent { get; set; } = false;
        public bool IsAccumulativePayment { get; set; } = false;
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentTypeBaseId { get; set; } = 28509;
        public short? IsAutomateState { get; set; } = 0;
        public List<FinancialAttachmentModel> Attachments { get; set; }
        public List<ReceiptModel> FinancialRequestDetails { get; set; }   

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCustomerReceiptCommand, Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>().IgnoreAllNonExisting();
        }


        public class CreateCustomerReceiptCommandHandler : IRequestHandler<CreateCustomerReceiptCommand, ServiceResult<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>
        {
            private readonly IMapper _autoMaper;
            private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> _financialRepository;
            private readonly IRepository<Years> _yearRepository;
            private readonly IRepository<FinancialRequestDetails> _financialDetailRepository;
            private readonly IRepository<Domain.Entities.ChequeSheets> _chequeSheetRepository;
            private readonly IRepository<Attachment> _attachmentRepository;
            private readonly IRepository<FinancialRequestAttachments> _financialRequestAttachment;
            private readonly IConfiguration _configuration;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IApplicationLogs _applicationLogs;

            public CreateCustomerReceiptCommandHandler(IMapper autoMaper, IConfiguration configuration,
                IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRepository,
                IRepository<Years> yearRepository, IRepository<FinancialRequestDetails> financialDetailRepository,
                IRepository<Domain.Entities.ChequeSheets> chequeSheetRepository,
                IRepository<Attachment> attachmentRepository,
                IRepository<FinancialRequestAttachments> financialRequestAttachment, ICurrentUserAccessor currentUserAccessor, IApplicationLogs applicationLogs)
            {
                _configuration = configuration;
                _autoMaper = autoMaper;
                _financialRepository = financialRepository;
                _yearRepository = yearRepository;
                _financialDetailRepository = financialDetailRepository;
                _chequeSheetRepository = chequeSheetRepository;
                _attachmentRepository = attachmentRepository;
                _financialRequestAttachment = financialRequestAttachment;
                _currentUserAccessor = currentUserAccessor;
                _applicationLogs = applicationLogs;
            }

            public async Task<ServiceResult<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>> Handle(CreateCustomerReceiptCommand request, CancellationToken cancellationToken)
            {
             //   await _applicationLogs.CommitLog(request);

                int yearId = _currentUserAccessor.GetYearId();

                request.YearId = yearId;
                request.FinancialStatusBaseId = (int)FinancialStatus.Insert;
                request.PaymentStatus = (int)Eefa.Bursary.Application.PaymentStatus.Settlement;

                foreach (var item in request.FinancialRequestDetails)
                {
                    var hasChequeSheet = item.ChequeSheet != null && item.ChequeSheet.Id != 0;
                    if (hasChequeSheet)
                    {
                        item.ChequeSheetId = item.ChequeSheet.Id;
                        item.Amount = item.ChequeSheet.TotalCost;
                        item.CurrencyTypeBaseId = (int)CurrencyTypes.IRR;
                        item.Description = item.ChequeSheet.SheetSeqNumber.ToString();

                        // isUsed == true --> Dont show This cheque in report again 
                        var chequeSheet = _chequeSheetRepository.GetAll().Where(x => x.Id == item.ChequeSheet.Id).Single();
                        chequeSheet.IsUsed = true;
                        item.ChequeSheet = null;
                     
                    }
                    else
                    {
                        item.ChequeSheet = null;
                        item.CurrencyTypeBaseId = item.CurrencyTypeBaseId == 0 ? (int)CurrencyTypes.IRR : item.CurrencyTypeBaseId;
                    }
                }


                var amount = request.FinancialRequestDetails.Sum(x => x.Amount);

                request.Amount = amount;
                request.TotalAmount = amount;

                var docNO = await _financialRepository.GetAll().Where(x => x.YearId == yearId).Select(x => x.DocumentNo).MaxAsync(cancellationToken);
                if (docNO == null) docNO = 0;
                // --- در صورتی که کاربر شماره عملیات مالی را وارد نکند سیستم اتوماتیک شماره را برای فرم می سازد ------
                request.DocumentNo = request.DocumentNo is not (0) ? request.DocumentNo : docNO + 1;

                var isDocumentNo = await _financialRepository.GetAll().AnyAsync(x => x.DocumentNo == request.DocumentNo && x.IsDeleted == false && x.YearId == yearId);

                if (isDocumentNo)
                    throw new ValidationError("کد تکراری می باشد لطفا دوباره تلاش نمایید");


                // --- به دلیل عدم دسترسی به شماره ردیف در قسمت فرانت از کاراکتر زیر استفاده می کنیم تا در سمت بک اند کد بتوانیم با شماره ردیف ان را جایگزین کنیم
                if (request.Description.Contains("|:"))
                    request.Description = request.Description.Replace("|:", request.DocumentNo.ToString());

                var financialEntity = new Domain.Aggregates.FinancialRequestAggregate.FinancialRequest()
                {
                    Amount = request.Amount,
                    CodeVoucherGroupId = request.CodeVoucherGroupId,
                    DocumentNo = request.DocumentNo,
                    DocumentDate = request.DocumentDate,
                    Description = request.Description,
                    YearId = request.YearId,
                    FinancialStatusBaseId = request.FinancialStatusBaseId,
                    PaymentStatus = request.PaymentStatus,
                    IsEmergent = request.IsEmergent,
                    IsAccumulativePayment = request.IsAccumulativePayment,
                    TotalAmount = request.TotalAmount,
                    PaymentTypeBaseId = request.PaymentTypeBaseId,
                    AutomateState = request.IsAutomateState,
                };



                //    _autoMaper.Map<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>(request);

                financialEntity.FinancialRequestDetails = new List<FinancialRequestDetails>();

                if (request.Attachments != null)
                { // -- Its better instead using of "request.Attachments == null" , replace it with "request.Attachments.Any()  == true" because checking null its also check length 
                    foreach (var file in request.Attachments)
                    {

                        var financialAttachment = new FinancialRequestAttachments()
                        {
                            AttachmentId = (int)file.AttachmentId, // AttachmentId come from front 
                            FinancialRequest = financialEntity,
                            IsVerified = false,
                            IsDeleted = false
                        };
                        _financialRequestAttachment.Insert(financialAttachment);
                    }

                    // when we use a attachment, IsUsed field must be true, otherwise this attachment will be remove phisycally
                    var attachmentIds = request.Attachments.Select(x => x.AttachmentId).ToList();
                    var attachments = await _attachmentRepository.GetAll().Where(x => attachmentIds.Contains(x.Id)).ToListAsync();
                    attachments.ForEach(x => x.IsUsed = true);
                }

                foreach (var item in request.FinancialRequestDetails)
                {
                    // var detailEntity = _autoMaper.Map<FinancialRequestDetails>(item);


                    var detail = new FinancialRequestDetails()
                    {
                        FinancialRequest = financialEntity,
                        DocumentTypeBaseId = item.DocumentTypeBaseId,
                        FinancialReferenceTypeBaseId = item.FinancialReferenceTypeBaseId,
                        Description = item.Description,
                        DebitAccountHeadId = item.DebitAccountHeadId,
                        DebitAccountReferenceGroupId = item.DebitAccountReferenceGroupId == 0 ? null : item.DebitAccountReferenceGroupId,
                        DebitAccountReferenceId = item.DebitAccountReferenceId == 0 ? null : item.DebitAccountReferenceId,
                        CreditAccountHeadId = item.CreditAccountHeadId,
                        CurrencyTypeBaseId = item.CurrencyTypeBaseId,
                        ChequeSheetId = item.ChequeSheetId,
                        CurrencyFee = item.CurrencyFee,
                        CurrencyAmount = item.CurrencyAmount,
                        CreditAccountReferenceGroupId = item.CreditAccountReferenceGroupId == 0 ? null : item.CreditAccountReferenceGroupId,
                        CreditAccountReferenceId = item.CreditAccountReferenceId == 0 ? null : item.CreditAccountReferenceId,
                        Amount = item.Amount,
                        SowiftCode = item.SowiftCode.ToString(),
                        DeliveryOrderCode = item.DeliveryOrderCode.ToString(),
                        RegistrationCode = item.RegistrationCode.ToString(),
                        PaymentCode = item.PaymentCode,
                        IsRial = item.IsRial,
                        NonRialStatus = item.NonRialStatus,
                        BesCurrencyStatus = item.BesCurrencyStatus,
                        BedCurrencyStatus = item.BedCurrencyStatus,

                    };

                    financialEntity.AddFinancialDetail(detail);
                }

                foreach (var item in financialEntity.FinancialRequestDetails.Where(item => item.ChequeSheetId is > 0))
                {
                    var chequeSheet = await _chequeSheetRepository.Find((int)item.ChequeSheetId);
                    chequeSheet.IsActive = false;
                    _chequeSheetRepository.Update(chequeSheet);
                }
                _financialRepository.Insert(financialEntity);

                 await _financialRepository.SaveChangesAsync(cancellationToken);

                return ServiceResult<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>.Success(financialEntity);
            }

        }

    }
}
