using System;
using System.Collections.Generic;
using System.Data;
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
using Eefa.Invertory.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Inventory.Application
{
    public class AddLeavingMaterialWarehouseCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<AddLeavingMaterialWarehouseCommand>, ICommand
    {
        public int CodeVoucherGroupId { get; set; } = default!;
        public int FromWarehouseId { get; set; } = default!;
        public int ToWarehouseId { get; set; } = default!;

        /// <description>
        /// توضیحات سند
        ///</description>
        public string DocumentDescription { get; set; }

        /// <description>
        /// دستی
        ///</description>
        public bool IsManual { get; set; } = default!;
        /// تاریخ سند
        ///</description>
        public DateTime DocumentDate { get; set; } = default!;

        public string Tags { get; set; }
        public int? ViewId { get; set; }
        public int? DebitAccountHeadId { get; set; } = default!;
        public int? CreditAccountHeadId { get; set; } = default!;
        public int? DebitAccountReferenceId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; } = default!;
        public int? CreditAccountReferenceId { get; set; } = default!;
        public int? CreditAccountReferenceGroupId { get; set; } = default!;
        public Nullable<bool> IsDocumentIssuance { get; set; } = default!;
        public ICollection<LeavingMaterialItem> ReceiptDocumentItems { get; set; }



        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddLeavingMaterialWarehouseCommand, Domain.Receipt>()
                .IgnoreAllNonExisting();
        }



    }
    public class LeavingMaterialItem : IMapFrom<DocumentItem>
    {
        public int CommodityId { get; set; } = default!;
        public int MainMeasureId { get; set; }
        public int? BomValueHeaderId { get; set; }
        public double Quantity { get; set; } = default!;
        public int DocumentMeasureId { get; set; }
        public string Description { get; set; }
        public bool IsWrongMeasure { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LeavingMaterialItem, DocumentItem>()
                .IgnoreAllNonExisting();
        }
    }
    public class LeavingMaterialWarehouseCommandHandler : IRequestHandler<AddLeavingMaterialWarehouseCommand, ServiceResult<Domain.Receipt>>
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
        private readonly IRepository<CodeVoucherGroup> _codeVoucherGroupRepository;
        private readonly IRepository<DocumentItemsBom> _documentItemsBomRepository;
        private readonly IRepository<DocumentHeadExtend> _documentHeadExtendRepository;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;


        Domain.Receipt ToReceipt;
        Domain.Receipt Fromreceipt;
        Domain.WarehouseLayout Fromlayous;
        Domain.WarehouseLayout Tolayous;
        Domain.CodeVoucherGroup codeVoucherGroup;

        public LeavingMaterialWarehouseCommandHandler(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , IReceiptRepository receiptRepository
            , ICurrentUserAccessor currentUserAccessor
            , IRepository<Document> documentRepository
            , IRepository<BaseValue> baseValueRepository
            , IProcedureCallService procedureCallService
            , IReceiptCommandsService receiptCommandsService
            , IRepository<DocumentItem> documentItemRepository
            , IRepository<CodeVoucherGroup> codeVoucherGroupRepository
            , IRepository<DocumentItemsBom> documentItemsBomRepository
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
        public async Task<ServiceResult<Domain.Receipt>> Handle(AddLeavingMaterialWarehouseCommand request, CancellationToken cancellationToken)
        {

            if (request.DocumentDate > DateTime.UtcNow)
            {
                throw new ValidationError("تاریخ انتخابی برای زمان آینده نمی تواند باشد");
            }
            if (!request.ReceiptDocumentItems.Any())
            {
                throw new ValidationError("هیچ کالایی انتخاب نشده است");
            }
            if (request.ReceiptDocumentItems.Where(a => a.Quantity == 0).Any())
            {
                throw new ValidationError("مقدار همه کالاها باید بزرگتر از صفر باشد");
            }
            var query = request.ReceiptDocumentItems.GroupBy(x => x.CommodityId)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();
            if (query.Count() > 0)
            {
                throw new ValidationError("در لیست  کالای تکراری وجود دارد");
            }
            Fromlayous = await _context.WarehouseLayouts.Where(_a => _a.WarehouseId == request.FromWarehouseId && _a.LastLevel == true).FirstOrDefaultAsync();
            if (Fromlayous == null && request.FromWarehouseId > 0)
            {
                throw new ValidationError("برای انبار تحویل دهنده انتخاب شده در این سند هیچ موقعیت بندی آخرین سطح تعریف نشده است");
            }

            Tolayous = await _context.WarehouseLayouts.Where(_a => _a.WarehouseId == request.ToWarehouseId && _a.LastLevel == true).FirstOrDefaultAsync();
            if (Tolayous == null && request.ToWarehouseId > 0)
            {
                throw new ValidationError("برای انبار تحویل گیرنده انتخاب شده در این سند هیچ موقعیت بندی آخرین سطح تعریف نشده است");
            }

            if( request.FromWarehouseId > 0 && Fromlayous.Status!= WarehouseLayoutStatus.Free && Fromlayous.Status != WarehouseLayoutStatus.OutputOnly)
            {
                throw new ValidationError("انبار اجازه خروج ندارد");
            }
            if (request.ToWarehouseId > 0 && Tolayous.Status != WarehouseLayoutStatus.Free && Fromlayous.Status != WarehouseLayoutStatus.InputOnly)
            {
                throw new ValidationError("انبار اجازه ورود ندارد");
            }

            var currency = await _baseValueRepository.GetAll().Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();

            if (currency == null)
            {
                throw new ValidationError("واحد ارز ریالی تعریف نشده است");
            }
            codeVoucherGroup = await _context.CodeVoucherGroups.Where(a => a.Id == request.CodeVoucherGroupId).FirstOrDefaultAsync();

            if (codeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.ProductReceiptWarehouse)
            {
                var bom = request.ReceiptDocumentItems.Where(a => a.BomValueHeaderId == null);
                if (bom.Any())
                {
                    throw new ValidationError("فرمول ساخت همه کالاها انتخاب نشده است");
                }
            }
            try
            {
                Domain.Receipt receipt = await Save(request, Fromlayous, Tolayous, currency, cancellationToken);
                return ServiceResult<Domain.Receipt>.Success(receipt);
            }
            catch (Exception ex)
            {
                if (!(ex is ValidationError))
                {
                    new LogWriter("AddLeavingMaterialWarehouseCommand Error:" + ex.Message.ToString(), "LeavingMaterialWarehouse");
                    if (ex.InnerException != null)
                    {
                        new LogWriter("AddLeavingMaterialWarehouseCommand InnerException:" + ex.InnerException.ToString(), "LeavingMaterialWarehouse");
                    }
                    ToReceipt.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
                    Fromreceipt.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
                    _receiptRepository.Update(ToReceipt);
                    _receiptRepository.Update(Fromreceipt);


                    await _receiptRepository.SaveChangesAsync();
                   
                    throw new ValidationError("مشکل در انجام عملیات خروج حواله مربوطه بایگانی شد");

                }
                else { throw new ValidationError(ex.Message); };

            }
           
        }

        private async Task<Domain.Receipt> Save(
            AddLeavingMaterialWarehouseCommand request,
            Domain.WarehouseLayout Fromlayous,
            Domain.WarehouseLayout Tolayous,
            int currency,
            CancellationToken cancellationToken)
        {

            ToReceipt = _mapper.Map<Domain.Receipt>(request);
            Fromreceipt = _mapper.Map<Domain.Receipt>(request);



            //***اول سندهای خروجی را ثبت می کنیم ، برای اینکه اگر محصول از فرمول ساخت ایجاد شده بود ، قیمت تمام شده آن را بتوان بدست آورد***

            //--------------------ثبت سند خروج از انبار تحویل دهنده---------------------------

            if (request.FromWarehouseId > 0)
            {

                //اگر جابه جایی انبار داشته باشیم سند خروج می زنیم برای اینکه در هنگام ویرایش به آن احتیاج داریم ولی نمی خواهیم موقع ریالی شدن این سند خروج آورده شود
                Fromreceipt.DocumentStauseBaseValue = codeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.RemoveMaterialWarhouse ? (int)DocumentStateEnam.invoiceAmountLeave : (int)DocumentStateEnam.Transfer;

                Fromreceipt = await InsertDocumentExit(request, currency, Fromreceipt, cancellationToken);

                //در رسید محصول خروج کالای انتخابی زده نمی شود ولی در انتقال نیاز به خروجی دارد
                if (codeVoucherGroup.UniqueName != ConstantValues.CodeVoucherGroupValues.ProductReceiptWarehouse)
                {

                    foreach (var item in request.ReceiptDocumentItems)
                    {
                        int documentItemId = await GetReceiptItems(item, Fromreceipt.Id);
                        //-----------------------کاهش موجودی انبار تحویل دهنده
                        await AddWarehouseLayout(request.FromWarehouseId, Fromlayous.Id, item.CommodityId, item.Quantity, (int)(WarehouseHistoryMode.Exit), documentItemId);

                    }
                }



            }

            //--------------------ثبت سند ورود از انبار تحویل گیرنده-------------------------
            if (request.ToWarehouseId > 0)
            {

                ToReceipt = await InsertDocumentEnter(request, currency, ToReceipt, cancellationToken);
                Fromreceipt.ParentId = ToReceipt.Id > 0 ? ToReceipt.Id : null;


                //تغییر موجودی انبار
                foreach (var item in request.ReceiptDocumentItems)
                {
                    int documentItemId = await GetReceiptItems(item, ToReceipt.Id);
                    //-----------------------افزایش موجودی انبار تحویل گیرنده
                    await AddWarehouseLayout(request.ToWarehouseId, Tolayous.Id, item.CommodityId, item.Quantity, (int)(WarehouseHistoryMode.Enter), documentItemId);

                }
            }

            return ToReceipt.Id > 0 ? ToReceipt : Fromreceipt;
        }

        private async Task<int> GetReceiptItems(LeavingMaterialItem item, int ReceiptId)
        {
            DocumentItem ReceiptItems = await _context.DocumentItems.Where(a => a.DocumentHeadId == ReceiptId && a.CommodityId == item.CommodityId).FirstOrDefaultAsync();
            if (ReceiptItems == null)
            {
                throw new ValidationError("برای این درخواست هیچ  اقلام سندی وجود ندارد");
            }
            var documentItemId = ReceiptItems.Id;

            return documentItemId;
        }


        //====================================InsertDocument==============================================
        private async Task<Domain.Receipt> InsertDocumentEnter(AddLeavingMaterialWarehouseCommand request, int currency, Domain.Receipt receipt, CancellationToken cancellationToken)
        {

            receipt.WarehouseId = request.ToWarehouseId;
            receipt.IsDocumentIssuance = request.IsDocumentIssuance;
            receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.invoiceAmount;

            receipt.CommandDescription = $"Command:LeavingMaterialWarehouseCommand - Method :InsertDocumentInput - ورود از انبار {request.CodeVoucherGroupId.ToString()}";
            receipt = await InsertReceipt(request, currency, receipt, cancellationToken);

            return receipt;
        }


        private async Task<Domain.Receipt> InsertDocumentExit(AddLeavingMaterialWarehouseCommand request, int currency, Domain.Receipt receipt, CancellationToken cancellationToken)
        {

            receipt.WarehouseId = request.FromWarehouseId;
            receipt.IsDocumentIssuance = request.IsDocumentIssuance;
            receipt.CommandDescription = $"Command:LeavingMaterialWarehouseCommand - Method :InsertDocumentExit - جابه جایی انبار -codeVoucherGroup.id={request.CodeVoucherGroupId.ToString()}";
            receipt = await InsertReceipt(request, currency, receipt, cancellationToken);

            return receipt;
        }

        private async Task<Domain.Receipt> InsertReceipt(
            AddLeavingMaterialWarehouseCommand request,
            int currency,
            Domain.Receipt receipt,
            CancellationToken cancellationToken)
        {

            _receiptCommandsService.ReceiptBaseDataInsert(request.DocumentDate, receipt);
            receipt.IsPlacementComplete = true;
            receipt.IsImportPurchase = false;
            receipt.CodeVoucherGroupId = request.CodeVoucherGroupId;
            await _receiptCommandsService.SerialFormula(receipt, codeVoucherGroup.Code, cancellationToken);
            int lastNo = await _receiptCommandsService.lastDocumentNo(receipt, cancellationToken);
            receipt.DocumentNo = lastNo + 1;
            await CreateCodeVoucher(receipt);//مستقیم ریالی شود.
            receipt = _receiptCommandsService.ConvertTagArray(request.Tags, receipt);

            _receiptCommandsService.GenerateInvoiceNumber(ConstantValues.WarehouseInvoiceNoEnam.LeaveMaterail, receipt, codeVoucherGroup);


            _receiptRepository.Insert(receipt);
            if (await _receiptRepository.SaveChangesAsync() > 0)
            {
                await InsertDocumentItems(request, receipt, currency);


                receipt.DocumentId = await _receiptCommandsService.InsertAndUpdateDocument(receipt);
                //-------------- receipt.TotalItemPrice--------------------------------
                await _receiptCommandsService.CalculateTotalItemPrice(receipt);
                _receiptRepository.Update(receipt);
                await _receiptRepository.SaveChangesAsync();

            }


            return receipt;
        }

        private async Task<int> InsertDocumentItems(AddLeavingMaterialWarehouseCommand request, Domain.Receipt receipt, int currency)
        {

            foreach (var items in request.ReceiptDocumentItems)
            {
                var MainMeasureId = await _context.Commodities.Where(a => a.Id == items.CommodityId).Select(a => a.MeasureId).FirstOrDefaultAsync();
                var documentItem = new DocumentItem()
                {
                    DocumentHeadId = receipt.Id,
                    CurrencyBaseId = currency,
                    CurrencyPrice = 1,
                    DocumentMeasureId = items.DocumentMeasureId,
                    MainMeasureId = Convert.ToInt32(MainMeasureId),
                    Quantity = items.Quantity,
                    Description = items.Description,
                    CommodityId = items.CommodityId,
                    RemainQuantity = items.Quantity,
                    IsWrongMeasure = items.IsWrongMeasure,
                    BomValueHeaderId = items.BomValueHeaderId,
                    YearId = _currentUserAccessor.GetYearId(),
                    ModifiedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = _currentUserAccessor.GetId(),
                    IsDeleted = false,
                    OwnerRoleId = _currentUserAccessor.GetRoleId(),
                };



                _documentItemRepository.Insert(documentItem);
                if (await _documentItemRepository.SaveChangesAsync() > 0)
                {
                    //-----------------اگر از روی فرمول ساخت ایجاد شده است ، مواد اولیه مرتبط ثبت و خارج شود
                    if (items.BomValueHeaderId > 0 && receipt.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmount)
                    {
                        await AddDocumentItemsBom(request, currency, items, documentItem);
                    }
                }
                await _receiptCommandsService.GetPriceBuyItems(items.CommodityId, receipt.WarehouseId, items.BomValueHeaderId > 0 ? documentItem.Id : null, items.Quantity, documentItem);

            }
            await _documentItemRepository.SaveChangesAsync();


            return 1;
        }


        private async Task AddDocumentItemsBom(AddLeavingMaterialWarehouseCommand request, int currency, LeavingMaterialItem items, DocumentItem documentItem)
        {
            var bomsValues = _context.BomValueView.Where(a => a.BomValueHeaderId == items.BomValueHeaderId).ToList();
            foreach (var Values in bomsValues)
            {
                var layouts = _context.WarehouseLayouts.Where(_a => _a.WarehouseId == Values.BomWarehouseId && _a.LastLevel == true).FirstOrDefault();
                DocumentItemsBom BomItem = await _receiptCommandsService.AddDocumentItemsBom(layouts.Id, Values.BomWarehouseId, currency, items.Quantity, items.CommodityId, documentItem.Id, Values);

                if (await _documentItemsBomRepository.SaveChangesAsync() > 0)
                {
                    //--------------------کاهش ظرفیت فعلی در مکان-----------------
                    var Quantity = Math.Round(items.Quantity * Values.Value, 2, MidpointRounding.AwayFromZero);
                    await AddWarehouseLayout(Values.BomWarehouseId, layouts.Id, BomItem.CommodityId, Quantity, (int)(WarehouseHistoryMode.Exit), documentItem.Id);
                }

            }

        }
        //======================================================================
        //----------------------------------------------------------------------
        //-----------------افزایش و کاهش ظرفیت فعلی در مکان--------------------
        private async Task AddWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int historyMode, int receiptItemsId)
        {

            var WarehouseLayoutQuantity = await _context.WarehouseLayoutQuantities.Where(a => a.WarehouseLayoutId == WarehouseLayoutId && a.CommodityId == CommodityId).FirstOrDefaultAsync();
            var stock = await _context.WarehouseStocks.Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId).FirstOrDefaultAsync();

            await _warehouseLayoutCommandsService.InsertAndUpdateWarehouseHistory(CommodityId, Quantity, WarehouseLayoutId, receiptItemsId, historyMode);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, WarehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, historyMode, stock);


        }

        private async Task<CodeVoucherGroup> CreateCodeVoucher(Receipt receipt)
        {

            CodeVoucherGroup NewCodeVoucherGroup = await _receiptCommandsService.GetNewCodeVoucherGroup(receipt);

            return NewCodeVoucherGroup;
        }
    }
}