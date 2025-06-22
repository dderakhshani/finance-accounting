using AutoMapper;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Sale.Application.Queries.PriceListDetails
{
    public class PriceListDetailsQueries : IPriceListDetailsQueries
    {
        SaleDbContext dbContext;
        IMapper mapper;
        public PriceListDetailsQueries(SaleDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }


        public Task<List<SalePriceListDetail>> GetAll()
        {
            return dbContext.SalePriceListDetails.ToListAsync();
        }
    }
}
