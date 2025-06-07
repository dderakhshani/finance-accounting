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
using Eefa.Invertory.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class UpdateLeavingMaterialWarehouseCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<UpdateLeavingMaterialWarehouseCommand>, ICommand
    {
        public int Id { get; set; } = default!;
        public int WarehouseId { get; set; } = default!;
        /// <description>
        /// توضیحات سند
        ///</description>
        public string DocumentDescription { get; set; }
        /// تاریخ سند
        ///</description>
        public DateTime DocumentDate { get; set; } = default!;
        public string Tags { get; set; }
        public int? DebitAccountHeadId { get; set; } = default!;
        public int? CreditAccountHeadId { get; set; } = default!;
        public int? DebitAccountReferenceId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; } = default!;
        public int? CreditAccountReferenceId { get; set; } = default!;
        public int? CreditAccountReferenceGroupId { get; set; } = default!;
        public Nullable<bool> IsDocumentIssuance { get; set; } = default!;
        public ICollection<UpdateLeavingMaterailItem> ReceiptDocumentItems { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateLeavingMaterialWarehouseCommand, Domain.Receipt>()
                .IgnoreAllNonExisting();
        }



    }
    public class UpdateLeavingMaterailItem : IMapFrom<DocumentItem>
    {
        public int Id { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public int DocumentHeadId { get; set; } = default!;
        public int MainMeasureId { get; set; } = default!;
        public int? BomValueHeaderId { get; set; }
        public double Quantity { get; set; } = default!;
        public int DocumentMeasureId { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsWrongMeasure { get; set; } = default!;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateLeavingMaterailItem, DocumentItem>()
                .IgnoreAllNonExisting();
        }
    }
    public class UpdateLeavingMaterialWarehouseCommandHandler : IRequestHandler<UpdateLeavingMaterialWarehouseCommand, ServiceResult<Domain.Receipt>>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRepository<BaseValue> _baseValueRepository;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<DocumentItemsBom> _documentItemsBomRepository;
        private readonly IRepository<CodeVoucherGroup> _codeVoucherGroupRepository;
        private readonly IRepository<DocumentHeadExtend> _documentHeadExtendRepository;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;

        int FromWarehouseId = 0; //انبار خروجی 
        int ToWarehouseId = 0; //انبار ورودی
        Domain.Receipt ToReceipt;
        Domain.Receipt FromReceipt;
        Domain.WarehouseLayout Fromlayous;
        Domain.WarehouseLayout Tolayous;
        CodeVoucherGroup codeVoucherGroup;
        List<DocumentItem> documentItems = new List<DocumentItem>();
        int currency;
        bool IsEnterWarehouse;

        public UpdateLeavingMaterialWarehouseCommandHandler(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , IReceiptRepository receiptRepository
            , ICurrentUserAccessor currentUserAccessor
            , IRepository<Document> documentRepository
            , IRepository<BaseValue> baseValueRepository
            , IProcedureCallService procedureCallService
            , IReceiptCommandsService receiptCommandsService
            , IRepository<DocumentItem> documentItemRepository
            , IRepository<DocumentItemsBom> documentItemsBomRepository
            , IRepository<CodeVoucherGroup> codeVoucherGroupRepository
            , IRepository<DocumentHeadExtend> documentHeadExtendRepository
            , IWarehouseLayoutCommandsService warehouseLayoutCommandsService
            )

        {
            _mapper = mapper;
            _context = context;
            _receiptRepository = receiptRepository;
            _documentRepository = documentRepository;
            _currentUserAccessor = currentUserAccessor;
            _baseValueRepository = baseValueRepository;
            _procedureCallService = procedureCallService;
            _receiptCommandsService = receiptCommandsService;
            _documentItemRepository = documentItemRepository;
            _documentItemsBomRepository = documentItemsBomRepository;
            _codeVoucherGroupRepository = codeVoucherGroupRepository;
            _documentHeadExtendRepository = documentHeadExtendRepository;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;

        }
        public async Task<ServiceResult<Domain.Receipt>> Handle(UpdateLeavingMaterialWarehouseCommand request, CancellationToken cancellationToken)
        {

            if (request.DocumentDate > DateTime.UtcNow)
            {
                throw new ValidationError("تاریخ انتخابی برای زمان آینده نمی تواند باشد");
            }
            if (!request.ReceiptDocumentItems.Any())
            {
                throw new ValidationError("هیچ کالایی انتخاب نشده است");
            }
            ToReceipt = await _receiptRepository.Find(request.Id);
            codeVoucherGroup = await _context.CodeVoucherGroups.Where(a => a.Id == ToReceipt.CodeVoucherGroupId).FirstOrDefaultAsync();
            //اگر جابه جایی انبار اتفاق افتاده باشد بدست آوردن انباری که کالا از آن خارج شده است
            if (codeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.ChangeMaterialWarhouse || codeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.ProductReceiptWarehouse)
            {
                FromWarehouseId = await _context.Warehouses.Where(a => a.AccountReferenceId == ToReceipt.CreditAccountReferenceId).Select(a => a.Id).FirstOrDefaultAsync();
                IsEnterWarehouse = true;
            }
            else
            {
                FromWarehouseId = 0;
                //----ورودی نداریم
                IsEnterWarehouse = false;
            }

            if (ToReceipt.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmount)
            {
                //به انباری وارد شده است
                ToWarehouseId = ToReceipt.WarehouseId;
            }
            //جابه جایی نداریم فقط خروج از انبار داریم.
            //از انباری خارج شده است
            if (ToReceipt.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmountLeave)
            {

                FromWarehouseId = ToReceipt.WarehouseId;
                FromReceipt = ToReceipt;
            }
            //اگر انتقال بین انبارها داشته باشیم ، انباری که از آن خارج شده است را پیدا کنیم و مقدار آیتم های آن را ویرایش کنیم.
            else
            {
                FromReceipt = await _receiptRepository.GetAll().Where(a => a.ParentId == ToReceipt.Id).FirstOrDefaultAsync();
                if (FromReceipt != null)
                    FromWarehouseId = FromReceipt.WarehouseId;
            }

            Fromlayous = await _context.WarehouseLayouts.Where(_a => _a.WarehouseId == FromWarehouseId && _a.LastLevel == true).FirstOrDefaultAsync();
            if (Fromlayous == null && FromWarehouseId > 0)
            {
                throw new ValidationError("برای انبار تحویل دهنده انتخاب شده در این سند هیچ موقعیت بندی آخرین سطح تعریف نشده است");
            }

            Tolayous = await _context.WarehouseLayouts.Where(_a => _a.WarehouseId == ToWarehouseId && _a.LastLevel == true).FirstOrDefaultAsync();
            if (Tolayous == null && ToWarehouseId > 0)
            {
                throw new ValidationError("برای انبار تحویل گیرنده انتخاب شده در این سند هیچ موقعیت بندی آخرین سطح تعریف نشده است");
            }
            currency = await _baseValueRepository.GetAll().Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();

            if (currency == null)
            {
                throw new ValidationError("واحد ارز ریالی تعریف نشده است");
            }
            
            ToReceipt = await Update(request, Fromlayous, Tolayous, cancellationToken);
            

            return ServiceResult<Domain.Receipt>.Success(ToReceipt);
        }

        private async Task<Domain.Receipt> Update(
            UpdateLeavingMaterialWarehouseCommand request,
            Domain.WarehouseLayout Fromlayous,
            Domain.WarehouseLayout Tolayous,

            CancellationToken cancellationToken)
        {

            documentItems = await _context.DocumentItems.Where(a => a.DocumentHeadId == request.Id).ToListAsync();
            //--------------------ویرایش سند ورود از انبار تحویل گیرنده-------------------------
            if (ToWarehouseId > 0)
            {
                ToReceipt = _mapper.Map<UpdateLeavingMaterialWarehouseCommand, Domain.Receipt>(request, ToReceipt);
                //------------------حذف آیتم های پاک شده---------------------
                await DeleteItems(request);
                //------------------ویرایش آیتم های موجود--------------------
                await UpdateDocumentItemToWarehouse(request);
                //------------------ویرایش سند-------------------------------
                ToReceipt = _receiptCommandsService.ConvertTagArray(request.Tags, ToReceipt);

                await UpdateReceipt(ToReceipt, ToReceipt.WarehouseId);

            }
            //--------------------از انباری کالا خارج شده است------------------------------------
            //باید کالاهای خارج شده را به انبار برگردانیم و دوباره با مقدار جدید خارج کنیم.-----
            if (FromWarehouseId > 0 && FromReceipt != null)
            {
                //رسید محصول
                if (codeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.ProductReceiptWarehouse)
                {
                    await UpdateBomValuesWarehouse(request, Fromlayous);
                }
                //انتقال کالا
                if (codeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.ChangeMaterialWarhouse || codeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.RemoveMaterialWarhouse)
                {

                    FromReceipt = _receiptCommandsService.ConvertTagArray(request.Tags, FromReceipt);
                    FromReceipt.DocumentDate = request.DocumentDate;
                    FromReceipt.IsDocumentIssuance = request.IsDocumentIssuance;
                    await UpdateReceipt(FromReceipt, FromWarehouseId);
                    //------------------حذف آیتم های قیلی---------------------
                    await DeleteItemsFromWarehouse(FromReceipt.Id);
                    //-----------------ثبت آیتم های جدید----------------------
                    await InsertDocumentItems(request, FromReceipt, currency, (int)(WarehouseHistoryMode.Exit), Fromlayous.Id);
                }
            }
            await _documentItemRepository.SaveChangesAsync();

            //-------------- receipt.TotalItemPrice--------------------------------
            await _receiptCommandsService.CalculateTotalItemPrice(ToReceipt);
            _receiptRepository.Update(ToReceipt);

            if (FromReceipt != null)
            {
                await _receiptCommandsService.CalculateTotalItemPrice(FromReceipt);
                _receiptRepository.Update(FromReceipt);
            }
               
            await _receiptRepository.SaveChangesAsync();


            return ToReceipt;
        }

        private async Task UpdateBomValuesWarehouse(UpdateLeavingMaterialWarehouseCommand request, WarehouseLayout Fromlayous)
        {

            foreach (var item in request.ReceiptDocumentItems)
            {
                if (item.Id > 0)//ویرایش آیتم های قبلی
                {
                    var oldDocumentItems = documentItems.Where(a => a.Id == item.Id && !a.IsDeleted).FirstOrDefault();

                    //اگر دارای فرمول ساخت باشد ، لازم است که مواد تشکیل دهنده را یافت .و آن مواد را از انبار خارج کرد
                    if (item.BomValueHeaderId > 0 && oldDocumentItems != null)
                    {
                        //اگر فرمول ساخت تغییر نکرده باشد
                        if (oldDocumentItems.BomValueHeaderId == item.BomValueHeaderId && oldDocumentItems.Quantity != item.Quantity)
                        {
                            await UpdateBomValuesCalculate(item);
                        }
                        //اگر فرمول ساخت تغییر کرده باشد.-------------------
                        else if (oldDocumentItems.BomValueHeaderId != item.BomValueHeaderId)
                        {
                            //حذف اقلام سابق 
                            await DeleteBoms(item);
                            //افزودن اقلام جدید
                            await AddBomValuesCalculate(item);
                        };

                    }
                    else
                    {
                        //اگر قبلا فرمول ساخت داشته باشد و موقع ویرایش آن را برداشته باشند مقدارهای قدیمی حذف شود.
                        await DeleteBoms(item);

                    }
                    var documentItem =await _context.DocumentItems.Where(a => a.Id == item.Id).FirstOrDefaultAsync();
                    await _receiptCommandsService.GetPriceBuyItems(documentItem.CommodityId, Convert.ToInt32(Fromlayous.WarehouseId), documentItem.BomValueHeaderId > 0 ? item.Id : null, item.Quantity, documentItem);
                }
                else
                {

                    //-----------------ثبت آیتم های جدید----------------------
                    await AddItems(FromReceipt, item);
                    //برای بدست آوردن فرمول ساخت به آیدی آیتمی که به انبار وارد شده است ، نیاز داریم.
                    var DocumentItemsId =await _context.DocumentItems.Where(a => a.DocumentHeadId == ToReceipt.Id && a.CommodityId == item.CommodityId && !a.IsDeleted).Select(a => a.Id).FirstOrDefaultAsync();
                    item.Id = DocumentItemsId;
                    //افزودن اقلام جدید

                    await AddBomValuesCalculate(item);
                }
            }

        }

        //--------------------------------------------------------------------
        //---------در هنگام ورودی به انبار------------------------------------
        private async Task UpdateDocumentItemToWarehouse(UpdateLeavingMaterialWarehouseCommand request)
        {

            foreach (var documentItem in request.ReceiptDocumentItems)
            {
                if (documentItem.Id > 0)
                {
                    //------------------ویرایش آیتم های موجود---------------------
                    var item = await _documentItemRepository.GetAll().Where(a => a.Id == documentItem.Id && a.CommodityId == documentItem.CommodityId).FirstOrDefaultAsync();
                    if (item != null)
                    {

                        //-----------------------ویرایش موجودی انبار------------------
                        var Quantity = (documentItem.Quantity - item.Quantity);

                        // اگر رسید از نوع ورودی باشد ظرفیت باید از انبار کسر شود
                        if (!IsEnterWarehouse)
                        {
                            Quantity = -1 * Quantity;
                        }
                        //اگر بزرگ تر از صفر باشد یعنی ورود جدید داریم و اگر کوچکتر از صفر باشد یعنی خروج داریم
                        var Status = documentItem.Quantity >= item.Quantity ? (int)(WarehouseHistoryMode.Enter) : (int)(WarehouseHistoryMode.Exit);
                        await UpdateWarehouseLayout(ToWarehouseId, Tolayous.Id, item.CommodityId, Math.Abs(Quantity), Status, documentItem.Id, documentItem.Quantity);

                    }
                    else //;کالا تغییر کرده است 
                    {
                        item = await _documentItemRepository.GetAll().Where(a => a.Id == documentItem.Id).FirstOrDefaultAsync();
                        //-----------------------کاهش موجودی کالای اولیه------------------
                        var documentItemId = await UpdateAnddDeleteWarehouseLayout(ToWarehouseId, Tolayous.Id, item.CommodityId, item.Quantity, (int)(WarehouseHistoryMode.Exit), item.Id, 0);

                        //-----------------------افزایش موجودی کالای جدید-----------------
                        await UpdateWarehouseLayout(ToWarehouseId, Tolayous.Id, documentItem.CommodityId, documentItem.Quantity, (int)(WarehouseHistoryMode.Enter), documentItem.Id, documentItem.Quantity);
                    }

                    _mapper.Map<UpdateLeavingMaterailItem, DocumentItem>(documentItem, item);


                    await _receiptCommandsService.GetPriceBuyItems(documentItem.CommodityId, ToWarehouseId, documentItem.BomValueHeaderId > 0 ? item.Id : null, documentItem.Quantity, item);

                    item.Description = documentItem.Description;
                    _documentItemRepository.Update(item);
                }
                else
                {
                    //-----------------ثبت آیتم های جدید----------------------

                    var items = await AddItems(ToReceipt, documentItem);
                    await UpdateWarehouseLayout(ToReceipt.WarehouseId, Tolayous.Id, items.CommodityId, items.Quantity, (int)(WarehouseHistoryMode.Enter), items.Id, items.Quantity);

                }
            }
            await _documentItemRepository.SaveChangesAsync();
        }
        private async Task AddBomValuesCalculate(UpdateLeavingMaterailItem items)
        {

            var bomsValues =await _context.BomValueView.Where(a => a.BomValueHeaderId == items.BomValueHeaderId).ToListAsync();
            foreach (var Values in bomsValues)
            {
                var layouts =await _context.WarehouseLayouts.Where(_a => _a.WarehouseId == Values.BomWarehouseId && _a.LastLevel == true).FirstOrDefaultAsync();
                DocumentItemsBom BomItem = await _receiptCommandsService.AddDocumentItemsBom(layouts.Id, Values.BomWarehouseId, currency, items.Quantity, items.CommodityId, items.Id, Values);
                if (await _documentItemsBomRepository.SaveChangesAsync() > 0)
                {
                    //--------------------کاهش ظرفیت فعلی در مکان-----------------
                    var Quantity = Math.Round(items.Quantity * Values.Value, 2, MidpointRounding.AwayFromZero);
                    await UpdateWarehouseLayout(Values.BomWarehouseId, layouts.Id, BomItem.CommodityId, Quantity, (int)(WarehouseHistoryMode.Exit), items.Id, BomItem.Quantity);
                }

            }


        }

        private async Task DeleteBoms(UpdateLeavingMaterailItem items)
        {
            var BomItems = await _documentItemsBomRepository.GetAll().Where(b => b.DocumentItemsId == items.Id && !b.IsDeleted).ToListAsync();
            if (!BomItems.Any())
            {
                return;
            }
            foreach (var bom in BomItems)
            {
                _documentItemsBomRepository.Delete(bom);
                if (await _documentItemsBomRepository.SaveChangesAsync() > 0)
                {
                    //-----------------------افزایش موجودی انبار------------------
                    var documentItemId = await UpdateAnddDeleteWarehouseLayout(ToWarehouseId, Tolayous.Id, bom.CommodityId, bom.Quantity, (int)(WarehouseHistoryMode.Enter), items.Id, bom.Quantity);

                };
            }
        }

        private async Task<bool> UpdateBomValuesCalculate(UpdateLeavingMaterailItem item)
        {

            var bomsValues =await _context.BomValueView.Where(a => a.BomValueHeaderId == item.BomValueHeaderId).ToListAsync();
            foreach (var Values in bomsValues)
            {
                DocumentItemsBom BomItem = await _documentItemsBomRepository.GetAll().Where(b => b.CommodityId == Values.UsedCommodityId && b.DocumentItemsId == item.Id && !b.IsDeleted).FirstOrDefaultAsync();
                if (BomItem == null)
                {
                    return false;//فرمول ساخت جدید است 
                }
                if (BomItem != null)
                {
                    var layouts =await _context.WarehouseLayouts.Where(_a => _a.WarehouseId == Values.BomWarehouseId && _a.LastLevel == true).FirstOrDefaultAsync();

                    var _Quantity = BomItem.Quantity;

                    BomItem.Quantity = Math.Round(Values.Value * item.Quantity, 2, MidpointRounding.AwayFromZero);

                    await _receiptCommandsService.GetPriceBuyBom(Values.UsedCommodityId, Values.BomWarehouseId, BomItem.Quantity, BomItem);

                    _documentItemsBomRepository.Update(BomItem);

                    if (await _documentItemsBomRepository.SaveChangesAsync() > 0)
                    {
                        //--------------------کاهش ظرفیت فعلی در مکان-----------------
                        await UpdateWarehouseLayout(Values.BomWarehouseId, layouts.Id, BomItem.CommodityId, BomItem.Quantity, (int)(WarehouseHistoryMode.Exit), item.Id, BomItem.Quantity);
                    }
                }

            }
            return true;
        }

        private async Task UpdateReceipt(Receipt receipt, int WarehouseId)
        {
            
            _receiptRepository.Update(receipt);
            await _receiptRepository.SaveChangesAsync();
        }

        

        private async Task<int> InsertDocumentItems(UpdateLeavingMaterialWarehouseCommand request, Domain.Receipt receipt, int currency, int HistoryMode, int layoutsId)
        {

            foreach (var items in request.ReceiptDocumentItems)
            {
                var documentItem = await AddItems(receipt, items);
                
                await UpdateWarehouseLayout(receipt.WarehouseId, layoutsId, documentItem.CommodityId, documentItem.Quantity, HistoryMode, documentItem.Id, documentItem.Quantity);
            }

            return 1;
        }

        private async Task<DocumentItem> AddItems(Receipt receipt, UpdateLeavingMaterailItem items)
        {
            var documentItem = new DocumentItem();
            documentItem.DocumentHeadId = receipt.Id;
            documentItem.CurrencyBaseId = currency;
            documentItem.CurrencyPrice = 1;
            documentItem.DocumentMeasureId = items.DocumentMeasureId;
            documentItem.MainMeasureId = items.MainMeasureId;
            documentItem.Quantity = items.Quantity;
            documentItem.Description = items.Description;
            documentItem.CommodityId = items.CommodityId;
            documentItem.RemainQuantity = items.Quantity;
            documentItem.IsWrongMeasure = items.IsWrongMeasure;
            documentItem.BomValueHeaderId = items.BomValueHeaderId;
            documentItem.YearId = _currentUserAccessor.GetYearId();
            documentItem.ModifiedAt = DateTime.UtcNow;
            documentItem.CreatedAt = DateTime.UtcNow;
            documentItem.CreatedById = _currentUserAccessor.GetId();
            documentItem.IsDeleted = false;
            documentItem.OwnerRoleId = _currentUserAccessor.GetRoleId();
            await _receiptCommandsService.GetPriceBuyItems(items.CommodityId, receipt.WarehouseId, items.BomValueHeaderId > 0 ? items.Id : null, items.Quantity, documentItem);

            _documentItemRepository.Insert(documentItem);
            await _documentItemRepository.SaveChangesAsync();
            
            return documentItem;
        }

      

        //------------------حذف آیتم های پاک شده---------------------
        private async Task DeleteItems(UpdateLeavingMaterialWarehouseCommand request)
        {
            var documentList = await _documentItemRepository.GetAll().Where(a => a.DocumentHeadId == request.Id).ToListAsync();
            var ListId = request.ReceiptDocumentItems.Where(a => a.Id > 0).Select(a => a.Id).ToList();

            var DeleteList = documentList.Where(a => !ListId.Contains(a.Id)).ToList();


            foreach (var item in DeleteList)
            {

                _documentItemRepository.Delete(item);
                if (await _documentItemRepository.SaveChangesAsync() > 0)
                {
                    //-----------------------کاهش موجودی انبار------------------
                    var documentItemId = await UpdateAnddDeleteWarehouseLayout(ToWarehouseId, Tolayous.Id, item.CommodityId, item.Quantity, (int)(WarehouseHistoryMode.Exit), item.Id, 0);

                };

            }

        }
        private async Task DeleteItemsFromWarehouse(int DocumentHeadId)
        {
            var documentList = await _documentItemRepository.GetAll().Where(a => a.DocumentHeadId == DocumentHeadId && !a.IsDeleted).ToListAsync();

            foreach (var item in documentList)
            {

                _documentItemRepository.Delete(item);
                if (await _documentItemRepository.SaveChangesAsync() > 0)
                {

                    //-----------------------افزایش موجودی انبار------------------
                    var documentItemId = await UpdateAnddDeleteWarehouseLayout(FromWarehouseId, Fromlayous.Id, item.CommodityId, item.Quantity, (int)(WarehouseHistoryMode.Enter), item.Id, 0);


                };

            }

        }

        //======================================================================
        //-----------------افزایش و کاهش ظرفیت فعلی در مکان--------------------
        private async Task<int> UpdateWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int historyMode, int documentItemId, double request_Quantity)
        {
            var WarehouseLayoutQuantity =await _context.WarehouseLayoutQuantities.Where(a => a.WarehouseLayoutId == WarehouseLayoutId && a.CommodityId == CommodityId).FirstOrDefaultAsync();
            var stock = await _context.WarehouseStocks.Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();


            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(CommodityId, request_Quantity, WarehouseLayoutId, documentItemId, historyMode);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, WarehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, historyMode, stock);

            return documentItemId;


        }
        private async Task<int> UpdateAnddDeleteWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int historyMode, int documentItemId, double request_Quantity)
        {
            var WarehouseLayoutQuantity =await _context.WarehouseLayoutQuantities.Where(a => a.WarehouseLayoutId == WarehouseLayoutId && a.CommodityId == CommodityId).FirstOrDefaultAsync();
            var stock = await _context.WarehouseStocks.Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();

            //حذف سابقه های ایجاد شده در انبار
            await _warehouseLayoutCommandsService.DeleteWarehouseHistory(documentItemId, CommodityId);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, WarehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, historyMode, stock);

            return documentItemId;


        }


    }
}