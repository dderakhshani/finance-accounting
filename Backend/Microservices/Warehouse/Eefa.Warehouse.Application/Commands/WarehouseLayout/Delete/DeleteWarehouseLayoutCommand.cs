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

namespace Eefa.Warehouse.Application.Commands
{
    public class DeleteWarehouseLayoutCommand : IRequest<bool>, IMapFrom<WarehouseLayout>
    {
        public int Id { get; set; }
    }
    public class DeleteWarehouseLayoutCommandHandler : IRequestHandler<DeleteWarehouseLayoutCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly WarehouseDbContext _dbContext;

        public DeleteWarehouseLayoutCommandHandler(IMapper mapper, WarehouseDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteWarehouseLayoutCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.WarehouseLayouts.FirstAsync(w => w.Id == request.Id && !w.IsDeleted);
            entity.IsDeleted = true;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
