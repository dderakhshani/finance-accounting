using System;
using System.Linq;
using System.Linq.Dynamic.Core;
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
    public class UpdateStatusDirectReceiptCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
    }

    public class UpdateStatusDirectReceiptCommandHandler : IRequestHandler<UpdateStatusDirectReceiptCommand, ServiceResult>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IWarehouseLayoutRepository _warehouseLayout;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<WarehouseHistory> _warehouseHistory;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<CodeVoucherGroup> _codeVoucherGroupRepository;
        private readonly IRepository<WarehouseLayoutQuantity> _warehouseLayoutQuantity;
        public UpdateStatusDirectReceiptCommandHandler(
            IMapper mapper,
            IInvertoryUnitOfWork context,
            IReceiptRepository receiptRepository,
            IWarehouseLayoutRepository warehouseLayout,
            IProcedureCallService procedureCallService,
            IRepository<WarehouseHistory> warehouseHistory,
            IReceiptCommandsService receiptCommandsService,
            IRepository<DocumentItem> documentItemRepository,
            IRepository<CodeVoucherGroup> codeVoucherGroupRepository,
            IRepository<WarehouseLayoutQuantity> warehouseLayoutQuantity
            )
        {
            _mapper = mapper;
            _context = context;
            _warehouseHistory = warehouseHistory;
            _warehouseLayout = warehouseLayout;
            _receiptRepository = receiptRepository;
            _procedureCallService = procedureCallService;
            _documentItemRepository = documentItemRepository;
            _warehouseLayoutQuantity = warehouseLayoutQuantity;
            _receiptCommandsService = receiptCommandsService;
            _codeVoucherGroupRepository = codeVoucherGroupRepository;
        }

        public async Task<ServiceResult> Handle(UpdateStatusDirectReceiptCommand request, CancellationToken cancellationToken)
        {
            Domain.Receipt receipt = await _receiptRepository.Find(request.Id);
            receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Direct;
            CodeVoucherGroup NewCodeVoucherGroup = await _receiptCommandsService.GetNewCodeVoucherGroup(receipt);

            if (NewCodeVoucherGroup != null)
            {
                //در رسیدهایی که مورد تایید کارشناس نیستند ، نیاز به ورود اطلاعات مربوط به شماره سریال ها نیست
                if (NewCodeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.EstateDirectReceipt && request.StatusId != ConstantValues.ConstBaseValue.NotConfirm)
                {
                    var Estate = _context.Assets.Where(a => a.DocumentHeadId == request.Id && !a.IsDeleted).Count();
                    
                    if (Estate ==0)
                    {
                        throw new ValidationError("شماره شناسه اموال وارد نشده است");
                    }
                }
                //اگر به مرجوعی یا برگشتی به انبار است مستقیما ریالی شوند  
                if(NewCodeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.ReturnDirectReceipt)
                {
                    receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.invoiceAmount;
                    receipt.DocumentId = await _receiptCommandsService.InsertAndUpdateDocument(receipt);
                }
            }


            //درخواست خرید مربوطه پیدا شود و سپس تعداد باقی مانده آپدیت شود.

            await UpdateByRequestBuy(request, receipt);
            //درخواست خرید مربوطه پیدا شود و سپس تعداد باقی مانده آپدیت شود.

            await UpdateByInvoice(request, receipt);


            receipt.IsPlacementComplete = true;

            receipt.CodeVoucherGroupId = NewCodeVoucherGroup.Id;
            receipt.DocumentStateBaseId = request.StatusId;



            var entity = _receiptRepository.Update(receipt);

            if (await _receiptRepository.SaveChangesAsync() > 0)
            {

                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }


        private async Task UpdateByRequestBuy(UpdateStatusDirectReceiptCommand request, Domain.Receipt receipt)
        {

            var documentBuyItems = await _documentItemRepository.GetAll().Where(a => a.DocumentHeadId == receipt.Id).ToListAsync();

            foreach (var item in documentBuyItems)
            {
                await _procedureCallService.CalculateRemainQuantityRequest(receipt.RequestNo, item.CommodityId);

            }
        }
        private async Task UpdateByInvoice(UpdateStatusDirectReceiptCommand request, Domain.Receipt receipt)
        {
           
            var Invoice = await _context.ReceiptView.Where(a => a.DocumentNo.ToString() == receipt.InvoiceNo).FirstOrDefaultAsync();
            if (Invoice != null)
            {
                var documentBuyItems = await _documentItemRepository.GetAll().Where(a => a.DocumentHeadId == Invoice.Id).ToListAsync();

                var VatPercentage = await _context.BaseValues.Where(a => a.UniqueName.ToLower() == ConstantValues.ConstBaseValue.vatDutiesTax.ToLower()).Select(a => a.Value).FirstOrDefaultAsync();
                if (VatPercentage == null)
                {
                    throw new ValidationError("درصد محاسبه مالیات ارزش افزوده وجود ندارد");
                }
                foreach (var InvoiceItem in documentBuyItems)
                {
                    //کالاهای موجود در رسید مستقیم
                    var Item = await _documentItemRepository.GetAll().Where(a => a.DocumentHeadId == request.Id && a.CommodityId == InvoiceItem.CommodityId).FirstOrDefaultAsync();
                    if (Item != null)
                    {
                        //مبلغ را از روی قرارداد یا پیش فاکتور خودش بخواند
                        Item.UnitPrice = InvoiceItem.UnitPrice;
                        Item.UnitBasePrice = Convert.ToInt64(InvoiceItem.UnitPrice);
                        Item.ProductionCost = Convert.ToDouble(Item.Quantity * InvoiceItem.UnitPrice);
                        receipt.TotalItemPrice = Convert.ToDouble(receipt.TotalItemPrice + Item.ProductionCost);
                        _documentItemRepository.Update(InvoiceItem);

                    }

                }
                if (Invoice.VatDutiesTax > 0)
                {
                    receipt.VatDutiesTax = (Convert.ToInt64(receipt.TotalProductionCost) * Convert.ToInt32(VatPercentage)) / 100;
                }
                receipt.TotalProductionCost = receipt.TotalItemPrice + receipt.VatDutiesTax;
            }


        }

    }
}

