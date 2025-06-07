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

namespace Eefa.Bursary.Application.Commands.CustomerReceipt.Update
{
    public class UpdateCustomerReceiptListCommand : Common.CommandQuery.CommandBase,
    IRequest<ServiceResult>,
    IMapFrom<UpdateCustomerReceiptListCommand>, ICommand
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
        public List<FinancialAttachmentModel> FinancialRequestAttachments { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCustomerReceiptListCommand, Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>().IgnoreAllNonExisting();

        }

        public class UpdateCustomerReceiptListCommandHandler : IRequestHandler<UpdateCustomerReceiptListCommand,
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
            public UpdateCustomerReceiptListCommandHandler(IMapper autoMaper,
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
                UpdateCustomerReceiptListCommand request, CancellationToken cancellationToken)
            {

                await _applicationLogs.CommitLog(request);

                var financialRequest = await _financialRepository.GetAll().Where(x => x.DocumentNo == request.DocumentNo && x.YearId == _currentUserAccessor.GetYearId()).Include(x => x.FinancialRequestDetails).Include(x => x.FinancialRequestAttachments).SingleAsync(cancellationToken);
                request.VoucherHeadId = financialRequest.VoucherHeadId;
                request.FinancialStatusBaseId = (int)FinancialStatus.Insert;

                request.PaymentStatus = (int)PaymentStatus.Settlement;

                financialRequest.SetAmount(amount: request.FinancialRequestDetails.Sum(x => x.Amount));
                financialRequest.SetDescription(description: request.Description);
                financialRequest.SetDocumentDate(request.DocumentDate);
                financialRequest.SetDocNumber(request.DocumentNo);
                //financialRequest.FinancialRequestDetails = new List<FinancialRequestDetail>();
                //financialRequest.FinancialRequestAttachments = new List<FinancialRequestAttachment>();

                foreach (var detail in request.FinancialRequestDetails)
                {
                     detail.Id = financialRequest.FinancialRequestDetails[0].Id;
                    var detailEntity = financialRequest.FinancialRequestDetails.FirstOrDefault(x => x.Id == detail.Id);
                    if (detailEntity != null)
                    {

                        _autoMaper.Map(detail, detailEntity);
                        detailEntity.ChequeSheet = null;
                    }

                    else
                    {
                        var entity = _autoMaper.Map<FinancialRequestDetails>(detail);
                        entity.ChequeSheet = null;
                    //    financialRequest.AddFinancialDetail(entity);
                    }
                }

                if (request.FinancialRequestAttachments != null)
                foreach (var attachment in request.FinancialRequestAttachments)
                    financialRequest.AddFinancialAttachment(_autoMaper.Map<FinancialRequestAttachments>(attachment));

                //اگر قبلا سند حسابداری داشته باشد
                if (request.VoucherHeadId != null)
                    financialRequest.IsPending = true;

                _financialRepository.Update(financialRequest);
                await _financialRepository.SaveChangesAsync(cancellationToken);


                //اگر قبلا سند حسابداری داشته باشد
                if (request.VoucherHeadId != null)
                {
                    var data = await CallUpdateAutoVoucher(financialRequest, request);

                    if (data is not null)
                    {
                        financialRequest.IsPending = false;
                        await _financialRepository.SaveChangesAsync(cancellationToken);
                    }
                }

                return ServiceResult.Success();

            }




            private async Task<object> CallUpdateAutoVoucher(Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate.FinancialRequest item, UpdateCustomerReceiptListCommand req)
            {
                //TODO : TOken and Address Must write into Setting
                var settings = _configuration.GetSection("AccountingConnectionDetails");

                string danaToken = settings.GetValue<string>("token");
                var creditReference = await _accountReferenceRepository.GetAll().Where(x => x.Id == req.FinancialRequestDetails[0].CreditAccountReferenceId).FirstAsync();

                var urlAddress = "http://192.168.2.151:50002/api/accounting/VouchersHead/UpdateAutoVoucher";

                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var requestUri = new Uri(urlAddress);

                        var myDataList = item.FinancialRequestDetails.Select(d => new AccountingDocument
                        {
                            DocumentNo = item.DocumentNo,
                            Amount = d.Amount,
                            DocumentId = item.Id,
                            DocumentDate = item.DocumentDate,
                            CodeVoucherGroupId = item.CodeVoucherGroupId,
                            DebitAccountHeadId = d.DebitAccountHeadId,
                            DebitAccountReferenceGroupId = (int)d.DebitAccountReferenceGroupId,
                            DebitAccountReferenceId = (int)d.DebitAccountReferenceId,
                            CreditAccountHeadId = d.CreditAccountHeadId,
                            CreditAccountReferenceGroupId = (int)d.CreditAccountReferenceGroupId,
                            CreditAccountReferenceId = (int)d.CreditAccountReferenceId,
                            DocumentTypeBaseId = d.DocumentTypeBaseId,
                            CurrencyFee = d.CurrencyFee,
                            CurrencyAmount = d.CurrencyAmount ?? 0,
                            CurrencyTypeBaseId = d.CurrencyTypeBaseId,
                            IsRial = d.IsRial ?? true,
                            NonRialStatus = d.NonRialStatus ?? 0,
                            ChequeSheetId = d.ChequeSheetId,
                            ReferenceCode = creditReference.Code,
                            ReferenceName = " " + creditReference.Title + " "
                        }).ToList();

                        var request = new AutoVoucherUpdate
                        {
                            DataList = myDataList,
                            VocherHeadId = req.VoucherHeadId
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
                                return JsonSerializer.Deserialize<AutoVoucherUpdate>(content);
                            }
                            else
                            {
                                Console.WriteLine("Response content was empty");
                                return false;
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

        }

    }

    public class AutoVoucherUpdate
    {
        public List<AccountingDocument> DataList { get; set; }
        public int? VocherHeadId { get; set; }
    }
    public class AccountingDocument
    {
        public int DocumentNo { get; set; }
        public int DocumentId { get; set; }
        public DateTime DocumentDate { get; set; }
        public int CodeVoucherGroupId { get; set; }
        public int DebitAccountHeadId { get; set; }
        public int DebitAccountReferenceGroupId { get; set; }
        public int DebitAccountReferenceId { get; set; }
        public int CreditAccountHeadId { get; set; }
        public int CreditAccountReferenceGroupId { get; set; }
        public int CreditAccountReferenceId { get; set; }
        public decimal Amount { get; set; }
        public int DocumentTypeBaseId { get; set; }
        public string SheetUniqueNumber { get; set; }
        public decimal? CurrencyFee { get; set; }
        public decimal? CurrencyAmount { get; set; }
        public int? CurrencyTypeBaseId { get; set; }
        public int NonRialStatus { get; set; }
        public int? ChequeSheetId { get; set; }
        public bool IsRial { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceCode { get; set; }
    }

}
