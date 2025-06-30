using AutoMapper;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Warehouse.Infrastructure.Data.Entities;
using Eefa.Warehouse.Infrastructure.Data.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Warehouse.Application.Commands
{
    public class CreateWarehouseCommand : IRequest<ServiceResult>, IMapFrom<Warehous>
    {
        public int TypeBaseId { get; set; }
        public int AccountHeadId { get; set; }
        public int? AccountRererenceGroupId { get; set; }
        public int? AccountReferenceId { get; set; }
        public string Title { get; set; } = null!;
        public bool IsActive { get; set; }
        public int? CommodityCategoryId { get; set; }
        public int? Sort { get; set; }
        public bool? Countable { get; set; }
    }

    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, ServiceResult>
    {
        private readonly IMapper _mapper;
        private readonly WarehouseDbContext _dbContext;

        public CreateWarehouseCommandHandler(IMapper mapper, WarehouseDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<ServiceResult> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {

            var warehouse = _mapper.Map<Warehous>(request);
            _dbContext.Warehouses.Add(warehouse);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult.Success();
        }
    }
}
