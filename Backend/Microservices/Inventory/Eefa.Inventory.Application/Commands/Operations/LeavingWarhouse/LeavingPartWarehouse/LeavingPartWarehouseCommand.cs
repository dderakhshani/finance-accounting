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
using Eefa.Invertory.Infrastructure.Context;
using Eefa.Invertory.Infrastructure.Services.Arani;
using Eefa.NotificationServices.Common.Enum;
using Eefa.NotificationServices.Dto;
using Eefa.NotificationServices.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;

namespace Eefa.Inventory.Application
{
    // : CommandBase, IRequest<ServiceResult<ReceiptQueryModel>>, IMapFrom<LeavingCommodityWarehouseCommand>, ICommand



    public class LeavingPartWarehouseCommand : CommandBase,  IRequest<ServiceResult<Receipt>>, IMapFrom<LeavingPartWarehouseCommand>, ICommand
    {

        public int request_No { get; set; } = default!;
        public int DocumentNo { get; set; } = default!;
        public int debitAccountReferenceId { get; set; } = default!;
        public int debitAccountReferenceGroupId { get; set; } = default!;
        public int debitAccountHeadId { get; set; } = default!;
        public int WarehouseId { get; set; } = default!;

        public Nullable<bool> IsDocumentIssuance { get; set; } = default!;
        public LeavingPartWarehouseDocumentItem[] warehouseDocumentItem { get; set; }

    }
    public class LeavingPartWarehouseDocumentItem
    {
        public int CommodityId { get; set; } = default!;
        public double Quantity { get; set; } = default!;
        public int WarehouseLayoutQuantitiyId { get; set; } = default!;
        public double QuantityTotal { get; set; } = default!;
        public double QuantityLayout { get; set; } = default!;
        public int RequestItemId { get; set; } = default!;
        public string Description { get; set; } = default!;


    }

    public class LeavingPartWarehouseCommandHandler : IRequestHandler<LeavingPartWarehouseCommand, ServiceResult<Receipt>>
    {


        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IAraniService _araniService;
        private readonly IRepository<WarehouseStocks> _stockRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly INotificationClient _notificationClient;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<BaseValue> _baseValueRepository;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<WarehouseHistory> _WarehouseHistoryRepository;
        private readonly IWarehouseLayoutRepository _WarehouseLayoutRepository;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;
        private readonly IRepository<WarehouseRequestExit> _warehouseRequestExitRepository;
        private readonly IRepository<WarehouseLayoutQuantity> _WarehouseLayoutQuantityRepository;
        private readonly IMediator _mediator;
        public LeavingPartWarehouseCommandHandler(
            IMapper mapper,
            IInvertoryUnitOfWork context,
            IAraniService araniService,
            IRepository<WarehouseStocks> stockRepository,
            IReceiptRepository receiptReceipt,
            INotificationClient notificationClient,
            ICurrentUserAccessor currentUserAccessor,
            IRepository<BaseValue> baseValueRepository,
            IRepository<Document> documentRepository,
            IWarehouseLayoutRepository WarehouseLayout,
            IRepository<WarehouseHistory> warehouseHistory,
            IReceiptCommandsService receiptCommandsService,
            IRepository<DocumentItem> documentItemRepository,
            IWarehouseLayoutCommandsService warehouseLayoutCommandsService,
            IRepository<WarehouseRequestExit> warehouseRequestExitRepository,
            IRepository<WarehouseLayoutQuantity> WarehouseLayoutQuantity,
            IMediator mediator

            )
        {
            _mapper = mapper;
            _context = context;
            _araniService = araniService;
            _stockRepository = stockRepository;
            _receiptRepository = receiptReceipt;
            _notificationClient = notificationClient;
            _documentRepository = documentRepository;
            _currentUserAccessor = currentUserAccessor;
            _baseValueRepository = baseValueRepository;
            _WarehouseHistoryRepository = warehouseHistory;
            _receiptCommandsService = receiptCommandsService;
            _WarehouseLayoutRepository = WarehouseLayout;
            _documentItemRepository = documentItemRepository;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;
            _warehouseRequestExitRepository = warehouseRequestExitRepository;
            _WarehouseLayoutQuantityRepository = WarehouseLayoutQuantity;
            _mediator = mediator;
        }

        public async Task<ServiceResult<Receipt>> Handle(LeavingPartWarehouseCommand request, CancellationToken cancellationToken)
        {

            foreach (var item in request.warehouseDocumentItem)
            {

                if (item.Quantity > item.QuantityTotal)
                {
                    throw new ValidationError($"تعداد کالا درخواست خروج بیشتر از تعداد کالا درخواست شده است، کد کالا :{item.CommodityId}");
                }
                if (item.QuantityTotal > item.QuantityLayout)
                {
                    throw new ValidationError($"تعداد کالا خروجی   بیشتر از تعداد کالا موجود در انبار است، کد کالا :{item.CommodityId}");
                }
            }
            var invalidItems = request.warehouseDocumentItem
            .GroupBy(item => item.CommodityId) // گروه‌بندی بر اساس CommodityId
            .Where(group => group.Sum(item => item.QuantityTotal) > group.Sum(item => item.QuantityLayout)) // بررسی شرط
            .SelectMany(group => group) // بازگرداندن آیتم‌های نامعتبر
            .ToList();

            // نمایش نتیجه
            if (invalidItems.Any())
            {
                throw new ValidationError("تعداد کالا خروجی   بیشتر از تعداد کالا موجود در انبار است ");
            }

            //-اطلاعات سند خروج از انبار-------------------------------------
            var codeVoucherGroup = await _context.CodeVoucherGroups.Where(t => t.UniqueName == ConstantValues.CodeVoucherGroupValues.RemoveCommodityWarhouse || t.UniqueName == ConstantValues.CodeVoucherGroupValues.RemoveConsumptionWarhouse).FirstOrDefaultAsync();
            if (codeVoucherGroup == null)
            {
                throw new ValidationError("برای این درخواست کد حسابی وجود ندارد");
            }
            var receipt = await _context.DocumentHeads.Where(a => a.RequestNo == request.request_No.ToString() && a.DocumentNo == request.DocumentNo && a.WarehouseId == request.WarehouseId).FirstOrDefaultAsync();

            try
            {
                var warehouse = await _context.Warehouses.Where(a => a.Id == request.WarehouseId).FirstOrDefaultAsync();

                receipt = await _receiptRepository.Find(receipt.Id);
                //------------------------انبار بستانکار-------------------------------
                receipt.IsDocumentIssuance = request.IsDocumentIssuance;
                receipt.CreditAccountHeadId = warehouse.AccountHeadId;

                receipt.DebitAccountReferenceId = request.debitAccountReferenceId;
                receipt.DebitAccountReferenceGroupId = request.debitAccountReferenceGroupId;
                receipt.DebitAccountHeadId = receipt.DebitAccountHeadId > 0 ? request.debitAccountHeadId : codeVoucherGroup.DefultDebitAccountHeadId;

                if (!await _receiptCommandsService.IsValidAccountHeadRelationByReferenceGroup(receipt.DebitAccountHeadId, receipt.DebitAccountReferenceGroupId))
                {
                    throw new ValidationError("عدم تطابق گروه حساب بستانکار و سرفصل حساب بستانکار");
                }
                if (!await _receiptCommandsService.IsValidAccountHeadRelationByReferenceGroup(receipt.CreditAccountHeadId, receipt.CreditAccountReferenceGroupId))
                {
                    throw new ValidationError("عدم تطابق گروه حساب بدهکار و سرفصل حساب بدهکار");
                }
                //-------------------------------------------------------------------------
                receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.invoiceAmountLeave;
                foreach (var item in request.warehouseDocumentItem)
                {
                    if(item.Quantity>0)
                    {
                        WarehouseLayoutQuantity warehouseLayoutQuantity = await _WarehouseLayoutQuantityRepository.Find(item.WarehouseLayoutQuantitiyId);

                        var warehouseLayout = await _context.WarehouseLayouts.Where(a => a.Id == warehouseLayoutQuantity.WarehouseLayoutId).FirstOrDefaultAsync();

                        DocumentItem ReceiptItems = await DocumentItemsSelect(item, receipt);

                        await UpdateWarehouseLayout(receipt.WarehouseId, warehouseLayoutQuantity.WarehouseLayoutId, item.CommodityId, item.Quantity, (int)WarehouseHistoryMode.Exit, ReceiptItems.Id, warehouseLayoutQuantity);

                        //-----آیا همه اقلام درخواست به طور کامل خارج شده است؟----------
                        await UpdateLeaveCompleteAndWarehouseIdReceipt(receipt, request.request_No, Convert.ToInt32(warehouseLayout.WarehouseId));
                    }
                   


                }
                foreach (var item in request.warehouseDocumentItem)
                {
                    if (item.Quantity > 0)
                    {
                        await UpdateInsertWarehouseRequestExit(item, receipt);
                    }
                }

                return ServiceResult<Receipt>.Success(receipt);

            }
            catch (Exception ex)
            {
                if (!(ex is ValidationError))
                {
                    new LogWriter("LeavingPartWarehouseCommand Error request_No: " + request.request_No + "**" + ex.Message.ToString(), "LeavingPartWarehouse");
                    if (ex.InnerException != null)
                    {
                        new LogWriter("LeavingPartWarehouseCommand InnerException:" + request.request_No + "**" + ex.InnerException.ToString(), "LeavingPartWarehouse");
                    }
                    ArchiveReceiptCommand ArchiveReceiptCommand = new ArchiveReceiptCommand() { Id = receipt.Id };
                    await _mediator.Send(ArchiveReceiptCommand);

                    await SendNotification(receipt, " خروجی شماره درخواست " + request.request_No + " بایگانی شد ");


                    throw new ValidationError("مشکل در انجام عملیات خروج ، حواله مربوطه بایگانی شد");

                }
                else { throw new ValidationError(ex.Message); };

            }

           
           



        }
        private async Task SendNotification(Receipt receipt, string MessageContent)
        {

            var message = new NotificationDto
            {
                MessageTitle = "خروجی انبار از حواله های ERP",
                MessageContent = MessageContent,
                MessageType = 1,
                Payload = "",
                SendForAllUser = false,
                Status = MessageStatus.Sent,
                OwnerRoleId = 1,
                Listener = "notifyInventoryReciept",
                ReceiverUserId = receipt.CreatedById
            };


            await _notificationClient.Send(message);

        }


        //--------------------بروزرسانی WarehouseRequestExit---------------------------------
        private async Task<int> UpdateInsertWarehouseRequestExit(LeavingPartWarehouseDocumentItem request, Receipt Receipt)
        {
            var exit = await _warehouseRequestExitRepository.GetAll().Where(a => a.RequestItemId == request.RequestItemId && a.WarehouseLayoutQuantityId == request.WarehouseLayoutQuantitiyId && !a.IsDeleted).FirstOrDefaultAsync();
            if (exit != null)
            {

                return await UpdateWarehouseRequestExit(request, exit);
            }
            else
            {

                return await AddWarehouseRequestExit(request, Receipt);
            }


        }
        //اطلاعات درخواست خروج انبار
        private async Task<int> UpdateWarehouseRequestExit(LeavingPartWarehouseDocumentItem request, WarehouseRequestExit model)
        {
            model.Quantity += request.Quantity;

            if (model.Quantity >= model.RequestQuantity)
            {
                return 1;
            }

            _warehouseRequestExitRepository.Update(model);
            return await _warehouseRequestExitRepository.SaveChangesAsync();

        }
        //اطلاعات درخواست خروج انبار
        private async Task<int> AddWarehouseRequestExit(LeavingPartWarehouseDocumentItem request, Receipt Receipt)
        {
            DocumentItem ReceiptItems = await DocumentItemsSelect(request, Receipt);
            if (ReceiptItems == null)
            {
                throw new ValidationError("برای این درخواست هیچ  اقلام سندی وجود ندارد");
            }
            WarehouseRequestExit exit = new WarehouseRequestExit()
            {
                WarehouseLayoutQuantityId = request.WarehouseLayoutQuantitiyId,
                Quantity = request.Quantity,
                Commodityld = request.CommodityId,
                RequestItemId = request.RequestItemId,
                RequestId = Convert.ToInt32(Receipt.RequestNo),
                RequestQuantity = request.QuantityTotal,
                DocumentHeadId = Receipt.Id,
                DocumentItemsId = ReceiptItems.Id,

            };


            _warehouseRequestExitRepository.Insert(exit);
            return await _warehouseRequestExitRepository.SaveChangesAsync();

        }
        //اطلاعات سند خروج انبار
        private async Task<DocumentItem> DocumentItemsSelect(LeavingPartWarehouseDocumentItem request, Receipt Receipt)
        {
            DocumentItem documentItem = await _context.DocumentItems.Where(a => a.DocumentHeadId == Receipt.Id
                                                                                && a.CommodityId == request.CommodityId
                                                                                && a.RequestDocumentItemId == request.RequestItemId
                                                                                && a.Quantity == request.Quantity
                                                                                ).FirstOrDefaultAsync();
            if (documentItem == null)
            {
                documentItem = await AddItems(request, Receipt);
            }

            return documentItem;
        }

        private async Task<DocumentItem> AddItems(LeavingPartWarehouseDocumentItem request, Receipt Receipt)
        {
            var documentItem = new DocumentItem();
            var commodity = await _context.Commodities.Where(a => a.Id == request.CommodityId).FirstOrDefaultAsync();

            documentItem.CurrencyBaseId = await _baseValueRepository.GetAll().Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();
            documentItem.CurrencyPrice = 1;
            documentItem.DocumentHeadId = Receipt.Id;
            documentItem.DocumentMeasureId = Convert.ToInt32(commodity.MeasureId);
            documentItem.MainMeasureId = Convert.ToInt32(commodity.MeasureId);
            documentItem.CommodityId = commodity.Id;
            documentItem.Quantity = request.Quantity;
            documentItem.UnitBasePrice = default;
            documentItem.ProductionCost = default;
            documentItem.Discount = default;
            documentItem.Weight = default;
            documentItem.UnitBasePrice = default;
            documentItem.RequestDocumentItemId = request.RequestItemId;
            documentItem.Description = request.Description;
            documentItem.YearId = _currentUserAccessor.GetYearId();

            await _receiptCommandsService.GetPriceBuyItems(documentItem.CommodityId, Receipt.WarehouseId, documentItem.BomValueHeaderId > 0 ? documentItem.Id : null, documentItem.Quantity, documentItem);
            _documentItemRepository.Insert(documentItem);

            await _documentItemRepository.SaveChangesAsync();


            return documentItem;
        }

        private async Task<Domain.Receipt> UpdateLeaveCompleteAndWarehouseIdReceipt(Domain.Receipt Receipt, int requestId, int warehouseId)
        {


            await _receiptCommandsService.CalculateTotalItemPrice(Receipt);
            Receipt.WarehouseId = warehouseId;

            _receiptRepository.Update(Receipt);

            var exit = _context.WarehouseRequestExit.Where(x => x.RequestId == requestId).Sum(a => a.Quantity);
            var request = _context.DocumentItems.Where(a => a.DocumentHeadId == Receipt.Id).Sum(a => a.Quantity);
            if (exit >= request)
            {
                Receipt.IsPlacementComplete = true;

            }

            await _receiptRepository.SaveChangesAsync();
            return Receipt;


        }
        //-----------------افزایش و کاهش ظرفیت فعلی در مکان--------------------
        private async Task UpdateWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int historyMode, int? documentItemId, WarehouseLayoutQuantity warehouseLayoutQuantity)
        {


            var stock = await _context.WarehouseStocks.Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();
            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(CommodityId, Quantity, WarehouseLayoutId, documentItemId, historyMode);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, warehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, historyMode, stock);

        }

    }
}