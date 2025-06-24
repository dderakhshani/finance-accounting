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
    public record UpdateWarehousCommand : IRequest<bool>, IMapFrom<Warehous>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string? LevelCode { get; set; }
        public int AccountHeadId { get; set; }
        public int? AccountRererenceGroupId { get; set; }
        public int? AccountReferenceId { get; set; }
        public string Title { get; set; } = null!;
        public bool IsActive { get; set; }
        public int? CommodityCategoryId { get; set; }
        public int? Sort { get; set; }
        public bool? Countable { get; set; }
        public byte[]? RowVersion { get; set; }
    }
    public class UpdateWarehousCommandHandler : IRequestHandler<UpdateWarehousCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly WarehouseDbContext _dbContext;

        public UpdateWarehousCommandHandler(IMapper mapper, WarehouseDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateWarehousCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Warehouses.FirstAsync(w => w.Id == request.Id && !w.IsDeleted);
            _mapper.Map(request, entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
