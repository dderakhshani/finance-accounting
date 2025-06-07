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
using Eefa.NotificationServices.Common.Enum;
using Eefa.NotificationServices.Dto;
using Eefa.NotificationServices.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using static Eefa.Inventory.Domain.Common.ConstantValues;

namespace Eefa.Inventory.Application
{
    /// <summary>
    ///دریافت کالا از انبار توسط پرسنل
    /// </summary>
    public class LeaveCommodityCommand : CommandBase, IRequest<ServiceResult<ReceiptQueryModel>>, IMapFrom<InsertDocumentHeads>, ICommand
    {
        public int Id { get; set; }
        public int codeVoucherGroupId { get; set; }
        public Nullable<bool> IsDocumentIssuance { get; set; } = default!;
        public List<InsertDocumentItems> DocumentItems { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<InsertDocumentHeadsForIOCommodity, InsertDocumentHeads>()
                .IgnoreAllNonExisting();
        }
    }

    public class LeaveCommodityCommandHandler : IRequestHandler<LeaveCommodityCommand, ServiceResult<ReceiptQueryModel>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptQueries _receiptQueries;
        private readonly IProcedureCallService _procedureCallService;
        private readonly INotificationClient _notificationClient;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<CodeVoucherGroup> _codeVoucherGroupRepository;
        private readonly IRepository<WarehouseHistory> _WarehouseHistory;
        private readonly IWarehouseLayoutRepository _WarehouseLayoutRepository;
        private readonly IRepository<WarehouseStocks> _stockRepository;
        private readonly IRepository<BaseValue> _baseValueRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<DocumentHeadExtend> _documentHeadExtendRepository;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;
        private readonly IRepository<PersonsDebitedCommodities> _personsDebitedCommoditiesRepository;
        private readonly IRepository<WarehouseLayoutQuantity> _WarehouseLayoutQuantityRepository;

        public LeaveCommodityCommandHandler(
            IReceiptRepository receiptRepository,
            IMapper mapper,
            IInvertoryUnitOfWork context,
            IRepository<WarehouseLayoutQuantity> WarehouseLayoutQuantity,
            IRepository<WarehouseHistory> warehouseHistory,
            IReceiptQueries receiptQueries,
            IRepository<WarehouseStocks> stockRepository,
            INotificationClient notificationClient,
            IWarehouseLayoutRepository WarehouseLayout,
            IRepository<DocumentItem> documentItemRepository,
            IRepository<CodeVoucherGroup> codeVoucherGroupRepository,
            IRepository<BaseValue> baseValueRepository,
            ICurrentUserAccessor currentUserAccessor,
            IRepository<DocumentHeadExtend> documentHeadExtendRepository,
            IRepository<PersonsDebitedCommodities> personsDebitedCommoditiesRepository,
            IReceiptCommandsService receiptCommandsService,
            IWarehouseLayoutCommandsService warehouseLayoutCommandsService,
            IProcedureCallService procedureCallService,
            IMediator mediator
            )
        {
            _receiptRepository = receiptRepository;
            _mapper = mapper;
            _WarehouseLayoutQuantityRepository = WarehouseLayoutQuantity;
            _WarehouseHistory = warehouseHistory;
            _receiptQueries = receiptQueries;
            _stockRepository = stockRepository;
            _WarehouseLayoutRepository = WarehouseLayout;
            _documentItemRepository = documentItemRepository;
            _codeVoucherGroupRepository = codeVoucherGroupRepository;
            _baseValueRepository = baseValueRepository;
            _currentUserAccessor = currentUserAccessor;
            _documentHeadExtendRepository = documentHeadExtendRepository;
            _context = context;
            _personsDebitedCommoditiesRepository = personsDebitedCommoditiesRepository;
            _receiptCommandsService = receiptCommandsService;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;
            _procedureCallService = procedureCallService;
            _mediator = mediator;
            _notificationClient = notificationClient;
        }
        public async Task<ServiceResult<ReceiptQueryModel>> Handle(LeaveCommodityCommand request, CancellationToken cancellationToken)
        {


            //foreach (var quantity in quantities)
            //{
            //    foreach (var item in request.DocumentItems)
            //    {
            //        if (item.WarehouseLayoutQuantityId == quantity.Id && item.Quantity > quantity.Quantity)
            //        {
            //            throw new ValidationError("تعداد کالا خروجی بیشتر از تعداد موجودی انبار است");
            //        }
            //    }
            //}
            var year = await _context.Years.Where(a => a.IsCurrentYear).FirstOrDefaultAsync();
            if (year == null)
            {
                throw new ValidationError("سال مالی تنظیم نشده است");
            }

            var RequestReceipt = await _receiptRepository.GetAll().Where(a => a.Id == request.Id).FirstOrDefaultAsync();
            var requester = await _context.DocumentHeadExtend.Where(a => a.DocumentHeadId == request.Id).FirstOrDefaultAsync();
            var docItems = await _documentItemRepository.GetAll().Where(x => x.DocumentHeadId == request.Id && !x.IsDeleted).ToListAsync();
            var warehouse = await _context.Warehouses.Where(a => a.Id == RequestReceipt.WarehouseId).FirstOrDefaultAsync();

            var codeVoucherGroup = await _codeVoucherGroupRepository.GetAll().Where(a => a.Id == RequestReceipt.CodeVoucherGroupId).FirstOrDefaultAsync();

            Receipt Receipt = await _receiptRepository.GetAll().Where(a => a.RequestNo == RequestReceipt.DocumentNo.ToString() && a.CodeVoucherGroupId == request.codeVoucherGroupId && a.WarehouseId == RequestReceipt.WarehouseId && a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveReceipt).FirstOrDefaultAsync();
            try
            {

                var currency = await _baseValueRepository.GetAll().Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();

                InsertDocumentHeadsForIOCommodity ReceiptParam = new InsertDocumentHeadsForIOCommodity();
                ReceiptParam.receiptdocument = _mapper.Map<InsertDocumentHeads>(request);

                ReceiptParam.receiptdocument.receiptDocumentItems = request.DocumentItems.ToList();
                ReceiptParam.userId = _currentUserAccessor.GetId();
                ReceiptParam.yearId = _currentUserAccessor.GetYearId();
                ReceiptParam.OwnerRoleId = _currentUserAccessor.GetRoleId();
                ReceiptParam.receiptdocument.WarehouseId = RequestReceipt.WarehouseId;
                ReceiptParam.receiptdocument.DocumentDate = RequestReceipt.DocumentDate;
                ReceiptParam.receiptdocument.CurrencyBaseId = currency;


                switch (codeVoucherGroup.UniqueName)
                {
                    case "RemoveAssetsWarhouse":
                        ReceiptParam.receiptdocument.DocumentStauseBaseValue = (int)DocumentStateEnam.Transfer;
                        break;
                    case "RemoveReturnReceipt":
                        ReceiptParam.receiptdocument.DocumentStauseBaseValue = (int)DocumentStateEnam.Leave;
                        break;
                    default:
                        ReceiptParam.receiptdocument.DocumentStauseBaseValue = (int)DocumentStateEnam.invoiceAmountLeave;
                        break;
                }

                ReceiptParam.receiptdocument.CodeVoucherGroupId = request.codeVoucherGroupId;
                ReceiptParam.receiptdocument.CommandDescription = "LeaveCommodityCommand :The operation of leaving the goods from the warehouse";
                ReceiptParam.receiptdocument.Tags = _receiptCommandsService.ConvertTagArray(RequestReceipt.Tags);
                ReceiptParam.receiptdocument.InvoiceNo = _receiptCommandsService.GenerateInvoiceNumber(ConstantValues.WarehouseInvoiceNoEnam.LeaveCommodity, codeVoucherGroup);
                ReceiptParam.receiptdocument.Mode = (int)(WarehouseHistoryMode.Exit);
                ReceiptParam.receiptdocument.CodeVoucherGroupUniqueName = codeVoucherGroup.UniqueName;
                ReceiptParam.receiptdocument.RequestNo = RequestReceipt.DocumentNo.ToString();
                ReceiptParam.receiptdocument.RequestId = RequestReceipt.Id;
                DebitAndCredit(RequestReceipt, warehouse, ReceiptParam);


                if (Receipt == null)
                {

                    Receipt = await _procedureCallService.InsertDocumentHeadsForIOCommodity(ReceiptParam);
                }
                else
                {
                    ReceiptParam.receiptdocument.DocumentHeadId = Receipt.Id;
                    Receipt = await _procedureCallService.UpdateDocumentHeadsForIOCommodity(ReceiptParam);
                }

            }
            catch (Exception ex)
            {
                if (!(ex is ValidationError))
                {
                    new LogWriter("LeavingPartWarehouseCommand Error:" + ex.Message.ToString());
                    if (ex.InnerException != null)
                    {
                        new LogWriter("LeavingPartWarehouseCommand InnerException:" + ex.InnerException.ToString());
                    }
                    if(Receipt!=null)
                    {
                        ArchiveReceiptCommand ArchiveReceiptCommand = new ArchiveReceiptCommand() { Id = Receipt.Id };
                        await _mediator.Send(ArchiveReceiptCommand);
                        await SendNotification(RequestReceipt, " خروجی شماره درخواست " + RequestReceipt.DocumentNo + " بایگانی شد ");
                        throw new ValidationError("مشکل در انجام عملیات خروج حواله مربوطه بایگانی شد");
                    }
                    else
                    {
                        throw;
                    }
                    

                }
                else { throw new ValidationError(ex.Message); };
            }


           

            return ServiceResult<ReceiptQueryModel>.Success(null);

        }

        private void DebitAndCredit(Receipt RequestReceipt, Warehouse warehouse, InsertDocumentHeadsForIOCommodity ReceiptParam)
        {

            //------------------------انبار بستانکار-------------------------

            ReceiptParam.receiptdocument.CreditAccountHeadId = warehouse.AccountHeadId;
            ReceiptParam.receiptdocument.DebitAccountReferenceId = RequestReceipt.DebitAccountReferenceId;
            ReceiptParam.receiptdocument.DebitAccountReferenceGroupId = RequestReceipt.DebitAccountReferenceGroupId;
            ReceiptParam.receiptdocument.DebitAccountHeadId = RequestReceipt.DebitAccountHeadId;

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

    }

}