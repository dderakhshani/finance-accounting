using AutoMapper;
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

        public WarehousQueries(WarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //public async Task<Warehous> GetById(int id)
        //{
        //    //  return await _dbContext.Warehouses.FirstAsync(x=>x.Id==id && !x.IsDeleted);
        //    return null;
        //}
    }
}
