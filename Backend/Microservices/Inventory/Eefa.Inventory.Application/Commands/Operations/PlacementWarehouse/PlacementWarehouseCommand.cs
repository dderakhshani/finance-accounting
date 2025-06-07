using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class PlacementWarehouseCommand : CommandBase, IRequest<ServiceResult<ReceiptQueryModel>>, IMapFrom<PlacementWarehouseCommand>, ICommand
    {
        public int Id { get; set; }
        public int DocumentItemId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public int Quantity { get; set; } = default!;
        public int WarehouseLayoutId { get; set; }
        public int WarehouseLayoutQuantityId { get; set; }
        public int WarehouseId { get; set; }




    }

    public class PlacementWarehouseCommandHandler : IRequestHandler<PlacementWarehouseCommand, ServiceResult<ReceiptQueryModel>>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptQueries _receiptQueries;
        private readonly IRepository<WarehouseStocks> _stockRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IRepository<WarehouseHistory> _WarehouseHistory;
        private readonly IWarehouseLayoutRepository _WarehouseLayoutRepository;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<WarehouseLayoutQuantity> _WarehouseLayoutQuantityRepository;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;

        public PlacementWarehouseCommandHandler(
            IMapper mapper,
            IInvertoryUnitOfWork context,
            IReceiptQueries receiptQueries,
            IReceiptRepository receiptRepository,
            IRepository<WarehouseStocks> stockRepository,
            IWarehouseLayoutRepository WarehouseLayout,
            IRepository<WarehouseHistory> warehouseHistory,
            IRepository<DocumentItem> documentItemRepository,
            IRepository<WarehouseLayoutQuantity> WarehouseLayoutQuantity,
            IWarehouseLayoutCommandsService warehouseLayoutCommandsService
            )
        {
            _context = context;
            _mapper = mapper;
            _WarehouseHistory = warehouseHistory;
            _receiptQueries = receiptQueries;
            _stockRepository = stockRepository;
            _receiptRepository = receiptRepository;
            _WarehouseLayoutRepository = WarehouseLayout;
            _documentItemRepository = documentItemRepository;
            _WarehouseLayoutQuantityRepository = WarehouseLayoutQuantity;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;

        }

        public async Task<ServiceResult<ReceiptQueryModel>> Handle(PlacementWarehouseCommand request, CancellationToken cancellationToken)
        {



            //--------------درصورتی که تغییر در انبار اتفاق افتاده باشد و از یک انبار دیگر به این انبار آورده شده باشد.---
            //-------------------یافتن محل گذشته در انبار-------------------------------------------------------------------
            var docItems = await _documentItemRepository.GetAll().Where(x => x.Id == request.DocumentItemId).FirstOrDefaultAsync();
            if (docItems == null)
            {
                throw new ValidationError("هیچ سندی برای این کالا در انبار پیدا نشد");
            }
            //ترتیب ویرایش و اضافه کردن محل جدید مهم است
            await UpdateOldPlacement(request, docItems);

            await InsertNewPlacement(request);

            ReceiptQueryModel model = await UpdateIsPlacementCompleteReceipt(request);

            return ServiceResult<ReceiptQueryModel>.Success(model);

        }

        private async Task<int> InsertNewPlacement(PlacementWarehouseCommand request)
        {
            WarehouseLayoutQuantity WarehouseLayoutQuantity =await _context.WarehouseLayoutQuantities.Where(a => a.Id == request.WarehouseLayoutQuantityId).FirstOrDefaultAsync();
            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(request.CommodityId, request.Quantity, request.WarehouseLayoutId, request.DocumentItemId, (int)(WarehouseHistoryMode.Enter));
            await UpdateWarehouseLayout(request.WarehouseId, request.WarehouseLayoutId, request.CommodityId, request.Quantity,  (int)(WarehouseHistoryMode.Enter), WarehouseLayoutQuantity);
           
            return 1;
        }

        private async Task<int> UpdateOldPlacement(PlacementWarehouseCommand request, DocumentItem docItems)
        {
            var receipt = await _receiptRepository.GetAll().Where(a => a.Id == docItems.DocumentHeadId).FirstOrDefaultAsync();
            var WarehouseLayoutId_Old = _context.WarehouseHistories.Where(a => a.DocumentItemId == docItems.Id)
                                                                       .OrderByDescending(a => a.Id)
                                                                       .Select(a => a.WarehouseLayoutId)
                                                                       .Last();

            if (WarehouseLayoutId_Old != null&& request.WarehouseLayoutId != WarehouseLayoutId_Old)
            {
                    var WarehouseLayoutQuantity = await _context.WarehouseLayoutQuantities.Where(a => a.WarehouseLayoutId == WarehouseLayoutId_Old && a.CommodityId == docItems.CommodityId)
                                                                                            .FirstOrDefaultAsync();
                await _warehouseLayoutCommandsService.DeleteWarehouseHistory(request.DocumentItemId, docItems.CommodityId);
                await UpdateWarehouseLayout(receipt.WarehouseId, WarehouseLayoutId_Old, request.CommodityId, request.Quantity, (int)(WarehouseHistoryMode.Exit), WarehouseLayoutQuantity);
              


            }
            return 1;

        }

        private async Task<ReceiptQueryModel> UpdateIsPlacementCompleteReceipt(PlacementWarehouseCommand request)
        {

            var warehouseId = await _WarehouseLayoutRepository.GetAll().Where(a => a.Id == request.WarehouseLayoutId).Select(a => a.WarehouseId).FirstOrDefaultAsync();
            Domain.Receipt receipt = await _receiptRepository.Find(request.Id);
            receipt.WarehouseId = Convert.ToInt32(warehouseId);
            var model = await _receiptQueries.GetPlacementById(request.Id, receipt.WarehouseId);
            var QuantityChose = model.Items.Sum(a => a.QuantityChose);

            if (QuantityChose <= 0)
            {
                receipt.IsPlacementComplete = true;
            }
            _receiptRepository.Update(receipt);
            await _receiptRepository.SaveChangesAsync();


            return model;
        }
        //-----------------افزایش و کاهش ظرفیت فعلی در مکان--------------------
        private async Task UpdateWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int historyMode, WarehouseLayoutQuantity warehouseLayoutQuantityId)
        {
            var stock =await _context.WarehouseStocks.Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();

            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, warehouseLayoutQuantityId, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, historyMode, stock);
           
        }


    }
}