using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;
using Eefa.Inventory.Domain.Enum;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OpenApi.Extensions;
using NetTopologySuite.Operation.Overlay;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Eefa.Inventory.Application
{
    public class WarehouseCountFormHeadQueries : IWarehouseCountFormHeadQueries
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _inventoryUnitOfWork;
        public WarehouseCountFormHeadQueries(IMapper mapper, IInvertoryUnitOfWork inventoryUnitOfWork)
        {
            _mapper = mapper;
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }
        public async Task<PagedList<WarehouseCountFormHeadModel>> GetAll(PaginatedQueryModel query)
        {
            var entities = GetWarehouseCountFormHeadQuery()
                    .FilterQuery(query.Conditions)
                    .OrderByMultipleColumns(query.OrderByProperty).AsNoTracking();

            var result = new PagedList<WarehouseCountFormHeadModel>()
            {
                TotalCount = query.PageIndex <= 1
                            ? entities.Count()
                            : 0,
                Data = await entities.Paginate(query.Paginator()).ToListAsync(),

            };
            return result;
        }
        public async Task<WarehouseCountFormHeadModel> GetWarehouseCountFormHeadById(int id)
        {
            var result = await GetWarehouseCountFormHeadQuery().FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }
        public async Task<List<WarehouseCountFormHeadModel>> GetWarehouseCountFormHeadByParentId(int parentId)
        {
            var result = await GetWarehouseCountFormHeadQuery().Where(x => x.ParentId == parentId || x.Id == parentId)
                .ToListAsync();
            return result;
        }
        public async Task<PagedList<WarehouseCountFormDetailsModel>> GetDetailsByHeadId(PaginatedQueryModel query, int warehouseCountFormHeadeId)
        {
            var entities = (from wcd in _inventoryUnitOfWork.WarehouseCountFormDetail
                            join wlq in _inventoryUnitOfWork.WarehouseLayoutQuantities on wcd.WarehouseLayoutQuantitiesId equals wlq.Id
                            join c in _inventoryUnitOfWork.Commodities on wlq.CommodityId equals c.Id
                            join m in _inventoryUnitOfWork.MeasureUnits on c.MeasureId equals m.Id
                            join wly in _inventoryUnitOfWork.WarehouseLayouts on wlq.WarehouseLayoutId equals wly.Id
                            where wcd.WarehouseCountFormHeadId == warehouseCountFormHeadeId
                            select new WarehouseCountFormDetailsModel
                            {
                                Id = wcd.Id,
                                CommodityCompactCode = c.CompactCode,
                                CommodityCode = c.Code,
                                MeasureTitle = m.Title,
                                CommodityName = c.Title,
                                CommodityId = c.Id,
                                CountedQuantity = wcd.CountedQuantity,
                                Description = wcd.Description,
                                LastWarehouseLayoutStatus = wcd.LastWarehouseLayoutStatus,
                                SystemQuantity = wcd.SystemQuantity.HasValue ? wcd.SystemQuantity.Value : 0,
                                WarehouseLayoutQuantityId = wcd.WarehouseLayoutQuantitiesId,
                                WarehouseLayoutTitle = wly.Title,
                            })
                            .FilterQuery(query.Conditions)
                            .OrderByMultipleColumns(query.OrderByProperty);

            var result = new PagedList<WarehouseCountFormDetailsModel>()
            {
                TotalCount = query.PageIndex <= 1 ? entities.Count() : 0,
                Data = await entities.Paginate(query.Paginator()).ToListAsync()
            };
            return result;
        }

        public async Task<PagedList<WarehouseCountFormDetailsModel>> GetAlldiscrepancies(PaginatedQueryModel query, int warehouseCountFormHeadeId)
        {
            var entities = (from wcd in _inventoryUnitOfWork.WarehouseCountFormDetail
                            join wlq in _inventoryUnitOfWork.WarehouseLayoutQuantities on wcd.WarehouseLayoutQuantitiesId equals wlq.Id
                            join c in _inventoryUnitOfWork.Commodities on wlq.CommodityId equals c.Id
                            join m in _inventoryUnitOfWork.MeasureUnits on c.MeasureId equals m.Id
                            join wly in _inventoryUnitOfWork.WarehouseLayouts on wlq.WarehouseLayoutId equals wly.Id
                            where wcd.WarehouseCountFormHeadId == warehouseCountFormHeadeId &&
                            wcd.CountedQuantity.Value != wlq.Quantity

                            select new WarehouseCountFormDetailsModel
                            {
                                Id= wlq.Id, 
                                CommodityCompactCode = c.CompactCode,
                                CommodityCode=c.Code,
                                MeasureTitle=m.Title,
                                CommodityName = c.Title,
                                CommodityId = c.Id,
                                CountedQuantity = wcd.CountedQuantity,
                                Description = wcd.Description,
                                LastWarehouseLayoutStatus = wcd.LastWarehouseLayoutStatus,
                                SystemQuantity = wcd.SystemQuantity.HasValue?wcd.SystemQuantity.Value:0,
                                WarehouseLayoutQuantityId = wcd.WarehouseLayoutQuantitiesId,                                
                                WarehouseLayoutTitle = wly.Title,
                                WarehouseLayoutId=wly.Id,
                                ConflictQuantity=wcd.CountedQuantity.HasValue? wlq.Quantity- wcd.CountedQuantity.Value :null
                            })
                            .FilterQuery(query.Conditions)
                            .OrderByMultipleColumns(query.OrderByProperty);

            var result = new PagedList<WarehouseCountFormDetailsModel>()
            {
                Data = await entities.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1 ? entities.Count() : 0
            };
            return result;
        }
        public async Task<PagedList<WarehouseCountFormReport>> GetWarehouseCountReport(PaginatedQueryModel query, int warehouseCountFormHeadeId)
        {           
            var groupedEntities = _inventoryUnitOfWork.GetWarehouseCountReport(warehouseCountFormHeadeId);                  
            groupedEntities = groupedEntities.FilterQuery(query.Conditions);          
            var result = new PagedList<WarehouseCountFormReport>
            {
                Data = await groupedEntities.Paginate(query.Paginator()).ToListAsync(), 
                TotalCount = query.PageIndex <= 1 ? await groupedEntities.CountAsync() : 0 
            };
            return result;
        }

        public async Task<PagedList<WarehouseCommodityWithPriceModel>> GetCommoditisWithPrice(PaginatedQueryModel query, int warehouseId)
        {
            var entities = (from wlq in _inventoryUnitOfWork.WarehouseLayoutQuantities
                            join c in _inventoryUnitOfWork.Commodities on wlq.CommodityId equals c.Id
                            join wly in _inventoryUnitOfWork.WarehouseLayouts on wlq.WarehouseLayoutId equals wly.Id
                            join ws in _inventoryUnitOfWork.WarehouseStocks on wlq.CommodityId equals ws.CommodityId
                            join m in _inventoryUnitOfWork.MeasureUnits on c.MeasureId equals m.Id
                            where wly.WarehouseId == warehouseId && !wly.IsDeleted && !ws.IsDeleted && !wlq.IsDeleted && !c.IsDeleted
                            group new { wlq, c, ws, wly,m }
                            by new
                            {
                                wlq.CommodityId,
                                c.CompactCode,
                                c.Code,
                                CommodityTitle = c.Title,                                
                                WarehouseLayoutTitle = wly.Title,
                                WarehouseLayoutId = wly.Id,
                                wlq.Id,                               
                                wlq.Quantity,
                                measureTitle=m.Title
                            } into g
                            select new WarehouseCommodityWithPriceModel
                            {
                                CommodityId = g.Key.CommodityId,
                                CommodityCompactCode = g.Key.CompactCode,
                                CommodityCode=g.Key.Code,
                                CommodityName = g.Key.CommodityTitle,
                                MeasureTitle = g.Key.measureTitle,
                                WarehouseLayoutQuantityId = g.Key.Id,
                                SystemQuantity = g.Key.Quantity,
                                WarehouseLayoutTitle = g.Key.WarehouseLayoutTitle,
                                WarehouseLayoutId = g.Key.WarehouseLayoutId,
                                Id = g.Key.Id,
                                Price = g.Max(x => x.ws.Price)
                            }).FilterQuery(query.Conditions).Distinct()
                            .OrderByMultipleColumns(query.OrderByProperty).AsNoTracking();
            
            var result = new PagedList<WarehouseCommodityWithPriceModel>()
            {
                TotalCount = query.PageIndex <= 1 ? entities.Count() : 0,
                Data = await entities.Paginate(query.Paginator()).ToListAsync()
            };
            return result;
        }
        //private method
        private IQueryable<WarehouseCountFormHeadModel> GetWarehouseCountFormHeadQuery()
        {
            var result = (from wcf in _inventoryUnitOfWork.WarehouseCountFormHead
                          join wl in _inventoryUnitOfWork.WarehouseLayouts on wcf.WarehouseLayoutId equals wl.Id
                          join w in _inventoryUnitOfWork.Warehouses on wl.WarehouseId equals w.Id
                          join u in _inventoryUnitOfWork.User on wcf.ConfirmerUserId equals u.Id
                          join u2 in _inventoryUnitOfWork.User on wcf.CounterUserId equals u2.Id
                          join p in _inventoryUnitOfWork.Persons on u.PersonId equals p.Id
                          join p2 in _inventoryUnitOfWork.Persons on u2.PersonId equals p2.Id
                          select new WarehouseCountFormHeadModel
                          {
                              ConfirmerUserId = wcf.ConfirmerUserId,
                              CounterUserId = wcf.CounterUserId,
                              Description = wcf.Description,
                              FormDate = wcf.FormDate,
                              ParentId = wcf.ParentId,
                              FormState = wcf.FormState,
                              FormStateMessage = wcf.FormState.GetAttributeOfType<DescriptionAttribute>().Description.ToString(),
                              Id = wcf.Id,
                              WarehouseLayoutTitle =w.Title +'-'+ wl.Title,
                              WarehouseLayoutId = wl.Id,
                              ConfirmerUserName = p.FirstName + ' ' + p.LastName,
                              CounterUserName = p2.FirstName + " " + p2.LastName,
                              FormNo = wcf.FormNo,
                              WarehouseId = wcf.WarehouseId
                          });
            return result;
        }

    }
}
