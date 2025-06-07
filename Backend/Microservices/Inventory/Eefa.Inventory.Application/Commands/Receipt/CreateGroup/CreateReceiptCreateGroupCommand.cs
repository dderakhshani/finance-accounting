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
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class CreateReceiptCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<CreateReceiptCommand>, ICommand
    {

        public int CodeVoucherGroupId { get; set; } = default!;
        public int RequesterReferenceId { get; set; } = default!;
        public int FollowUpReferenceId { get; set; } = default!;

        public int WarehouseId { get; set; } = default!;

        /// <description>
        /// توضیحات سند
        ///</description>

        public string DocumentDescription { get; set; }

        /// <description>
        /// دستی
        ///</description>
        public bool IsManual { get; set; } = default!;
        public Nullable<bool> IsDocumentIssuance { get; set; } = default!;


        /// تاریخ سند
        ///</description>

        public DateTime DocumentDate { get; set; } = default!;

        public int RequestNumber { get; set; }

        public string Tags { get; set; }
        public string PartNumber { get; set; }

        public List<ReceiptDocumentItemProduct> ReceiptDocumentItems { get; set; }



        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateReceiptCommand, Domain.Receipt>()
                .IgnoreAllNonExisting();
        }

        public class ReceiptDocumentItemProduct : IMapFrom<DocumentItem>
        {
            public int Id { get; set; }
            public int CommodityId { get; set; } = default!;


            public int? RequesterReferenceId { get; set; } = default!;
            public int? FollowUpReferenceId { get; set; } = default!;
            /// <description>
            /// سریال کالا
            ///</description>

            public string CommoditySerial { get; set; }
            public double? SecondaryQuantity { get; set; } = default!;

            /// <description>
            /// قیمت واحد 
            ///</description>
            public int MainMeasureId { get; set; }
            public double ConversionRatio { get; set; }
            public long UnitPrice { get; set; } = default!;
            /// <description>
            /// قیمت در سیستم  درخواست 
            ///</description>

            public long? UnitBasePrice { get; set; } = default!;

            /// <description>
            /// قیمت پایه
            ///</description>

            public long ProductionCost { get; set; } = default!;
            /// <description>
            /// تاریخ انقضا
            ///</description>


            public DateTime? ExpireDate { get; set; }
            /// <description>
            /// تاریخ درخواست
            ///</description>
            public DateTime? RequestDate { get; set; }


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

            /// <description>
            /// نقش صاحب سند
            ///</description>
            public int DocumentMeasureId { get; set; }
            public int? MeasureUnitConversionId { get; set; }
            public string Description { get; set; }

            public string InvoiceNo { get; set; }
            public string RequestNo { get; set; }

            public int? CreditAccountHeadId { get; set; } = default!;
            public int? CreditAccountReferenceId { get; set; } = default!;
            public int? CreditAccountReferenceGroupId { get; set; } = default!;

            public bool? IsWrongMeasure { get; set; } = default!;
            public AssetsModel Assets { get; set; }

            public void Mapping(Profile profile)
            {
                profile.CreateMap<ReceiptDocumentItemProduct, DocumentItem>()
                    .IgnoreAllNonExisting();
            }
        }
    }

    public class CreateReceiptCommandHandler : IRequestHandler<CreateReceiptCommand, ServiceResult<Domain.Receipt>>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IAssetsRepository _assetsRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly IRepository<BaseValue> _baseValueRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<DocumentHeadExtend> _documentHeadExtend;
        public CreateReceiptCommandHandler(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , IAssetsRepository assetsRepository
            , IReceiptRepository receiptRepository
            , ICurrentUserAccessor currentUserAccessor
            , IRepository<BaseValue> baseValueRepository
            , IRepository<Document> documentRepository
            , IRepository<DocumentHeadExtend> documentHeadExtend
            , IReceiptCommandsService receiptCommandsService)
        {
            _mapper = mapper;
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _baseValueRepository = baseValueRepository;
            _receiptRepository = receiptRepository;
            _documentRepository = documentRepository;
            _documentHeadExtend = documentHeadExtend;
            _assetsRepository = assetsRepository;
            _receiptCommandsService = receiptCommandsService;
        }
        public async Task<ServiceResult<Domain.Receipt>> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
        {
            if (request.DocumentDate > DateTime.UtcNow)
            {
                throw new ValidationError("تاریخ انتخابی برای زمان آینده نمی تواند باشد");
            }
            var layous = await _context.WarehouseLayouts.Where(_a => _a.WarehouseId == request.WarehouseId && _a.LastLevel == true).FirstOrDefaultAsync();
            if (layous == null)
            {
                throw new ValidationError("برای انبار انتخاب شده در این سند هیچ موقعیت بندی آخرین سطح تعریف نشده است");
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

            var CurrencyBaseId = await _baseValueRepository.GetAll().Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();
            var item = request.ReceiptDocumentItems[0];



            var HasRequestBuyReceipt = new Domain.Receipt();
            var receipt = _mapper.Map<Domain.Receipt>(request);

            await _receiptCommandsService.SerialFormula(receipt, CodeVoucher.Code, cancellationToken);
            _receiptCommandsService.ReceiptBaseDataInsert(request.DocumentDate, receipt);

            receipt.RequestDate = item.RequestDate;
            receipt.RequestNo = item.RequestNo;
            receipt.IsDocumentIssuance = request.IsDocumentIssuance;
            receipt.InvoiceNo = item.InvoiceNo;
            receipt.WarehouseId = request.WarehouseId;
            receipt.CodeVoucherGroupId = request.CodeVoucherGroupId;
            receipt.DocumentDescription = request.DocumentDescription;
            receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Temp;


            receipt.DebitAccountHeadId = warehouse.AccountHeadId;

            receipt.ExpireDate = item.ExpireDate == null ? DateTime.Now.AddDays(30) : item.ExpireDate;



            receipt.CommandDescription = "Command:CreateReceiptCommand - ثبت گروهی رسید موقت-codeVoucherGroup.id=" + request.CodeVoucherGroupId.ToString();
            
            await DebitAndCredit(CodeVoucher, item, receipt);
            int lastNo = await _receiptCommandsService.lastDocumentNo(receipt, cancellationToken);


            receipt.DocumentNo = lastNo + 1;
            receipt = _receiptCommandsService.ConvertTagArray(request.Tags, receipt);
            //--------------------------------------مشخص کردن که آیا این صوتحساب وارداتی است یا خیر؟
            if (receipt.DebitAccountReferenceId != null)
            {
               
            }

            await InsertDocumentItem(CurrencyBaseId, item, receipt);

            if (await CheckAddNewRequest(receipt))
            {
                HasRequestBuyReceipt = await InsertRequest(receipt, request, cancellationToken);
            }

            _receiptRepository.Insert(receipt);
            await _receiptRepository.SaveChangesAsync();

            //اگر نوع سند از اموال بود ، شماره سریال ها ثبت شود
            await _receiptCommandsService.InsertAssets(item.Assets, item.MainMeasureId, item.CommodityId, receipt);

            //---------------------insert documentHeadExtend --------------------------

            await _receiptCommandsService.InsertDocumentHeadExtend(item.RequesterReferenceId, item.FollowUpReferenceId, receipt);

            if (HasRequestBuyReceipt != null && await CheckAddNewRequest(receipt))
            {
                await _receiptCommandsService.InsertDocumentHeadExtend(request.RequesterReferenceId, request.FollowUpReferenceId, HasRequestBuyReceipt);
            }

            return ServiceResult<Domain.Receipt>.Success(receipt);

        }
        private async Task<bool> CheckAddNewRequest(Receipt receipt)
        {
            var HasRequestBuy = await _context.DocumentHeads.Where(a => a.DocumentNo.ToString() == receipt.RequestNo && a.DocumentStauseBaseValue == (int)DocumentStateEnam.requestBuy).FirstOrDefaultAsync();
            return HasRequestBuy == null && (int)DocumentStateEnam.Temp == receipt.DocumentStauseBaseValue && !String.IsNullOrEmpty(receipt.RequestNo) && Convert.ToInt32(receipt.RequestNo) > 0;
        }
        private async Task DebitAndCredit(CodeVoucherGroup CodeVoucher, CreateReceiptCommand.ReceiptDocumentItemProduct item, Receipt receipt)
        {
            //------------------تامین کننده بستانکار--------------------
            receipt.CreditAccountReferenceId = item.CreditAccountReferenceId;
            receipt.CreditAccountReferenceGroupId = item.CreditAccountReferenceGroupId;
            receipt = await _receiptCommandsService.UpdateImportPurchaseReceipt(receipt);

            receipt.CreditAccountHeadId = receipt.IsImportPurchase == true ? ConstantValues.AccountReferenceGroup.AccountHeadExternalProvider : CodeVoucher.DefultCreditAccountHeadId;


        }

        private async Task InsertDocumentItem(int CurrencyBaseId, CreateReceiptCommand.ReceiptDocumentItemProduct item, Domain.Receipt receipt)
        {

            var documentItem = new DocumentItem();
            documentItem.CurrencyBaseId = CurrencyBaseId;
            documentItem.CurrencyPrice = 1;
            documentItem.MainMeasureId = item.MainMeasureId;
            documentItem.Quantity = item.Quantity;
            documentItem.Description = item.Description;
            documentItem.SecondaryQuantity = item.SecondaryQuantity;
            documentItem.CommodityId = item.CommodityId;
            documentItem.RemainQuantity = item.Quantity;
            documentItem.IsWrongMeasure = item.IsWrongMeasure;
            documentItem.CreatedById = _currentUserAccessor.GetId();
            documentItem.OwnerRoleId = _currentUserAccessor.GetRoleId();
            documentItem.DocumentMeasureId = documentItem.DocumentMeasureId == 0 ? documentItem.MainMeasureId : documentItem.DocumentMeasureId;

            //محاسبه حدود قیمت 
            await _receiptCommandsService.GetPriceEstimateItems(documentItem.CommodityId, receipt.WarehouseId, documentItem);
            receipt.AddItem(documentItem);

        }

        private async Task<Domain.Receipt> InsertRequest(Domain.Receipt receipt, CreateReceiptCommand request, CancellationToken cancellationToken)
        {
            var item = request.ReceiptDocumentItems[0];
            var NewReceipt = _mapper.Map<Domain.Receipt>(request);


            var CodeVoucher = await CreateCodeVoucher(NewReceipt);
            NewReceipt.CodeVoucherGroupId = CodeVoucher.Id;
            NewReceipt.Id = default;
            await _receiptCommandsService.SerialFormula(NewReceipt, CodeVoucher.Code, cancellationToken);
            NewReceipt.DocumentNo = Convert.ToInt32(receipt.RequestNo);


            _receiptCommandsService.ReceiptBaseDataInsert(request.DocumentDate, NewReceipt);


            NewReceipt.ExpireDate = item.ExpireDate == null ? DateTime.Now.AddDays(30) : item.ExpireDate;

            NewReceipt = _receiptCommandsService.ConvertTagArray(request.Tags, NewReceipt);

            //--------------------------------------مشخص کردن که آیا این صوتحساب وارداتی است یا خیر؟

            NewReceipt = await _receiptCommandsService.UpdateImportPurchaseReceipt(NewReceipt);
            NewReceipt.CreditAccountReferenceId = null;
            NewReceipt.CreditAccountReferenceGroupId = null;

            foreach (var items in request.ReceiptDocumentItems)
            {

                var documentItem = new DocumentItem()
                {
                    CurrencyBaseId = 0,
                    CurrencyPrice = 1,
                    DocumentMeasureId = items.DocumentMeasureId,
                    MainMeasureId = items.MainMeasureId,
                    Quantity = items.QuantityChose,
                    Description = "",
                    SecondaryQuantity = items.SecondaryQuantity,
                    CommodityId = items.CommodityId,
                    RemainQuantity = items.QuantityChose,
                    IsWrongMeasure = items.IsWrongMeasure,
                    CreatedById = _currentUserAccessor.GetId(),
                    OwnerRoleId = _currentUserAccessor.GetRoleId(),
                };

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