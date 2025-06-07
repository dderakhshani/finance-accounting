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
using Eefa.Inventory.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Eefa.Inventory.Domain.Common.ConstantValues;

namespace Eefa.Inventory.Application
{
    public class AddWarehouseInventoryCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<AddWarehouseInventoryCommand>, ICommand
    {
        public int CommodityId { get; set; } = default!;
        public int Quantity { get; set; } = default!;
        public int WarehouseLayoutId { get; set; }
        public string LayoutTitle { get; set; }
        public int WarehouseId { get; set; }
        public int DebitAccountHeadId { get; set; } = default!;
        public int DebitAccountReferenceId { get; set; } = default!;
        public int DebitAccountReferenceGroupId { get; set; } = default!;
        public int CreditAccountHeadId { get; set; } = default!;
        public int CreditAccountReferenceId { get; set; } = default!;
        public int CreditAccountReferenceGroupId { get; set; } = default!;

    }

    public class AddWarehouseInventoryCommandHandler : IRequestHandler<AddWarehouseInventoryCommand, ServiceResult>
    {


        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;
        private readonly IRepository<WarehouseLayoutQuantity> _WarehouseLayoutQuantity;
        private readonly IRepository<WarehouseHistory> _WarehouseHistory;
        private readonly IWarehouseLayoutRepository _WarehouseLayout;

        private readonly IReceiptRepository _receiptRepository;
        private readonly IRepository<Document> _documentRepository;

        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly ICurrentUserAccessor _currentUserAccessor;


        public AddWarehouseInventoryCommandHandler(
            IMapper mapper,
            IInvertoryUnitOfWork context,
            IRepository<WarehouseLayoutQuantity> WarehouseLayoutQuantity,
            IWarehouseLayoutCommandsService warehouseLayoutCommandsService,
            IRepository<WarehouseHistory> warehouseHistory,
            IWarehouseLayoutRepository WarehouseLayout

            )
        {
            _mapper = mapper;
            _context = context;
            _WarehouseLayoutQuantity = WarehouseLayoutQuantity;
            _WarehouseHistory = warehouseHistory;
            _WarehouseLayout = WarehouseLayout;

            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;
        }

        public async Task<ServiceResult> Handle(AddWarehouseInventoryCommand request, CancellationToken cancellationToken)
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
            var receipt = new Receipt();
            receipt.WarehouseId = Convert.ToInt32(request.WarehouseId);
            receipt.ExpireDate = ConstBaseValue.DocumnetDateUtc;
            _receiptCommandsService.ReceiptBaseDataInsert(ConstBaseValue.DocumnetDateUtc, receipt);
            receipt.RequestNo = "";
            receipt.CodeVoucherGroupId = codeVoucherGroup.Id;

            receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.invoiceAmount;

            receipt.CommandDescription = $"Command:AddWarehouseInventoryCommand -اصلاح موجودی انبار-codeVoucherGroup.id={receipt.CodeVoucherGroupId.ToString()}";

            await _receiptCommandsService.SerialFormula(receipt, codeVoucherGroup.Code, new CancellationToken());

            
            int lastNo = await _receiptCommandsService.lastDocumentNo(receipt, new CancellationToken());


            receipt.DocumentNo = lastNo + 1;

            _receiptCommandsService.GenerateInvoiceNumber(((int)DocumentStateEnam.InventoryModification).ToString(), receipt, codeVoucherGroup);

            _receiptRepository.Insert(receipt);

            await _receiptRepository.SaveChangesAsync();

            DocumentItem documentItem = await InsertDocumentItems(request, currency, receipt);

            receipt.DocumentId = await _receiptCommandsService.InsertAndUpdateDocument(receipt);
            _receiptRepository.Update(receipt);
            
            await _receiptRepository.SaveChangesAsync();

            await UpdateWarehouseLayout(request, documentItem);

            return ServiceResult.Success();
        }

        private async Task<DocumentItem> InsertDocumentItems(AddWarehouseInventoryCommand request, int currency, Receipt receipt)
        {
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

            _documentItemRepository.Update(documentItem);

            await _receiptCommandsService.CalculateTotalItemPrice(receipt);

            await _documentItemRepository.SaveChangesAsync();
            
            return documentItem;
        }

        private async Task UpdateWarehouseLayout(AddWarehouseInventoryCommand request, DocumentItem documentItem)
        {
            WarehouseLayout warehouseLayout = new WarehouseLayout()
            {
                ParentId = request.WarehouseLayoutId,
                Title = request.LayoutTitle,
                Capacity = 20000,
                LastLevel = true,
                IsDefault = false,
                Status = Domain.Enum.WarehouseLayoutStatus.Free,
                EntryMode = 0,
                WarehouseId = request.WarehouseId,
            };

            _WarehouseLayout.Insert(warehouseLayout);

            await _WarehouseLayout.SaveChangesAsync();

            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(request.CommodityId, request.Quantity, warehouseLayout.Id, documentItem.Id, (int)(WarehouseHistoryMode.Enter));
            await _warehouseLayoutCommandsService.InsertStock(request.WarehouseId, request.CommodityId, request.Quantity, (int)(WarehouseHistoryMode.Enter), null);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(request.CommodityId, request.Quantity, (int)(WarehouseHistoryMode.Enter), null, warehouseLayout.Id);

        }
    }
}