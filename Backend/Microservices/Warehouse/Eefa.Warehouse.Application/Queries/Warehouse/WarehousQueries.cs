using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Warehouse.Application.Models;
using Eefa.Warehouse.Application.Queries;
using Eefa.Warehouse.Infrastructure.Data.Context;
using Eefa.Warehouse.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Warehouse.Application.Queries
{
    public class WarehousQueries : IWarehousQueries
    {
        private readonly WarehouseDbContext _dbContext;

        public WarehousQueries(WarehouseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<WarehouseModel>> GetAll(PaginatedQueryModel query)
        {
            var projected = _dbContext.Warehouses
                .Select(w => new WarehouseModel
                {
                    Id = w.Id,
                    Title = w.Title,
                });

            return await projected.ToPagedList(query);
        }

    }
}
