using AutoMapper;
using Eefa.Bursary.Application.Models;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
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

namespace Eefa.Bursary.Application.Commands.CustomerReceipt.Delete
{
    public class DeleteCustomerReceiptCommand :CommandBase, IRequest<ServiceResult>, ICommand
    {
        public List<int> Ids { get; set; }
    }
    public class DeleteCustomerReceiptCommandHandler : IRequestHandler<DeleteCustomerReceiptCommand, ServiceResult>
    {
        private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IApplicationLogs _applicationLogs;
        public DeleteCustomerReceiptCommandHandler(IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> repository, IMapper mapper, IConfiguration configuration, ICurrentUserAccessor currentUserAccessor, IApplicationLogs applicationLogs)
        {
            _mapper = mapper;
            _repository = repository;
            _configuration = configuration;
            _currentUserAccessor = currentUserAccessor;
            _applicationLogs = applicationLogs;
        }

        public async Task<ServiceResult> Handle(DeleteCustomerReceiptCommand request, CancellationToken cancellationToken)
        {

            await _applicationLogs.CommitLog(request);

            var financialRequests = await _repository.GetAll().Where(x=> request.Ids.Contains(x.Id)).Include(x=>x.FinancialRequestDetails).ToListAsync(cancellationToken);

            foreach(var item in financialRequests)
            {
                var random = new Random();
                var randomNumber = random.Next(10000000, 100000000);
                item.Delete(randomNumber);

                if (item.VoucherHeadId > 0) {
                    var financialRequestsId =new List<int>();
                    financialRequestsId.Add(item.Id);
                 var result =  await CallUpdateAutoVoucher((int)item.VoucherHeadId, financialRequestsId);
                    if (result == null)
                        return ServiceResult.Failed();
                }
            }

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
                return ServiceResult.Success();
             
            return ServiceResult.Failed();
        }




        private async Task<object> CallUpdateAutoVoucher(int voucherHeadId,List<int> financialRequestId)
        {
            //TODO : TOken and Address Must write into Setting

            var settings = _configuration.GetSection("AccountingConnectionDetails");

            var urlAddress = settings.GetValue<string>("delete_voucher_head");
#if DEBUG
            urlAddress = settings.GetValue<string>("delete_voucher_head_dev");
#endif

            string danaToken = await _currentUserAccessor.GetAccessToken();



            using (var httpClient = new HttpClient())
            {
                try
                {
                    var request = new DeleteVouchersDetailsByDocumentIdsCommand
                    {
                        VoucherId = voucherHeadId,
                        DocumentIds = financialRequestId
                    };

                    var requestUri = new Uri(urlAddress);

                    var jsonRequestData = JsonSerializer.Serialize(request);

                    var httpContent = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", danaToken);

                    var httpRequestMessage = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = requestUri,
                        Content = httpContent
                    };

                    var response = await httpClient.SendAsync(httpRequestMessage);

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
