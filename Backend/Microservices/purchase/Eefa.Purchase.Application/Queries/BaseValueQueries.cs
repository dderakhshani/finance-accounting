using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common.Data;
using Eefa.Purchase.Application.Models;
using Eefa.Purchase.Application.Queries.Abstraction;
using Eefa.Purchase.Domain.Common;
using Eefa.Purchase.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Purchase.Application.Queries
{
    public class BaseValueQueriesQueries : IBaseValueQueries
    {

        private readonly IMapper _mapper;
        private readonly PurchaseContext _contex;
       
        
        public BaseValueQueriesQueries(
              IMapper mapper
            , PurchaseContext contex
            
            )
        {
            _mapper = mapper;
            _contex = contex;
        }
        public async Task<PagedList<BaseValueModel>> GetCurrencyBaseValue()
        {

            var entitis = _contex.BaseValues.Where(a => a.BaseValueTypeId == ConstantValues.BaseValue.currencyBaseValueId)
                    .ProjectTo<BaseValueModel>(_mapper.ConfigurationProvider);
                    
            return new PagedList<BaseValueModel>()
            {
                Data = (IEnumerable<BaseValueModel>)await entitis
                    
                    .ToListAsync(),
                TotalCount =0
            };
        }
        public async Task<List<string>> GetDocumentTagBaseValue()
        {

            var entitis =await _contex.BaseValues.Where(a => a.BaseValueTypeId == ConstantValues.BaseValue.DocumentTagBaseValueId).Select(a => a.Title).ToListAsync();


            return entitis;
        }
       
        public async Task<PagedList<BaseValueModel>> GetInvoiceALLBaseValue()
        {
            var query = await _contex.BaseValues.Where(a => a.BaseValueTypeId == ConstantValues.BaseValue.InvoiceALLBaseValue).
                ProjectTo<BaseValueModel>(_mapper.ConfigurationProvider)

                .ToListAsync();

            var result = new PagedList<BaseValueModel>()
            {
                Data = query,
                TotalCount = 0
            };
            return result;
        }

        public async Task<string> GeVatDutiesTaxValue()
        {
            var entitis = await _contex.BaseValues.Where(a => a.UniqueName == ConstantValues.BaseValue.vatDutiesTax).Select(a => a.Value).FirstOrDefaultAsync();


            return entitis;
        }
    }
}
