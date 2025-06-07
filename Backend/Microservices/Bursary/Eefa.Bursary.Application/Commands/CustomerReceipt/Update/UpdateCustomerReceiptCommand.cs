using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Eefa.Bursary.Application.Models;
using Eefa.Common.Exceptions;
using Eefa.Bursary.Infrastructure;

namespace Eefa.Bursary.Application.Commands.CustomerReceipt.Update
{
    public class UpdateCustomerReceiptCommand : Common.CommandQuery.CommandBase,
        IRequest<ServiceResult>, IMapFrom<UpdateCustomerReceiptCommand>, ICommand
    {
        public int Id { get; set; }
        public int DocumentNo { get; set; }
        public int CodeVoucherGroupId { get; set; }
        public DateTime DocumentDate { get; set; }
        public int? VoucherHeadId { get; set; }
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
        public List<ReceiptModel> FinancialRequestDetails { get; set; }
        public List<FinancialAttachmentModel> Attachments { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCustomerReceiptCommand, Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>().IgnoreAllNonExisting();

        }

        public class UpdateCustomerReceiptCommandHandler : IRequestHandler<UpdateCustomerReceiptCommand,
            ServiceResult>
        {
            private readonly IMapper _autoMaper;

            private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>
                _financialRepository;

            private readonly IRepository<Years> _yearRepository;
            private readonly IRepository<FinancialRequestDetails> _financialDetailRepository;
            private readonly IRepository<Domain.Entities.ChequeSheets> _chequeSheetRepository;
            private readonly IRepository<FinancialRequestAttachments> _financialAttachmentRepository;
            private readonly IRepository<Attachment> _attachmentRepository;
            private readonly IRepository<AccountReferences> _accountReferenceRepository;
            private readonly IConfiguration _configuration;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IApplicationLogs _applicationLogs;
            public UpdateCustomerReceiptCommandHandler(IMapper autoMaper,
                IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRepository,
                IRepository<Years> yearRepository, IRepository<FinancialRequestDetails> financialDetailRepository,
                IRepository<Domain.Entities.ChequeSheets> chequeSheetRepository,
                IRepository<FinancialRequestAttachments> financialAttachmentRepository,
                IRepository<Attachment> attachmentRepository, IRepository<AccountReferences> accountReferenceRepository, IConfiguration configuration, ICurrentUserAccessor currentUserAccessor, IApplicationLogs applicationLogs)
            {
                _autoMaper = autoMaper;
                _financialRepository = financialRepository;
                _yearRepository = yearRepository;
                _financialDetailRepository = financialDetailRepository;
                _chequeSheetRepository = chequeSheetRepository;
                _financialAttachmentRepository = financialAttachmentRepository;
                _attachmentRepository = attachmentRepository;
                _accountReferenceRepository = accountReferenceRepository;
                _configuration = configuration;
                _currentUserAccessor = currentUserAccessor;
                _applicationLogs = applicationLogs;
            }

            public async Task<ServiceResult> Handle(
                UpdateCustomerReceiptCommand request, CancellationToken cancellationToken)
            {
                await _applicationLogs.CommitLog(request);

                var financialRequest = _financialRepository.GetAll().Where(x => x.Id == request.Id && x.YearId == _currentUserAccessor.GetYearId()).AsNoTracking().FirstOrDefault();


                var detailsId = request.FinancialRequestDetails.Select(x => x.Id).ToList();

                var details =await (from fdr in _financialDetailRepository.GetAll()
                               where !detailsId.Contains(fdr.Id) && fdr.FinancialRequestId == request.Id
                               select fdr).ToListAsync();

                foreach (var item in details)
                {
                    item.IsDeleted = true;
                }

                if (financialRequest == null)
                    financialRequest = _financialRepository.GetAll().Where(x => x.DocumentNo == request.DocumentNo && x.YearId == _currentUserAccessor.GetYearId()).Include(x => x.FinancialRequestDetails).Include(x => x.FinancialRequestAttachments).AsNoTracking().FirstOrDefault();

                request.FinancialStatusBaseId = (int)FinancialStatus.Insert;

                request.PaymentStatus = (int)PaymentStatus.Settlement;

                var sumDetailAmount = request.FinancialRequestDetails.Sum(x => x.Amount);
                financialRequest.SetAmount(sumDetailAmount);
                financialRequest.SetDescription(description: request.Description);
                financialRequest.SetDocumentDate(request.DocumentDate);
                financialRequest.SetDocNumber(request.DocumentNo);

                foreach (var detail in request.FinancialRequestDetails)
                {
                    var detailEntity =await _financialDetailRepository.GetAll().FirstOrDefaultAsync(x => x.Id == detail.Id);
                    if (detailEntity != null && detail.Id != 0)
                    {
                        detailEntity.DocumentTypeBaseId = detail.DocumentTypeBaseId;
                        detailEntity.FinancialReferenceTypeBaseId = detail.FinancialReferenceTypeBaseId;
                        detailEntity.Description = detail.Description;
                        detailEntity.DebitAccountHeadId = detail.DebitAccountHeadId;
                        detailEntity.DebitAccountReferenceGroupId = detail.DebitAccountReferenceGroupId == 0 ? null : detail.DebitAccountReferenceGroupId;
                        detailEntity.DebitAccountReferenceId = detail.DebitAccountReferenceId == 0 ? null : detail.DebitAccountReferenceId;
                        detailEntity.CreditAccountHeadId = detail.CreditAccountHeadId;
                        detailEntity.CurrencyTypeBaseId = detail.CurrencyTypeBaseId;
                        detailEntity.ChequeSheetId = detail.ChequeSheetId;
                        detailEntity.CurrencyFee = detail.CurrencyFee;
                        detailEntity.CurrencyAmount = detail.CurrencyAmount;
                        detailEntity.CreditAccountReferenceGroupId = detail.CreditAccountReferenceGroupId == 0 ? null : detail.CreditAccountReferenceGroupId;
                        detailEntity.CreditAccountReferenceId = detail.CreditAccountReferenceId == 0 ? null : detail.CreditAccountReferenceId;
                        detailEntity.Amount = detail.Amount;
                        detailEntity.SowiftCode = detail.SowiftCode.ToString();
                        detailEntity.DeliveryOrderCode = detail.DeliveryOrderCode.ToString();
                        detailEntity.RegistrationCode = detail.RegistrationCode.ToString();
                        detailEntity.PaymentCode = detail.PaymentCode;
                        detailEntity.IsRial = detail.IsRial;
                        detailEntity.NonRialStatus = detail.NonRialStatus;
                        detailEntity.ChequeSheet = null;
                        detailEntity.IsDeleted = detail.IsDeleted;
                        detailEntity.BesCurrencyStatus = detail.BesCurrencyStatus;
                        detailEntity.BedCurrencyStatus = detail.BedCurrencyStatus;
                    }

                    else
                    {
                        var entity = new FinancialRequestDetails()
                        {
                            FinancialRequestId = request.Id, 
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
                            SowiftCode = detail.SowiftCode.ToString(),
                            DeliveryOrderCode = detail.DeliveryOrderCode.ToString(),
                            RegistrationCode = detail.RegistrationCode.ToString(),
                            PaymentCode = detail.PaymentCode,
                            IsRial = detail.IsRial,
                            NonRialStatus = detail.NonRialStatus,
                            ChequeSheet = null,
                             BesCurrencyStatus = detail.BesCurrencyStatus,
                        BedCurrencyStatus = detail.BedCurrencyStatus,


                    };

                    //    var entity = _autoMaper.Map<FinancialRequestDetails>(detail);
                   //     entity.ChequeSheet = null;
                        financialRequest.AddFinancialDetail(entity);
                    }
                }

                // remove all attachments
                var attachmentsByRequestId = await _financialAttachmentRepository.GetAll().Where(x => x.FinancialRequestId == request.Id).ToListAsync();
                attachmentsByRequestId.ForEach(x => x.IsDeleted = true);

                // add new attachments
                if (request.Attachments != null && request.Attachments.Count != 0)
                {
                    foreach (var file in request.Attachments)
                    {
                        var financialAttachment = new FinancialRequestAttachments()
                        {
                            AttachmentId = (int)file.AttachmentId, // AttachmentId come from front 
                            FinancialRequestId = request.Id,
                            IsVerified = false,
                            IsDeleted = false
                        };
                        _financialAttachmentRepository.Insert(financialAttachment);
                    }

                    // when we use a attachment, IsUsed field must be true, otherwise this attachment will be remove phisycally
                    var attachmentIds = request.Attachments.Select(x => x.AttachmentId).ToList();
                    var attachments = await _attachmentRepository.GetAll().Where(x => attachmentIds.Contains(x.Id)).ToListAsync();
                    attachments.ForEach(x => x.IsUsed = true);
                }


                //اگر قبلا سند حسابداری داشته باشد
                if (request.VoucherHeadId != null)
                    financialRequest.IsPending = true;


                financialRequest.FinancialRequestAttachments = null;
                _financialRepository.Update(financialRequest);
                //    await _financialRepository.SaveChangesAsync(cancellationToken);


                //اگر قبلا سند حسابداری داشته باشد
                if (request.VoucherHeadId != null)
                {
                    var data = await CallUpdateAutoVoucher(financialRequest, request);
                    if (data is not null)
                    {
                        financialRequest.IsPending = false;
                        var voucherResult = (ResultModel)data;
                        foreach(var item in voucherResult.objResult)
                            if (financialRequest.Id == item.documentId)
                                financialRequest.VoucherHeadId = item.voucherHeadId;
                    }
                    else
                    {

                        throw new ValidationError("سند اپدیت نشد لطفا با واحد نرم افزار تماس بگیرید");
                    }
                };



                await _financialRepository.SaveChangesAsync(cancellationToken);
                return ServiceResult.Success();

            }

            public enum FinancialStatus
            {
                Archive = 28517, Temporary = 28518, Deleted = 28519, InProgress = 28520, Insert = 28574
            }

            public enum PaymentStatus
            {
                PayPart = 1, PrePayment = 2, Settlement = 3
            }

            public enum CurrencyTypes
            {
                IRR = 28306, USD = 28309, EUR = 28310, IraqiDinar = 28311, CNY = 28312
            }

            private async Task<object> CallUpdateAutoVoucher(Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate.FinancialRequest item, UpdateCustomerReceiptCommand req)
            {
                //TODO : TOken and Address Must write into Setting

                var settings = _configuration.GetSection("AccountingConnectionDetails");
                var urlAddress = settings.GetValue<string>("url");
    #if DEBUG
             //    urlAddress = settings.GetValue<string>("url_Dev");
    #endif
                string danaToken = await _currentUserAccessor.GetAccessToken();
                List<BursaryDocumentModel> dataList = new List<BursaryDocumentModel>();
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var requestUri = new Uri(urlAddress);

                        foreach (var detail in item.FinancialRequestDetails)
                        {

                         //   var creditReference = await _accountReferenceRepository.GetAll().Where(x => x.Id == detail.CreditAccountReferenceId).FirstAsync();

                            if(detail.IsDeleted != true)
                            {


                            var dateTime = new DateTime(item.DocumentDate.Year, item.DocumentDate.Month, item.DocumentDate.Day, 0, 0, 0);


                                try
                                {
                                    var myDataList = new BursaryDocumentModel();

                                    myDataList.DocumentNo = item.DocumentNo.ToString();
                                    myDataList.Amount = detail.Amount;
                                    myDataList.DocumentId = item.Id;
                                    myDataList.DocumentDate = item.DocumentDate;
                                    myDataList.CodeVoucherGroupId = item.CodeVoucherGroupId;
                                    myDataList.DebitAccountHeadId = detail.DebitAccountHeadId;
                                    myDataList.DebitAccountReferenceGroupId = (int?)detail.DebitAccountReferenceGroupId == 0 ? null : (int?)detail.DebitAccountReferenceGroupId;
                                    myDataList.DebitAccountReferenceId = (int?)detail.DebitAccountReferenceId == 0 ? null : (int?)detail.DebitAccountReferenceId;
                                    myDataList.CreditAccountHeadId = detail.CreditAccountHeadId;
                                    myDataList.CreditAccountReferenceGroupId = (int?)detail.CreditAccountReferenceGroupId == 0 ? null : (int?)detail.CreditAccountReferenceGroupId;
                                    myDataList.CreditAccountReferenceId = (int?)detail.CreditAccountReferenceId == 0 ? null : (int?)detail.CreditAccountReferenceId;
                                    myDataList.DocumentTypeBaseId = detail.DocumentTypeBaseId;
                                    myDataList.CurrencyAmount = detail.CurrencyAmount ?? 0;
                                    myDataList.CurrencyTypeBaseId = detail.CurrencyTypeBaseId;
                                    myDataList.IsRial = detail.IsRial ?? true;
                                    myDataList.CurrencyFee = detail.IsRial == true ? 0 : detail.CurrencyFee; //
                                    myDataList.NonRialStatus = detail.NonRialStatus ?? 0;
                                    myDataList.ChequeSheetId = detail.ChequeSheetId;
                                    myDataList.ReferenceCode = "";//creditReference.Code,
                                    myDataList.ReferenceName = "";// + creditReference.Title + " ",
                                    myDataList.Description = item.Description;
                                    myDataList.SheetUniqueNumber ="";
                                    myDataList.BesCurrencyStatus = detail.BesCurrencyStatus;
                                    myDataList.BedCurrencyStatus = detail.BedCurrencyStatus;



                                    dataList.Add(myDataList);
                                }catch(Exception e)
                                {
                                    Console.WriteLine(e.ToString());
                                    throw new Exception(e.ToString());
                                }
                                

                            }

                        }
                        var request = new AutoVoucherUpdateModel
                        {
                            DataList = dataList,
                            VoucherHeadId = req.VoucherHeadId
                        };

                        var jsonRequestData = JsonSerializer.Serialize(request);

                        var httpContent = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", danaToken);

                        var response = await httpClient.PostAsync(requestUri, httpContent);

                         if (response != null && response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(content))
                            {
                                var voucherResult = JsonSerializer.Deserialize<ResultModel>(content);
                                if (voucherResult.succeed) return voucherResult;
                                else
                                    return null;
                            }
                            else
                            {
                                Console.WriteLine("Response content was empty");
                                return null;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Call to API failed with status code {response?.StatusCode}");
                            return null;
                        }
                    }
                    catch (ArgumentNullException ex)
                    {
                        Console.WriteLine("Error: URL address is null or empty");
                        return null;
                    }
                    catch (UriFormatException ex)
                    {
                        Console.WriteLine("Error: Invalid URL format");
                        return null;
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return null;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return null;
                    }
                }
            }
        }
    }
}
