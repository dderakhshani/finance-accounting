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
    public class UpdateLeavingWarehouseMaterialCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<InsertDocumentHeads>, ICommand
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
        public ICollection<UpdateDocumentItems> ReceiptDocumentItems { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateLeavingWarehouseMaterialCommand, Domain.Receipt>()
                .IgnoreAllNonExisting();
        }



    }
    public class UpdateDocumentItems : IMapFrom<InsertDocumentItems>
    {
        public int? Id { get; set; }
       
        public int CommodityId { get; set; }
        public int MainMeasureId { get; set; }
        public int BomValueHeaderId { get; set; }
        public float Quantity { get; set; }
        public int DocumentMeasureId { get; set; }
        public string Description { get; set; }
        public int WarehouseLayoutId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateDocumentItems, InsertDocumentItems>().ForMember(dest => dest.DocumentItemId, opt => opt.MapFrom(src => src.Id));

        }

    }

    public class UpdateLeavingWarehouseMaterialCommandHandler : IRequestHandler<UpdateLeavingWarehouseMaterialCommand, ServiceResult<Domain.Receipt>>
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

        public UpdateLeavingWarehouseMaterialCommandHandler(
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
        public async Task<ServiceResult<Domain.Receipt>> Handle(UpdateLeavingWarehouseMaterialCommand request, CancellationToken cancellationToken)
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

            var list = _mapper.Map<List<InsertDocumentItems>>(request.ReceiptDocumentItems.ToList());

            
            if (ToWarehouseId>0)
            {
                await toWarehouse(request, currency, list);

            }

            if (FromWarehouseId >0)
            {
                await fromWarehouse(request, currency, list);

            }


            return ServiceResult<Domain.Receipt>.Success(ToReceipt);
        }

        private async Task fromWarehouse(UpdateLeavingWarehouseMaterialCommand request, int currency, List<InsertDocumentItems> list)
        {
            InsertDocumentHeadsForIOCommodity fromReceiptParam = new InsertDocumentHeadsForIOCommodity();
            fromReceiptParam.receiptdocument = _mapper.Map<InsertDocumentHeads>(request);

            
            fromReceiptParam.receiptdocument.DefaultLayoutId = Fromlayous != null ? Fromlayous.Id : 0;
            fromReceiptParam.receiptdocument.receiptDocumentItems = list;
            fromReceiptParam.userId = _currentUserAccessor.GetId();
            fromReceiptParam.yearId = _currentUserAccessor.GetYearId();
            fromReceiptParam.OwnerRoleId = _currentUserAccessor.GetRoleId();
            fromReceiptParam.receiptdocument.WarehouseId = FromReceipt.WarehouseId;
            
            fromReceiptParam.receiptdocument.CurrencyBaseId = currency;
            fromReceiptParam.receiptdocument.DocumentHeadId = FromReceipt.Id;

            //اگر جابه جایی انبار داشته باشیم سند خروج می زنیم برای اینکه در هنگام ویرایش به آن احتیاج داریم ولی نمی خواهیم موقع ریالی شدن این سند خروج آورده شود
            fromReceiptParam.receiptdocument.Tags = _receiptCommandsService.ConvertTagArray(request.Tags);
            fromReceiptParam.receiptdocument.InvoiceNo = _receiptCommandsService.GenerateInvoiceNumber(ConstantValues.WarehouseInvoiceNoEnam.LeaveMaterail, codeVoucherGroup);
            fromReceiptParam.receiptdocument.Mode = (int)(WarehouseHistoryMode.Exit);
            fromReceiptParam.receiptdocument.CodeVoucherGroupUniqueName = codeVoucherGroup.UniqueName;
            fromReceiptParam.receiptdocument.ParentId = ToReceipt != null && ToReceipt.Id > 0 ? ToReceipt.Id : null;


            foreach (var item in list)
            {
                item.WarehouseLayoutId = fromReceiptParam.receiptdocument.DefaultLayoutId;
            }
            FromReceipt = await _procedureCallService.UpdateDocumentHeadsForIOCommodityMaterial(fromReceiptParam);
        }

        private async Task toWarehouse(UpdateLeavingWarehouseMaterialCommand request, int currency, List<InsertDocumentItems> list)
        {
            InsertDocumentHeadsForIOCommodity toReceiptParam = new InsertDocumentHeadsForIOCommodity();
            toReceiptParam.receiptdocument = _mapper.Map<InsertDocumentHeads>(request);
            toReceiptParam.receiptdocument.receiptDocumentItems = list;
            toReceiptParam.userId = _currentUserAccessor.GetId();
            toReceiptParam.yearId = _currentUserAccessor.GetYearId();
            toReceiptParam.OwnerRoleId = _currentUserAccessor.GetRoleId();
            toReceiptParam.receiptdocument.WarehouseId = ToReceipt.WarehouseId;
            toReceiptParam.receiptdocument.DefaultLayoutId = Tolayous != null ? Tolayous.Id : 0;
            toReceiptParam.receiptdocument.CurrencyBaseId = currency;

            toReceiptParam.receiptdocument.DocumentHeadId = ToReceipt.Id;
           
            toReceiptParam.receiptdocument.Mode = (int)(WarehouseHistoryMode.Enter);

            foreach (var item in list)
            {
                item.WarehouseLayoutId = toReceiptParam.receiptdocument.DefaultLayoutId;
            }

            ToReceipt = await _procedureCallService.UpdateDocumentHeadsForIOCommodityMaterial(toReceiptParam);
        }

        
    }
}