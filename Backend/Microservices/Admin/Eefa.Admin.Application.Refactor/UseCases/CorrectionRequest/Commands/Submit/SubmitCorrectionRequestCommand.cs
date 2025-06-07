using AutoMapper;
using MediatR;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

public class SubmitCorrectionRequestCommand : IRequest<ServiceResult<string>>
{
    public int Id { get; set; }
    public bool IsAccepted { get; set; }
}

public class SubmitCorrectionRequestCommandHandler : IRequestHandler<SubmitCorrectionRequestCommand, ServiceResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public SubmitCorrectionRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<ServiceResult<string>> Handle(SubmitCorrectionRequestCommand request, CancellationToken cancellationToken)
    {
        var correctionRequest = await _unitOfWork.CorrectionRequests.GetByIdAsync(request.Id);
        if (correctionRequest.Status != 0) throw new Exception("Correction request is already submitted");


        if (request.IsAccepted)
        {
            correctionRequest.Status = 1;

            var response = await this.SendRequest(correctionRequest);

            if (response == true)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Exception("Failed on submitting request");
            }
        }
        else
        {
            correctionRequest.Status = 2;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

        return ServiceResult.Success("Success.");
    }

    public async Task<bool> SendRequest(CorrectionRequest request)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var settings = _configuration.GetSection("CorrectionRequestSettings");
                var baseUrl = settings.GetValue<string>("ApiBaseUrl");
                string token = settings.GetValue<string>("Token");

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