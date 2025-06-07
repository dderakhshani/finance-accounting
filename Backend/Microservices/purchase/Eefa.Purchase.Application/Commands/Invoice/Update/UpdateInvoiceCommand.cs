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

namespace Eefa.Purchase.Application.Commands.Invoice.Update
{
    public class UpdateInvoiceCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Domain.Aggregates.InvoiceAggregate.Invoice>, ICommand
    {
        public int Id { get; set; }
        /// <description>
        /// کد مرجع
        ///</description>

        public int? CreditAccountReferenceId { get; set; }
        public int? CreditAccountReferenceGroupId { get; set; }
        public int CodeVoucherGroupId { get; set; } = default!;

        public int WarehouseId { get; set; } = default!;
        public int? RequesterReferenceId { get; set; } = default!;
        public int? FollowUpReferenceId { get; set; } = default!;

        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; }

       

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

        public DateTime DocumentDate { get; set; } = default!;
        /// <description>
        /// شماره فاکتور فروشنده
        ///</description>
        public string InvoiceNo { get; set; }
        public string Tags { get; set; }

        public long? vatDutiesTax { get; set; }
        public long? TotalItemPrice { get; set; } = default!;
        public long? TotalProductionCost { get; set; } = default!;

        public ICollection<DocumentItem> InvoiceDocumentItems { get; set; }
        public List<int> AttachmentIds { get; set; } = default!;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateInvoiceCommand, Domain.Aggregates.InvoiceAggregate.Invoice>()
                .IgnoreAllNonExisting();
        }





    }


    public class UpdateWarehouseInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, ServiceResult>
    {
        private readonly IMapper _mapper;
        private readonly PurchaseContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRepository<BaseValue> _baseValueRepository;
        private readonly IProcedureCallService _iProcedureCallService;
        private readonly IRepository<DocumentItem> _documentItemRepository;

        private readonly IRepository<DocumentHeadExtend> _documentHeadExtendRepository;
        private readonly IRepository<Domain.Aggregates.InvoiceAggregate.Invoice> _InvoiceRepository;



        public UpdateWarehouseInvoiceCommandHandler(IRepository<Domain.Aggregates.InvoiceAggregate.Invoice> InvoiceRepository
            , IMapper mapper
            , PurchaseContext context
            , IRepository<DocumentItem> DocumentItem
            , IProcedureCallService iProcedureCallService
            , IRepository<BaseValue> baseValueRepository
            , IRepository<DocumentHeadExtend> documentHeadExtendRepository

       )
        {
            _mapper = mapper;
            _context = context;
            _InvoiceRepository = InvoiceRepository;
            _documentItemRepository = DocumentItem;
            _baseValueRepository = baseValueRepository;
            _iProcedureCallService = iProcedureCallService;
            _documentHeadExtendRepository = documentHeadExtendRepository;
        }

        public async Task<ServiceResult> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (request.InvoiceDocumentItems.Count() == 0)
            {
                throw new ValidationError("هیچ کالایی برای این سند وارد نشده است");
            }
            if (request.InvoiceDocumentItems.Where(a=>a.ProductionCost<=0).Count() > 0)
            {
                throw new ValidationError("مبلغ کل به درستی وارد نشده است");
            }
            if (request.InvoiceDocumentItems.Where(a => a.UnitPrice <= 0).Count() > 0)
            {
                throw new ValidationError("قیمت واحد به درستی وارد نشده است");
            }
            if (request.InvoiceDocumentItems.Where(a => a.Quantity<=0).Count() > 0)
            {
                throw new ValidationError("تعداد کالا به طور صحیح وارد نشده است");
            }
            var year = await _context.Years.Where(a => a.IsCurrentYear).FirstOrDefaultAsync();
            if (year == null)
            {
                throw new ValidationError("سال مالی تنظیم نشده است");
            }
            var entity = (Domain.Aggregates.InvoiceAggregate.Invoice)await _InvoiceRepository.Find(request.Id);
            _mapper.Map<UpdateInvoiceCommand, Domain.Aggregates.InvoiceAggregate.Invoice>(request, entity);


            //----------------بدست آوردن درصد مالیات ارزش افزوده------------------------
            var VatPercentage = _context.BaseValues.Where(a => a.UniqueName.ToLower() == ConstantValues.BaseValue.vatDutiesTax.ToLower()).Select(a => a.Value).FirstOrDefault();
            //----------------جمع کل اقلام در یک فاکتور----------------------------------
            var TotalItemPrice = request.InvoiceDocumentItems.Sum(a => a.UnitPrice * a.Quantity);

            long vatDutiesTax = 0;
            if (request.vatDutiesTax > 0)
            {

                if (VatPercentage == null)
                {
                    throw new ValidationError("درصد محاسبه مالیات ارزش افزوده وجود ندارد");
                }

                vatDutiesTax = (Convert.ToInt64(TotalItemPrice) * Convert.ToInt32(VatPercentage)) / 100;
            }
            entity.ExpireDate = Domain.Aggregates.InvoiceAggregate.Invoice.SetExpireDate(request.ExpireDate, year.LastDate);
            entity.InvoiceNo = request.InvoiceNo;
            entity.CreditAccountReferenceId = request.CreditAccountReferenceId;
            entity.CreditAccountReferenceGroupId = request.CreditAccountReferenceGroupId;
            entity.TotalItemPrice = Convert.ToInt64(TotalItemPrice) + vatDutiesTax;
            entity.TotalProductionCost = Convert.ToInt64(TotalItemPrice);
            entity.VatDutiesTax = vatDutiesTax;
            entity.VatPercentage = Convert.ToInt32(VatPercentage);
            

            var CurrencyBaseId = _baseValueRepository.GetAll().Where(a => a.UniqueName == ConstantValues.BaseValue.currencyIRR).Select(a => a.Id).FirstOrDefault();

            var documentList = await _documentItemRepository.GetAll().Where(a => a.DocumentHeadId == request.Id).ToListAsync();
            var ListId = request.InvoiceDocumentItems.Where(a => a.Id > 0).Select(a => a.Id).ToList();

            var DeleteList = documentList.Where(a => !ListId.Contains(a.Id)).ToList();

            foreach (var documentItem in request.InvoiceDocumentItems)
            {
                var item = await _documentItemRepository.Find(documentItem.Id);
                if (item != null)
                {
                    item.DocumentMeasureId = documentItem.DocumentMeasureId;
                    item.Quantity = documentItem.Quantity;
                    item.UnitPrice = documentItem.UnitPrice;
                    item.CurrencyBaseId = documentItem.CurrencyBaseId;
                    item.Description = documentItem.Description;
                    item.ProductionCost = documentItem.ProductionCost;
                    item.SecondaryQuantity = documentItem.SecondaryQuantity;
                    item.CommodityId = documentItem.CommodityId;
                    //محاسبه حدود قیمت 
                    var PriceBuyItems = await _iProcedureCallService.GetPriceBuy(documentItem.CommodityId);
                    item.UnitBasePrice = PriceBuyItems != null ? PriceBuyItems.AveragePurchasePrice : 0;

                    Domain.Aggregates.InvoiceAggregate.Invoice.UpdateDocumentItem(item, _documentItemRepository);
                }
                else
                {
                    var documentItems = _mapper.Map<DocumentItem>(documentItem);


                    documentItems.CurrencyBaseId = CurrencyBaseId;
                    documentItems.CurrencyPrice = 1;
                    entity.AddItem(documentItems);

                }

            }

            entity = Domain.Aggregates.InvoiceAggregate.Invoice.ConvertTagArray(request.Tags, entity);

            await Domain.Aggregates.InvoiceAggregate.Invoice.UpdateInvoiceAsync(entity, _InvoiceRepository);
            foreach (var item in DeleteList)
            {

                 _documentItemRepository.Delete(item);
                await _documentItemRepository.SaveChangesAsync();
            }
            var _documentHeadExtend = await _documentHeadExtendRepository.GetAll().Where(a => a.DocumentHeadId == request.Id).FirstOrDefaultAsync();
            if (request.RequesterReferenceId != null)
            {
                _documentHeadExtend.RequesterReferenceId = Convert.ToInt32(request.RequesterReferenceId);
            }
            if (request.FollowUpReferenceId != null)
            {
                _documentHeadExtend.FollowUpReferenceId = Convert.ToInt32(request.FollowUpReferenceId);
            }
            if (_documentHeadExtend != null)
            {

                await Domain.Aggregates.InvoiceAggregate.Invoice.UpdateDocumentHeadExtendAsync(_documentHeadExtend, _documentHeadExtendRepository);


            }
            await _iProcedureCallService.ModifyDocumentAttachments(request.AttachmentIds, entity.Id);
            return ServiceResult.Success();


        }
    }
}
