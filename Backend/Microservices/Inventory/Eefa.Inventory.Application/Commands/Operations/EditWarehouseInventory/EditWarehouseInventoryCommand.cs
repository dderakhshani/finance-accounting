using System;
using System.Collections.Generic;
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
using static Eefa.Inventory.Domain.Common.ConstantValues;

namespace Eefa.Inventory.Application
{
    public class EditWarehousInventoryCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<EditWarehousInventoryCommand>, ICommand
    {
        public int CommodityId { get; set; } = default!;
        public int Quantity { get; set; } = default!;
        public int warehouseLayoutsCommodityQuantitId { get; set; }
        public int WarehouseLayoutId { get; set; }
        public int WarehouseId { get; set; }
        public int Mode { get; set; }
        public int DebitAccountHeadId { get; set; } = default!;
        public int DebitAccountReferenceId { get; set; } = default!;
        public int DebitAccountReferenceGroupId { get; set; } = default!;
        public int CreditAccountHeadId { get; set; } = default!;
        public int CreditAccountReferenceId { get; set; } = default!;
        public int CreditAccountReferenceGroupId { get; set; } = default!;


    }

    public class EditWarehouseInventoryCommandHandler : IRequestHandler<EditWarehousInventoryCommand, ServiceResult>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;

        private readonly IReceiptRepository _receiptRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly IWarehouseLayoutRepository _WarehouseLayout;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;
        public EditWarehouseInventoryCommandHandler(

            IMapper mapper,
            IInvertoryUnitOfWork context,
            IWarehouseLayoutRepository WarehouseLayout,
            IReceiptRepository receiptRepository,
            IRepository<Document> documentRepository,
            IReceiptCommandsService receiptCommandsService,
            IWarehouseLayoutCommandsService warehouseLayoutCommandsService,
            IRepository<DocumentItem> documentItemRepository,
            ICurrentUserAccessor currentUserAccessor
            )
        {
            _mapper = mapper;
            _context = context;
            _WarehouseLayout = WarehouseLayout;
            _documentRepository = documentRepository;
            _receiptRepository = receiptRepository;
            _receiptCommandsService = receiptCommandsService;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;
            _documentItemRepository = documentItemRepository;
            _currentUserAccessor = currentUserAccessor;


        }

        public async Task<ServiceResult> Handle(EditWarehousInventoryCommand request, CancellationToken cancellationToken)
        {

            var currency = await _context.BaseValues.Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();

            if (currency == null)
            {
                throw new ValidationError("واحد ارز ریالی تعریف نشده است");
            }
            var warehouse = await _context.Warehouses.Where(a => a.Id == request.WarehouseId).FirstOrDefaultAsync();
            if (warehouse == null)
            {
                throw new ValidationError("کد انبار اشتباه است");
            }
            var codeVoucherGroup = await _context.CodeVoucherGroups.Where(a => a.UniqueName == ConstantValues.CodeVoucherGroupValues.InventoryModification).FirstOrDefaultAsync();

            if (codeVoucherGroup == null)
            {
                throw new ValidationError("کد گروه سند وجود ندارد");
            }
            Receipt receipt = await InsertDocumentHead(request, codeVoucherGroup);
            await _receiptRepository.SaveChangesAsync();

            DocumentItem documentItem = await InsertDocumentsItem(request, currency, receipt);

            await _receiptCommandsService.CalculateTotalItemPrice(receipt);

            await UpdateWarehouseLayout(request.WarehouseId, request.WarehouseLayoutId, request.CommodityId, request.Quantity, request.Mode, request.warehouseLayoutsCommodityQuantitId, documentItem.Id);


            return ServiceResult.Success();

        }

        private async Task<Receipt> InsertDocumentHead(EditWarehousInventoryCommand request, CodeVoucherGroup codeVoucherGroup)
        {
            var receipt = new Receipt();
            receipt.WarehouseId = Convert.ToInt32(request.WarehouseId);
            receipt.ExpireDate = ConstBaseValue.DocumnetDateUtc;
            _receiptCommandsService.ReceiptBaseDataInsert(ConstBaseValue.DocumnetDateUtc, receipt);
            receipt.RequestNo = "";
            receipt.CodeVoucherGroupId = codeVoucherGroup.Id;

            receipt.DocumentStauseBaseValue = request.Mode == -1 ? (int)DocumentStateEnam.invoiceAmountLeave : (int)DocumentStateEnam.invoiceAmount;

            receipt.CommandDescription = $"Command:EditWarehouseInventory -اصلاح موجودی انبار-codeVoucherGroup.id={receipt.CodeVoucherGroupId.ToString()}";

            await _receiptCommandsService.SerialFormula(receipt, codeVoucherGroup.Code, new CancellationToken());

            int lastNo = await _receiptCommandsService.lastDocumentNo(receipt, new CancellationToken());


            receipt.DocumentNo = lastNo + 1;

            _receiptCommandsService.GenerateInvoiceNumber(((int)DocumentStateEnam.InventoryModification).ToString(), receipt, codeVoucherGroup);

            _receiptRepository.Insert(receipt);
            return receipt;
        }

        private async Task<DocumentItem> InsertDocumentsItem(EditWarehousInventoryCommand request, int currency, Receipt receipt)
        {
            var documentItems = new List<DocumentItem>();
            var commodity = await _context.Commodities.Where(a => a.Id == request.CommodityId).FirstOrDefaultAsync();

            var documentItem = new DocumentItem()
            {
                CurrencyBaseId = currency,
                CurrencyPrice = 1,
                DocumentHeadId = receipt.Id,
                DocumentMeasureId = Convert.ToInt32(commodity.MeasureId),
                MainMeasureId = Convert.ToInt32(commodity.MeasureId),
                CommodityId = commodity.Id,
                Quantity = request.Quantity,
                UnitBasePrice = default,
                ProductionCost = default,
                Discount = default,
                Weight = default,

            };


            await _receiptCommandsService.GetPriceBuyItems(documentItem.CommodityId, request.WarehouseId, documentItem.BomValueHeaderId > 0 ? documentItem.Id : null, documentItem.Quantity, documentItem);
            documentItem.YearId = _currentUserAccessor.GetYearId();

            _documentItemRepository.Insert(documentItem);
            await _documentItemRepository.SaveChangesAsync();
            return documentItem;
        }

        //-----------------افزایش و کاهش ظرفیت فعلی در مکان--------------------
        private async Task UpdateWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int hitoryMode, int WarehouseLayoutQuantityId, int documentItemsId)
        {
            var WarehouseLayoutQuantity = await _context.WarehouseLayoutQuantities.Where(a => a.Id == WarehouseLayoutQuantityId).FirstOrDefaultAsync();

            var stock = await _context.WarehouseStocks.Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();
            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(CommodityId, Quantity, WarehouseLayoutId, documentItemsId, hitoryMode);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, hitoryMode, WarehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, hitoryMode, stock);

        }



    }
}