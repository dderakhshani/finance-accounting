using AutoMapper;
using Eefa.Common.Data;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Sale.Application.Queries.PriceList
{
    public class PriceListQueries : IPriceListQueries
    {
        private readonly SaleDbContext _dbContext;
        private readonly IMapper _mapper;

        public PriceListQueries(SaleDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<SalePriceList>> GetAll()
        {
            return await _dbContext.SalePriceLists.Where(x => x.IsDeleted != true).ToListAsync();
        }

        public async Task<SalePriceList> GetById(int id)
        {
            return await _dbContext.SalePriceLists.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted != true);
        }
    }
}
