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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Inventory.Application
{
    public class InsertLeavingWarehouseMaterialCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<InsertDocumentHeads>, ICommand
    {
        public int CodeVoucherGroupId { get; set; }
        public int FromWarehouseId { get; set; }
        public int ToWarehouseId { get; set; }

        /// <description>
        /// توضیحات سند
        ///</description>
        public string DocumentDescription { get; set; }

        /// <description>
        /// دستی
        ///</description>
        public bool IsManual { get; set; }
        /// تاریخ سند
        ///</description>
        public DateTime DocumentDate { get; set; }

        public string Tags { get; set; }

        public int? ViewId { get; set; }
        public int? DebitAccountHeadId { get; set; }
        public int? CreditAccountHeadId { get; set; }
        public int? DebitAccountReferenceId { get; set; }
        public int? DebitAccountReferenceGroupId { get; set; }
        public int? CreditAccountReferenceId { get; set; }
        public int? CreditAccountReferenceGroupId { get; set; }
        public Nullable<bool> IsDocumentIssuance { get; set; }
        public ICollection<InsertDocumentItems> ReceiptDocumentItems { get; set; }



        public void Mapping(Profile profile)
        {
            profile.CreateMap<InsertLeavingWarehouseMaterialCommand, InsertDocumentHeads>()
                .IgnoreAllNonExisting();
        }



    }
    
    public class InsertLeavingWarehouseMaterialCommandHandler : IRequestHandler<InsertLeavingWarehouseMaterialCommand, ServiceResult<Domain.Receipt>>
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
        

        public InsertLeavingWarehouseMaterialCommandHandler(
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
        public async Task<ServiceResult<Domain.Receipt>> Handle(InsertLeavingWarehouseMaterialCommand request, CancellationToken cancellationToken)
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

                var list= request.ReceiptDocumentItems.ToList();

                
                if (request.ToWarehouseId > 0)
                {
                    await toWarehouse(request, currency, list);

                }

                if (request.FromWarehouseId > 0)
                {
                    await fromWarehouse(request, currency, list);

                }

                return ServiceResult<Domain.Receipt>.Success(ToReceipt != null && ToReceipt.Id > 0 ? ToReceipt : Fromreceipt) ;

            }
            catch (Exception ex)
            {
                if (!(ex is ValidationError))
                {
                    new LogWriter("InsertLeavingWarehouseMaterialCommand Error:" + ex.Message.ToString(), "LeavingMaterialWarehouse");
                    if (ex.InnerException != null)
                    {
                        new LogWriter("InsertLeavingWarehouseMaterialCommand InnerException:" + ex.InnerException.ToString(), "LeavingMaterialWarehouse");
                    }

                    if(ToReceipt!=null)
                    {
                        ToReceipt.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
                        _receiptRepository.Update(ToReceipt);
                    }
                    if (Fromreceipt != null)
                    {
                        Fromreceipt.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
                        _receiptRepository.Update(Fromreceipt);
                    }
                    

                    await _receiptRepository.SaveChangesAsync();
                   
                    throw new ValidationError("مشکل در انجام عملیات خروج حواله مربوطه بایگانی شد");

                }
                else { throw new ValidationError(ex.Message); };

            }
           
        }

        private async Task fromWarehouse(InsertLeavingWarehouseMaterialCommand request, int currency, List<InsertDocumentItems> list)
        {
            InsertDocumentHeadsForIOCommodity fromReceiptParam = new InsertDocumentHeadsForIOCommodity();
            fromReceiptParam.receiptdocument = _mapper.Map<InsertDocumentHeads>(request);

            fromReceiptParam.receiptdocument.receiptDocumentItems = list;
            fromReceiptParam.userId = _currentUserAccessor.GetId();
            fromReceiptParam.yearId = _currentUserAccessor.GetYearId();
            fromReceiptParam.OwnerRoleId = _currentUserAccessor.GetRoleId();
            fromReceiptParam.receiptdocument.WarehouseId = request.FromWarehouseId;
            fromReceiptParam.receiptdocument.DefaultLayoutId = Fromlayous != null ? Fromlayous.Id : 0;
            fromReceiptParam.receiptdocument.CurrencyBaseId = currency;

            //اگر جابه جایی انبار داشته باشیم سند خروج می زنیم برای اینکه در هنگام ویرایش به آن احتیاج داریم ولی نمی خواهیم موقع ریالی شدن این سند خروج آورده شود
            fromReceiptParam.receiptdocument.DocumentStauseBaseValue = codeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.RemoveMaterialWarhouse ? (int)DocumentStateEnam.invoiceAmountLeave : (int)DocumentStateEnam.Transfer;
            fromReceiptParam.receiptdocument.CommandDescription = $"Command:InsertLeavingWarehouseMaterialCommand - Method :InsertDocumentExit - جابه جایی انبار -codeVoucherGroup.id={request.CodeVoucherGroupId.ToString()}";
            fromReceiptParam.receiptdocument.Tags = _receiptCommandsService.ConvertTagArray(request.Tags);
            fromReceiptParam.receiptdocument.InvoiceNo = _receiptCommandsService.GenerateInvoiceNumber(ConstantValues.WarehouseInvoiceNoEnam.LeaveMaterail, codeVoucherGroup);
            fromReceiptParam.receiptdocument.Mode = (int)(WarehouseHistoryMode.Exit);
            fromReceiptParam.receiptdocument.CodeVoucherGroupUniqueName = codeVoucherGroup.UniqueName;
            fromReceiptParam.receiptdocument.ParentId = ToReceipt != null && ToReceipt.Id > 0 ? ToReceipt.Id : null;

            Fromreceipt = await _procedureCallService.InsertDocumentHeadsForIOCommodity(fromReceiptParam);
        }

        private async Task toWarehouse(InsertLeavingWarehouseMaterialCommand request, int currency, List<InsertDocumentItems> list)
        {
            InsertDocumentHeadsForIOCommodity toReceiptParam = new InsertDocumentHeadsForIOCommodity();
            toReceiptParam.receiptdocument = _mapper.Map<InsertDocumentHeads>(request);
            toReceiptParam.receiptdocument.receiptDocumentItems = list;
            toReceiptParam.userId = _currentUserAccessor.GetId();
            toReceiptParam.yearId = _currentUserAccessor.GetYearId();
            toReceiptParam.OwnerRoleId = _currentUserAccessor.GetRoleId();
            toReceiptParam.receiptdocument.WarehouseId = request.ToWarehouseId;
            toReceiptParam.receiptdocument.DefaultLayoutId = Tolayous != null ? Tolayous.Id : 0;
            toReceiptParam.receiptdocument.CurrencyBaseId = currency;

            
            toReceiptParam.receiptdocument.DocumentStauseBaseValue = (int)DocumentStateEnam.invoiceAmount;
            toReceiptParam.receiptdocument.CommandDescription = $"Command:LeavingMaterialWarehouseCommand - Method :InsertDocumentInput - ورود از انبار {request.CodeVoucherGroupId.ToString()}";

            toReceiptParam.receiptdocument.Mode = (int)(WarehouseHistoryMode.Enter);

            ToReceipt = await _procedureCallService.InsertDocumentHeadsForIOCommodity(toReceiptParam);
        }

    }
}