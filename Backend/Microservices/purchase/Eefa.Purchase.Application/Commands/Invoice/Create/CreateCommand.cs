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
using Eefa.Purchase.Domain.Aggregates.InvoiceAggregate;
using Eefa.Purchase.Domain.Common;
using Eefa.Purchase.Domain.Entities;
using Eefa.Purchase.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Purchase.Application.Commands.Invoice.Create
{
    public class CreateCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateCommand>, ICommand
    {
       
        public int CodeVoucherGroupId { get; set; } = default!;
        public int RequesterReferenceId { get; set; } = default!;
        public int FollowUpReferenceId { get; set; } = default!;

        public int WarehouseId { get; set; } = default!;

        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; }

        /// <description>
        /// کد مرجع
        ///</description>

        public int? CreditAccountReferenceId { get; set; }
        public int? CreditAccountReferenceGroupId { get; set; }

        /// <description>
        /// توضیحات سند
        ///</description>

        public string DocumentDescription { get; set; }

        /// <description>
        /// دستی
        ///</description>
        public bool IsManual { get; set; } = default!;


        /// <description>
        /// درصد تخفیف کل فاکتور
        ///</description>

        public double? DiscountPercent { get; set; }

        /// <description>
        /// نوع پرداخت
        ///</description>

        public int PaymentTypeBaseId { get; set; } = default!;

        /// <description>
        /// تاریخ انقضا
        ///</description>

        public DateTime? ExpireDate { get; set; }

        /// <description>
        /// شماره بخش
        ///</description>
        public string SerialFormulaBaseValueUniquename { get; set; }

        public string PartNumber { get; set; }

        public int RequestNumber { get; set; }
        /// تاریخ سند
        ///</description>

        public DateTime DocumentDate { get; set; } = default!;

        /// <description>
        /// شماره فاکتور فروشنده
        ///</description>
        public string InvoiceNo { get; set; }
        public string Tags { get; set; }
        public long vatDutiesTax { get; set; }
        public List<int> AttachmentIds { get; set; } = default!;
        public ICollection<InvoiceDocumentItemCreate> InvoiceDocumentItems { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCommand, Domain.Aggregates.InvoiceAggregate.Invoice>()
                .IgnoreAllNonExisting();
        }

    }
    public class InvoiceDocumentItemCreate : IMapFrom<DocumentItem>
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
        /// تعداد
        ///</description>

        public double Quantity { get; set; } = default!;

        public int? CurrencyBaseId { get; set; }

        /// <description>
        /// نرخ ارز
        ///</description>

        public double? CurrencyPrice { get; set; }

        public int DocumentMeasureId { get; set; }
        public int? MeasureUnitConversionId { get; set; }
        public string Description { get; set; }
        public bool IsWrongMeasure { get; set; } = default!;
       
        public void Mapping(Profile profile)
        {
            profile.CreateMap<InvoiceDocumentItemCreate, DocumentItem>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateCommandHandler : IRequestHandler<CreateCommand, ServiceResult>
    {
        private readonly IMapper _mapper;
        private readonly PurchaseContext _context;
        private readonly IRepository<Commodity> _commodityRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly IRepository<BaseValue> _baseValueRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IProcedureCallService _iProcedureCallService;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<DocumentHeadExtend> _documentHeadExtend;
        
        private readonly IRepository<CodeVoucherGroup> _codeVoucherGroupRepository;
        private readonly IRepository<Purchase.Domain.Attachment> _repositoryAttachment;
        private readonly IRepository<Domain.Aggregates.InvoiceAggregate.Invoice> _InvoiceRepository;
        private readonly IRepository<Domain.Aggregates.InvoiceAggregate.DocumentAttachment> _repositoryDocumentAttachments;


        public CreateCommandHandler(
              IMapper mapper
            , PurchaseContext context
            , IRepository<Document> documentRepository
            , ICurrentUserAccessor currentUserAccessor
            , IRepository<Commodity> commodityRepository
            , IRepository<BaseValue> baseValueRepository
            , IProcedureCallService iProcedureCallService
            , IRepository<DocumentItem> documentItemRepository
            , IRepository<DocumentHeadExtend> documentHeadExtend
            , IRepository<Purchase.Domain.Attachment> repositoryAttachment
            , IRepository<CodeVoucherGroup> codeVoucherGroupRepository
            , IRepository<Domain.Aggregates.InvoiceAggregate.Invoice> InvoiceRepository
            , IRepository<Domain.Aggregates.InvoiceAggregate.DocumentAttachment> repositoryDocumentAttachments

            )

        {
            _mapper = mapper;
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _commodityRepository = commodityRepository;
            _baseValueRepository = baseValueRepository;
            _InvoiceRepository = InvoiceRepository;
            _documentRepository = documentRepository;
            _documentHeadExtend = documentHeadExtend;
            _iProcedureCallService = iProcedureCallService;
            _repositoryAttachment = repositoryAttachment;
            _documentItemRepository = documentItemRepository;
            _codeVoucherGroupRepository = codeVoucherGroupRepository;
            _repositoryDocumentAttachments = repositoryDocumentAttachments;

        }


        public async Task<ServiceResult> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            if (request.InvoiceDocumentItems.Count() == 0)
            {
                throw new ValidationError("هیچ کالایی برای این سند وارد نشده است");
            }
            if (request.InvoiceDocumentItems.Where(a => a.ProductionCost <= 0).Count() > 0)
            {
                throw new ValidationError("مبلغ کل به درستی وارد نشده است");
            }
            if (request.InvoiceDocumentItems.Where(a => a.UnitPrice <= 0).Count() > 0)
            {
                throw new ValidationError("قیمت واحد به درستی وارد نشده است");
            }
            if (request.InvoiceDocumentItems.Where(a => a.Quantity <= 0).Count() > 0)
            {
                throw new ValidationError("تعداد کالا به طور صحیح وارد نشده است");
            }

            var year = await _context.Years.Where(a => a.IsCurrentYear).FirstOrDefaultAsync();
            if (year == null)
            {
                throw new ValidationError("سال مالی تنظیم نشده است");
            }




            var Invoice = _mapper.Map<Domain.Aggregates.InvoiceAggregate.Invoice>(request);
            Invoice.ExpireDate = Domain.Aggregates.InvoiceAggregate.Invoice.SetExpireDate(request.ExpireDate, year.LastDate);
            Invoice.YearId = _currentUserAccessor.GetYearId();
            Invoice.TotalWeight = default;
            Invoice.TotalQuantity = default;
            Invoice.DocumentDiscount = default;
            Invoice.DiscountPercent = default;
            Invoice.DocumentStateBaseId = ConstantValues.BaseValue.NotChecked;
           
            Invoice.PaymentTypeBaseId = 1;
            if (request.CodeVoucherGroupId == 0)
            {
                var codeVoucherGroup = (await _codeVoucherGroupRepository
               .GetAll().Where(x => x.UniqueName == ConstantValues.CodeVoucherGroup.ContractVoucherGroup)
               .FirstOrDefaultAsync());


                Invoice.CodeVoucherGroupId = codeVoucherGroup.Id;
            }

            long vatDutiesTax = 0;
            //----------------بدست آوردن درصد مالیات ارزش افزوده------------------------
            var VatPercentage = _context.BaseValues.Where(a => a.UniqueName.ToLower() == ConstantValues.BaseValue.vatDutiesTax.ToLower()).Select(a => a.Value).FirstOrDefault();
            //----------------جمع کل اقلام در یک فاکتور----------------------------------
            var TotalItemPrice = request.InvoiceDocumentItems.Sum(a => a.UnitPrice * a.Quantity);


            if (request.vatDutiesTax > 0)
            {

                if (VatPercentage == null)
                {
                    throw new ValidationError("درصد محاسبه مالیات ارزش افزوده وجود ندارد");
                }

                vatDutiesTax = (Convert.ToInt64(TotalItemPrice) * Convert.ToInt32(VatPercentage)) / 100;
            }

            Invoice.InvoiceNo = request.InvoiceNo;
            Invoice.CreditAccountReferenceId = request.CreditAccountReferenceId;
            Invoice.CreditAccountReferenceGroupId = request.CreditAccountReferenceGroupId;
            Invoice.TotalItemPrice = Convert.ToInt64(TotalItemPrice) + vatDutiesTax;
            Invoice.TotalProductionCost = Convert.ToInt64(TotalItemPrice);
            Invoice.VatDutiesTax = vatDutiesTax;
            Invoice.VatPercentage = Convert.ToInt32(VatPercentage);
            Invoice.CommandDescription = "Command: CreateCommand -Purchase-codeVoucherGroup.id=" + request.CodeVoucherGroupId.ToString();

            var CurrencyBaseId = _baseValueRepository.GetAll().Where(a => a.UniqueName == ConstantValues.BaseValue.currencyIRR).Select(a => a.Id).FirstOrDefault();

            foreach (var documentItem in _mapper.Map<ICollection<DocumentItem>>(request.InvoiceDocumentItems))
            {

                documentItem.CurrencyBaseId = CurrencyBaseId;
                documentItem.CurrencyPrice = 1;
                //محاسبه حدود قیمت 
                var PriceBuyItems = await _iProcedureCallService.GetPriceBuy(documentItem.CommodityId);
                documentItem.UnitBasePrice = PriceBuyItems != null ? PriceBuyItems.AveragePurchasePrice : 0;
                //--------------------------
                Invoice.AddItem(documentItem);


            }

            var serialFormula = (await _baseValueRepository
                .GetAll(x =>
                    x.ConditionExpression(c =>
                        c.UniqueName == ConstantValues.BaseValue.UtilityDocument)
                        )
                .FirstOrDefaultAsync(
                    cancellationToken:
                    cancellationToken)).Value;


            foreach (var s in serialFormula.Split("-"))
            {
                switch (s)
                {
                    case "[YearId]":
                        Invoice.DocumentSerial += _currentUserAccessor.GetYearId() + "-";
                        break;
                    case "[BranchId]":
                        Invoice.DocumentSerial += _currentUserAccessor.GetBranchId() + "-";
                        break;
                    default:
                        if (s.StartsWith("[\"") && s.EndsWith("\"]"))
                        {
                            var temp = s.Remove(0, 2);
                            temp = temp.Remove(temp.Length - 2, 2);
                            Invoice.DocumentSerial += temp + "-";
                        }
                        break;
                }
            }

            if (Invoice.DocumentSerial.EndsWith("-"))
            {
                Invoice.DocumentSerial =
                    Invoice.DocumentSerial.Remove(Invoice.DocumentSerial.Length - 1, 1);
            }


            var lastNo = (await _InvoiceRepository.GetAll()
                .Where(x => x.YearId == _currentUserAccessor.GetYearId())
                .Select(x => x.DocumentNo).MaxAsync(cancellationToken: cancellationToken)) ?? 0;

            Invoice.DocumentNo = lastNo + 1;
            Invoice.RequestNo = request.RequestNumber > 0 ? request.RequestNumber.ToString() : Invoice.DocumentNo.ToString();
            Invoice = Domain.Aggregates.InvoiceAggregate.Invoice.ConvertTagArray(request.Tags, Invoice);
            
            
            
            if (await Domain.Aggregates.InvoiceAggregate.Invoice.AddInvoiceAsync(Invoice, _InvoiceRepository) > 0)
            {
                //---------------------insert Document --------------------------
                Document document = AddDocument(Invoice, lastNo);

                if (await Domain.Aggregates.InvoiceAggregate.Invoice.AddDocumentAsync(document, _documentRepository) > 0)
                {
                    //---------------------insert documentHeadExtend --------------------------
                    if (request.RequesterReferenceId > 0)
                    {
                        await AdddocumentHeadExtend(request, Invoice);
                        await _iProcedureCallService.ModifyDocumentAttachments(request.AttachmentIds, Invoice.Id);

                    }
                    return ServiceResult.Success();


                }
                else
                {
                    return ServiceResult.Failed();
                }
            }
            else
            {
                return ServiceResult.Failed();
            }



        }


        private static Document AddDocument(Domain.Aggregates.InvoiceAggregate.Invoice Invoice, int lastNo)
        {
            Document document = new Document();
            document.DocumentNo = lastNo + 1;
            document.DocumentDate = Invoice.DocumentDate;
            document.ReferenceId = Convert.ToInt32(Invoice.CreditAccountReferenceId);
            document.DocumentTypeBaseId = Invoice.DocumentStateBaseId;
            document.DocumentId = Invoice.Id;
            return document;
        }

        private async Task AdddocumentHeadExtend(CreateCommand request, Domain.Aggregates.InvoiceAggregate.Invoice Invoice)
        {
            DocumentHeadExtend documentHeadExtend = new DocumentHeadExtend();
            documentHeadExtend.DocumentHeadId = Invoice.Id;
            if (request.RequesterReferenceId > 0)
            {
                documentHeadExtend.RequesterReferenceId = request.RequesterReferenceId;
            }
            if (request.FollowUpReferenceId > 0)
            {
                documentHeadExtend.FollowUpReferenceId = request.FollowUpReferenceId;
            }

            documentHeadExtend.CorroborantReferenceId = request.RequesterReferenceId;

            await Domain.Aggregates.InvoiceAggregate.Invoice.AddDocumentHeadExtendAsync(documentHeadExtend, _documentHeadExtend);


        }
        

    }
}