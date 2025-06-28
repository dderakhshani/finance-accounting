using AutoMapper;
using Eefa.Common;
using Eefa.Warehouse.Infrastructure.Data.Context;
using Eefa.Warehouse.Infrastructure.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Warehouse.Application.Commands
{
    public class CreateWarehouseLayoutCommand : IRequest<bool>, IMapFrom<WarehouseLayout>
    {
        public int? WarehouseId { get; set; }
        public int? ParentId { get; set; }
        public string? LevelCode { get; set; }
        public string Title { get; set; } = null!;
        public int Capacity { get; set; }
        public bool LastLevel { get; set; }
        public int? UnitBaseTypeId { get; set; }
        public int OrderIndex { get; set; }
        public int? CommodityId { get; set; }
    }
    public class CreateWarehouseLayoutCommandHandler : IRequestHandler<CreateWarehouseLayoutCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly WarehouseDbContext _dbContext;

        public CreateWarehouseLayoutCommandHandler(IMapper mapper, WarehouseDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(CreateWarehouseLayoutCommand request, CancellationToken cancellationToken)
        {

            var warehouseLayout = _mapper.Map<WarehouseLayout>(request);
            _dbContext.WarehouseLayouts.Add(warehouseLayout);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
