using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Inventory.Domain.Common;

namespace Eefa.Inventory.Application
{
    public class AssetsQueries : IAssetsQueries
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public AssetsQueries(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , ICurrentUserAccessor currentUserAccessor
            )
        {
            _mapper = mapper;
            _context = context;
            _currentUserAccessor = currentUserAccessor;

        }
        public async Task<PagedList<AssetsModel>> GetAll(string FromDate, string ToDate, PaginatedQueryModel query)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            List<int> AllowAccessToWareHouse = AccessWarehouse();
            var list =  _context.AssetsView.Where(a =>
                                                      (a.DocumentDate >= from || from == null)
                                                   && (a.DocumentDate <= to || to == null)
                                                   && (AllowAccessToWareHouse.Contains((int)a.WarehouseId)))
                       .FilterQuery(query.Conditions)
                       .ProjectTo<AssetsModel>(_mapper.ConfigurationProvider)
                       .OrderByMultipleColumns().Distinct();
                       
            var result = new PagedList<AssetsModel>()
            {
                Data = await list.Paginate(query.Paginator())
                       .ToListAsync(),
                TotalCount = query.PageIndex <= 1
                            ? list.Count()

                            : 0
            };
            return result;
        }

        public async Task<AssetsModel> GetByDocumentId(int DocumentItemsId, int CommodityId)
        {

            List<AssetsSerialModel> AssetsSerials = new List<AssetsSerialModel>();

            var entities = queryable().Where(a => a.DocumentItemId == DocumentItemsId).FirstOrDefault();
            List<int> AllowAccessToWareHouse = AccessWarehouse();
            AssetsSerials = await (from assets in _context.AssetsView.Where(a => a.DocumentItemId == DocumentItemsId && a.CommodityId == CommodityId && (AllowAccessToWareHouse.Contains((int)a.WarehouseId)))
                                   select new AssetsSerialModel
                                   {
                                       Id = assets.Id,
                                       CommodityId = assets.CommodityId,
                                       Serial = assets.AssetSerial,
                                       Title = assets.CommodityTitle,
                                       CommoditySerial = assets.CommoditySerial
                                   }
                       ).ToListAsync();
            if (AssetsSerials.Count() > 0)
            {
                entities.AssetsSerials = AssetsSerials;
            }

            return entities;


        }
        public async Task<AssetsModel> GetById(int id)
        {
            var entity = await _context.Assets.Where(a => a.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<AssetsModel>(entity);
        }
        public async Task<int[]> GetAssetAttachmentsIdByPersonsDebitedCommoditiesId(int AssetId,int PersonsDebitedCommoditiesId)
        {
            var Ids = await _context.AssetAttachments.Where(a => a.AssetId == AssetId && a.PersonsDebitedCommoditiesId== PersonsDebitedCommoditiesId).Select(a=>a.AttachmentId).ToArrayAsync();
            return Ids;
        }
        public async Task<string> GetLastNumber(int AssetGroupId)
        {
            var AssetSerial = await _context.Assets.Where(a => a.AssetGroupId == AssetGroupId &&  !a.IsDeleted).MaxAsync(a=>a.AssetSerial);
            return AssetSerial;
        }
        public async Task<AssetsSerialModel> GetDuplicateAssets(AssetsSerialModel[] AssetsSerialModels)
        {
            foreach(var item in AssetsSerialModels)
            {
                var isDuplicate = await _context.Assets.Where(a => a.AssetSerial == item.Serial && (a.Id!=item.Id || item.Id ==null) && !a.IsDeleted).CountAsync() > 0 ? true : false;
                if(isDuplicate)
                {
                    return item;
                }
            }
            
            return new AssetsSerialModel();
        }
        private IQueryable<AssetsModel> queryable()
        {
            return (from assets in _context.AssetsView

                    select new AssetsModel
                    {
                        WarehouseId = assets.WarehouseId,
                        WarehouseTitle = assets.WarehousesTitle,
                        CommodityId = assets.CommodityId,
                        CommodityTitle = assets.CommodityTitle,
                        MeasureId = assets.MeasureId,
                        MeasureTitle = assets.MeasureTitle,
                        AssetGroupId = assets.AssetGroupId,
                        AssetGroupTitle = assets.AssetGroupTitle,
                        UnitId = assets.UnitId,
                        UnitTitle = assets.UnitsTitle,
                        DocumentDate = assets.DocumentDate,
                        CommoditySerial = assets.CommoditySerial,
                        AssetSerial = assets.AssetSerial,
                        DepreciationTypeBaseId = assets.DepreciationTypeBaseId,
                        DepreciationTitle = assets.DepreciationTitle,
                        Price = assets.Price,
                        DepreciatedPrice = assets.DepreciatedPrice,
                        IsActive = (bool)assets.IsActive,
                        DocumentHeadId = assets.DocumentHeadId,
                        DocumentItemId = assets.DocumentItemId,


                    }
                       );

        }
        private List<int> AccessWarehouse()
        {
            return _context.AccessToWarehouse.Where(a => a.TableName == ConstantValues.AccessToWarehouseEnam.Warehouses && a.UserId == _currentUserAccessor.GetId() && !a.IsDeleted).Select(a => a.WarehouseId).ToList();
        }

    }
}
