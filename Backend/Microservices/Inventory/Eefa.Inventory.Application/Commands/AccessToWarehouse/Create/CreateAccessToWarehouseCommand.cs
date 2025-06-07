using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class CreateAccessToWarehouseCommand : CommandBase, IRequest<ServiceResult<AccessToWarehouse>>, IMapFrom<CreateAccessToWarehouseCommand>, ICommand
    {
        public int WarehouseId { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAccessToWarehouseCommand, Domain.AccessToWarehouse>()
                .IgnoreAllNonExisting();
        }
    }
    
    public class CreateAccessToWarehouseCommandHandler : IRequestHandler<CreateAccessToWarehouseCommand, ServiceResult<AccessToWarehouse>>
    {
        private readonly IAccessToWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;

        public CreateAccessToWarehouseCommandHandler(
            IAccessToWarehouseRepository warehouseRepository,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;

        }

        public async Task<ServiceResult<AccessToWarehouse>> Handle(CreateAccessToWarehouseCommand request, CancellationToken cancellationToken)
        {
            var entity =await _warehouseRepository.GetAll().Where(a => a.WarehouseId == request.WarehouseId && a.UserId == request.UserId && a.TableName == request.TableName).FirstOrDefaultAsync();
            if (entity == null)
            {
                Domain.AccessToWarehouse warehouse;

                warehouse = _mapper.Map<Domain.AccessToWarehouse>(request);
                entity = _warehouseRepository.Insert(warehouse);
                await _warehouseRepository.SaveChangesAsync();
                return ServiceResult<AccessToWarehouse>.Success(warehouse);


            }
            if(entity.IsDeleted==true)
            {
                entity.IsDeleted = false;
                entity = _warehouseRepository.Update(entity);
                await _warehouseRepository.SaveChangesAsync();
               
            }
            return ServiceResult<AccessToWarehouse>.Success(entity);
        }

    }


}
