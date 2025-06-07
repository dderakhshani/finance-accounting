using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Eefa.Inventory.Domain.Common;

namespace Eefa.Inventory.Application
{
    public class WarehouseQueries : IWarehousQueries
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IProcedureCallService _procedureCallService;
        public WarehouseQueries(
            IMapper mapper,
            IInvertoryUnitOfWork context,
            IWarehouseRepository warehouseRepository,
            ICurrentUserAccessor currentUserAccessor,
            IProcedureCallService procedureCallService

            )
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _warehouseRepository = warehouseRepository;
            _context = context;
            _procedureCallService = procedureCallService;
        }

        public async Task<WarehouseModel> GetById(int id)
        {
            var entity = await _warehouseRepository.Find(id);
            var models=_mapper.Map<WarehouseModel>(entity);

            var queryCategories = await (from war in _context.WarehousesCategories.Where(a => a.WarehouseId == id && !a.IsDeleted)
                               join cat in _context.CommodityCategories
                                    on war.CommodityCategoryId equals cat.Id
                               select new CommodityCategoryModel
                               {
                                   Id = cat.Id,
                                   Title = cat.Title
                               }).ToListAsync();
            var queryStatus = await (from war in _context.WarehousesCodeVoucherGroups.Where(a => a.WarehouseId == id && !a.IsDeleted)
                                         join cod in _context.CodeVoucherGroups
                                              on war.CodeVoucherGroupId equals cod.Id
                                         select new ReceiptALLStatusModel
                                         {
                                             Id = cod.Id,
                                             Title = cod.Title
                                         }).ToListAsync();
            models.CommodityCategories = queryCategories;
            models.ReceiptAllStatus = queryStatus;

            return models;
        }
        public async Task<PagedList<WarehousesLastLevelViewModel>> GetWarehousesLastLevel(PaginatedQueryModel paginatedQuery)
        {
            var AllowAccessToWareHouse = _context.AccessToWarehouse.Where(a =>
                                                                            a.TableName == ConstantValues.AccessToWarehouseEnam.Warehouses
                                                                            && a.UserId == _currentUserAccessor.GetId()
                                                                            && !a.IsDeleted
                                                                            ).Select(a => a.WarehouseId).ToList();

            var entity = await _context.WarehousesLastLevelView.Where(a => AllowAccessToWareHouse.Contains(a.Id)).ProjectTo<WarehousesLastLevelViewModel>(_mapper.ConfigurationProvider)
                .OrderBy(a => a.Sort)
                .ToListAsync();

            var result = new PagedList<WarehousesLastLevelViewModel>()
            {
                Data = entity,
                TotalCount = 0
            };
            return result;
        }
       
        public async Task<PagedList<WarehousesLastLevelViewModel>> GetWarehousesLastLevelByCodeVoucherGroupId(int CodeVoucherGroupId,PaginatedQueryModel paginatedQuery)
        {
            var AllowAccessToWareHouse = _context.AccessToWarehouse.Where(a => 
                                                                            a.TableName == ConstantValues.AccessToWarehouseEnam.Warehouses 
                                                                            && a.UserId == _currentUserAccessor.GetId()
                                                                            && !a.IsDeleted
                                                                            ).Select(a => a.WarehouseId).ToList();
            var entity = await _procedureCallService.GetWarehousesLastLevelByCodeVoucherGroupId(CodeVoucherGroupId);

           
            var result = new PagedList<WarehousesLastLevelViewModel>()
            {
                Data = entity.Where(a => AllowAccessToWareHouse.Contains(a.Id)),
                TotalCount = 0
            };
            return result;
        }
        public async Task<PagedList<WarehouseModel>> GetAll(PaginatedQueryModel query)
        {
            
            var entities = _warehouseRepository.GetAll().Where(a=>!a.IsDeleted )
                           .ProjectTo<WarehouseModel>(_mapper.ConfigurationProvider)
                           .FilterQuery(query.Conditions)
                           .OrderByMultipleColumns(query.OrderByProperty);
            return new PagedList<WarehouseModel>()
            {

                Data = await entities .Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            };
        }

        
    }
}
