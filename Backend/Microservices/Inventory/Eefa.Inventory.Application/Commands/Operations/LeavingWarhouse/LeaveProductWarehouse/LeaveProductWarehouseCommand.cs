using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    ///دریافت کالا از انبار توسط پرسنل
    /// </summary>
    public class LeaveProductWarehouseCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<LeaveProductWarehouseCommand>, ICommand
    {
        public int CodeVoucherGroupId { get; set; } = default!;
        public int? DebitAccountReferenceId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; } = default!;
        public Nullable<bool> IsDocumentIssuance { get; set; } = default!;
        public DateTime DocumentDate { get; set; } = default!;


        public ICollection<LeavingDocumentItemProduct> ReceiptDocumentItems { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateProductCommand, Domain.Receipt>()
                .IgnoreAllNonExisting();
        }

    }
    public class LeavingDocumentItemProduct : IMapFrom<DocumentItem>
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

        public long? ProductionCost { get; set; } = default!;


        /// <description>
        /// تعداد
        ///</description>

        public double Quantity { get; set; } = default!;
        public int? DocumentMeasureId { get; set; }
        public int? MeasureUnitConversionId { get; set; }
        public string Description { get; set; }

        public string RequestNo { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReceiptDocumentItemCreate, DocumentItem>()
                .IgnoreAllNonExisting();
        }
    }
    public class LeaveProductWarehouseCommandHandler : IRequestHandler<LeaveProductWarehouseCommand, ServiceResult<Domain.Receipt>>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IProcedureCallService _procedureCallService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;
        public LeaveProductWarehouseCommandHandler(

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
        public async Task<ServiceResult<Domain.Receipt>> Handle(LeaveProductWarehouseCommand request, CancellationToken cancellationToken)
        {

            if (request.DocumentDate > DateTime.UtcNow)
            {
                throw new ValidationError("تاریخ انتخابی برای زمان آینده نمی تواند باشد");
            }
            if (request.ReceiptDocumentItems.Count() == 0)
            {
                throw new ValidationError("هیچ کالایی انتخاب نشده است");
            }
            var DocumentStatuesBaseValue = (int)DocumentStateEnam.Leave;
            CodeVoucherGroup NewCodeVoucherGroup = await _receiptCommandsService.GetNewCodeVoucherGroup(new Receipt() { DocumentStauseBaseValue = DocumentStatuesBaseValue, CodeVoucherGroupId = request.CodeVoucherGroupId });


            foreach (var item in request.ReceiptDocumentItems)
            {
                var receipt = new Domain.Receipt();
                receipt.DocumentStauseBaseValue = DocumentStatuesBaseValue;
                receipt.CodeVoucherGroupId = NewCodeVoucherGroup.Id;


                await Insert(request, item, receipt, cancellationToken);

            }
            return ServiceResult<Domain.Receipt>.Success(null);
        }

        private async Task Insert(LeaveProductWarehouseCommand request, LeavingDocumentItemProduct item, Domain.Receipt receipt, CancellationToken cancellationToken)
        {

            var currency = await _context.BaseValues.Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();


            var warehouse = await _context.Warehouses.Where(a => a.Id == item.WarehouseId).FirstOrDefaultAsync();

            CodeVoucherGroup CodeVoucher = await _context.CodeVoucherGroups.Where(a => a.Id == request.CodeVoucherGroupId).FirstOrDefaultAsync();

            if (CodeVoucher == null)
            {
                throw new ValidationError("کد گروه سند وجود ندارد");
            }


            receipt.WarehouseId = item.WarehouseId;

            await _receiptCommandsService.SerialFormula(receipt, CodeVoucher.Code, cancellationToken);

            int lastNo = await _receiptCommandsService.lastDocumentNo(receipt, cancellationToken);

            receipt.DocumentNo = lastNo + 1;

            _receiptCommandsService.ReceiptBaseDataInsert(request.DocumentDate, receipt);


            receipt.IsPlacementComplete = true;
            receipt.IsDocumentIssuance = request.IsDocumentIssuance;
            receipt.CommandDescription = $"Command:LeaveProductWarehouseCommand -codeVoucherGroup.id={request.CodeVoucherGroupId.ToString()}";
            receipt.ExpireDate = DateTime.Now.AddDays(30);
            receipt.InvoiceNo = item.RequestNo;


            DebitAndCredit(request, warehouse, CodeVoucher, receipt);

            await InsertDocumentItems(item, receipt, currency, CodeVoucher);


            _receiptRepository.Insert(receipt);
            await _receiptRepository.SaveChangesAsync();

            await _receiptCommandsService.CalculateTotalItemPrice(receipt);

            receipt.DocumentId = await _receiptCommandsService.InsertAndUpdateDocument(receipt);
            _receiptRepository.Update(receipt);
            await _receiptRepository.SaveChangesAsync();

            //----------------اولین محلی که پیدا شد--------------------------
            WarehouseLayout layouts = await _warehouseLayoutCommandsService.FindLayout(item.WarehouseId, item.CommodityId);

            var receiptItemsId = receipt.Items.Select(a => a.Id).FirstOrDefault();
            await AddWarehouseLayout(item.WarehouseId, layouts.Id, item.CommodityId, item.Quantity, (int)(WarehouseHistoryMode.Exit), receiptItemsId);


        }

        //رسید موقت زده شود
        private static void DebitAndCredit(LeaveProductWarehouseCommand request, Warehouse warehouse, CodeVoucherGroup CodeVoucher, Receipt receipt)
        {
            //-----------------انبار بدهکار---------------------------

            receipt.CreditAccountHeadId = warehouse.AccountHeadId;

            //------------------تامین کننده بستانکار--------------------
            receipt.DebitAccountReferenceId = request.DebitAccountReferenceId;
            receipt.DebitAccountReferenceGroupId = request.DebitAccountReferenceGroupId;
            receipt.DebitAccountHeadId = CodeVoucher.DefultDebitAccountHeadId;
        }

        private async Task InsertDocumentItems(LeavingDocumentItemProduct items, Domain.Receipt receipt, int currency, CodeVoucherGroup CodeVoucher)
        {


            var documentItem = new DocumentItem()
            {
                CurrencyBaseId = currency,
                CurrencyPrice = 1,
                DocumentMeasureId = items.MainMeasureId,
                MainMeasureId = items.MainMeasureId,
                Quantity = items.Quantity,
                Description = items.Description,
                SecondaryQuantity = 0,
                CommodityId = items.CommodityId,
                RemainQuantity = items.Quantity,
                CreatedById = _currentUserAccessor.GetId(),
                OwnerRoleId = _currentUserAccessor.GetRoleId(),
            };



            receipt.AddItem(documentItem);

            await _receiptCommandsService.GetPriceBuyItems(documentItem.CommodityId, receipt.WarehouseId, null, documentItem.Quantity, documentItem);

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
