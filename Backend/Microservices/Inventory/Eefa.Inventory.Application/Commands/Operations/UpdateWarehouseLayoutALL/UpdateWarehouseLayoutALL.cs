using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class UpdateWarehouseLayoutALLCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int WarehouseId { get; set; }
      
    }

    public class UpdateWarehouseLayoutALLCommandHandler : IRequestHandler<UpdateWarehouseLayoutALLCommand, ServiceResult>
    {
        private readonly IReceiptCommandsService _Repository;
        
        private readonly IMapper _mapper;

        public UpdateWarehouseLayoutALLCommandHandler(
            IReceiptCommandsService Repository,
            IMapper mapper)
        {
            _mapper = mapper;
            _Repository = Repository;
           
        }
        public async Task<ServiceResult> Handle(UpdateWarehouseLayoutALLCommand request, CancellationToken cancellationToken)
        {

           await _Repository.UpdateWarehouseLayout(request.WarehouseId);
            return ServiceResult.Success();
           
        }
        
    }
}
