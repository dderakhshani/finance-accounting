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
    public class UpdateDocumentItemsBomQuantityCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<UpdateDocumentItemsBomQuantityCommand>, ICommand
    {
        public int Id { get; set; } = default!;
        public double Quantity { get; set; } = default!;
    }

    public class UpdateDocumentItemsBomQuantityCommandHandler : IRequestHandler<UpdateDocumentItemsBomQuantityCommand, ServiceResult<Domain.Receipt>>
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IRepository<WarehouseLayoutQuantity> _WarehouseLayoutQuantityRepository;
        private readonly IRepository<WarehouseHistory> _WarehouseHistoryRepository;
        private readonly IRepository<WarehouseStocks> _stockRepository;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<DocumentItemsBom> _documentItemsBiorepository;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;



        public UpdateDocumentItemsBomQuantityCommandHandler(
              
              IReceiptRepository receiptRepository
            , ICurrentUserAccessor currentUserAccessor
            , IInvertoryUnitOfWork context
            , IMapper mapper
            , IRepository<WarehouseLayoutQuantity> WarehouseLayoutQuantityRepository
            , IRepository<WarehouseHistory> WarehouseHistoryRepository
            , IRepository<WarehouseStocks> stockRepository
            , IRepository<DocumentItem> documentItemRepository
            , IRepository<DocumentItemsBom> documentItemsBiorepository
            , IReceiptCommandsService receiptCommandsService
            , IWarehouseLayoutCommandsService warehouseLayoutCommandsService
            , IProcedureCallService procedureCallService



            )

        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _receiptRepository = receiptRepository;
            _context = context;
            _WarehouseLayoutQuantityRepository = WarehouseLayoutQuantityRepository;
            _WarehouseHistoryRepository = WarehouseHistoryRepository;
            _stockRepository = stockRepository;
            _documentItemRepository = documentItemRepository;
            _documentItemsBiorepository = documentItemsBiorepository;
            _receiptCommandsService = receiptCommandsService;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;
            _procedureCallService = procedureCallService;
        }
        public async Task<ServiceResult<Domain.Receipt>> Handle(UpdateDocumentItemsBomQuantityCommand request, CancellationToken cancellationToken)
        {


            int historyMode = 0;

            var documentItemsBiome = await _documentItemsBiorepository.Find(request.Id);
            if (documentItemsBiome == null)
            {
                throw new ValidationError("کالا موجود نیست");
            }
            if (documentItemsBiome.Quantity != request.Quantity)
            {


                var receiptItems = await _documentItemRepository.GetAll().Where(a => a.Id == documentItemsBiome.DocumentItemsId).FirstOrDefaultAsync();
                if (receiptItems == null)
                {
                    throw new ValidationError("اقلام سند موجود نیست");
                }
                var receipt = await _receiptRepository.GetAll().Where(a => a.Id == receiptItems.DocumentHeadId).FirstOrDefaultAsync();
                if (receipt == null)
                {
                    throw new ValidationError("سند موجود نیست");
                }
                var layouts =await _context.WarehouseLayouts.Where(_a => _a.Id == documentItemsBiome.WarehouseLayoutsId).FirstOrDefaultAsync();
                if (layouts == null)
                {
                    throw new ValidationError("برای انبار تحویل دهنده انتخاب شده در این سند هیچ موقعیت بندی آخرین سطح تعریف نشده است");
                }


                double Quantity = request.Quantity;

                //-----------------------ویرایش موجودی انبار------------------
                Quantity = (request.Quantity - documentItemsBiome.Quantity);

                //اگر بزرگ تر از صفر باشد یعنی ورود جدید داریم و اگر کوچکتر از صفر باشد یعنی خروج داریم
                historyMode = request.Quantity <= documentItemsBiome.Quantity ? (int)(WarehouseHistoryMode.Enter) : (int)(WarehouseHistoryMode.Exit);
                await UpdateWarehouseLayout(Convert.ToInt32(layouts.WarehouseId), layouts.Id, documentItemsBiome.CommodityId, Math.Abs(Quantity), historyMode, receiptItems.Id, request.Quantity);

                //-----------------------------------------------------------
                documentItemsBiome.Quantity = request.Quantity;
                documentItemsBiome.ProductionCost = documentItemsBiome.UnitPrice * documentItemsBiome.Quantity;
                _documentItemsBiorepository.Update(documentItemsBiome);

                if (await _documentItemRepository.SaveChangesAsync() > 0)
                {

                    await _receiptCommandsService.GetPriceBuyItems(receiptItems.CommodityId, Convert.ToInt32(layouts.WarehouseId), receiptItems.Id, receiptItems.Quantity, receiptItems);
                    //-------------- receipt.TotalItemPrice--------------------------------
                   await _receiptCommandsService.CalculateTotalItemPrice(receipt);

                    _documentItemRepository.Update(receiptItems);
                    await _documentItemRepository.SaveChangesAsync();
                };



                return ServiceResult<Domain.Receipt>.Success(receipt);

            }
            return ServiceResult<Domain.Receipt>.Success(null);
        }



        //======================================================================
        //----------------------------------------------------------------------
        //-----------------افزایش و کاهش ظرفیت فعلی در مکان--------------------
        private async Task UpdateWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int historyMode, int receiptItems, double request_Quantity)
        {
            var WarehouseLayoutQuantity = await _context.WarehouseLayoutQuantities.Where(a => a.WarehouseLayoutId == WarehouseLayoutId && a.CommodityId == CommodityId).FirstOrDefaultAsync();
            var stock = await _stockRepository.GetAll().Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();

            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(CommodityId, request_Quantity, WarehouseLayoutId, receiptItems, historyMode);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, WarehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, historyMode, stock);
            
        }



    }
}