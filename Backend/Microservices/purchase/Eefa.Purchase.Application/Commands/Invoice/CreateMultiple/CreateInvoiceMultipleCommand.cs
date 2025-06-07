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

namespace Eefa.Purchase.Application.Commands.Invoice.CreateMultiple
{
    public class CreateInvoiceMultipleCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateInvoiceMultipleCommand>, ICommand
    {
        public int? AccountReferencesGroupId { get; set; }
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

        public int? ReferenceId { get; set; }

        /// <description>
        /// توضیحات سند
        ///</description>

        public string? DocumentDescription { get; set; }

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

        public string? PartNumber { get; set; }

        public int RequestNumber { get; set; }
        /// تاریخ سند
        ///</description>

        public string _DocumentDate { get; set; } = default!;

        /// <description>
        /// شماره فاکتور فروشنده
        ///</description>
        public string InvoiceNo { get; set; }
        public string Tag { get; set; }

        public ICollection<InvoiceDocumentItemCommand> InvoiceDocumentItems { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateInvoiceMultipleCommand, Domain.Aggregates.InvoiceAggregate.Invoice>()
                .IgnoreAllNonExisting();
        }

        public class InvoiceDocumentItemCommand : IMapFrom<DocumentItem>
        {
            public int Id { get; set; }
            public int CommodityId { get; set; } = default!;

            public int? RequesterReferenceId { get; set; } = default!;
            public int? FollowUpReferenceId { get; set; } = default!;
            /// <description>
            /// سریال کالا
            ///</description>

            public string? CommoditySerial { get; set; }
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

            public double Weight { get; set; } = default!;

            /// <description>
            /// تعداد
            ///</description>

            public double Quantity { get; set; } = default!;

            public int? CurrencyBaseId { get; set; }

            /// <description>
            /// نرخ ارز
            ///</description>

            public int? CurrencyPrice { get; set; }

            /// <description>
            /// تخفیف
            ///</description>

            public long Discount { get; set; } = default!;

            /// <description>
            /// نقش صاحب سند
            ///</description>
            public int DocumentMeasureId { get; set; }
            public int? MeasureUnitConversionId { get; set; }
            public string? Description { get; set; }

            public string InvoiceNo { get; set; }
            public string RequestNo { get; set; }
            public int? ReferenceId { get; set; }
            /// <description>
            /// تاریخ انقضا
            ///</description>

            public DateTime? ExpireDate { get; set; }

            public void Mapping(Profile profile)
            {
                profile.CreateMap<InvoiceDocumentItemCommand, DocumentItem>()
                    .IgnoreAllNonExisting();
            }
        }
    }

    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceMultipleCommand, ServiceResult>
    {
        private readonly IRepository<Domain.Aggregates.InvoiceAggregate.Invoice> _InvoiceRepository;
        private readonly IRepository<Commodity> _commodityRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly IRepository<BaseValue> _baseValueRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRepository<DocumentHeadExtend> _documentHeadExtend;
        private readonly PurchaseContext _contex;



        public CreateInvoiceCommandHandler(
              IRepository<Domain.Aggregates.InvoiceAggregate.Invoice> InvoiceRepository
            , ICurrentUserAccessor currentUserAccessor
            , IRepository<Commodity> commodityRepository
            , IRepository<BaseValue> baseValueRepository
            , IRepository<Document> documentRepository
            , IRepository<DocumentHeadExtend> documentHeadExtend
            , PurchaseContext contex

            , IMapper mapper
            )

        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _commodityRepository = commodityRepository;
            _baseValueRepository = baseValueRepository;
            _InvoiceRepository = InvoiceRepository;
            _documentRepository = documentRepository;
            _documentHeadExtend = documentHeadExtend;
            _contex = contex;
        }
        public async Task<ServiceResult> Handle(CreateInvoiceMultipleCommand request, CancellationToken cancellationToken)
        {
            var layous = _contex.WarehouseLayouts.Where(_a => _a.WarehouseId == request.WarehouseId && _a.LastLevel==true).FirstOrDefault();
            if (layous == null)
            {
                throw new ValidationError("برای انبار انتخاب شده در این سند هیچ موقعیت بندی آخرین سطح تعریف نشده است");
            }
            var codeVoucherGroup = await _contex.CodeVoucherGroups.Where(t => t.UniqueName == ConstantValues.CodeVoucherGroup.ContractVoucherGroup).FirstOrDefaultAsync();
            var CurrencyBaseId = _baseValueRepository.GetAll().Where(a => a.UniqueName == ConstantValues.BaseValue.currencyIRR).Select(a => a.Id).FirstOrDefault();
            var serialFormula = (await _baseValueRepository
                   .GetAll(x =>
                       x.ConditionExpression(c =>
                           c.UniqueName == ConstantValues.BaseValue.UtilityDocument)
                           )
                   .FirstOrDefaultAsync(
                       cancellationToken:
                       cancellationToken)).Value;
            string DocumentSerial=null;
            foreach (var s in serialFormula.Split("-"))
            {
                switch (s)
                {
                    case "[YearId]":
                        DocumentSerial += _currentUserAccessor.GetYearId() + "-";
                        break;
                    case "[BranchId]":
                        DocumentSerial += _currentUserAccessor.GetBranchId() + "-";
                        break;
                    default:
                        if (s.StartsWith("[\"") && s.EndsWith("\"]"))
                        {
                            var temp = s.Remove(0, 2);
                            temp = temp.Remove(temp.Length - 2, 2);
                            DocumentSerial += temp + "-";
                        }
                        break;
                }
            }

            if (DocumentSerial.EndsWith("-"))
            {
                DocumentSerial = DocumentSerial.Remove(DocumentSerial.Length - 1, 1);

            }
            

            foreach (var item in request.InvoiceDocumentItems)
            {
                var documentItem = _mapper.Map<DocumentItem>(item);
                var Invoice = _mapper.Map<Domain.Aggregates.InvoiceAggregate.Invoice>(request);
                Invoice.YearId = _currentUserAccessor.GetYearId();
                Invoice.TotalWeight = default;
                Invoice.TotalQuantity = default;
                Invoice.DocumentDiscount = default;
                Invoice.DiscountPercent = default;
                Invoice.DocumentStateBaseId = ConstantValues.BaseValue.NotChecked;
                Invoice.PaymentTypeBaseId = 1;
                Invoice.DocumentDate = Convert.ToDateTime(request._DocumentDate); //DateTime.Now;
                Invoice.RequestNo = item.RequestNo;
                Invoice.CreditAccountReferenceId = item.ReferenceId;
                Invoice.InvoiceNo = item.InvoiceNo;
                Invoice.WarehouseId = request.WarehouseId;
                Invoice.CodeVoucherGroupId = codeVoucherGroup.Id;
                Invoice.DocumentSerial = DocumentSerial;
                Invoice.ExpireDate = item.ExpireDate;
                //--------------------------------------مشخص کردن که آیا این صوتحساب وارداتی است یا خیر؟
                if (Invoice.CreditAccountReferenceId != null)
                {
                    var referenceGroupList = await _contex.AccountReferenceView.Where(r => r.Id == Invoice.CreditAccountReferenceId).ToListAsync();

                    Invoice = Domain.Aggregates.InvoiceAggregate.Invoice.UpdateImportPurchaseInvoice(Invoice, referenceGroupList);
                }
                
                //--------------------------------------------------------------------------


                documentItem.CurrencyBaseId = CurrencyBaseId;
                documentItem.CurrencyPrice = 1;
                documentItem.DocumentMeasureId = documentItem.DocumentMeasureId == null ? documentItem.MainMeasureId : documentItem.DocumentMeasureId;
                
                Invoice.AddItem(documentItem);




                var lastNo = (await _InvoiceRepository.GetAll()
                    .Where(x => x.DocumentSerial == Invoice.DocumentSerial)
                    .Select(x => x.DocumentNo).MaxAsync(cancellationToken: cancellationToken)) ?? 0;

                Invoice.DocumentNo = lastNo + 1;
                Invoice= Domain.Aggregates.InvoiceAggregate.Invoice.ConvertTagArray(request.Tag, Invoice);


                if (await Domain.Aggregates.InvoiceAggregate.Invoice.AddInvoiceAsync(Invoice, _InvoiceRepository) > 0)
                {
                    //---------------------insert Document --------------------------
                    Document document = new Document();
                    document.DocumentNo = lastNo + 1;
                    document.DocumentDate = DateTime.Now;
                    document.ReferenceId = Convert.ToInt32(Invoice.CreditAccountReferenceId);
                    document.DocumentTypeBaseId = Invoice.DocumentStateBaseId;
                    document.DocumentId = Invoice.Id;

                    if (await Domain.Aggregates.InvoiceAggregate.Invoice.AddDocumentAsync(document, _documentRepository) > 0)
                    {
                        //---------------------insert documentHeadExtend --------------------------
                        DocumentHeadExtend documentHeadExtend = new DocumentHeadExtend();
                        documentHeadExtend.DocumentHeadId = Invoice.Id;
                        documentHeadExtend.RequesterReferenceId = Convert.ToInt32(item.RequesterReferenceId);
                        documentHeadExtend.FollowUpReferenceId = Convert.ToInt32(item.FollowUpReferenceId);
                        documentHeadExtend.CorroborantReferenceId = Convert.ToInt32(item.RequesterReferenceId);


                        await Domain.Aggregates.InvoiceAggregate.Invoice.AddDocumentHeadExtendAsync(documentHeadExtend, _documentHeadExtend);
                        return ServiceResult.Success();
                    }
                    else
                    {
                        throw new ValidationError("اشکال در ثبت Document");
                    }
                }
                else
                {
                    throw new ValidationError("اشکال در ثبت DocumentHead");
                }

            }
            return ServiceResult.Success();

        }

    }
}