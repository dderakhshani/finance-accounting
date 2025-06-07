using System.Linq;
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
    public class DeleteWarehouseLayoutCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteWarehouseLayoutCommandHandler : IRequestHandler<DeleteWarehouseLayoutCommand, ServiceResult>
    {
        private readonly IWarehouseLayoutRepository _warehouseLayoutRepository;
        private readonly IRepository<WarehouseLayoutCategories> _RepositoryCategorys;
        private readonly IRepository<Domain.WarehouseLayoutProperty> _RepositoryProperty;
        private readonly IMapper _mapper;

        public DeleteWarehouseLayoutCommandHandler(
            IWarehouseLayoutRepository warehouseLayoutRepository,
            IRepository<WarehouseLayoutCategories> RepositoryCategorys,
            IRepository<Domain.WarehouseLayoutProperty> RepositoryProperty,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _warehouseLayoutRepository = warehouseLayoutRepository;
            _RepositoryCategorys = RepositoryCategorys;
            _RepositoryProperty = RepositoryProperty;
        }

        public async Task<ServiceResult> Handle(DeleteWarehouseLayoutCommand request, CancellationToken cancellationToken)
        {


            var entity = await _warehouseLayoutRepository.Find(request.Id);
            entity.IsDeleted = true;
            _warehouseLayoutRepository.Update(entity);
            if (await _warehouseLayoutRepository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }

            return ServiceResult.Failed();
        }
    }
}
