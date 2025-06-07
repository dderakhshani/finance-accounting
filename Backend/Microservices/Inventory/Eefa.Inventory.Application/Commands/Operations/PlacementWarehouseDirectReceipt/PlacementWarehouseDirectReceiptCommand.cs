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
    public class PlacementWarehouseDirectReceiptCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<PlacementWarehouseDirectReceiptCommand>, ICommand
    {
        public int Id { get; set; }
    }

    public class PlacementWarehouseDirectReceiptCommandHandler : IRequestHandler<PlacementWarehouseDirectReceiptCommand, ServiceResult>
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<CodeVoucherGroup> _codeVoucherGroupRepository;
        private readonly IMapper _mapper;
        private readonly IReceiptQueries _receiptQueries;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;
        public PlacementWarehouseDirectReceiptCommandHandler(
            IMapper mapper,
            IInvertoryUnitOfWork context,
            IReceiptQueries receiptQueries,
            IReceiptRepository receiptRepository,
            IRepository<DocumentItem> documentItemRepository,
            IRepository<CodeVoucherGroup> codeVoucherGroupRepository,
            IWarehouseLayoutCommandsService warehouseLayoutCommandsService
            )
        {
            _mapper = mapper;
            _context = context;
            _receiptQueries = receiptQueries;
            _receiptRepository = receiptRepository;
            _documentItemRepository = documentItemRepository;
            _codeVoucherGroupRepository = codeVoucherGroupRepository;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;
        }

        public async Task<ServiceResult> Handle(PlacementWarehouseDirectReceiptCommand request, CancellationToken cancellationToken)
        {

            //--------------درصورتی که در انبار قرنطینه یا از محل دیگری به این محل جدید آمده باشد--------------------------
            //-------------------یافتن محل گذشته در انبار-------------------------------------------------------------------
            var receipt = await _receiptRepository.GetAll().Where(a => a.Id == request.Id).FirstOrDefaultAsync();
            if (receipt == null)
            {
                throw new ValidationError("هیچ سندی برای این کالا در انبار پیدا نشد");
            }
            var DocumentHeadItems = await _documentItemRepository.GetAll().Where(a => a.DocumentHeadId == request.Id).ToListAsync();
            if (DocumentHeadItems == null)
            {
                throw new ValidationError("اقلام هیچ سندی برای این کالا در انبار پیدا نشد");
            }

            var warehouseId = receipt.WarehouseId;
            foreach (var documentItem in DocumentHeadItems)
            {
                //----------------اولین محلی که پیدا شد--------------------------
                WarehouseLayout layouts = await _warehouseLayoutCommandsService.FindLayout(warehouseId, documentItem.CommodityId);

                await AddWarehouseLayout(warehouseId, layouts.Id, documentItem.CommodityId, documentItem.Quantity, (int)(WarehouseHistoryMode.Enter), documentItem.Id);
            }


            return ServiceResult.Success();

        }

        private async Task AddWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int historyMode, int receiptItemsId)
        {

            var WarehouseLayoutQuantity =await _context.WarehouseLayoutQuantities.Where(a => a.WarehouseLayoutId == WarehouseLayoutId && a.CommodityId == CommodityId).FirstOrDefaultAsync();
            var stock =await _context.WarehouseStocks.Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();

            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(CommodityId, Quantity, WarehouseLayoutId, receiptItemsId, historyMode);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, WarehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, historyMode, stock);
           
        }
      

        
    }
}