using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;
using System.Linq.Dynamic.Core;

namespace Eefa.Inventory.Application
{
    public class CommodityQueries : ICommodityQueries
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IProcedureCallService _procedureCallService;


        public CommodityQueries(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , IProcedureCallService procedureCallService
            )
        {
            _mapper = mapper;
            _context = context;
            _procedureCallService = procedureCallService;
        }

        public async Task<PagedList<ViewCommodityModel>> GetCommodity(int? warehouseId, bool? isOnlyFilterByWarehouse, string searchTerm, PaginatedQueryModel query)
        {
            if (warehouseId != null)
            {
                return await GetCommodityWithWarehouse(warehouseId, searchTerm, isOnlyFilterByWarehouse, query);
            }
            else
            {
                return await GetAllCommodity(searchTerm, query);
            }


        }

        private async Task<PagedList<ViewCommodityModel>> GetCommodityWithWarehouse(int? warehouseId, string searchTerm, bool? isOnlyFilterByWarehouse, PaginatedQueryModel query)
        {
            var list = (from com in _context.ViewCommodity.FilterQuery(query.Conditions)
                        join stock in _context.WarehouseStocks on com.Id equals stock.CommodityId
                        where   (stock.WarehousId == warehouseId)                    
                        select com);


            if (list.Count() == 0 && isOnlyFilterByWarehouse==false)//ممکن است این کالا اصلا در این انبار وجود نداشته باشد و به تازگی به لیست اضافه شده باشد
            {
                list = _context.ViewCommodity.FilterQuery(query.Conditions);
            }

            var result = (List<ViewCommodityModel>)await list
                           .Paginate(query.Paginator()).ProjectTo<ViewCommodityModel>(_mapper.ConfigurationProvider)
                          .ToListAsync();

            return new PagedList<ViewCommodityModel>()
            {
                Data = (IEnumerable<ViewCommodityModel>)result,
                TotalCount = 0

            };
        }

        private async Task<PagedList<ViewCommodityModel>> GetAllCommodity(string searchTerm, PaginatedQueryModel query)
        {

            var result = (List<ViewCommodityModel>)await _context.ViewCommodity
                           .FilterQuery(query.Conditions).Paginate(query.Paginator()).ProjectTo<ViewCommodityModel>(_mapper.ConfigurationProvider)
                          .ToListAsync();

            return new PagedList<ViewCommodityModel>()
            {
                Data = (IEnumerable<ViewCommodityModel>)result,
                TotalCount = 0

            };

        }
        public async Task<PagedList<ViewCommodityModel>> GetCommodityById(int Id, PaginatedQueryModel query)
        {
            var list = _context.ViewCommodity.Where(a => a.Id == Id);


            var result = (List<ViewCommodityModel>)await list
                          .ProjectTo<ViewCommodityModel>(_mapper.ConfigurationProvider)
                          .Paginate(query.Paginator())
                          .ToListAsync();

            return new PagedList<ViewCommodityModel>()
            {
                Data = (IEnumerable<ViewCommodityModel>)result,
                TotalCount = 0

            };

        }
       
        public  double GetQuantityCommodity(int warehouseId, int CommodityId)
        {
            var result = _context.WarehouseStocks.Where(a => a.WarehousId == warehouseId && a.CommodityId == CommodityId).Select(a => a.Quantity).FirstOrDefault();

            return result;
        }


    }
}
