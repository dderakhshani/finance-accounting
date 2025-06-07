using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using Eefa.Common;
using System.Linq.Dynamic.Core;

namespace Eefa.Inventory.Application
{
    public class WarehouseStocksQueries : IWarehouseStocksQueries
    {
        
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public WarehouseStocksQueries( 
            IMapper mapper,
            IInvertoryUnitOfWork context,
            ICurrentUserAccessor currentUserAccessor

            )               
        {
            _mapper = mapper;
            _context = context;
            _currentUserAccessor = currentUserAccessor;
        }
        /// <summary>
        /// آیا کالا موجودی دارد؟
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        public async Task<bool> GetIsAvailableCommodity(int? CommodityId, string CommodityCode)
        {
            var entities = await (_context.WarehouseLayoutsCommoditiesQuantityView
                .Where(a => (a.CommodityId == CommodityId || CommodityId == null) &&
                            (a.CommodityCode == CommodityCode || string.IsNullOrEmpty(CommodityCode)))
                .SumAsync(a => a.Quantity));
            if (entities <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        /// <summary>
        /// موجودی کالا براساس انبار
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        public async Task<PagedList<WarehouseStockModel>> GetAll(PaginatedQueryModel query)
        {
            List<int> AllowAccessToWareHouse = AccessWarehouse();
            var entities = (from Stocks in _context.WarehouseStocks
                           join com in _context.Commodities on Stocks.CommodityId equals com.Id
                           join war in _context.Warehouses.Where(a=>(AllowAccessToWareHouse.Contains((int)a.Id))) on Stocks.WarehousId equals war.Id
                           select new WarehouseStockModel()
                           {
                               Id = Stocks.Id,
                               WarehouseId = Stocks.WarehousId,
                               CommodityId = Stocks.CommodityId,
                               Quantity = Stocks.Quantity,
                               CommodityCode = com.Code,
                               CommodityTadbirCode = com.TadbirCode,
                               CommodityTitle = com.Title,
                               WarehouseTitle = war.Title,
                               AvailableQuantity = Stocks.Quantity - Stocks.ReservedQuantity,
                               Price= Stocks.Price

                           })
                          
                          .FilterQuery(query.Conditions).OrderBy(a=>a.CommodityCode)
                          .OrderByMultipleColumns(query.OrderByProperty);

            var result = (List<WarehouseStockModel>)await entities.Paginate(query.Paginator()).ToListAsync();
                   
            return new PagedList<WarehouseStockModel>()
            {
                Data = result,
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            };
        }
        private List<int> AccessWarehouse()
        {
            return _context.AccessToWarehouse.Where(a => a.TableName == ConstantValues.AccessToWarehouseEnam.Warehouses && a.UserId == _currentUserAccessor.GetId() && !a.IsDeleted).Select(a => a.WarehouseId).ToList();
        }
        public async Task<string> Url(int Id)
        {
            
            return await _context.Attachment.Where(a => a.Id == Id).Select(a => a.Url).FirstOrDefaultAsync();
        }


    }
}
