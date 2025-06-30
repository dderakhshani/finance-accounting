using AutoMapper;
using Eefa.Common;
using Eefa.Warehouse.Application.Commands.Warehouse.Update;
using Eefa.Warehouse.Infrastructure.Data.Context;
using Eefa.Warehouse.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Warehouse.Application.Commands
{
    public class UpdateWarehouseLayoutCommand : IRequest<ServiceResult>, IMapFrom<WarehouseLayout>
    {
        public int Id { get; set; }
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

    public class UpdateWarehouseLayoutCommandHandler : IRequestHandler<UpdateWarehouseLayoutCommand, ServiceResult>
    {
        private readonly IMapper _mapper;
        private readonly WarehouseDbContext _dbContext;

        public UpdateWarehouseLayoutCommandHandler(IMapper mapper, WarehouseDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<ServiceResult> Handle(UpdateWarehouseLayoutCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.WarehouseLayouts.FirstAsync(w => w.Id == request.Id);
            _mapper.Map(request, entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult.Success();
        }
    }
}
