using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{


    public class UpdateAssetsAttachmentCommand : CommandBase, IRequest<ServiceResult<PersonsDebitedCommoditiesModel>>, IMapFrom<Domain.PersonsDebitedCommodities>, ICommand
    {
        public int Id { get; set; } = default!;

        public List<AttachmentAssetsRequest> attachmentAssets { get; set; } = default!;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAssetsAttachmentCommand, Domain.PersonsDebitedCommodities>()
                .IgnoreAllNonExisting();
        }
    }
   

    public class UpdateAssetsAttachmentCommandHandler : IRequestHandler<UpdateAssetsAttachmentCommand, ServiceResult<PersonsDebitedCommoditiesModel>>
    {

        private readonly IMapper _mapper;
        private readonly IReceiptCommandsService _receiptCommandsService;
        public UpdateAssetsAttachmentCommandHandler(
            IMapper mapper,
            IReceiptCommandsService receiptCommandsService
            )
        {
            _mapper = mapper;
            _receiptCommandsService = receiptCommandsService;
        }

        public async Task<ServiceResult<PersonsDebitedCommoditiesModel>> Handle(UpdateAssetsAttachmentCommand request, CancellationToken cancellationToken)
        {
            await _receiptCommandsService.ModifyAttachmentAssets(request.attachmentAssets, request.Id);
            return ServiceResult<PersonsDebitedCommoditiesModel>.Success(new PersonsDebitedCommoditiesModel());
        }

       
    }
}
