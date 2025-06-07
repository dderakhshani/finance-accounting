using Eefa.Bursary.Application.Models;
using Eefa.Bursary.Application.UseCases.AutoVouchers.GeneralAV.Models;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.AutoVouchers.GeneralAV.Commands.Add
{
    public class AddAutoVoucherCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int menueId { get; set; } = 0;
        public List<dynamic> dataList { get; set; }

    }
    public class AddAutoVoucherCommandHandler : IRequestHandler<AddAutoVoucherCommand, ServiceResult>
    {
        private readonly IBursaryUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public AddAutoVoucherCommandHandler(IBursaryUnitOfWork uow, IConfiguration configuration, ICurrentUserAccessor currentUserAccessor)
        {
            _uow = uow;
            _configuration = configuration;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult> Handle(AddAutoVoucherCommand request, CancellationToken cancellationToken)
        {
            var settings = _configuration.GetSection("AccountingConnectionDetails");
            var urlAddress = settings.GetValue<string>("url");
            string danaToken = await _currentUserAccessor.GetAccessToken();

            var requestdata = new AddVoucherGenericModel()
            {
                menueId = request.menueId,
                dataList = request.dataList,
            };

            var jsonRequestData = JsonSerializer.Serialize(requestdata);
            var httpContent = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var requestUri = new Uri(urlAddress);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", danaToken);
                    var response = await httpClient.PostAsync(requestUri, httpContent);

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(content))
                        {
                            var isSucced = JsonSerializer.Deserialize<ResultModel>(content);
                            if (isSucced.succeed) return ServiceResult.Success();
                            else
                                return ServiceResult.Failed();
                        }
                        else
                        {
                            return ServiceResult.Failed();
                        }
                    }
                    else
                    {
                        return ServiceResult.Failed();
                    }
                }
                catch (Exception ex)
                {
                    return ServiceResult.Failed();
                }
            }
            return null;
        }
    }

}
