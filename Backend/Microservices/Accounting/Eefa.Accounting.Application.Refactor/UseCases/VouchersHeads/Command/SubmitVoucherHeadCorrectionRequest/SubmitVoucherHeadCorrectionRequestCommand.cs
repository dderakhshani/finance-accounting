using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class SubmitVoucherHeadCorrectionRequestCommand : IRequest<ServiceResult<VouchersHeadModel>>
{
    public UpdateVouchersHeadCommand Payload { get; set; }
    public string Message { get; set; }
}

public class SubmitVoucherHeadCorrectionRequestCommandHandler : IRequestHandler<SubmitVoucherHeadCorrectionRequestCommand, ServiceResult<VouchersHeadModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IApplicationUser _applicationUser;
    private readonly IConfiguration _configuration;

    public SubmitVoucherHeadCorrectionRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser, IMediator mediator, IConfiguration configuration)
    {
        _mapper = mapper;
        _applicationUser = applicationUser;
        _mediator = mediator;
        _unitOfWork= unitOfWork;
        _configuration = configuration;
    }

    public async Task<ServiceResult<VouchersHeadModel>> Handle(SubmitVoucherHeadCorrectionRequestCommand request, CancellationToken cancellationToken)
    {
        var voucherHead = await _unitOfWork.VouchersHeads.GetByIdAsync(request.Payload.Id, x => x.Include(y => y.VouchersDetails));

        if (voucherHead.VoucherStateId != 1)
        {
            var settings = _configuration.GetSection("CorrectionRequestSettings");
            var urlAddress = settings.GetValue<string>("SubmitApiUrl");
            var viewUrl = settings.GetValue<string>("ViewUrl");
            var correctionRequest = new CorrectionRequestModel
            {
                CodeVoucherGroupId = voucherHead.CodeVoucherGroupId,
                Description = request.Message,
                PayLoad = JsonConvert.SerializeObject(request.Payload),
                VerifierUserId = 1,
                ApiUrl = urlAddress,
                ViewUrl = viewUrl,
                DocumentId = voucherHead.Id,
                OldData = JsonConvert.SerializeObject(request.Payload),
                Status = 0
            };

            await this.SendCorrectionRequest(correctionRequest);

            return ServiceResult.Success(_mapper.Map<VouchersHeadModel>(voucherHead));

        }
        else
        {
            throw new Exception("Invalid Voucher State");
        }
    }

    public async Task<bool> SendCorrectionRequest(CorrectionRequestModel payload)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var settings = _configuration.GetSection("CorrectionRequestSettings");
                var urlAddress = settings.GetValue<string>("ApiUrl");
                string token = settings.GetValue<string>("Token");

                var requestUri = new Uri(urlAddress);

                var httpContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsync(requestUri, httpContent);

                if (response != null && response.IsSuccessStatusCode)
                {
                    //var content = await response.Content.ReadAsStringAsync();
                    //var correctionRequest = JsonConvert.DeserializeObject<CorrectionRequestModel>(content);
                    //if (correctionRequest.Id != default)
                    //{
                    return true;
                    //}
                }
                else
                {
                    throw new Exception("Failed to send correction request");
                }

            }
            catch (Exception e)
            {
                throw new Exception("Failed to send correction request");
            }
        }
    }
}