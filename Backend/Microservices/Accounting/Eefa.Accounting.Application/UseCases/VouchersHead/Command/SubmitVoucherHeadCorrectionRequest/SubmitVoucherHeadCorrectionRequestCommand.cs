using AutoMapper;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.Update;
using Eefa.NotificationServices.Common.Enum;
using Eefa.NotificationServices.Dto;
using Eefa.NotificationServices.Services.Interfaces;
using Library.Common;
using Library.Interfaces;
using Library.Models;
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

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.SubmitVoucherHeadCorrectionRequest
{
    public class SubmitVoucherHeadCorrectionRequestCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public UpdateVouchersHeadCommand Payload { get; set; }
        public string Message { get; set; }
    }


    public class SubmitVoucherHeadCorrectionRequestCommandHandler : IRequestHandler<SubmitVoucherHeadCorrectionRequestCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;     
        private readonly IConfiguration _configuration;
        private readonly INotificationClient _notificationClient;
        public SubmitVoucherHeadCorrectionRequestCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor, IMediator mediator, IConfiguration configuration,
           INotificationClient notificationClient)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _mediator = mediator;
            _repository = repository;
            _configuration = configuration;
            _notificationClient = notificationClient;

        }

        public async Task<ServiceResult> Handle(SubmitVoucherHeadCorrectionRequestCommand request, CancellationToken cancellationToken)
        {           
            var voucherHead = await _repository
              .Find<Data.Entities.VouchersHead>(c =>
          c.ObjectId(request.Payload.Id))
              .Include(x => x.VouchersDetails)
          .FirstOrDefaultAsync(cancellationToken);

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
                    VerifierUserId = 1427,
                    ApiUrl = urlAddress,
                    ViewUrl = viewUrl,
                    DocumentId = voucherHead.Id,
                    OldData = JsonConvert.SerializeObject(request.Payload),
                    Status = 0
                };

                var isSuccessful = await this.SendCorrectionRequest(correctionRequest);

                if (isSuccessful)
                {
                    var message = new NotificationDto
                    {
                        ReceiverUserId = 1427,
                        MessageTitle = "درخواست تغییرات",
                        MessageContent = request.Message,                        
                        MessageType = 1,
                        Payload = correctionRequest.PayLoad,
                        SendForAllUser = false,
                        Status = MessageStatus.Sent,
                        OwnerRoleId = 1,
                        Listener= "notifyUpdateVoucherHead"                        
                    };                  
                    await _notificationClient.Send( message);
                    return ServiceResult.Success();
                }
                else return ServiceResult.Failure();

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
                    string token = await _currentUserAccessor.GetAccessToken();

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
}