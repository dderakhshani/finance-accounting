using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class CreateProductCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<CreateProductCommand>, ICommand
    {

        public int CodeVoucherGroupId { get; set; } = default!;
        public int? CreditAccountReferenceId { get; set; } = default!;
        public int? CreditAccountReferenceGroupId { get; set; } = default!;
        public DateTime DocumentDate { get; set; } = default!;
        public Nullable<bool> IsDocumentIssuance { get; set; } = default!;
        public ICollection<ReceiptDocumentItemProductCreate> ReceiptDocumentItems { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateProductCommand, Domain.Receipt>()
                .IgnoreAllNonExisting();
        }



    }
    public class ReceiptDocumentItemProductCreate : IMapFrom<DocumentItem>
    {
        public int CommodityId { get; set; } = default!;

        public int WarehouseId { get; set; } = default!;
        public int MainMeasureId { get; set; }

        public long UnitPrice { get; set; } = default!;
        /// <description>
        /// قیمت در سیستم  درخواست 
        ///</description>

        public long? UnitBasePrice { get; set; } = default!;

        /// <description>
        /// قیمت پایه
        ///</description>

        public long ProductionCost { get; set; } = default!;
        /// <description>
        /// تعداد
        ///</description>

        public double Quantity { get; set; } = default!;
        public int DocumentMeasureId { get; set; }
        public int? MeasureUnitConversionId { get; set; }
        public string Description { get; set; }
        public string RequestNo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReceiptDocumentItemCreate, DocumentItem>()
                .IgnoreAllNonExisting();
        }
    }
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ServiceResult<Domain.Receipt>>
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IProcedureCallService _procedureCallService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;


        public CreateProductCommandHandler(
             IMapper mapper
            , IInvertoryUnitOfWork context
            , IReceiptRepository receiptRepository
            , ICurrentUserAccessor currentUserAccessor
            , IProcedureCallService procedureCallService
            , IReceiptCommandsService receiptCommandsService
            , IWarehouseLayoutCommandsService warehouseLayoutCommandsService

            )
        {
            _mapper = mapper;
            _context = context;
            _receiptRepository = receiptRepository;
            _currentUserAccessor = currentUserAccessor;
            _procedureCallService = procedureCallService;
            _receiptCommandsService = receiptCommandsService;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;


        }


        public async Task<ServiceResult<Domain.Receipt>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (request.DocumentDate > DateTime.UtcNow)
            {
                throw new ValidationError("تاریخ انتخابی برای زمان آینده نمی تواند باشد");
            }
            if (request.ReceiptDocumentItems.Count() == 0)
            {
                throw new ValidationError("هیچ کالایی انتخاب نشده است");
            }
            if (request.ReceiptDocumentItems.Where(a => a.WarehouseId == 0).Any())
            {
                throw new ValidationError("انبار کالایی انتخاب نشده است");
            }
            var DocumentStatuesBaseValue = (int)DocumentStateEnam.Direct;
            CodeVoucherGroup NewCodeVoucherGroup = await _receiptCommandsService.GetNewCodeVoucherGroup(new Receipt() { DocumentStauseBaseValue = DocumentStatuesBaseValue, CodeVoucherGroupId = request.CodeVoucherGroupId });

            var receiptInsertCount = await _context.ReceiptView.Where(a => a.DocumentDate == request.DocumentDate && a.CodeVoucherGroupId == NewCodeVoucherGroup.Id && a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveReceipt).CountAsync();

            if (receiptInsertCount >= request.ReceiptDocumentItems.Count())
            {
                throw new ValidationError("ورودی های این تاریخ قبلا زده شده است");
            }



            foreach (var item in request.ReceiptDocumentItems)
            {
                var receipt = _mapper.Map<Domain.Receipt>(request);
                receipt.DocumentStauseBaseValue = DocumentStatuesBaseValue;
                receipt.CodeVoucherGroupId = NewCodeVoucherGroup.Id;
                var documentItem = await _context.ReceiptItemsView.Where(a => a.CommodityId == item.CommodityId
                                                                                                        && (a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveReceipt)
                                                                                                        && a.CodeVoucherGroupId == NewCodeVoucherGroup.Id
                                                                                                        && a.DocumentDate == request.DocumentDate
                                                                                                        && a.WarehouseId == item.WarehouseId
                                                                                                        ).FirstOrDefaultAsync();

                if (documentItem == null)
                {
                    await Insert(request, item, receipt, cancellationToken);
                }

            }
            return ServiceResult<Domain.Receipt>.Success(null);
        }

        private async Task Insert(CreateProductCommand request, ReceiptDocumentItemProductCreate item, Domain.Receipt receipt, CancellationToken cancellationToken)
        {

            var currency = await _context.BaseValues.Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();


            var warehouse = await _context.Warehouses.Where(a => a.Id == item.WarehouseId).FirstOrDefaultAsync();

            CodeVoucherGroup CodeVoucher = await _context.CodeVoucherGroups.Where(a => a.Id == request.CodeVoucherGroupId).FirstOrDefaultAsync();

            if (CodeVoucher == null)
            {
                throw new ValidationError("کد گروه سند وجود ندارد");
            }


            receipt.WarehouseId = item.WarehouseId;
            receipt.IsDocumentIssuance = request.IsDocumentIssuance;
            await _receiptCommandsService.SerialFormula(receipt, CodeVoucher.Code, cancellationToken);


            int lastNo = await _receiptCommandsService.lastDocumentNo(receipt, cancellationToken);

            receipt.DocumentNo = lastNo + 1;
            _receiptCommandsService.ReceiptBaseDataInsert(request.DocumentDate, receipt);

            receipt.InvoiceNo = item.RequestNo;
            receipt.CommandDescription = "Command:CreateProductCommand -codeVoucherGroup.id=" + request.CodeVoucherGroupId.ToString();
            receipt.ExpireDate = DateTime.Now.AddDays(30);
            receipt.IsPlacementComplete = true;
            DebitAndCredit(request, warehouse, CodeVoucher, receipt);


            InsertDocumentItems(item, receipt, currency, CodeVoucher);


            _receiptRepository.Insert(receipt);
            await _receiptRepository.SaveChangesAsync();
            await _receiptCommandsService.CalculateTotalItemPrice(receipt);

            receipt.DocumentId = await _receiptCommandsService.InsertAndUpdateDocument(receipt);
            _receiptRepository.Update(receipt);
            await _receiptRepository.SaveChangesAsync();

            //----------------اولین محلی که پیدا شد--------------------------
            WarehouseLayout layouts = await _warehouseLayoutCommandsService.FindLayout(item.WarehouseId, item.CommodityId);

            var receiptItemsId = receipt.Items.Select(a => a.Id).FirstOrDefault();
            await AddWarehouseLayout(item.WarehouseId, layouts.Id, item.CommodityId, item.Quantity, (int)(WarehouseHistoryMode.Enter), receiptItemsId);


        }

        //رسید موقت زده شود
        private static void DebitAndCredit(CreateProductCommand request, Warehouse warehouse, CodeVoucherGroup CodeVoucher, Receipt receipt)
        {
            //-----------------انبار بدهکار---------------------------

            receipt.DebitAccountHeadId = warehouse.AccountHeadId;

            //------------------تامین کننده بستانکار--------------------
            receipt.CreditAccountReferenceId = request.CreditAccountReferenceId;
            receipt.CreditAccountReferenceGroupId = request.CreditAccountReferenceGroupId;
            receipt.CreditAccountHeadId = CodeVoucher.DefultCreditAccountHeadId;
        }

        private void InsertDocumentItems(ReceiptDocumentItemProductCreate items, Domain.Receipt receipt, int currency, CodeVoucherGroup CodeVoucher)
        {
            var documentItem = new DocumentItem();
            documentItem.CurrencyBaseId = currency;
            documentItem.CurrencyPrice = 1;
            documentItem.DocumentMeasureId = items.DocumentMeasureId;
            documentItem.MainMeasureId = items.MainMeasureId;
            documentItem.Quantity = items.Quantity;
            documentItem.Description = items.Description;
            documentItem.SecondaryQuantity = 0;
            documentItem.CommodityId = items.CommodityId;
            documentItem.RemainQuantity = items.Quantity;
            documentItem.UnitBasePrice = items.UnitPrice;
            documentItem.ProductionCost = Convert.ToInt64(items.UnitPrice * items.Quantity);
            documentItem.CreatedById = _currentUserAccessor.GetId();
            documentItem.OwnerRoleId = _currentUserAccessor.GetRoleId();

            receipt.AddItem(documentItem);


        }
        private async Task AddWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int historyMode, int receiptItemsId)
        {

            var WarehouseLayoutQuantity = await _context.WarehouseLayoutQuantities.Where(a => a.WarehouseLayoutId == WarehouseLayoutId && a.CommodityId == CommodityId).FirstOrDefaultAsync();
            var stock = await _context.WarehouseStocks.Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();

            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(CommodityId, Quantity, WarehouseLayoutId, receiptItemsId, historyMode);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, WarehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, historyMode, stock);

        }

    }
}