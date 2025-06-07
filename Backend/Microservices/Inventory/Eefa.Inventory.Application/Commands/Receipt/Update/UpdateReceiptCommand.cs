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
using Eefa.Inventory.Application.Commands.Receipt.Update;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;

using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Inventory.Application
{
    public class UpdateReceiptCommand : CommandBase, IRequest<ServiceResult<ReceiptQueryModel>>, IMapFrom<Domain.Receipt>, ICommand
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; } = default!;
        public int? CreditAccountReferenceId { get; set; } = default!;
        public int? CreditAccountReferenceGroupId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; } = default!;
        public int? DebitAccountReferenceId { get; set; } = default!;
        public int? CodeVoucherGroupId { get; set; } = default!;
        public int? RequesterReferenceId { get; set; } = default!;
        public int? FollowUpReferenceId { get; set; } = default!;

        /// <description>
        /// توضیحات سند
        ///</description>

        public string DocumentDescription { get; set; }
        public string RequestNo { get; set; }
        public string PartNumber { get; set; }
        public string ScaleBill { get; set; } = default!;
        /// تاریخ سند
        ///</description>

        public DateTime DocumentDate { get; set; } = default!;

        /// <description>
        /// تاریخ انقضا
        ///</description>

        public DateTime? ExpireDate { get; set; }
        /// <description>
        /// تاریخ انقضا
        ///</description>

        public DateTime? RequestDate { get; set; }
        public int RequestNumber { get; set; }

        /// <description>
        /// شماره فاکتور فروشنده
        ///</description>
        public string InvoiceNo { get; set; }
        public string Tags { get; set; }
        public int DocumentNo { get; set; } = default!;

        public long? ExtraCost { get; set; } = default!;
        public bool? IsFreightChargePaid { get; set; } = default!;
        public bool? IsImportPurchase { get; set; } = default!;
        public Nullable<bool> IsDocumentIssuance { get; set; } = default!;
        public ICollection<ReceiptDocumentItemUpdate> ReceiptDocumentItems { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateReceiptCommand, Domain.Receipt>()
                .IgnoreAllNonExisting();
        }

        public class UpdateWarehouseReceiptCommandHandler : IRequestHandler<UpdateReceiptCommand, ServiceResult<ReceiptQueryModel>>
        {

            private readonly IMapper _mapper;
            private readonly IInvertoryUnitOfWork _context;
            private readonly IAssetsRepository _assetsRepository;
            private readonly IReceiptRepository _receiptRepository;
            private readonly ITemporaryReceipQueries _receiptQueries;
            private readonly IRepository<BaseValue> _baseValueRepository;
            private readonly IProcedureCallService _procedureCallService;
            private readonly IRepository<DocumentItem> _documentItemRepository;
            private readonly IReceiptCommandsService _receiptCommandsService;
            private readonly IRepository<DocumentHeadExtend> _documentHeadExtendRepository;
            private readonly IRepository<PersonsDebitedCommodities> _PersonsDebitedCommoditiesRepository;
            private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;
            public UpdateWarehouseReceiptCommandHandler(IReceiptRepository receiptRepository
              , IMapper mapper
              , IInvertoryUnitOfWork context
              , IAssetsRepository assetsRepository
              , IRepository<DocumentItem> DocumentItem
              , ITemporaryReceipQueries receiptQueries
              , IProcedureCallService procedureCallService
              , IRepository<BaseValue> baseValueRepository
              , IReceiptCommandsService receiptCommandsService
              , IRepository<DocumentHeadExtend> documentHeadExtendRepository

                ,
    IRepository<PersonsDebitedCommodities> personsDebitedCommoditiesRepository
                ,
    IWarehouseLayoutCommandsService warehouseLayoutCommandsService

         )
            {
                _mapper = mapper;
                _context = context;
                _receiptQueries = receiptQueries;
                _assetsRepository = assetsRepository;
                _receiptRepository = receiptRepository;
                _documentItemRepository = DocumentItem;
                _baseValueRepository = baseValueRepository;
                _procedureCallService = procedureCallService;
                _receiptCommandsService = receiptCommandsService;
                _documentHeadExtendRepository = documentHeadExtendRepository;
                _PersonsDebitedCommoditiesRepository = personsDebitedCommoditiesRepository;
                _warehouseLayoutCommandsService = warehouseLayoutCommandsService;

            }

            public async Task<ServiceResult<ReceiptQueryModel>> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
            {
                if (request.DocumentDate > DateTime.UtcNow)
                {
                    throw new ValidationError("تاریخ انتخابی برای زمان آینده نمی تواند باشد");
                }
                if (request.ReceiptDocumentItems.Count() == 0)
                {
                    throw new ValidationError("هیچ کالایی برای این سند وارد نشده است");
                }

                if (request.ReceiptDocumentItems.Where(a => a.Quantity <= 0).Count() > 0)
                {
                    throw new ValidationError("تعداد کالا به طور صحیح وارد نشده است");
                }
                var year = await _context.Years.Where(a => a.IsCurrentYear).FirstOrDefaultAsync();
                if (year == null)
                {
                    throw new ValidationError("سال مالی تنظیم نشده است");
                }
                var CurrencyBaseId = await _baseValueRepository.GetAll().Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();
                if (CurrencyBaseId == null)
                {
                    throw new ValidationError("واحد ارز ریالی تعریف نشده است");
                }
                var warehouse = await _context.Warehouses.Where(a => a.Id == request.WarehouseId).FirstOrDefaultAsync();
                if (warehouse == null)
                {
                    throw new ValidationError("کد انبار اشتباه است");
                }
                var CodeVoucher = await _context.CodeVoucherGroups.Where(a => a.Id == request.CodeVoucherGroupId).FirstOrDefaultAsync();

                if (CodeVoucher == null)
                {
                    throw new ValidationError("کد گروه سند وجود ندارد");
                }

                return await Update(request, year, CurrencyBaseId, warehouse, CodeVoucher, cancellationToken);

            }

            private async Task<ServiceResult<ReceiptQueryModel>> Update(UpdateReceiptCommand request, Year year, int CurrencyBaseId, Warehouse warehouse, CodeVoucherGroup CodeVoucher, CancellationToken cancellationToken)
            {
                var entity = await _receiptRepository.Find(request.Id);

                entity.ExpireDate = _receiptCommandsService.SetExpireDate(request.ExpireDate, year.LastDate);

                var OldRequestNo = entity.RequestNo;

                var Oldreceipt = entity;
                _mapper.Map<UpdateReceiptCommand, Domain.Receipt>(request, entity);

                // اگر شماره را دستی وارد کردند نباید تکراری باشد..
                //if (await _receiptCommandsService.IsDuplicateDocumentNo(entity, cancellationToken))
                //{
                //    throw new ValidationError("شماره درخواست تکراری است");
                //}
                //------------------------------------------------------------
                await DebitAndCredit(request, warehouse, CodeVoucher, entity);

                entity = _receiptCommandsService.ConvertTagArray(request.Tags, entity);



                //------------------حذف آیتم های پاک شده---------------------
                await DeleteItems(request, Oldreceipt);

                //------------------ویرایش آیتم های موجود--------------------
                await InsertAndUpdateDocumentItem(request, entity, Oldreceipt, CurrencyBaseId);

                await UpdateDocumentHeadExtend(request);



                //اگر نوع سند از اموال بود ، شماره سریال ها ثبت شود
                await InsertAndUpdateAssets(request, entity);



                await _receiptCommandsService.CalculateTotalItemPrice(entity);
                //اگر درخواست دریافت کالا باشد و درخواست دهنده تغییر کرده باشد.
                await EditAssistsDebit(entity);
                _receiptRepository.Update(entity);
                await _receiptRepository.SaveChangesAsync();


                //update RemainQuantity
                if (entity.DocumentStauseBaseValue != (int)DocumentStateEnam.Temp)
                    foreach (var item in request.ReceiptDocumentItems)
                    {
                        var RequestNo = (int)DocumentStateEnam.requestBuy == entity.DocumentStauseBaseValue || (int)DocumentStateEnam.requestReceive == entity.DocumentStauseBaseValue ? entity.DocumentNo.ToString() : entity.RequestNo;



                        if (entity.DocumentStauseBaseValue == (int)DocumentStateEnam.requestReceive)
                        {
                            await _procedureCallService.CalculateRemainQuantityRequestCommodityByPerson(RequestNo, item.CommodityId, item.Id);
                        }
                        else
                        {
                            await _procedureCallService.CalculateRemainQuantityRequest(RequestNo, item.CommodityId);
                        }
                        //اگر شماره درخواست  تغییر کرده باشد.
                        if (OldRequestNo != entity.RequestNo)
                        {
                            await _procedureCallService.CalculateRemainQuantityRequest(OldRequestNo, item.CommodityId);
                        }

                        await AddWarehouseLayout(entity, item.CommodityId, item.Quantity, item.Id);
                    }

                var model = await _receiptQueries.GetById(request.Id);
                return ServiceResult<ReceiptQueryModel>.Success(model);
            }
            private async Task EditAssistsDebit(Receipt receipt)
            {
                if ((int)DocumentStateEnam.requestReceive == receipt.DocumentStauseBaseValue)
                {
                    var exitReceipt = await _context.DocumentHeads.Where(a => a.RequestNo == receipt.DocumentNo.ToString() && a.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmountLeave).FirstOrDefaultAsync();
                    if (exitReceipt != null)
                    {
                        exitReceipt.DebitAccountReferenceGroupId = receipt.DebitAccountReferenceGroupId;
                        exitReceipt.DebitAccountReferenceId = receipt.DebitAccountReferenceId;
                    }
                    foreach (var item in receipt.Items)
                    {
                        var exitReceiptAssets = await _context.PersonsDebitedCommodities.Where(a => a.DocumentItemId == item.Id).ToListAsync();
                        foreach (var ass in exitReceiptAssets)
                        {
                            ass.DebitAccountReferenceId = receipt.DebitAccountReferenceId;
                            ass.DebitAccountReferenceGroupId = receipt.DebitAccountReferenceGroupId;
                            _PersonsDebitedCommoditiesRepository.Update(ass);
                        };
                    }

                }
            }

            private async Task DebitAndCredit(UpdateReceiptCommand request, Warehouse warehouse, CodeVoucherGroup CodeVoucher, Receipt entity)
            {
                //-----------------انبار بدهکار--------------------------------

                entity.DebitAccountHeadId = warehouse.AccountHeadId;
                //------------------تامین کننده بستانکار----------------------
                entity.CreditAccountReferenceId = request.CreditAccountReferenceId;
                entity.CreditAccountReferenceGroupId = request.CreditAccountReferenceGroupId;

                entity.DebitAccountReferenceId = request.DebitAccountReferenceId;
                entity.DebitAccountReferenceGroupId = request.DebitAccountReferenceGroupId;
                entity = await _receiptCommandsService.UpdateImportPurchaseReceipt(entity);
                entity.CreditAccountHeadId = entity.IsImportPurchase == true ? ConstantValues.AccountReferenceGroup.AccountHeadExternalProvider : CodeVoucher.DefultCreditAccountHeadId;

            }

            private async Task InsertAndUpdateDocumentItem(UpdateReceiptCommand request, Domain.Receipt entity, Domain.Receipt OldReceipt, int CurrencyBaseId)
            {

                foreach (var documentItem in request.ReceiptDocumentItems)
                {

                    var item = await _documentItemRepository.Find(documentItem.Id);

                    if (item != null)
                    {
                        if(item.CommodityId!= documentItem.CommodityId)
                        {
                            await DeleteWarehouseLayout(OldReceipt, item.CommodityId, item.Quantity, item.Id);
                        }
                       
                        await AddWarehouseLayout(OldReceipt, documentItem.CommodityId, documentItem.Quantity, item.Id);
                        UpdateDocumentItems(documentItem, item);

                    }
                    else
                    {

                        item = InsertDocumentItems(entity, CurrencyBaseId, documentItem);
                    }
                    if(entity.DocumentStauseBaseValue<40)
                    {
                        await _receiptCommandsService.GetPriceBuyItems(documentItem.CommodityId, entity.WarehouseId, null, documentItem.Quantity, item);
                    }

                   
                }
            }

            private DocumentItem InsertDocumentItems(Receipt entity, int CurrencyBaseId, ReceiptDocumentItemUpdate documentItem)
            {
                var InvoiceItem = new DocumentItem();

                var newItems = _mapper.Map<DocumentItem>(documentItem);
                newItems.CurrencyBaseId = CurrencyBaseId;
                newItems.CurrencyPrice = 1;
                newItems.RemainQuantity = newItems.Quantity;
                entity.AddItem(newItems);
                return newItems;
            }

            private void UpdateDocumentItems(ReceiptDocumentItemUpdate documentItem, DocumentItem item)
            {
                item.DocumentMeasureId = documentItem.DocumentMeasureId;
                item.MainMeasureId = documentItem.MainMeasureId;
                item.Quantity = documentItem.Quantity;
                item.UnitPrice = documentItem.UnitPrice;
                item.CurrencyBaseId = documentItem.CurrencyBaseId;
                item.Description = documentItem.Description;
                item.ProductionCost = documentItem.ProductionCost;
                item.SecondaryQuantity = documentItem.SecondaryQuantity;
                item.CommodityId = documentItem.CommodityId;
                item.IsWrongMeasure = documentItem.IsWrongMeasure;
                _documentItemRepository.Update(item);
            }



            private async Task DeleteItems(UpdateReceiptCommand request, Receipt receipt)
            {
                var documentList = await _documentItemRepository.GetAll().Where(a => a.DocumentHeadId == request.Id).ToListAsync();
                var ListId = request.ReceiptDocumentItems.Where(a => a.Id > 0).Select(a => a.Id).ToList();

                var DeleteList = documentList.Where(a => !ListId.Contains(a.Id)).ToList();

                await DeleteItemsForRequest(DeleteList);

                foreach (var item in DeleteList)
                {

                    _documentItemRepository.Delete(item);
                    await DeleteWarehouseLayout(receipt, item.CommodityId, item.Quantity, item.Id);

                }

                await _documentItemRepository.SaveChangesAsync();
            }

            //اگر از روی این رسید خروج زده شده باشد 
            private async Task DeleteItemsForRequest(List<DocumentItem> DeleteList)
            {

                var RequestDocumentItemIdList = await _documentItemRepository.GetAll().Where(a => DeleteList.Select(b => b.Id).ToList().Contains(a.Id) && a.RemainQuantity == a.Quantity).ToListAsync();
                foreach (var item in RequestDocumentItemIdList)
                {

                    _documentItemRepository.Delete(item);
                }
            }

            private async Task UpdateDocumentHeadExtend(UpdateReceiptCommand request)
            {
                var _documentHeadExtend = await _documentHeadExtendRepository.GetAll().Where(a => a.DocumentHeadId == request.Id).FirstOrDefaultAsync();
                if (_documentHeadExtend != null)
                {
                    _documentHeadExtend.RequesterReferenceId = request.RequesterReferenceId;
                    _documentHeadExtend.FollowUpReferenceId = request.RequesterReferenceId;

                    _documentHeadExtendRepository.Update(_documentHeadExtend);
                    await _documentHeadExtendRepository.SaveChangesAsync();


                }
                else if (request.RequesterReferenceId != null)
                {
                    var documentHeadExtend = new DocumentHeadExtend();

                    documentHeadExtend.RequesterReferenceId = request.RequesterReferenceId;
                    documentHeadExtend.FollowUpReferenceId = request.RequesterReferenceId;
                    documentHeadExtend.DocumentHeadId = request.Id;
                    _documentHeadExtendRepository.Insert(documentHeadExtend);
                    await _documentHeadExtendRepository.SaveChangesAsync();

                }

            }
            //---------------------حذف موجودی فعلی------------------
            private async Task DeleteWarehouseLayout(Receipt OldReceipt, int CommodityId, double Quantity, int receiptItemsId)
            {
                //رسیدهای سند شده و رسیدهای جابه جایی انبار را نتوان کد کالا را تغییر داد.
                if (OldReceipt.ViewId == 122 )
                {
                    return;
                }
                if (
                    (int)DocumentStateEnam.requestBuy == OldReceipt.DocumentStauseBaseValue ||
                    (int)DocumentStateEnam.requestReceive == OldReceipt.DocumentStauseBaseValue ||
                    (int)DocumentStateEnam.Temp == OldReceipt.DocumentStauseBaseValue)
                {

                    return;
                }
                var history = await _context.WarehouseHistories.Where(a => a.DocumentItemId == receiptItemsId && !a.IsDeleted).FirstOrDefaultAsync();
                if (history == null)
                {
                    return;
                }
                
                await _warehouseLayoutCommandsService.DeleteWarehouseHistory(receiptItemsId, CommodityId);
                

            }
            //-----------------افزایش موجودی جدید--------------------
            private async Task AddWarehouseLayout(Receipt NewReceipt, int CommodityId, double Quantity, int receiptItemsId)
            {
                //رسیدهای سند شده و رسیدهای جابه جایی انبار را نتوان کد کالا را تغییر داد.
                if (NewReceipt.ViewId == 122)
                {
                    return;
                }
                if (
                    (int)DocumentStateEnam.requestBuy == NewReceipt.DocumentStauseBaseValue ||
                    (int)DocumentStateEnam.requestReceive == NewReceipt.DocumentStauseBaseValue ||
                    (int)DocumentStateEnam.Temp == NewReceipt.DocumentStauseBaseValue)
                {

                    return;
                }

                int historyMode = ((int)DocumentStateEnam.Leave == NewReceipt.DocumentStauseBaseValue || (int)DocumentStateEnam.invoiceAmountLeave == NewReceipt.DocumentStauseBaseValue || (int)DocumentStateEnam.registrationAccountingLeave == NewReceipt.DocumentStauseBaseValue) ? -1 : 1;
                //----------------اولین محلی که پیدا شد--------------------------
                WarehouseLayout layouts = await _warehouseLayoutCommandsService.FindLayout(NewReceipt.WarehouseId, CommodityId);
                var WarehouseLayoutQuantity = await _context.WarehouseLayoutQuantities.Where(a => a.WarehouseLayoutId == layouts.Id && a.CommodityId == CommodityId).FirstOrDefaultAsync();
                var stock = await _context.WarehouseStocks.Where(a => a.CommodityId == CommodityId && a.WarehousId == NewReceipt.WarehouseId).FirstOrDefaultAsync();

                await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(CommodityId, Quantity, layouts.Id, receiptItemsId, historyMode);
                await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, WarehouseLayoutQuantity, layouts.Id);
                await _warehouseLayoutCommandsService.InsertStock(NewReceipt.WarehouseId, CommodityId, Quantity, historyMode, stock);

            }
            private async Task InsertAndUpdateAssets(UpdateReceiptCommand request, Domain.Receipt receipt)
            {
                foreach (var item in request.ReceiptDocumentItems)
                {
                    if (item.Assets == null)
                        continue;

                    foreach (var serial in item.Assets.AssetsSerials)
                    {

                        //جدید
                        if (serial.Id == 0)
                        {
                            InsertAssets(receipt, item, serial);
                        }//ویرایش
                        if (serial.Id > 0)
                        {
                            await UpdateAssets(receipt, item, serial);

                        }

                    }
                    //------------------حذف آیتم های پاک شده---------------------
                    await DeleteAssets(receipt, item);


                }

            }

            private async Task DeleteAssets(Receipt receipt, ReceiptDocumentItemUpdate item)
            {
                var serialListId = item.Assets.AssetsSerials.Select(a => a.Id);
                var serialListDelete = await _assetsRepository.GetAll().Where(a => a.DocumentHeadId == receipt.Id && a.CommodityId == item.CommodityId && !serialListId.Contains(a.Id) && !a.IsDeleted).ToListAsync();
                foreach (var serial in serialListDelete)
                {
                    _assetsRepository.Delete(serial);
                }
            }

            private async Task UpdateAssets(Receipt receipt, ReceiptDocumentItemUpdate item, AssetsSerialModel serial)
            {
                var assets = await _assetsRepository.Find(Convert.ToInt32(serial.Id));
                if (assets == null)
                {
                    return;
                }

                assets.AssetGroupId = item.Assets.AssetGroupId;
                assets.WarehouseId = receipt.WarehouseId;
                assets.DepreciationTypeBaseId = item.Assets.DepreciationTypeBaseId;
                assets.DocumentDate = receipt.DocumentDate;
                assets.MeasureId = item.MainMeasureId;
                assets.CommodityId = item.CommodityId;
                assets.IsActive = true;
                assets.DocumentHeadId = receipt.Id;
                assets.AssetSerial = serial.Serial;
                assets.CommoditySerial = serial.CommoditySerial;
                assets.DocumentItemId = item.Id;
                _assetsRepository.Update(assets);

            }

            private void InsertAssets(Receipt receipt, ReceiptDocumentItemUpdate item, AssetsSerialModel serial)
            {
                var assets = new Assets()
                {
                    AssetGroupId = item.Assets.AssetGroupId,
                    WarehouseId = receipt.WarehouseId,
                    DepreciationTypeBaseId = item.Assets.DepreciationTypeBaseId,
                    DocumentDate = receipt.DocumentDate,
                    MeasureId = item.MainMeasureId,
                    CommodityId = item.CommodityId,
                    IsActive = true,
                    DocumentHeadId = receipt.Id,
                    AssetSerial = serial.Serial,
                    CommoditySerial = serial.CommoditySerial,
                    DocumentItemId = item.Id,
                };

                _assetsRepository.Insert(assets);
            }
        }
    }
}


