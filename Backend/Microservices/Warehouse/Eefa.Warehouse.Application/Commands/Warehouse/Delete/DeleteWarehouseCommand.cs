using AutoMapper;
using Eefa.Common;
using Eefa.Warehouse.Infrastructure.Data.Context;
using Eefa.Warehouse.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Warehouse.Application.Commands.Warehouse.Delete
{
    public class DeleteWarehouseCommand : IRequest<bool>, IMapFrom<Warehous>
    {
        public int Id { get; set; }
    }
    public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly WarehouseDbContext _dbContext;

        public DeleteWarehouseCommandHandler(IMapper mapper, WarehouseDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Warehouses.FirstAsync(w => w.Id == request.Id);
            _dbContext.RemoveEntity(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
