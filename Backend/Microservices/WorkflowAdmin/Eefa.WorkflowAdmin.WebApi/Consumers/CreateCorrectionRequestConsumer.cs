using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Contract.AdminWorkflow;
using Eefa.Contract.InventoryAccouting;
using Eefa.WorkflowAdmin.WebApi.Application;
using Library.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Eefa.WorkflowAdmin.WebApi.Consumers
{
    public class CreateCorrectionRequestConsumer : Consumer<CreateCorrectionRequestEvent>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository _repository;
        private readonly ICorrectionRequestService _correctionRequestService;
        private readonly IMapper _mapper;
        public CreateCorrectionRequestConsumer(ICurrentUserAccessor currentUserAccessor, IHttpContextAccessor httpContextAccessor, IRepository repository, ICorrectionRequestService correctionRequestService, IMapper mapper)
        {
            _currentUserAccessor = currentUserAccessor;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _correctionRequestService = correctionRequestService;
            _mapper = mapper;
        }

        public override async Task _Consume(ConsumeContext<CreateCorrectionRequestEvent> context)
        {
            try
            {
                var data = context.Message;
                var correctionRequest = _mapper.Map<CreateCorrectionRequestModel>(data);
                await _correctionRequestService.Create(correctionRequest);
                await context.RespondAsync(new CreateCorrectionResponseEvent(){Done = true});
            }
            catch
            {
                await context.RespondAsync(new CreateCorrectionResponseEvent() { Done = false });
            }
        }
    }
}