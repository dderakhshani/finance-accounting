using AutoMapper;
using Eefa.Common;
using Eefa.Warehouse.Application.Queries;
using Eefa.Warehouse.Infrastructure.Data.Context;
using Eefa.Warehouse.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Warehouse.Application.Commands.Warehouse.Update
{
    public record UpdateWarehouseCommand : IRequest<ServiceResult>, IMapFrom<Warehous>
    {
        public int Id { get; set; }
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
    public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, ServiceResult>
    {
        private readonly IMapper _mapper;
        private readonly WarehouseDbContext _dbContext;

        public UpdateWarehouseCommandHandler(IMapper mapper, WarehouseDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<ServiceResult> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Warehouses.FirstAsync(w => w.Id == request.Id);
            _mapper.Map(request, entity);

            if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }
    }
}
