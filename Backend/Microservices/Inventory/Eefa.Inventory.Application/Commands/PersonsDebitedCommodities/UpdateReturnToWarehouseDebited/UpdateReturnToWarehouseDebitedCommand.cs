using System;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain.Common;
using Eefa.Common.Exceptions;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{
    //------------------برگشت کالا اموال به انبار-----------------------------
    public class UpdateReturnToWarehouseDebitedCommand : CommandBase, IRequest<ServiceResult<PersonsDebitedCommoditiesModel>>, IMapFrom<Domain.PersonsDebitedCommodities>, ICommand
    {
        public int Id { get; set; } = default!;
        public int WarehouseId { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime CreateDate { get; set; } = default!;

    }


    public class UpdateReturnToWarehouseDebitedHandler : IRequestHandler<UpdateReturnToWarehouseDebitedCommand, ServiceResult<PersonsDebitedCommoditiesModel>>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;
        private readonly IPersonsDebitedCommoditiesRepository _PersonsDebitedCommoditiesRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly IRepository<Assets> _assets;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly ICurrentUserAccessor _currentUserAccessor;


        public UpdateReturnToWarehouseDebitedHandler(
            IMapper mapper,
            IInvertoryUnitOfWork context,
            IWarehouseLayoutCommandsService warehouseLayoutCommandsService,
            IPersonsDebitedCommoditiesRepository PersonsDebitedCommoditiesRepository,
            IReceiptRepository receiptRepository,
            IRepository<Document> documentRepository,
            IReceiptCommandsService receiptCommandsService,
            IRepository<Assets> assets,
            IRepository<DocumentItem> documentItemRepository,
            ICurrentUserAccessor currentUserAccessor
            )
        {
            _mapper = mapper;
            _context = context;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;
            _PersonsDebitedCommoditiesRepository = PersonsDebitedCommoditiesRepository;
            _documentRepository = documentRepository;
            _receiptRepository = receiptRepository;
            _receiptCommandsService = receiptCommandsService;
            _assets = assets;
            _documentItemRepository = documentItemRepository;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult<PersonsDebitedCommoditiesModel>> Handle(UpdateReturnToWarehouseDebitedCommand request, CancellationToken cancellationToken)
        {


            var entity = await _PersonsDebitedCommoditiesRepository.Find(request.Id);
            Domain.PersonsDebitedCommodities newEntity = entity;

            var currency = await _context.BaseValues.Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();

            if (currency == null)
            {
                throw new ValidationError("واحد ارز ریالی تعریف نشده است");
            }
            var warehouse = await _context.Warehouses.Where(a => a.Id == request.WarehouseId).FirstOrDefaultAsync();
            if (warehouse == null)
            {
                throw new ValidationError("کد انبار اشتباه است");
            }
            var codeVoucherGroup = await _context.CodeVoucherGroups.Where(a => a.UniqueName == ConstantValues.CodeVoucherGroupValues.Transfer).FirstOrDefaultAsync();

            if (codeVoucherGroup == null)
            {
                throw new ValidationError("کد گروه سند وجود ندارد");
            }
            Receipt receipt = await InsertDocumentHead(request, codeVoucherGroup);
            if (await _receiptRepository.SaveChangesAsync() > 0)
            {
                DocumentItem documentItem = await AddDocumentItems(entity, currency, warehouse, receipt,request.Description);
                
              
                await _receiptRepository.SaveChangesAsync();

                //----------------اولین محلی که پیدا شد--------------------------
                WarehouseLayout layouts = await _warehouseLayoutCommandsService.FindLayout(request.WarehouseId, Convert.ToInt32(entity.CommodityId));
                

                if (layouts != null)
                {
                    //----------------اولین محلی که پیدا شد--------------------------
                    var warehouseLayoutQuantity = await _context.WarehouseLayoutQuantities.Where(a => a.WarehouseLayoutId == layouts.Id && a.CommodityId == entity.CommodityId).FirstOrDefaultAsync();
                    await UpdateWarehouseLayout(request.WarehouseId, layouts.Id, Convert.ToInt32(entity.CommodityId), entity.Quantity, (int)(WarehouseHistoryMode.Enter), documentItem.Id, warehouseLayoutQuantity);
                }

                await UpdateDocumentHeadPersonsDebited(entity, receipt);

                await UpdatePerson(request, entity, cancellationToken, documentItem.Id);
                var model = _mapper.Map<PersonsDebitedCommoditiesModel>(entity);
                return ServiceResult<PersonsDebitedCommoditiesModel>.Success(model);

            }

            return ServiceResult<PersonsDebitedCommoditiesModel>.Failed();


        }

        private async Task<Receipt> InsertDocumentHead(UpdateReturnToWarehouseDebitedCommand request, CodeVoucherGroup codeVoucherGroup)
        {
            var receipt = new Receipt();
            receipt.WarehouseId = Convert.ToInt32(request.WarehouseId);
            receipt.ExpireDate = request.CreateDate;
            receipt.YearId = _currentUserAccessor.GetYearId();
            receipt.TotalWeight = default;
            receipt.TotalQuantity = default;
            receipt.DocumentDiscount = default;
            receipt.DiscountPercent = default;
            receipt.DocumentStateBaseId = ConstantValues.ConstBaseValue.NotChecked;
            receipt.PaymentTypeBaseId = 1;
            receipt.DocumentDate = request.CreateDate;
            receipt.RequestNo = "";
            receipt.CodeVoucherGroupId = codeVoucherGroup.Id;

            receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Transfer;
            receipt.DocumentDescription = "بازگشت کالا اموال به انبار";
            receipt.CommandDescription = "Command:EditWarehouseInventory -انتقال اموال-codeVoucherGroup.id=" + receipt.CodeVoucherGroupId.ToString();

            await _receiptCommandsService.SerialFormula(receipt, codeVoucherGroup.Code, new CancellationToken());

            int lastNo = await _receiptCommandsService.lastDocumentNo(receipt, new CancellationToken());


            receipt.DocumentNo = lastNo + 1;

            _receiptCommandsService.GenerateInvoiceNumber(((int)DocumentStateEnam.InventoryModification).ToString(), receipt, codeVoucherGroup);

            _receiptRepository.Insert(receipt);
            return receipt;
        }

        private async Task<DocumentItem> AddDocumentItems(PersonsDebitedCommodities entity, int currency, Warehouse warehouse, Receipt receipt,string Description)
        {
            var documentItems = new List<DocumentItem>();


            var documentItem = new DocumentItem();
            var commodity = await _context.Commodities.Where(a => a.Id == entity.CommodityId).FirstOrDefaultAsync();

            documentItem.CurrencyBaseId = currency;
            documentItem.CurrencyPrice = 1;
            documentItem.DocumentHeadId = receipt.Id;
            documentItem.DocumentMeasureId = Convert.ToInt32(commodity.MeasureId);
            documentItem.MainMeasureId = Convert.ToInt32(commodity.MeasureId);
            documentItem.CommodityId = commodity.Id;
            documentItem.Quantity = entity.Quantity;
            documentItem.UnitBasePrice = default;
            documentItem.ProductionCost = default;
            documentItem.Discount = default;
            documentItem.Weight = default;
            documentItem.Description = Description;
            documentItem.UnitBasePrice = default;

            await _receiptCommandsService.GetPriceBuyItems(documentItem.CommodityId, warehouse.Id, documentItem.BomValueHeaderId > 0 ? documentItem.Id : null, documentItem.Quantity, documentItem);
            documentItem.YearId = _currentUserAccessor.GetYearId();

            _documentItemRepository.Insert(documentItem);

            await _documentItemRepository.SaveChangesAsync();

            return documentItem;
        }

        private async Task UpdateDocumentHeadPersonsDebited(PersonsDebitedCommodities entity, Receipt receipt)
        {
            var DocumentHeadId = await _context.DocumentItems.Where(a => a.Id == entity.DocumentItemId).FirstOrDefaultAsync();
            if (DocumentHeadId != null)
            {
                var Assets = await _context.Assets.Where(a => a.Id == entity.AssetId).FirstOrDefaultAsync();
                Assets.DocumentHeadId = receipt.Id;
                Assets.WarehouseId = receipt.WarehouseId;
                _assets.Update(Assets);
                await _assets.SaveChangesAsync();
            }

        }

        private async Task UpdatePerson(UpdateReturnToWarehouseDebitedCommand request, PersonsDebitedCommodities entity, CancellationToken cancellationToken, int DocumentItemId)
        {
            entity.ExpierDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToUniversalTime();
            entity.IsActive = false;
            entity.DocumentItemId = DocumentItemId;
            _PersonsDebitedCommoditiesRepository.Update(entity);

            await _PersonsDebitedCommoditiesRepository.SaveChangesAsync(cancellationToken);
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
