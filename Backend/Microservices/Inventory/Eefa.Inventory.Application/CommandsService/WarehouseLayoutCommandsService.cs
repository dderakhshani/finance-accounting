using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Invertory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;
using System.Collections.Generic;
using Eefa.Common.Exceptions;
using Eefa.Invertory.Infrastructure.Repositories;
using System;

namespace Eefa.Inventory.Application
{
    public class WarehouseLayoutCommandsService : IWarehouseLayoutCommandsService
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IRepository<WarehouseLayoutQuantity> _WarehouseLayoutQuantityRepository;
        private readonly IRepository<WarehouseHistory> _WarehouseHistoryRepository;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IRepository<WarehouseStocks> _stockRepository;


        public WarehouseLayoutCommandsService(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , IRepository<WarehouseLayoutQuantity> WarehouseLayoutQuantityRepository
            , IRepository<WarehouseHistory> WarehouseHistoryRepository
            , IRepository<WarehouseStocks> stockRepository
            , IProcedureCallService procedureCallService

            )
        {
            _mapper = mapper;
            _context = context;
            _WarehouseLayoutQuantityRepository = WarehouseLayoutQuantityRepository;
            _WarehouseHistoryRepository = WarehouseHistoryRepository;
            _stockRepository = stockRepository;
            _procedureCallService = procedureCallService;


        }

        public async Task<int> InsertAndUpdateWarehouseHistory(int CommodityId, double Quantity, int WarehouseLayoutId, int? documentItemId, int historyMode)
        {

            var history = await _context.WarehouseHistories.Where(a => a.DocumentItemId == documentItemId && a.Commodityld == CommodityId && !a.IsDeleted).FirstOrDefaultAsync();
            var WarehouseId = await _context.WarehouseLayouts.Where(a => a.Id == WarehouseLayoutId).Select(a => a.WarehouseId).FirstOrDefaultAsync();
            var DocumentItems = await _context.DocumentItems.Where(a => a.Id == documentItemId && !a.IsDeleted).FirstOrDefaultAsync();
            var DocumentHeadId = DocumentItems.DocumentHeadId;


            if (history != null)
            {
                await UpdateHistory(Quantity, history, DocumentItems);


            }
            else
            {
                await InsertHistory(CommodityId, Quantity, WarehouseLayoutId, historyMode, WarehouseId, DocumentHeadId, DocumentItems);
            }



            await _procedureCallService.UpdateWarehouseLayoutQuantities(CommodityId, WarehouseLayoutId);

            //await _procedureCallService.UpdateStockQuantity(CommodityId, WarehouseId);




            return 1;

        }

        private async Task UpdateHistory(double Quantity, WarehouseHistory history, DocumentItem DocumentItems)
        {
            try
            {
                history.Quantity = Quantity;

                if (DocumentItems.BomValueHeaderId != null)
                {
                    history.AVGPrice = await _context.DocumentItemsBom.Where(a => a.BomValueHeaderId == DocumentItems.BomValueHeaderId && a.CommodityId == history.Commodityld).Select(a => a.UnitPrice).FirstOrDefaultAsync();
                }
                else
                {
                    history.AVGPrice = DocumentItems.UnitPrice;
                }

                _WarehouseHistoryRepository.Update(history);
                await _WarehouseHistoryRepository.SaveChangesAsync();
            }
            catch (Exception ex) { }
           
        }

        private async Task InsertHistory(int CommodityId, double Quantity, int WarehouseLayoutId, int historyMode, int? WarehouseId, int DocumentHeadId, DocumentItem DocumentItems)
        {


            WarehouseHistory history = new WarehouseHistory()
            {
                Commodityld = CommodityId,
                WarehouseLayoutId = WarehouseLayoutId,
                Quantity = Quantity,
                WarehousesId = WarehouseId,
                DocumentHeadId = DocumentHeadId,
                DocumentItemId = DocumentItems.Id,
                AvailableCount = await _context.WarehouseHistoriesDocumentView.Where(a => a.Commodityld == CommodityId && a.WarehouseLayoutId == WarehouseLayoutId).SumAsync(a => a.Quantity * a.Mode),
                AVGPrice = DocumentItems.BomValueHeaderId == null ? DocumentItems.UnitPrice : await _context.DocumentItemsBom.Where(a => a.BomValueHeaderId == DocumentItems.BomValueHeaderId && a.CommodityId == CommodityId).Select(a => a.UnitPrice).FirstOrDefaultAsync(),
                Mode = historyMode,
            };

            _WarehouseHistoryRepository.Insert(history);
            await _WarehouseHistoryRepository.SaveChangesAsync();

        }

        public async Task<int> DeleteWarehouseHistory(int? documentItemId, int CommodityId)
        {
            var history = await _WarehouseHistoryRepository.GetAll().Where(a => a.DocumentItemId == documentItemId && a.Commodityld == CommodityId && !a.IsDeleted).FirstOrDefaultAsync();

            if (history != null)
            {
                var WarehouseId = await _context.WarehouseLayouts.Where(a => a.Id == history.WarehouseLayoutId).Select(a => a.WarehouseId).FirstOrDefaultAsync();
                _WarehouseHistoryRepository.Delete(history);
                if (await _WarehouseHistoryRepository.SaveChangesAsync() > 0)
                {
                    await _procedureCallService.UpdateWarehouseLayoutQuantities(CommodityId, history.WarehouseLayoutId);

                    await _procedureCallService.UpdateStockQuantity(CommodityId, WarehouseId);
                }
            }


            return 1;
        }

        public async Task<int> InsertLayoutQuantity(int CommodityId, double Quantity, int historyMode, WarehouseLayoutQuantity warehouseLayoutQuantity, int WarehouseLayoutId)
        {

            if (warehouseLayoutQuantity == null)
            {
                WarehouseLayoutQuantity model = new WarehouseLayoutQuantity()
                {
                    Quantity = (historyMode) * Quantity,
                    CommodityId = CommodityId,
                    WarehouseLayoutId = WarehouseLayoutId
                };

                _WarehouseLayoutQuantityRepository.Insert(model);


                return await _WarehouseLayoutQuantityRepository.SaveChangesAsync();



            }
            return 0;

        }

        public async Task<int> InsertStock(int WarehouseId, int CommodityId, double Quantity, int historyMode, WarehouseStocks stock)
        {

            if (stock == null)
            {
                WarehouseStocks model = new WarehouseStocks()
                {
                    Quantity = (historyMode) * Quantity,
                    WarehousId = WarehouseId,
                    CommodityId = CommodityId
                };

                _stockRepository.Insert(model);


                return await _stockRepository.SaveChangesAsync();


            }
            return 1;
        }
        public async Task<WarehouseLayout> FindLayout(int warehouseId, int CommodityId)
        {

            var warehouseLayoutQuantity = new Domain.WarehouseLayoutQuantity();
            var layouts = new Domain.WarehouseLayout();
            var CompactCode = await _context.Commodities.Where(a => a.Id == CommodityId).Select(a => a.CompactCode).FirstOrDefaultAsync();

            List<int> CommodityIds = new List<int>(CommodityId);


            if (CompactCode != null)
            {
                var com = await _context.Commodities.Where(a => a.CompactCode == CompactCode && !a.IsDeleted).Select(a => a.Id).ToListAsync();
                CommodityIds.AddRange(com);
            }

            var warehouseLayouts = await (from quantity in _context.WarehouseLayoutQuantities.Where(a => CommodityIds.Contains(a.CommodityId) && !a.IsDeleted)
                                          join layout in _context.WarehouseLayouts.Where(a => a.WarehouseId == warehouseId )
                                          on quantity.WarehouseLayoutId equals layout.Id
                                          select quantity).ToListAsync();

            warehouseLayoutQuantity = warehouseLayouts.Where(s => s.Quantity == warehouseLayouts.Max(x => x.Quantity)).FirstOrDefault();


            //---------------------------اگر محلی یافت نشد که کالا قبلا در آن جایگذاری شده باشد
           
            if (warehouseLayoutQuantity == null)
            {
                layouts = await _context.WarehouseLayouts.Where(a => a.WarehouseId == warehouseId && a.IsDefault == true).FirstOrDefaultAsync();

                if (layouts == null)
                {
                    throw new ValidationError("برای انبار انتخاب شده در این سند هیچ محل جایگذاری تعریف نشده است");
                }
            }
            else
            {
                layouts =    await _context.WarehouseLayouts.Where(a => a.Id == warehouseLayoutQuantity.WarehouseLayoutId).FirstOrDefaultAsync(); ;
            }

            return layouts;
        }
    }
}
