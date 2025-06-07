using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class DeleteAccessToWarehouseCommand : CommandBase, IRequest<ServiceResult<AccessToWarehouse>>, IMapFrom<Domain.AccessToWarehouse>, ICommand
    {
        public int WarehouseId { get; set; } = default!;

        public int UserId { get; set; } = default!;

        public string TableName { get; set; } = default!;
    }

   
    public class DeleteAccessToWarehouseCommandHandler : IRequestHandler<DeleteAccessToWarehouseCommand, ServiceResult<AccessToWarehouse>>
    {
        private readonly IAccessToWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;

        public DeleteAccessToWarehouseCommandHandler(IAccessToWarehouseRepository warehouseRepository, IMapper mapper)
        {
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<ServiceResult<AccessToWarehouse>> Handle(DeleteAccessToWarehouseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _warehouseRepository.GetAll().Where(a=>a.WarehouseId == request.WarehouseId && a.UserId==request.UserId && a.TableName== request.TableName).FirstOrDefaultAsync();
            if (entity != null)
            {
                _warehouseRepository.Delete(entity);
                if (await _warehouseRepository.SaveChangesAsync(cancellationToken) > 0)
                {
                    var model = _mapper.Map<AccessToWarehouse>(entity);
                    return ServiceResult<AccessToWarehouse>.Success(model);
                }
                else
                {
                    return ServiceResult<AccessToWarehouse>.Failed();
                }
            }
            else return  ServiceResult<AccessToWarehouse>.Success(entity);
            

           
        }
    }
}
