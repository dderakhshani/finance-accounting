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
using Eefa.NotificationServices.Common.Enum;
using Eefa.NotificationServices.Dto;
using Eefa.NotificationServices.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class UpdateQuantityCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<UpdateQuantityCommand>, ICommand
    {
        public int Id { get; set; }
        public double Quantity { get; set; } = default!;

    }

    public class UpdateQuantityCommandHandler : IRequestHandler<UpdateQuantityCommand, ServiceResult<Domain.Receipt>>
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly INotificationClient _notificationClient;
        private readonly IProcedureCallService _procedureCallService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRepository<WarehouseStocks> _stockRepository;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<WarehouseHistory> _WarehouseHistoryRepository;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;
        private readonly IRepository<WarehouseLayoutQuantity> _WarehouseLayoutQuantityRepository;
       



        public UpdateQuantityCommandHandler(
             IMapper mapper
            , IInvertoryUnitOfWork context
            , IReceiptRepository receiptRepository
            , INotificationClient notificationClient
            , IProcedureCallService procedureCallService
            , ICurrentUserAccessor currentUserAccessor
            , IRepository<WarehouseStocks> stockRepository
            , IRepository<DocumentItem> documentItemRepository
            , IReceiptCommandsService receiptCommandsService
            , IRepository<WarehouseHistory> WarehouseHistoryRepository
            , IWarehouseLayoutCommandsService warehouseLayoutCommandsService
            , IRepository<WarehouseLayoutQuantity> WarehouseLayoutQuantityRepository
            )

        {
            _mapper = mapper;
            _context = context;
            _stockRepository = stockRepository;
            _receiptRepository = receiptRepository;
            _procedureCallService= procedureCallService;
            _notificationClient = notificationClient;
            _currentUserAccessor = currentUserAccessor;
            _receiptCommandsService = receiptCommandsService;
            _documentItemRepository = documentItemRepository;
            _WarehouseHistoryRepository = WarehouseHistoryRepository;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;
            _WarehouseLayoutQuantityRepository = WarehouseLayoutQuantityRepository;
        }
        public async Task<ServiceResult<Domain.Receipt>> Handle(UpdateQuantityCommand request, CancellationToken cancellationToken)
        {


            int historyMode = 0;
            bool IsEnterWarehouse = true;
            var documentItem = await _documentItemRepository.Find(request.Id);

            if (documentItem == null)
            {
                throw new ValidationError("کالا موجود نیست");
            }
            //چیزی برای تغییر وجود ندارد
            if (documentItem.Quantity == request.Quantity)
            {
                return null;
            }
            var receipt = await _receiptRepository.GetAll().Where(a => a.Id == documentItem.DocumentHeadId).FirstOrDefaultAsync();
            if (receipt == null)
            {
                throw new ValidationError("سند موجود نیست");
            }


            double Quantity = request.Quantity;
            //به انبار وارد شده

            if (receipt.DocumentStauseBaseValue == (int)DocumentStateEnam.Direct || receipt.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmount)
            {

                IsEnterWarehouse = true;
            }

            //از انباری خارج شده است
            if (receipt.DocumentStauseBaseValue == (int)DocumentStateEnam.Leave || receipt.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmountLeave)
            {

                IsEnterWarehouse = false;
            }


            var commodity = await _context.Commodities.Where(a => a.Id == documentItem.CommodityId).FirstOrDefaultAsync();
            string MessageContent = " تعداد کالا کد " + commodity.Code + " در رسید شماره " + receipt.DocumentNo + " از تعداد  " + documentItem.Quantity.ToString() + " به تعداد " + request.Quantity.ToString() + " تغییر یافت ";

            Quantity = (request.Quantity - documentItem.Quantity);
            // اگر رسید از نوع ورودی باشد ظرفیت باید از انبار کسر شود
            if (!IsEnterWarehouse)
            {
                Quantity = -1 * Quantity;
            }
            //اگر بزرگ تر از صفر باشد یعنی ورود جدید داریم و اگر کوچکتر از صفر باشد یعنی خروج داریم
            historyMode = request.Quantity >= documentItem.Quantity ? (int)(WarehouseHistoryMode.Enter) : (int)(WarehouseHistoryMode.Exit);
            //----------------اولین محلی که پیدا شد--------------------------
            WarehouseLayout layouts = await _warehouseLayoutCommandsService.FindLayout(receipt.WarehouseId, documentItem.CommodityId);

            await UpdateWarehouseLayout(receipt.WarehouseId, layouts.Id, documentItem.CommodityId, Math.Abs(Quantity), historyMode, documentItem.Id, request.Quantity);

            //-----------------------------------------------------------
            documentItem.Quantity = request.Quantity;
            _documentItemRepository.Update(documentItem);

            await _documentItemRepository.SaveChangesAsync();

            //بازسازی کاردکس انبار
            await RepairCardex(documentItem);

            await SendNotification(receipt, MessageContent);

            return ServiceResult<Domain.Receipt>.Success(receipt);
        }

        private async Task RepairCardex(DocumentItem documentItem)
        {
            var receipt1 = await _context.DocumentHeads.Where(a => a.Id == documentItem.DocumentHeadId).FirstOrDefaultAsync();
            //await _receiptCommandsService.ComputeAvgPrice(documentItem.CommodityId, receipt1);
            var spComputeAvgPrice = new spComputeAvgPriceParam() { CommodityId = Convert.ToInt32(documentItem.CommodityId), ReceiptId = receipt1.Id, YearId = _currentUserAccessor.GetYearId(), CaredxRepairId = Guid.NewGuid() };
            var CardexRepair = new spUpdateinventory_CaredxRepairParam() { CaredxRepairId = Guid.NewGuid(), YearId = _currentUserAccessor.GetYearId() };
            await _procedureCallService.ComputeAvgPrice(spComputeAvgPrice, CardexRepair);
        }


        //-----------------افزایش و کاهش ظرفیت فعلی در مکان--------------------
        private async Task UpdateWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int hitoryMode, int receiptItems, double request_Quantity)
        {
            var WarehouseLayoutQuantity = await _context.WarehouseLayoutQuantities.Where(a => a.WarehouseLayoutId == WarehouseLayoutId && a.CommodityId == CommodityId).FirstOrDefaultAsync();
            var stock = await _stockRepository.GetAll().Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();

            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(CommodityId, request_Quantity, WarehouseLayoutId, receiptItems, hitoryMode);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, hitoryMode, WarehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, hitoryMode, stock);

        }
        private async Task SendNotification(Receipt receipt,string MessageContent)
        {

            var message = new NotificationDto
            {
                MessageTitle = "تغییر تعداد کالا در انبار",
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

    }
}