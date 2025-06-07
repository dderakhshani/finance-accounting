using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using MediatR;

namespace Eefa.Inventory.Application
{ 
    public class DeleteWarehouseCommand : CommandBase, IRequest<ServiceResult<WarehouseModel>>, IMapFrom<Domain.Warehouse>, ICommand
    {
        public int Id { get; set; }
    }

   
    public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand, ServiceResult<WarehouseModel>>
    {
        private readonly IWarehouseRepository _warehousRepository;
        private readonly IMapper _mapper;

        public DeleteWarehouseCommandHandler(IWarehouseRepository warehousRepository, IMapper mapper)
        {
            _mapper = mapper;
            _warehousRepository = warehousRepository;
        }

        public async Task<ServiceResult<WarehouseModel>> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _warehousRepository.Find(request.Id);

            

            _warehousRepository.Delete(entity);
            if(await _warehousRepository.SaveChangesAsync(cancellationToken) > 0)
            {
                var model = _mapper.Map<WarehouseModel>(entity);
                return ServiceResult<WarehouseModel>.Success(model);
            }
            else
            {
                return ServiceResult<WarehouseModel>.Failed();
            }

           
        }
    }
}
