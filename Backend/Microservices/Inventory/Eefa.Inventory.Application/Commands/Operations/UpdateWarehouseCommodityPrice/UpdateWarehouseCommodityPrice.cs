using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class UpdateWarehouseCommodityPriceCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int WarehouseId { get; set; }
        public int YearId { get; set; }
    }

    public class UpdateWarehouseCommodityPriceCommandHandler : IRequestHandler<UpdateWarehouseCommodityPriceCommand, ServiceResult>
    {
        private readonly IReceiptCommandsService _Repository;
        
        private readonly IMapper _mapper;

        public UpdateWarehouseCommodityPriceCommandHandler(
            IReceiptCommandsService Repository,
            IMapper mapper)
        {
            _mapper = mapper;
            _Repository = Repository;
           
        }
        public async Task<ServiceResult> Handle(UpdateWarehouseCommodityPriceCommand request, CancellationToken cancellationToken)
        {

           await _Repository.UpdateWarehouseCommodityPrice(request.WarehouseId, request.YearId);
            return ServiceResult.Success();
           
        }
        
    }
}
