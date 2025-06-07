using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Eefa.Admin.Application.CommandQueries.CorrectionRequest.Commands.Submit
{
    public class SubmitCorrectionRequestCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
        public bool IsAccepted { get; set; }
    }

    public class SubmitCorrectionRequestCommandHandler : IRequestHandler<SubmitCorrectionRequestCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserAccessor currentUser;

        public SubmitCorrectionRequestCommandHandler(IRepository repository, IMapper mapper, IConfiguration configuration, ICurrentUserAccessor currentUser)
        {
            _mapper = mapper;
            _repository = repository;
            _configuration = configuration;
            this.currentUser = currentUser;
        }


        public async Task<ServiceResult> Handle(SubmitCorrectionRequestCommand request, CancellationToken cancellationToken)
        {
            var correctionRequest = await _repository.Find<Data.Databases.Entities.CorrectionRequest>().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (correctionRequest.Status != 0) throw new Exception("Correction request is already submitted");


            if (request.IsAccepted)
            {
                correctionRequest.Status = 1;

                var response = await this.SendRequest(correctionRequest);

                if (response == true)
                {
                    await _repository.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    throw new Exception("Failed on submitting request");
                }
            }
            else
            {
                correctionRequest.Status = 2;
                await _repository.SaveChangesAsync(cancellationToken);

            }

            return ServiceResult.Success(null);
        }


        public async Task<bool> SendRequest(Data.Databases.Entities.CorrectionRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var settings = _configuration.GetSection("CorrectionRequestSettings");
                    var baseUrl = settings.GetValue<string>("ApiBaseUrl");
                    string token = await currentUser.GetAccessToken();

                    var json = JObject.Parse(request.PayLoad);
                    json.Add("key", "TestKey");
                    var requestUri = new Uri(baseUrl + request.ApiUrl);

                    var httpContent = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await httpClient.PutAsync(requestUri, httpContent);

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        //var content = await response.Content.ReadAsStringAsync();
                        //var correctionRequest = JsonConvert.DeserializeObject<CorrectionRequestModel>(content);
                        //if (correctionRequest.Id != default)
                        //{
                        return true;
                        //}
                    }

                    return false;
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to send correction request");
                }
            }
        }
    }
}
