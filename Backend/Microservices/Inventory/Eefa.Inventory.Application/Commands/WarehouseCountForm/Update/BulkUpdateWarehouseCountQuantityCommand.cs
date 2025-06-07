using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using MediatR;
using System.Linq;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;
using Eefa.Inventory.Domain.Enum;

namespace Eefa.Inventory.Application
{
    public class BulkUpdateWarehouseCountQuantityCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public List<UpdateWarehousCountQuantityCommand> WarehouseCountedQuantities { get; set; }
        public BulkUpdateWarehouseCountQuantityCommand()
        {
            WarehouseCountedQuantities = new List<UpdateWarehousCountQuantityCommand>();
        }
        public class UpdateWarehouseCountQuantitiesBatchHandler : IRequestHandler<BulkUpdateWarehouseCountQuantityCommand, ServiceResult>
        {
            private readonly IMapper _mapper;
            private readonly IRepository<WarehouseCountFormDetail> _warehouseCountFormDetailRepository;

            public UpdateWarehouseCountQuantitiesBatchHandler(
                IRepository<WarehouseCountFormDetail> warehouseCountFormDetailRepository,
                IMapper mapper)
            {
                _mapper = mapper;
                _warehouseCountFormDetailRepository = warehouseCountFormDetailRepository;
            }
            public async Task<ServiceResult> Handle(BulkUpdateWarehouseCountQuantityCommand request, CancellationToken cancellationToken)
            {
                foreach (var command in request.WarehouseCountedQuantities)
                {
                    var entity=await _warehouseCountFormDetailRepository.Find(command.Id);
                    entity.CountedQuantity=command.CountedQuantity;
                    entity.Description=command.Description;                   
                    _warehouseCountFormDetailRepository.Update(entity);
                }

                await _warehouseCountFormDetailRepository.SaveChangesAsync();
                return ServiceResult.Success();
            }
        }
    }
}
