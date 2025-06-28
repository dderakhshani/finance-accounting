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
        SaleDbContext _dbContext;
        IMapper _mapper;
        public PriceListDetailsQueries(SaleDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public Task<List<SalePriceListDetail>> GetAll()
        {
            return _dbContext.SalePriceListDetails.ToListAsync();
        }
    }
}
