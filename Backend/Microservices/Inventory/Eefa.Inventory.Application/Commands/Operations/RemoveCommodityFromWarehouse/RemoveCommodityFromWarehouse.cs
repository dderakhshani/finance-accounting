using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class RemoveCommodityFromWarehouseCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int WarehouseId { get; set; }
      
    }

    public class RemoveCommodityFromWarehouseCommandHandler : IRequestHandler<RemoveCommodityFromWarehouseCommand, ServiceResult>
    {
        private readonly IReceiptCommandsService _Repository;
        
        private readonly IMapper _mapper;

        public RemoveCommodityFromWarehouseCommandHandler(
            IReceiptCommandsService Repository,
            IMapper mapper)
        {
            _mapper = mapper;
            _Repository = Repository;
           
        }
        public async Task<ServiceResult> Handle(RemoveCommodityFromWarehouseCommand request, CancellationToken cancellationToken)
        {

           await _Repository.RemoveCommodityFromWarehouse(request.WarehouseId);
            return ServiceResult.Success();
           
        }

       
    }
}
