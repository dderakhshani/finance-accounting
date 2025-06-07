using Eefa.Bursary.Application.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Commands.ChequeSheet.Update
{
    public class CreateChequeSheetDocumentCommand : Common.CommandQuery.CommandBase, IRequest<ServiceResult>, IMapFrom<CreateChequeSheetDocumentCommand>, ICommand
    {
        public bool saveChanges { get; set; } = true;
        public int menueId { get; set; } = 0;
        public int? Status { get; set; }
        public List<DataListModel> dataList { get; set; }


        public class CreateChequeSheetDocumentCommandHandler : IRequestHandler<CreateChequeSheetDocumentCommand, ServiceResult>
        {
            private readonly IRepository<ChequeSheets> _chequeSheetRepository;
            private readonly IRepository<AccountReferences> _accountReferenceRepository;
            private readonly IConfiguration _configuration;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IApplicationLogs _applicationLogs;
            public CreateChequeSheetDocumentCommandHandler(IRepository<ChequeSheets> chequeSheetRepository, IConfiguration configuration, IRepository<AccountReferences> accountReferenceRepository, ICurrentUserAccessor currentUserAccessor, IApplicationLogs applicationLogs)
            {
                _chequeSheetRepository = chequeSheetRepository;
                _configuration = configuration;
                _accountReferenceRepository = accountReferenceRepository;
                _currentUserAccessor = currentUserAccessor;
                _applicationLogs = applicationLogs;
            }

            public async Task<ServiceResult> Handle(CreateChequeSheetDocumentCommand request, CancellationToken cancellationToken)
            {

                await _applicationLogs.CommitLog(request);

                var list = new List<DataListModel>();
                foreach(var item in request.dataList)
                {
                    if (item.DebitAccountHeadId == "1880" || item.DebitAccountHeadId == "1879")// اگر کد این مقدار بود نباید هیچ گروه و تفصیلی داشته باشد
                    {
                        item.DebitAccountReferenceGroupId = null;   
                        item.DebitAccountReferenceId = null;
                    }
                    if (item.CreditAccountHeadId == "1880" || item.CreditAccountHeadId == "1879") // اگر کد این مقدار بود نباید هیچ گروه و تفصیلی داشته باشد
                    {
                        item.CreditAccountReferenceGroupId = null;
                        item.CreditAccountReferenceId = null;
                    }

                    list.Add(item);
                }

                var isDocumentSave =await InsertDocument(list);

                if (  isDocumentSave == null || isDocumentSave.succeed == false)
                    throw new ValidationError(isDocumentSave.error[0].message);

                var chequeSheetIds =  request.dataList.Select(x => x.DocumentId).ToList();
                var chequeSheets = await _chequeSheetRepository.GetAll().Where(x => chequeSheetIds.Contains(x.Id) && x.IsDeleted != true).ToListAsync();
                chequeSheets.ForEach(x => x.ChequeDocumentState = request.Status);
                if (request.Status == 1 || (request.Status == 2 && request.dataList.Count > 1))
                {

                    if(request.dataList[0].DebitAccountReferenceId!=null)
                    chequeSheets.ForEach(x => x.IssueReferenceBankId = int.Parse(request.dataList[0].DebitAccountReferenceId));
                    else
                        chequeSheets.ForEach(x => x.IssueReferenceBankId = null);

                }
                await _chequeSheetRepository.SaveChangesAsync();
                return ServiceResult.Success();

            }


            private async Task<ResultModel> InsertDocument(List<DataListModel> dataList)
            {
                var settings = _configuration.GetSection("AccountingConnectionDetails");


                var urlAddress = settings.GetValue<string>("url");
#if DEBUG
             //     urlAddress = settings.GetValue<string>("add_url_dev");
#endif
            

                string danaToken = await _currentUserAccessor.GetAccessToken();
                List<BursaryDocumentModel> list = new List<BursaryDocumentModel>();
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var requestUri = new Uri(urlAddress);

                        foreach (var item in dataList)
                        {
                             
                             //   var creditReference = await _accountReferenceRepository.GetAll().Where(x => x.Id == int.Parse(item.CreditAccountReferenceId)).FirstAsync();

                            var myDataList = new BursaryDocumentModel
                            {
                                DocumentNo = item.DocumentNo.ToString(),
                                Amount = item.Amount,
                                DocumentId = item.DocumentId,
                                DocumentDate = item.DocumentDate,
                                CodeVoucherGroupId = item.CodeVoucherGroupId,
                                DebitAccountHeadId = int.Parse(item.DebitAccountHeadId),
                                DebitAccountReferenceGroupId = item.DebitAccountReferenceGroupId != null ? int.Parse(item.DebitAccountReferenceGroupId):null,
                                DebitAccountReferenceId = item.DebitAccountReferenceId !=null ? int.Parse(item.DebitAccountReferenceId): null,
                                CreditAccountHeadId = int.Parse(item.CreditAccountHeadId),
                                CreditAccountReferenceGroupId =item.CreditAccountReferenceGroupId != null? int.Parse(item.CreditAccountReferenceGroupId):null,
                                CreditAccountReferenceId = item.CreditAccountReferenceId != null ? int.Parse(item.CreditAccountReferenceId): null,
                                DocumentTypeBaseId = item.DocumentTypeBaseId,
                                CurrencyAmount =  0,
                                CurrencyTypeBaseId = item.CurrencyTypeBaseId,
                                IsRial =   true,
                                CurrencyFee =   0  , //
                                NonRialStatus = item.NonRialStatus,
                                ChequeSheetId = item.ChequeSheetId,
                                ReferenceCode =  " ",
                                ReferenceName =  " ",
                                Description = item.Description
                            };
                            list.Add(myDataList);
                        }
                        var request = new AddAutoVoucherModel
                        {
                            dataList = list, 
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
                                var isSucced = JsonSerializer.Deserialize<ResultModel>(content);
                                if (isSucced.succeed) return isSucced;
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
                            var content = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(content))
                            {
                                var isSucced = JsonSerializer.Deserialize<ResultModel>(content);
                                return isSucced;
                            }
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
