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
    public class AddDocumentItemsBomQuantityCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<AddDocumentItemsBomQuantityCommand>, ICommand
    {
        public int DocumentItemsId { get; set; }
        public int CommodityId { get; set; }
        public double Quantity { get; set; } = default!;
    }

    public class AddDocumentItemsBomQuantityCommandHandler : IRequestHandler<AddDocumentItemsBomQuantityCommand, ServiceResult<Domain.Receipt>>
    {

        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<DocumentItemsBom> _documentItemsBomepository;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;



        public AddDocumentItemsBomQuantityCommandHandler(

             ICurrentUserAccessor currentUserAccessor
            , IInvertoryUnitOfWork context
            , IMapper mapper
            , IRepository<DocumentItem> documentItemRepository
            , IRepository<DocumentItemsBom> documentItemsBomrepository
            , IReceiptCommandsService receiptCommandsService
            , IWarehouseLayoutCommandsService warehouseLayoutCommandsService
            , IProcedureCallService procedureCallService
            )

        {
            _mapper = mapper;
            _context = context;
            _documentItemRepository = documentItemRepository;
            _procedureCallService = procedureCallService;
            _receiptCommandsService = receiptCommandsService;
            _currentUserAccessor = currentUserAccessor;
            _documentItemsBomepository = documentItemsBomrepository;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;

        }
        public async Task<ServiceResult<Domain.Receipt>> Handle(AddDocumentItemsBomQuantityCommand request, CancellationToken cancellationToken)
        {


            var receiptItems = await _documentItemRepository.GetAll().Where(a => a.Id == request.DocumentItemsId).FirstOrDefaultAsync();
            if (receiptItems == null)
            {
                throw new ValidationError("اقلام سند موجود نیست");
            }
            var ToReceipt = await _context.DocumentHeads.Where(a => a.Id == receiptItems.DocumentHeadId).FirstOrDefaultAsync();
            if (ToReceipt == null)
            {
                throw new ValidationError("سند موجود نیست");
            }
            var FromReceipt = await _context.DocumentHeads.Where(a => a.ParentId == ToReceipt.Id).FirstOrDefaultAsync();
            if (ToReceipt == null)
            {
                throw new ValidationError("سند موجود نیست");
            }
            var Fromlayouts =await _context.WarehouseLayouts.Where(_a => _a.WarehouseId == FromReceipt.WarehouseId && _a.LastLevel == true).FirstOrDefaultAsync();

            var MeasureId =await _context.Commodities.Where(a => a.Id == request.CommodityId).Select(a => a.MeasureId).FirstOrDefaultAsync();

            await Save(request, receiptItems, ToReceipt, Fromlayouts, MeasureId);

            return ServiceResult<Domain.Receipt>.Success(ToReceipt);



        }

        private async Task Save(AddDocumentItemsBomQuantityCommand request, DocumentItem receiptItems, Receipt ToReceipt, WarehouseLayout Fromlayouts, int? MeasureId)
        {
            await AddBom(request, receiptItems, Fromlayouts, MeasureId);

            await UpdateWarehouseLayout(Convert.ToInt32(Fromlayouts.WarehouseId), Fromlayouts.Id, request.CommodityId, request.Quantity, (int)(WarehouseHistoryMode.Exit), receiptItems.Id, request.Quantity);


            await _receiptCommandsService.GetPriceBuyItems(receiptItems.CommodityId, Convert.ToInt32(Fromlayouts.WarehouseId), receiptItems.Id, receiptItems.Quantity, receiptItems);
            //-------------- receipt.TotalItemPrice--------------------------------
            await _receiptCommandsService.CalculateTotalItemPrice(ToReceipt);

            _documentItemRepository.Update(receiptItems);
            await _documentItemRepository.SaveChangesAsync();

        }

        private async Task AddBom(AddDocumentItemsBomQuantityCommand request, DocumentItem receiptItems, WarehouseLayout Fromlayouts, int? MeasureId)
        {
            DocumentItemsBom BomItem = new DocumentItemsBom();


            BomItem.ParentCommodityId = receiptItems.CommodityId;
            BomItem.DocumentItemsId = request.DocumentItemsId;
            BomItem.CommodityId = request.CommodityId;
            BomItem.Quantity = request.Quantity;
            BomItem.MainMeasureId = Convert.ToInt32(MeasureId);
            BomItem.BomValueHeaderId = Convert.ToInt32(receiptItems.BomValueHeaderId);
            BomItem.WarehouseLayoutsId = Fromlayouts.Id;
            BomItem.Weight = 0;
            BomItem.UnitBasePrice = Convert.ToInt64(receiptItems.CurrencyBaseId);
            await _receiptCommandsService.GetPriceBuyBom(request.CommodityId, Convert.ToInt32(Fromlayouts.WarehouseId), BomItem.Quantity, BomItem);

            _documentItemsBomepository.Insert(BomItem);
            await _documentItemsBomepository.SaveChangesAsync();
        }

        //======================================================================
        //----------------------------------------------------------------------
        //-----------------افزایش و کاهش ظرفیت فعلی در مکان--------------------
        private async Task UpdateWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int historyMode, int receiptItems, double request_Quantity)
        {
            var WarehouseLayoutQuantity =await _context.WarehouseLayoutQuantities.Where(a => a.WarehouseLayoutId == WarehouseLayoutId && a.CommodityId == CommodityId).FirstOrDefaultAsync();
            var stock =await _context.WarehouseStocks.Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();

            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(CommodityId, request_Quantity, WarehouseLayoutId, receiptItems, historyMode);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, WarehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, historyMode, stock);
           
        }



    }
}