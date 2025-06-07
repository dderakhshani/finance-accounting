using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class UpdateAddNewCommodityCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        
      
    }

    public class UpdateAddNewCommodityCommandHandler : IRequestHandler<UpdateAddNewCommodityCommand, ServiceResult>
    {
        private readonly IReceiptCommandsService _Repository;
        
        private readonly IMapper _mapper;

        public UpdateAddNewCommodityCommandHandler(
            IReceiptCommandsService Repository,
            IMapper mapper)
        {
            _mapper = mapper;
            _Repository = Repository;
           
        }
        public async Task<ServiceResult> Handle(UpdateAddNewCommodityCommand request, CancellationToken cancellationToken)
        {

           await _Repository.UpdateAddNewCommodity();
            return ServiceResult.Success();
           
        }
        
    }
}
