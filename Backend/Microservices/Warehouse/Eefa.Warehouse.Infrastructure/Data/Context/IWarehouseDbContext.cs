using Eefa.Common.Data;
using Eefa.Warehouse.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Warehouse.Infrastructure.Data.Context
{
    public interface IWarehouseDbContext:IUnitOfWork
    {
        public DbSet<Warehous> Warehouses { get; set; }

        public DbSet<WarehouseLayout> WarehouseLayouts { get; set; }
    }
}