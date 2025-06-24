using Eefa.Common.Data.Query;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Warehouse.Infrastructure.Data.Entities;
using Eefa.Warehouse.Application.Models;

namespace Eefa.Warehouse.Application.Queries
{
    public interface IWarehousQueries : IQuery
    {
        Task<PagedList<WarehouseModel>> GetAll(PaginatedQueryModel query);
    }
}
