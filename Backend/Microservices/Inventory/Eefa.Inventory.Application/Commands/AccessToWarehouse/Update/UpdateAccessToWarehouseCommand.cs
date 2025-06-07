using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using MediatR;
namespace Eefa.Inventory.Application
{
    public class UpdateAccessToWarehouseCommand : CommandBase, IRequest<ServiceResult<AccessToWarehouse>>, IMapFrom<Domain.AccessToWarehouse>, ICommand
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; } = default!;
        public string Description { get; set; } = default!;
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAccessToWarehouseCommand, Domain.AccessToWarehouse>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateAccessToWarehouseCommandHandler : IRequestHandler<UpdateAccessToWarehouseCommand, ServiceResult<AccessToWarehouse>>
    {
        private readonly IAccessToWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;
        

        public UpdateAccessToWarehouseCommandHandler(IAccessToWarehouseRepository warehouseRepository,
            IMapper mapper,
            IRepository<AccountReference> accountReferenceRepository,
            IRepository<WarehousesCodeVoucherGroups> warehousesCodeVoucherGroupsRepository,
            IRepository<WarehousesCategories> warehousesCategoriesRepository
            )
        {
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;
           
        }

        public async Task<ServiceResult<AccessToWarehouse>> Handle(UpdateAccessToWarehouseCommand request, CancellationToken cancellationToken)
        {

            var entity = await _warehouseRepository.Find(request.Id);
            _mapper.Map<UpdateAccessToWarehouseCommand, Domain.AccessToWarehouse>(request, entity);
            _warehouseRepository.Update(entity);
            await _warehouseRepository.SaveChangesAsync(cancellationToken);

            var model = _mapper.Map<AccessToWarehouse>(entity);
            return ServiceResult<AccessToWarehouse>.Success(model);
        }
    }
}
