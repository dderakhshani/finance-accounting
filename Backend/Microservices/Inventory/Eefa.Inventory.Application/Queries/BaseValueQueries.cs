using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common.Data;
using Eefa.Inventory.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public class BaseValueQueriesQueries : IBaseValueQueries
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _contex;
       
        
        public BaseValueQueriesQueries(
              IMapper mapper
            , IInvertoryUnitOfWork contex
            
            )
        {
            _mapper = mapper;
            _contex = contex;
        }
        public async Task<PagedList<BaseValueModel>> GetCurrencyBaseValue()
        {

            var entitis = _contex.BaseValues.Where(a => a.BaseValueTypeId == ConstantValues.ConstBaseValue.currencyBaseValueId)
                    .ProjectTo<BaseValueModel>(_mapper.ConfigurationProvider);
                    
            return new PagedList<BaseValueModel>()
            {
                Data = (IEnumerable<BaseValueModel>)await entitis
                    
                    .ToListAsync(),
                TotalCount =0
            };
        }
        public async Task<PagedList<BaseValueModel>> GetCommodityGroupBaseValue()
        {

            var entitis = _contex.BaseValues.Where(a =>  a.BaseValueTypeId == ConstantValues.ConstBaseValue.CommodityGroupsValueId)
                    .ProjectTo<BaseValueModel>(_mapper.ConfigurationProvider);

            return new PagedList<BaseValueModel>()
            {
                Data = (IEnumerable<BaseValueModel>)await entitis

                    .ToListAsync(),
                TotalCount = 0
            };
        }
        public async Task<PagedList<BaseValueModel>> GetDepreciationTypeBaseValue()
        {

            var entitis = _contex.BaseValues.Where(a => a.BaseValueTypeId == ConstantValues.ConstBaseValue.DepreciationTypeBaseValueId)
                    .ProjectTo<BaseValueModel>(_mapper.ConfigurationProvider);

            return new PagedList<BaseValueModel>()
            {
                Data = (IEnumerable<BaseValueModel>)await entitis

                    .ToListAsync(),
                TotalCount = 0
            };
        }
        public async Task<List<string>> GetDocumentTagBaseValue()
        {

            var entitis =await _contex.BaseValues.Where(a => a.BaseValueTypeId == ConstantValues.ConstBaseValue.DocumentTagBaseValueId).Select(a => a.Title).ToListAsync();


            return entitis;
        }
        public async Task<PagedList<BaseValueModel>> GetReceiptALLBaseValue()
        {
            var query = await _contex.BaseValues.Where(a => a.BaseValueTypeId == ConstantValues.ConstBaseValue.BaseValueWarehousePartTypeId).
                ProjectTo<BaseValueModel>(_mapper.ConfigurationProvider)

                .ToListAsync();

            var result = new PagedList<BaseValueModel>()
            {
                Data = query,
                TotalCount = 0
            };
            return result;
        }


    }
}
