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
    public class CreateSingleRowCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<CreateSingleRowCommand>, ICommand
    {

        public int CodeVoucherGroupId { get; set; } = default!;
        public int? RequesterReferenceId { get; set; } = default!;
        public int? FollowUpReferenceId { get; set; } = default!;

        public int WarehouseId { get; set; } = default!;
        public int? DocumentNo { get; set; } = default!;

        public int? CreditAccountReferenceId { get; set; } = default!;
        public int? CreditAccountReferenceGroupId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; } = default!;
        public int? DebitAccountReferenceId { get; set; } = default!;
        public int? DebitAccountHeadId { get; set; } = default!;

        /// <description>
        /// توضیحات سند
        ///</description>

        public string DocumentDescription { get; set; }

        /// <description>
        /// دستی
        ///</description>
        public bool IsManual { get; set; } = default!;

        public string ScaleBill { get; set; } = default!;


        /// <description>
        /// تاریخ انقضا
        ///</description>

        public DateTime? ExpireDate { get; set; }



        public string RequestNumber { get; set; }
        /// تاریخ سند
        ///</description>

        public DateTime DocumentDate { get; set; } = default!;
        /// تاریخ درخواست
        ///</description>

        public DateTime? RequestDate { get; set; } = default!;

        /// <description>
        /// شماره فاکتور فروشنده
        ///</description>
        public string InvoiceNo { get; set; }
        public string Tags { get; set; }
        public int? ViewId { get; set; } = default!;
        public bool? IsImportPurchase { get; set; } = default!;

        public int DocumentStauseBaseValue { get; set; } = default!;
        public string PartNumber { get; set; }
        public Nullable<bool> IsDocumentIssuance { get; set; } = default!;

        public ICollection<ReceiptDocumentItemCreate> ReceiptDocumentItems { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateSingleRowCommand, Domain.Receipt>()
                .IgnoreAllNonExisting();
        }



    }
    public class ReceiptDocumentItemCreate : IMapFrom<DocumentItem>
    {
        public int CommodityId { get; set; } = default!;

        /// <description>
        /// سریال کالا
        ///</description>
        public string CommoditySerial { get; set; }
        public double? SecondaryQuantity { get; set; } = default!;

        /// <description>
        /// قیمت واحد 
        ///</description>
        public int? MainMeasureId { get; set; }
        public double? ConversionRatio { get; set; }
        public long UnitPrice { get; set; } = default!;
        /// <description>
        /// قیمت در سیستم  درخواست 
        ///</description>

        public long UnitBasePrice { get; set; } = default!;

        /// <description>
        /// قیمت پایه
        ///</description>

        public long ProductionCost { get; set; } = default!;


        /// <description>
        /// تعداد
        ///</description>

        public double Quantity { get; set; } = default!;
        public double QuantityChose { get; set; } = default!;
        public int? CurrencyBaseId { get; set; }

        /// <description>
        /// نرخ ارز
        ///</description>

        public int? CurrencyPrice { get; set; }

        public int? DocumentMeasureId { get; set; }
        public int? MeasureUnitConversionId { get; set; }
        public string Description { get; set; }
        public bool IsWrongMeasure { get; set; } = default!;
        public AssetsModel Assets { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReceiptDocumentItemCreate, DocumentItem>()
                .IgnoreAllNonExisting();
        }
    }
    public class CreateSingleRowCommandHandler : IRequestHandler<CreateSingleRowCommand, ServiceResult<Domain.Receipt>>
    {

        private readonly IMapper _mapper;

        private readonly IAssetsRepository _assetsRepository;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IRepository<Document> _documentRepository;
        private readonly IRepository<BaseValue> _baseValueRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<DocumentHeadExtend> _documentHeadExtendRepository;



        public CreateSingleRowCommandHandler(
             IMapper mapper
            , IInvertoryUnitOfWork context
            , IReceiptRepository receiptRepository
            , IAssetsRepository assetsRepository
            , ICurrentUserAccessor currentUserAccessor
            , IRepository<BaseValue> baseValueRepository
            , IRepository<Document> documentRepository
            , IProcedureCallService procedureCallService
            , IReceiptCommandsService receiptCommandsService
            , IRepository<DocumentHeadExtend> documentHeadExtendRepository


            )
        {
            _mapper = mapper;
            _context = context;
            _assetsRepository = assetsRepository;
            _receiptRepository = receiptRepository;
            _documentRepository = documentRepository;
            _currentUserAccessor = currentUserAccessor;
            _baseValueRepository = baseValueRepository;
            _procedureCallService = procedureCallService;
            _receiptCommandsService = receiptCommandsService;
            _documentHeadExtendRepository = documentHeadExtendRepository;

        }


        public async Task<ServiceResult<Domain.Receipt>> Handle(CreateSingleRowCommand request, CancellationToken cancellationToken)
        {
            if (request.DocumentDate > DateTime.UtcNow.AddDays(1))
            {
                throw new ValidationError("تاریخ انتخابی برای زمان آینده نمی تواند باشد" );
            }
            if (request.ReceiptDocumentItems.Count() == 0)
            {
                throw new ValidationError("هیچ کالایی انتخاب نشده است");
            }
            var layouts = await _context.WarehouseLayouts.Where(_a => _a.WarehouseId == request.WarehouseId && _a.LastLevel == true).FirstOrDefaultAsync();
            if (layouts == null)
            {
                throw new ValidationError("برای انبار انتخاب شده در این سند هیچ موقعیت بندی آخرین سطح تعریف نشده است");
            }
            var currency = await _baseValueRepository.GetAll().Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();

            if (currency == null)
            {
                throw new ValidationError("واحد ارز ریالی تعریف نشده است");
            }
            var warehouse = await _context.Warehouses.Where(a => a.Id == request.WarehouseId).FirstOrDefaultAsync();
            if (warehouse == null)
            {
                throw new ValidationError("کد انبار اشتباه است");
            }

            CodeVoucherGroup CodeVoucher = await _context.CodeVoucherGroups.Where(a => a.Id == request.CodeVoucherGroupId).FirstOrDefaultAsync();

            if (CodeVoucher == null)
            {
                throw new ValidationError("کد گروه سند وجود ندارد");
            }
            
            
            return await Insert(request, currency, warehouse, CodeVoucher, cancellationToken);

        }

        private async Task<ServiceResult<Receipt>> Insert(CreateSingleRowCommand request, int currency, Warehouse warehouse, CodeVoucherGroup CodeVoucher, CancellationToken cancellationToken)
        {


            var HasRequestBuyReceipt = new Domain.Receipt();
            var receipt = _mapper.Map<Domain.Receipt>(request);
            receipt.DocumentStauseBaseValue = request.DocumentStauseBaseValue;

            await _receiptCommandsService.SerialFormula(receipt, CodeVoucher.Code, cancellationToken);

            int lastNo = await _receiptCommandsService.lastDocumentNo(receipt, cancellationToken);
            if (request.DocumentNo == default)
            {
                receipt.DocumentNo = lastNo + 1;
            }
            // اگر شماره را دستی وارد کردند نباید تکراری باشد..
            else
            {
                if (await _receiptCommandsService.IsDuplicateDocumentNo(receipt, cancellationToken))
                {
                    throw new ValidationError("شماره درخواست تکراری است");
                }
            }
            _receiptCommandsService.ReceiptBaseDataInsert(request.DocumentDate, receipt);
            receipt.IsDocumentIssuance = request.IsDocumentIssuance;
            receipt.CommandDescription = $"Command:CreateSingleRowCommand -codeVoucherGroup.id={request.CodeVoucherGroupId.ToString()}";
            receipt.ExpireDate = request.ExpireDate == null ? DateTime.Now.AddDays(30) : request.ExpireDate;
            receipt.RequestNo = request.RequestNumber;
            receipt = _receiptCommandsService.ConvertTagArray(request.Tags, receipt);


            await DebitAndCredit(request, warehouse, CodeVoucher, receipt);
            if ( !await _receiptCommandsService.IsValidAccountHeadRelationByReferenceGroup(receipt.DebitAccountHeadId, receipt.DebitAccountReferenceGroupId))
            {
                throw new ValidationError("عدم تطابق گروه حساب بستانکار و سرفصل حساب بستانکار");
            }
            if (!await _receiptCommandsService.IsValidAccountHeadRelationByReferenceGroup(receipt.CreditAccountHeadId, receipt.CreditAccountReferenceGroupId))
            {
                throw new ValidationError("عدم تطابق گروه حساب بدهکار و سرفصل حساب بدهکار");
            }

            await InsertDocumentItems(request, receipt, currency, CodeVoucher);

            if (await CheckAddNewRequest(request, receipt))
            {
                HasRequestBuyReceipt = await InsertRequest(receipt, request, cancellationToken);
            }


            _receiptRepository.Insert(receipt);

            await _receiptRepository.SaveChangesAsync();

            await _receiptCommandsService.CalculateTotalItemPrice(receipt);



            _receiptRepository.Update(receipt);

            await _receiptRepository.SaveChangesAsync();


            //اگر نوع سند از اموال بود ، شماره سریال ها ثبت شود

            InsertAssets(request, receipt);
            //---------------------Insert documentHeadExtend --------------------------
            await _receiptCommandsService.InsertDocumentHeadExtend(request.RequesterReferenceId, request.FollowUpReferenceId, receipt);

            if (await CheckAddNewRequest(request, receipt))
            {
                await _receiptCommandsService.InsertDocumentHeadExtend(request.RequesterReferenceId, request.FollowUpReferenceId, HasRequestBuyReceipt);
            }
            return ServiceResult<Domain.Receipt>.Success(receipt);


        }


        private async Task<bool> CheckAddNewRequest(CreateSingleRowCommand request, Receipt receipt)
        {
            var HasRequestBuy = await _context.DocumentHeads.Where(a => a.DocumentNo.ToString() == request.RequestNumber && a.DocumentStauseBaseValue == (int)DocumentStateEnam.requestBuy).FirstOrDefaultAsync();
            return HasRequestBuy == null && (int)DocumentStateEnam.Temp == receipt.DocumentStauseBaseValue && !String.IsNullOrEmpty(receipt.RequestNo) && Convert.ToInt32(receipt.RequestNo) > 0;
        }


        private async Task DebitAndCredit(CreateSingleRowCommand request, Warehouse warehouse, CodeVoucherGroup CodeVoucher, Receipt receipt)
        {
            //-----------------انبار بدهکار---------------------------

            receipt.DebitAccountHeadId = request.DebitAccountHeadId>0? request.DebitAccountHeadId: warehouse.AccountHeadId ;

            //------------------تامین کننده بستانکار--------------------
            receipt.CreditAccountReferenceId = request.CreditAccountReferenceId;
            receipt.CreditAccountReferenceGroupId = request.CreditAccountReferenceGroupId;

            //--------------------------------------مشخص کردن که آیا این صوتحساب وارداتی است یا خیر؟
            if (request.CreditAccountReferenceId != null)
                receipt = await _receiptCommandsService.UpdateImportPurchaseReceipt(receipt);

            receipt.CreditAccountHeadId = receipt.IsImportPurchase == true ? ConstantValues.AccountReferenceGroup.AccountHeadExternalProvider : CodeVoucher.DefultCreditAccountHeadId;
        }

        private async Task InsertDocumentItems(CreateSingleRowCommand request, Domain.Receipt receipt, int currency, CodeVoucherGroup CodeVoucher)
        {
            foreach (var items in request.ReceiptDocumentItems)
            {
                DocumentItem documentItem = AddItems(currency, items, false);

                receipt.AddItem(documentItem);
                //محاسبه قیمت تخمینی 
                await _receiptCommandsService.GetPriceEstimateItems(documentItem.CommodityId, receipt.WarehouseId, documentItem);
                //اگر برگشتی و مرجوعی به انبار باشد ، قیمت کالا را خودش محاسبه کند.
                if (ConstantValues.CodeVoucherGroupValues.ReturnTemporaryReceipt == CodeVoucher.UniqueName || ConstantValues.CodeVoucherGroupValues.loanTemporaryReceipt == CodeVoucher.UniqueName || ConstantValues.CodeVoucherGroupValues.InventoryModification == CodeVoucher.UniqueName)
                {
                    await _receiptCommandsService.GetPriceBuyItems(documentItem.CommodityId, receipt.WarehouseId, null, documentItem.Quantity, documentItem);

                    _receiptCommandsService.GenerateInvoiceNumber("T", receipt, CodeVoucher);
                }

            }

        }

        private  DocumentItem AddItems(int currency, ReceiptDocumentItemCreate items, bool isRequest)
        {
            var MeasureId = items.DocumentMeasureId==null? _context.Commodities.Where(a => a.Id == items.CommodityId).Select(a=>a.MeasureId).FirstOrDefault(): items.DocumentMeasureId;
            return new DocumentItem()
            {
                CurrencyBaseId = currency,
                CurrencyPrice = 1,
                DocumentMeasureId = MeasureId.Value,
                MainMeasureId = MeasureId.Value,
                Quantity = isRequest ? items.QuantityChose : items.Quantity,
                Description = items.Description,
                SecondaryQuantity = items.SecondaryQuantity,
                CommodityId = items.CommodityId,
                RemainQuantity = isRequest ? items.QuantityChose : items.Quantity,
                IsWrongMeasure = items.IsWrongMeasure,
                CreatedById = _currentUserAccessor.GetId(),
                OwnerRoleId = _currentUserAccessor.GetRoleId(),
            };
        }

        private async void InsertAssets(CreateSingleRowCommand request, Domain.Receipt receipt)
        {

            foreach (var item in request.ReceiptDocumentItems)
            {
                var items = _context.DocumentItems.Where(a => a.CommodityId == item.CommodityId && a.Quantity == item.Quantity && a.DocumentHeadId == receipt.Id).FirstOrDefault();

                if (items != null && item.Assets != null)
                {
                    item.Assets.DocumentItemId = items.Id;
                }


                await _receiptCommandsService.InsertAssets(item.Assets, Convert.ToInt32(item.MainMeasureId), item.CommodityId, receipt);
            }
        }
        private async Task<Domain.Receipt> InsertRequest(Domain.Receipt receipt, CreateSingleRowCommand request, CancellationToken cancellationToken)
        {

            var NewReceipt = _mapper.Map<Domain.Receipt>(request);


            var CodeVoucher = await CreateCodeVoucher(NewReceipt);
            NewReceipt.CodeVoucherGroupId = CodeVoucher.Id;
            NewReceipt.Id = default;
            await _receiptCommandsService.SerialFormula(NewReceipt, CodeVoucher.Code, cancellationToken);
            NewReceipt.DocumentNo = Convert.ToInt32(receipt.RequestNo);


            _receiptCommandsService.ReceiptBaseDataInsert(request.DocumentDate, NewReceipt);


            NewReceipt.ExpireDate = request.ExpireDate == null ? DateTime.Now.AddDays(30) : request.ExpireDate;

            NewReceipt = _receiptCommandsService.ConvertTagArray(request.Tags, NewReceipt);

            //--------------------------------------مشخص کردن که آیا این صوتحساب وارداتی است یا خیر؟

            NewReceipt = await _receiptCommandsService.UpdateImportPurchaseReceipt(NewReceipt);
            NewReceipt.CreditAccountReferenceId = null;
            NewReceipt.CreditAccountReferenceGroupId = null;

            foreach (var items in request.ReceiptDocumentItems)
            {

                DocumentItem documentItem = AddItems(0, items, true);

                NewReceipt.AddItem(documentItem);

            }


            _receiptRepository.Insert(NewReceipt);

            return NewReceipt;


        }
        private async Task<CodeVoucherGroup> CreateCodeVoucher(Receipt receipt)
        {
            receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.requestBuy;

            CodeVoucherGroup NewCodeVoucherGroup = await _receiptCommandsService.GetNewCodeVoucherGroup(receipt);
            return NewCodeVoucherGroup;
        }

    }
}