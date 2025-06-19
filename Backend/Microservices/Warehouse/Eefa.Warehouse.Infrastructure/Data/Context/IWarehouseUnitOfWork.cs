using Eefa.Common.Data;
using Eefa.Warehouse.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Eefa.Warehouse.Infrastructure.Data.Context
{
    public interface IWarehouseUnitOfWork : IUnitOfWork
    {
        public DbSet<Warehous> Warehouses { get; set; }

        public DbSet<WarehouseLayout> WarehouseLayouts { get; set; }
    }
}