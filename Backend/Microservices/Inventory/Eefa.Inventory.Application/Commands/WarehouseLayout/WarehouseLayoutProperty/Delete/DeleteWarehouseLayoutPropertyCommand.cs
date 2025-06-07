using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class DeleteWarehouseLayoutPropertyCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int categoryPropertyId { get; set; }
        public int warehouseLayoutPropertiesId { get; set; }
    }

    public class DeleteWarehouseLayoutPropertyCommandHandler : IRequestHandler<DeleteWarehouseLayoutPropertyCommand, ServiceResult>
    {

        private readonly IRepository<Domain.WarehouseLayoutProperty> _warehouseLayoutPropertyRepository;
        private readonly IMapper _mapper;

        public DeleteWarehouseLayoutPropertyCommandHandler(IRepository<Domain.WarehouseLayoutProperty> warehouseLayoutPropertyRepository, IMapper mapper)
        {
            _mapper = mapper;
            _warehouseLayoutPropertyRepository = warehouseLayoutPropertyRepository;
        }

        public async Task<ServiceResult> Handle(DeleteWarehouseLayoutPropertyCommand request, CancellationToken cancellationToken)
        {
           
            var Property = _warehouseLayoutPropertyRepository.GetAll().Where(a => a.CategoryPropertyId == request.categoryPropertyId && a.Id == request.warehouseLayoutPropertiesId).ToList();
            Property.ForEach(a =>
            {
                _warehouseLayoutPropertyRepository.Delete(a);
              
            });
            
            if (await _warehouseLayoutPropertyRepository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }
    }
}
