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
    public class LeavingCommodityWarehouseCommand : CommandBase, IRequest<ServiceResult<ReceiptQueryModel>>, IMapFrom<LeavingCommodityWarehouseCommand>, ICommand
    {
        public int Id { get; set; }
        public int codeVoucherGroupId { get; set; }
        public Nullable<bool> IsDocumentIssuance { get; set; } = default!;
        public List<LeavingConsumableDocumentItem> DocumentItems { get; set; }
    }
    public class LeavingConsumableDocumentItem
    {
        public int DocumentItemId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public double Quantity { get; set; } = default!;
        public int WarehouseLayoutId { get; set; }
        public int WarehouseLayoutQuantityId { get; set; }
        public int WarehouseId { get; set; }
        public List<int> AssetsId { get; set; }
        public int MeasureId { get; set; }
        public string Description { get; set; }


    }
    public class LeavingCommodityWarehouseCommandHandler : IRequestHandler<LeavingCommodityWarehouseCommand, ServiceResult<ReceiptQueryModel>>
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

        public LeavingCommodityWarehouseCommandHandler(
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
        public async Task<ServiceResult<ReceiptQueryModel>> Handle(LeavingCommodityWarehouseCommand request, CancellationToken cancellationToken)
        {
            var quantities = await _context.WarehouseLayoutQuantities.Where(a => request.DocumentItems.Select(a => a.WarehouseLayoutQuantityId).Contains(a.Id)).ToListAsync();

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
            
            Receipt newReceipt = await _receiptRepository.GetAll().Where(a => a.RequestNo == RequestReceipt.DocumentNo.ToString() && a.CodeVoucherGroupId == request.codeVoucherGroupId && a.WarehouseId == RequestReceipt.WarehouseId && a.DocumentStauseBaseValue!= (int)DocumentStateEnam.archiveReceipt).FirstOrDefaultAsync();
            try
            {
                if (newReceipt == null)
                {
                    
                    newReceipt = await AddNewReceipt(request, year, RequestReceipt, docItems, warehouse, codeVoucherGroup, cancellationToken);
                }
                else
                {
                    newReceipt.Items = await _context.DocumentItems.Where(a => a.DocumentHeadId == newReceipt.Id && !a.IsDeleted).ToListAsync();
                }

                if (newReceipt.Items == null || newReceipt.Items.Count() == 0)
                {
                    foreach (var item in docItems)
                    {

                        await AddItems(newReceipt, item, request);

                    };
                }
                await _receiptRepository.SaveChangesAsync();
                await ModifyDocumentItems(request, RequestReceipt, newReceipt);
                var RemainQuantity = _documentItemRepository.GetAll().Where(x => x.DocumentHeadId == newReceipt.Id && !x.IsDeleted).Sum(x => x.RemainQuantity);

                if (RemainQuantity <= 0)
                {
                    await _receiptCommandsService.CalculateTotalItemPrice(newReceipt);
                    _receiptRepository.Update(newReceipt);


                }

                var model = await _receiptQueries.GetByDocumentNoAndCodeVoucherGroupId(Convert.ToInt32(newReceipt.DocumentNo), Convert.ToInt32(newReceipt.CodeVoucherGroupId));

                return ServiceResult<ReceiptQueryModel>.Success(model);
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
                    ArchiveReceiptCommand ArchiveReceiptCommand = new ArchiveReceiptCommand() { Id = newReceipt.Id };
                    await _mediator.Send(ArchiveReceiptCommand);
                    await SendNotification(RequestReceipt, " خروجی شماره درخواست " + RequestReceipt.DocumentNo + " بایگانی شد ");
                    throw new ValidationError("مشکل در انجام عملیات خروج حواله مربوطه بایگانی شد");

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
        private async Task<Receipt> AddNewReceipt(LeavingCommodityWarehouseCommand request, Year year, Receipt RequestReceipt, List<DocumentItem> docItems, Warehouse warehouse, CodeVoucherGroup codeVoucherGroup, CancellationToken cancellationToken)
        {
            Receipt newReceipt = new Receipt();
            var NewcodeVoucherGroup = await _codeVoucherGroupRepository.GetAll().Where(a => a.Id == request.codeVoucherGroupId).FirstOrDefaultAsync();

            switch (codeVoucherGroup.UniqueName)
            {
                case "RemoveAssetsWarhouse":
                    newReceipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Transfer;
                    break;
                case "RemoveReturnReceipt":
                    newReceipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Leave;
                    break;
                default:
                    newReceipt.DocumentStauseBaseValue = (int)DocumentStateEnam.invoiceAmountLeave;
                    break;
            }
            newReceipt.IsDocumentIssuance = request.IsDocumentIssuance;
            newReceipt.WarehouseId = RequestReceipt.WarehouseId;
            newReceipt.CodeVoucherGroupId = request.codeVoucherGroupId;

            _receiptCommandsService.ReceiptBaseDataInsert(RequestReceipt.DocumentDate, newReceipt);
            newReceipt.RequestDate = RequestReceipt.DocumentDate;
            newReceipt.ViewId = null;
            newReceipt.ExpireDate = year.LastDate;

            DebitAndCredit(RequestReceipt, warehouse, newReceipt, NewcodeVoucherGroup);
            newReceipt.IsImportPurchase = false;
            newReceipt.IsPlacementComplete = true;


            newReceipt.CommandDescription = "LeavingCommodityWarehouseCommand :The operation of leaving the goods from the warehouse";

            await _receiptCommandsService.SerialFormula(newReceipt, NewcodeVoucherGroup.Code, cancellationToken);

            int lastNo = await _receiptCommandsService.lastDocumentNo(newReceipt, cancellationToken);

            newReceipt.DocumentNo = lastNo + 1;
            newReceipt.RequestNo = RequestReceipt.DocumentNo.ToString();
            _receiptCommandsService.GenerateInvoiceNumber(ConstantValues.WarehouseInvoiceNoEnam.LeaveCommodity, newReceipt, NewcodeVoucherGroup);


            //-------------- receipt.TotalItemPrice--------------------------------
            newReceipt.VatPercentage = RequestReceipt.VatPercentage;
            newReceipt.VatDutiesTax = RequestReceipt.VatDutiesTax;
            newReceipt.ExtraCost = RequestReceipt.ExtraCost;


            _receiptRepository.Insert(newReceipt);
            if (await _receiptRepository.SaveChangesAsync() > 0)
            {
                foreach (var item in docItems)
                {
                    await AddItems(newReceipt, item, request);

                };
                await _receiptCommandsService.CalculateTotalItemPrice(newReceipt);

                newReceipt.DocumentId = await _receiptCommandsService.InsertAndUpdateDocument(newReceipt);
                _receiptRepository.Update(newReceipt);
                await _receiptRepository.SaveChangesAsync();

                //---------------------insert documentHeadExtend --------------------------

                await InsertDocumentHeadExtend(newReceipt);

            }

            return newReceipt;
        }


        private void DebitAndCredit(Receipt RequestReceipt, Warehouse warehouse, Receipt newReceipt, CodeVoucherGroup NewCodeVoucherGroup)
        {

            //------------------------انبار بستانکار-------------------------

            newReceipt.CreditAccountHeadId = warehouse.AccountHeadId;
            newReceipt.DebitAccountReferenceId = RequestReceipt.DebitAccountReferenceId;
            newReceipt.DebitAccountReferenceGroupId = RequestReceipt.DebitAccountReferenceGroupId;
            newReceipt.DebitAccountHeadId =RequestReceipt.DebitAccountHeadId;

        }

        private async Task ModifyDocumentItems(LeavingCommodityWarehouseCommand request, Receipt RequstReceipt, Receipt newReceipt)
        {
            foreach (var item in request.DocumentItems)
            {

                if (item.Quantity > 0)
                {
                    var docItem = await _documentItemRepository.GetAll().Where(x => x.Id == item.DocumentItemId && !x.IsDeleted).FirstOrDefaultAsync();
                    var docItemNew = await _documentItemRepository.GetAll().Where(x => x.DocumentHeadId == newReceipt.Id && x.RequestDocumentItemId == item.DocumentItemId && !x.IsDeleted).FirstOrDefaultAsync();
                    if (docItem != null)
                    {
                        docItem.RemainQuantity = docItem.RemainQuantity - item.Quantity;
                        docItem.Description = item.Description;

                    }
                    if (docItemNew != null)
                    {
                        docItemNew.RemainQuantity = docItem.RemainQuantity;
                        docItemNew.Description = item.Description;
                        docItemNew.Quantity = Convert.ToDouble(docItem.Quantity) - Convert.ToDouble(docItemNew.RemainQuantity);
                    }

                    //محاسبه و ویرایش تعداد کالا درخواستی باقی مانده دریافت نشده
                    await _procedureCallService.CalculateRemainQuantityRequestCommodityByPerson(RequstReceipt.RequestNo, item.CommodityId, item.DocumentItemId);

                    //اگر درخواست مرجوعی کالا بود  به تعداد درخواست خرید آن اضافه شود
                    await _procedureCallService.CalculateRemainQuantityRequest(RequstReceipt.RequestNo, item.CommodityId);

                    await PersonsDebitedCommodities(item, RequstReceipt);

                    await UpdateWarehouseLayout(item.WarehouseId, item.WarehouseLayoutId, item.CommodityId, item.Quantity, (int)(WarehouseHistoryMode.Exit), docItemNew.Id, item.WarehouseLayoutQuantityId);



                }
            };
            await _documentItemRepository.SaveChangesAsync();
            
        }

        private async Task AddItems(Receipt newReceipt, DocumentItem item, LeavingCommodityWarehouseCommand request)
        {
            var requ = request.DocumentItems.Where(a => a.CommodityId == a.CommodityId).FirstOrDefault();
           var documentItem = new DocumentItem()
            {
               
                CurrencyBaseId = item.CurrencyBaseId,
                CurrencyPrice = 1,
                DocumentMeasureId = item.DocumentMeasureId,
                MainMeasureId = item.MainMeasureId,
                Quantity = requ.Quantity,
                RequestDocumentItemId = item.Id,
                RemainQuantity = item.Quantity,
                Description = requ.Description,
                SecondaryQuantity = item.SecondaryQuantity,
                CommodityId = item.CommodityId,
                IsWrongMeasure = item.IsWrongMeasure,
                ModifiedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                CreatedById = _currentUserAccessor.GetId(),
                IsDeleted = false,
                OwnerRoleId = _currentUserAccessor.GetRoleId(),
            };


            await _procedureCallService.CalculateRemainQuantityRequest(newReceipt.RequestNo, item.CommodityId);
            await _receiptCommandsService.GetPriceBuyItems(documentItem.CommodityId, newReceipt.WarehouseId, null, documentItem.Quantity, documentItem);

            if (AvoidInsertDuplicateItems(newReceipt, item))
            {
                newReceipt.AddItem(documentItem);

                await _receiptRepository.SaveChangesAsync();
            }

        }
        //از هر درخواست بیش از یکبار به وجود نیاید
        private static bool AvoidInsertDuplicateItems(Receipt newReceipt, DocumentItem item)
        {
            if (newReceipt.Items == null)
            {
                return true;
            }
            else
            {
                return newReceipt.Items.Where(a => a.RequestDocumentItemId == item.RequestDocumentItemId).Count() == 0;
            }

        }

        private async Task InsertDocumentHeadExtend(Domain.Receipt receipt)
        {
            var RequesterReferenceId = await _documentHeadExtendRepository.GetAll().Where(x => x.DocumentHeadId == receipt.Id).Select(a => a.RequesterReferenceId).FirstOrDefaultAsync();

            if (RequesterReferenceId != null)
            {
                //---------------------insert documentHeadExtend --------------------------
                await _receiptCommandsService.InsertDocumentHeadExtend(RequesterReferenceId, null, receipt);
            }

        }
        private async Task PersonsDebitedCommodities(LeavingConsumableDocumentItem item, Domain.Receipt receipt)
        {
            var codeVoucherGroupUniqueName = await _codeVoucherGroupRepository.GetAll().Where(a => a.Id == receipt.CodeVoucherGroupId).Select(a => a.UniqueName).FirstOrDefaultAsync();
            var RequesterReferenceId = await _documentHeadExtendRepository.GetAll().Where(a => a.DocumentHeadId == receipt.Id).Select(a => a.RequesterReferenceId).FirstOrDefaultAsync();

            var person = await _context.EmployeesUnitsView.Where(a => a.AccountReferenceId == RequesterReferenceId).FirstOrDefaultAsync();

            if (ConstantValues.CodeVoucherGroupValues.RequestReceiveConsumption != codeVoucherGroupUniqueName && person != null)
            {
                if (item.AssetsId.Count() > 0)
                {
                    foreach (var ass in item.AssetsId)
                    {
                        PersonsDebitedCommodities PersonsDebitedCommodities = new PersonsDebitedCommodities() { Quantity = 1, AssetId = ass };

                        AddAssets(item, receipt, person, PersonsDebitedCommodities);

                    }
                }
                else
                {
                    PersonsDebitedCommodities PersonsDebitedCommodities = new PersonsDebitedCommodities() { Quantity = item.Quantity };

                    AddAssets(item, receipt, person, PersonsDebitedCommodities);
                }
                
            }

        }

        private void AddAssets(LeavingConsumableDocumentItem item, Receipt receipt, EmployeesUnitsView person, PersonsDebitedCommodities PersonsDebitedCommodities)
        {
            PersonsDebitedCommodities.WarehouseId = item.WarehouseId;
            PersonsDebitedCommodities.CommodityId = item.CommodityId;
            PersonsDebitedCommodities.DocumentDate = (Convert.ToDateTime(DateTime.Now.ToShortDateString())).ToUniversalTime();
            PersonsDebitedCommodities.ExpierDate = (Convert.ToDateTime(DateTime.Now.ToShortDateString())).ToUniversalTime();
            PersonsDebitedCommodities.IsActive = true;
            PersonsDebitedCommodities.PersonId = person.PersonId;
            PersonsDebitedCommodities.UnitId = person.UnitId;
            PersonsDebitedCommodities.DebitTypeId = receipt.CodeVoucherGroupId;
            PersonsDebitedCommodities.MeasureId = item.MeasureId;
            PersonsDebitedCommodities.DocumentItemId = item.DocumentItemId;
            PersonsDebitedCommodities.DebitAccountReferenceId = receipt.DebitAccountReferenceId;
            PersonsDebitedCommodities.Description = item.Description;
            _personsDebitedCommoditiesRepository.Insert(PersonsDebitedCommodities);
        }

        //======================================================================
        //----------------------------------------------------------------------
        //-----------------افزایش و کاهش ظرفیت فعلی در مکان--------------------
        private async Task UpdateWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int historyMode, int receiptItems, int WarehouseLayoutQuantityId)
        {
            var WarehouseLayoutQuantity = await _context.WarehouseLayoutQuantities.Where(a => a.Id == WarehouseLayoutQuantityId).FirstOrDefaultAsync();
            var stock = await _stockRepository.GetAll().Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();

            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(CommodityId, Quantity, WarehouseLayoutId, receiptItems, historyMode);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, WarehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, historyMode, stock);

        }


    }

}