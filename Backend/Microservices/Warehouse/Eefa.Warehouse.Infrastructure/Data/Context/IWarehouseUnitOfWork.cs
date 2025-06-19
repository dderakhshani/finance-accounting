using Eefa.Commodity.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Eefa.Warehouse.Infrastructure.Data.Context
{
    public interface IWarehouseUnitOfWork
    {
        public  DbSet<Warehous> Warehouses { get; set; }

        public  DbSet<WarehouseLayout> WarehouseLayouts { get; set; }
    }
}