using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
using Eefa.Invertory.Infrastructure.Context;
using Eefa.Invertory.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Inventory.Application
{
    public class ConvertToRailsReceiptCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Domain.Receipt>, ICommand
    {
        public int? EditType { get; set; } = default!;
        public string InvoiceNo { get; set; }
        public string FinancialOperationNumber { get; set; }
        public string Tags { get; set; }
        public int? DebitAccountHeadId { get; set; } = default!;
        public int? DebitAccountReferenceId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; } = default!;
        public int CreditAccountHeadId { get; set; } = default!;
        public int? CreditAccountReferenceId { get; set; } = default!;
        public int? CreditAccountReferenceGroupId { get; set; } = default!;
        public long? VatDutiesTax { get; set; } = default!;
        public long? ExtraCost { get; set; } = default!;
        public double? ExtraCostCurrency { get; set; } = default!;
        public string documentDescription { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public Nullable<int> ExtraCostAccountHeadId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceGroupId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceId { get; set; }
        public ICollection<RialsReceiptDocumentItemCommand> ReceiptDocumentItems { get; set; }
        public List<int> AttachmentIds { get; set; } = default!;
        //--------------مورد نیاز در اصلاحیه سند-------------
        public int? VoucherHeadId { get; set; }
        public int? DocumentId { get; set; }

        public bool? IsNegative { get; set; }

        public string ScaleBill { get; set; } = default!;
        public Nullable<int> CorrectionRequestId { get; set; } = default!;

        public bool? IsFreightChargePaid { get; set; } = default!;


        public class RialsReceiptDocumentItemCommand : IMapFrom<DocumentItem>
        {
            public int Id { get; set; }
            public int CommodityId { get; set; } = default!;
            public int DocumentHeadId { get; set; } = default!;
            public double UnitPrice { get; set; } = default!;
            public double? UnitPriceWithExtraCost { get; set; } = default!;
            public double CurrencyPrice { get; set; } = default!;
            public double CurrencyRate { get; set; } = default!;
            public int CurrencyBaseId { get; set; } = default!;
            public string Description { get; set; }
            public double productionCost { get; set; } = default!;
            //--------------مورد نیاز در اصلاحیه سند-------------
            public double Quantity { get; set; } = default!;

        }

    }

    public class ConvertToRailsReceiptCommandHandler : IRequestHandler<ConvertToRailsReceiptCommand, ServiceResult>
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<DocumentItem> _DocumentItemRepository;
        private readonly IRepository<CodeVoucherGroup> _codeVoucherGroupRepository;
        public ConvertToRailsReceiptCommandHandler(

              IMapper mapper
            , IInvertoryUnitOfWork context
            , IReceiptRepository receiptRepository
            , IRepository<DocumentItem> DocumentItem
            , IReceiptCommandsService receiptCommandsService
            , IRepository<CodeVoucherGroup> codeVoucherGroupRepository
            )
        {
            _mapper = mapper;
            _context = context;
            _receiptRepository = receiptRepository;
            _DocumentItemRepository = DocumentItem;
            _receiptCommandsService = receiptCommandsService;
            _codeVoucherGroupRepository = codeVoucherGroupRepository;
        }

        public async Task<ServiceResult> Handle(ConvertToRailsReceiptCommand request, CancellationToken cancellationToken)
        {
            if (!request.ReceiptDocumentItems.Any())
            {
                throw new ValidationError("هیچ کالایی برای قیمت گذاری وجود ندارد");
            }

            if (request.ReceiptDocumentItems.Where(a => a.UnitPrice == 0).Count() > 0)
            {
                throw new ValidationError("قیمت صفر قابل ثبت نمی باشد");
            }

            var DocumentHeadIds = request.ReceiptDocumentItems.Select(a => a.DocumentHeadId);
            if (!DocumentHeadIds.Any())
            {
                throw new ValidationError("اقلام این سند برای قیمت گذاری وجود ندارد");
            }
            var receipts = await _receiptRepository.GetAll().Where(a => DocumentHeadIds.Contains(a.Id)).ToListAsync();

            if (request.EditType != 4)
            {
                await documentItemUpdate(request, receipts);

                await ReceiptUpdate(request, receipts, cancellationToken);
            }
            else
            {
                await __documentItemUpdateExtraCost(request);

                await __ReceiptUpdateExtraCost(request, DocumentHeadIds, cancellationToken);
            }



            return ServiceResult.Success();
        }

        private async Task ReceiptUpdate(ConvertToRailsReceiptCommand request,List<Receipt> receipts, CancellationToken cancellationToken)
        {
            
            var entityGroupBy = receipts.GroupBy(a => a.DocumentDate).ToList();
           

            //----------------بدست آوردن درصد مالیات ارزش افزوده------------------------
            var VatPercentage = await _context.BaseValues.Where(a => a.UniqueName.ToLower() == ConstantValues.ConstBaseValue.vatDutiesTax.ToLower()).Select(a => a.Value).FirstOrDefaultAsync();
            if (VatPercentage == null)
            {
                throw new ValidationError("درصد محاسبه مالیات ارزش افزوده وجود ندارد");
            }
            foreach (var receipt in entityGroupBy)
            {

                var Receipt = receipt.FirstOrDefault();
                //اطلاعات مورد نیاز جدول Document
                if (Receipt != null)
                {


                    Receipt.DocumentDescription = request.documentDescription;
                    Receipt.FinancialOperationNumber = request.FinancialOperationNumber;

                }
                //اگر یک سطر بود بتوان شماره صورتحساب داده شود.
                if (receipts.Count() == 1)
                {
                    Receipt.InvoiceNo = request.InvoiceNo;
                }
                var documentId = await _receiptCommandsService.InsertAndUpdateDocument(Receipt);
                var documentHeads = await _receiptRepository.GetAll().Where(a => a.DocumentId == documentId).ToListAsync();
                await DeleteDocuments(receipt.ToList(), documentHeads);

                await UpdatePrice(request, receipt.ToList(), VatPercentage, documentId);

                //---------------------Insert Attachments----------------------------------
                await _receiptCommandsService.ModifyDocumentAttachments(request.AttachmentIds, documentId);
            }
            //حذف شده ها

            await _receiptRepository.SaveChangesAsync(cancellationToken);
        }

        private async Task UpdatePrice(ConvertToRailsReceiptCommand request, List<Receipt> receipts, string VatPercentage, int documentId)
        {
            int i = 0;
            foreach (var receipt in receipts)
            {

                if (receipt.ViewId != 122)
                {
                    CodeVoucherGroup NewCodeVoucherGroup = await CreateCodeVoucher(receipt);
                    receipt.CodeVoucherGroupId = NewCodeVoucherGroup.Id;
                }

                receipt.DocumentId = documentId;
                receipt.InvoiceDate = request.InvoiceDate;
                receipt.ScaleBill = request.ScaleBill;
                DebitAndCredit(request, receipt);
                if (!await _receiptCommandsService.IsValidAccountHeadRelationByReferenceGroup(receipt.DebitAccountHeadId, receipt.DebitAccountReferenceGroupId))
                {
                    throw new ValidationError("عدم تطابق گروه حساب بستانکار و سرفصل حساب بستانکار");
                }
                if (!await _receiptCommandsService.IsValidAccountHeadRelationByReferenceGroup(receipt.CreditAccountHeadId, receipt.CreditAccountReferenceGroupId))
                {
                    throw new ValidationError("عدم تطابق گروه حساب بدهکار و سرفصل حساب بدهکار");
                }

                await _receiptCommandsService.UpdateImportPurchaseReceipt(receipt);
                await UpdateExtraCost(request, receipt, i);

                i++;


                receipt.VatDutiesTax = Convert.ToInt64(request.VatDutiesTax);
                receipt.VatPercentage = request.VatDutiesTax > 0 ? Convert.ToInt32(VatPercentage) : 0;
                receipt.FinancialOperationNumber = request.FinancialOperationNumber;

                await _receiptCommandsService.CalculateTotalItemPrice(receipt);




                _receiptCommandsService.ConvertTagArray(request.Tags, receipt);
                _receiptRepository.Update(receipt);

            };

        }

        private async Task UpdateExtraCost(ConvertToRailsReceiptCommand request, Receipt receipt, int i)
        {
            receipt.ExtraCost = 0;//این خط باید باشد ، چون مقدار را به صفر برگرداند خالی شود
            if (request.ExtraCost > 0)
            {
                //وارداتی
                if (i == 0 && receipt.IsImportPurchase == true)
                {
                    receipt.ExtraCost = request.ExtraCost;
                }
                else //داخلی
                {
                    var ExtraCost = await _context.DocumentHeadExtraCost.Where(a => a.DocumentHeadId == receipt.Id && !a.IsDeleted).SumAsync(a => a.ExtraCostAmount);
                    if (ExtraCost > 0)
                    {
                        receipt.ExtraCost = Convert.ToInt64(ExtraCost);
                        
                    }
                }
                receipt.ExtraCostAccountHeadId = request.ExtraCostAccountHeadId;
                receipt.ExtraCostAccountReferenceGroupId = request.ExtraCostAccountReferenceGroupId;
                receipt.ExtraCostAccountReferenceId = request.ExtraCostAccountReferenceId;
                if (receipt.IsImportPurchase == false)
                {
                    receipt.IsFreightChargePaid = request.IsFreightChargePaid == null ? false : true;
                }
            }
            if (request.ExtraCostCurrency > 0)
            {
                if (request.IsNegative == true)
                {
                    receipt.ExtraCostCurrency = -1 * request.ExtraCostCurrency;
                }
                else
                {
                    receipt.ExtraCostCurrency = request.ExtraCostCurrency;
                }
            }
            else
            {
                receipt.ExtraCostCurrency = 0;
            }
        }

       

        private async Task DeleteDocuments(List<Receipt> receipts, List<Receipt> documentHeads)
        {
            var documentsDelete = documentHeads.Where(a => !receipts.Select(b => b.Id).Contains(a.Id) && a.DocumentId > 0).ToList();
            //حذف شده ها
            foreach (var item in documentsDelete)
            {
                item.DocumentId = null;
                item.VoucherHeadId = null;

                switch (item.DocumentStauseBaseValue)
                {

                    case (int)DocumentStateEnam.invoiceAmount:
                        item.DocumentStauseBaseValue = (int)DocumentStateEnam.Direct;
                        break;

                    case (int)DocumentStateEnam.registrationAccounting:
                        if (item.DocumentId == null)
                            item.DocumentStauseBaseValue = (int)DocumentStateEnam.Direct;
                        break;

                    default:

                        break;
                }
                CodeVoucherGroup NewCodeVoucherGroup = await _receiptCommandsService.GetNewCodeVoucherGroup(item);
                item.CodeVoucherGroupId = NewCodeVoucherGroup.Id;

            }
        }

        private static void DebitAndCredit(ConvertToRailsReceiptCommand request, Receipt receipt)
        {
            if (request.DebitAccountReferenceId != -1)
            {
                receipt.DebitAccountReferenceId = request.DebitAccountReferenceId;
                receipt.DebitAccountReferenceGroupId = request.DebitAccountReferenceGroupId;
            }
            if (request.CreditAccountReferenceId != -1)
            {
                receipt.CreditAccountReferenceId = request.CreditAccountReferenceId;
                receipt.CreditAccountReferenceGroupId = request.CreditAccountReferenceGroupId;
            }
            receipt.DebitAccountHeadId = request.DebitAccountHeadId != -1 ? request.DebitAccountHeadId : receipt.DebitAccountHeadId;
            receipt.CreditAccountHeadId = request.CreditAccountHeadId != -1 ? request.CreditAccountHeadId : receipt.CreditAccountHeadId;

        }

        private async Task documentItemUpdate(ConvertToRailsReceiptCommand request, List<Receipt> receipts)
        {
            foreach (var documentItem in request.ReceiptDocumentItems)
            {
                var item = await _DocumentItemRepository.Find(documentItem.Id);
                item.UnitPrice = documentItem.UnitPrice;
                item.CurrencyPrice = documentItem.CurrencyPrice;
                item.ProductionCost = documentItem.productionCost;
                item.Description = documentItem.Description;
                item.CurrencyBaseId = documentItem.CurrencyBaseId > 0 ? documentItem.CurrencyBaseId : _context.BaseValues.Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefault();
                
                var extraCost= receipts.Where(a => a.Id== documentItem.DocumentHeadId).Select(a=>a.ExtraCost).FirstOrDefault();
               
                item.UnitPriceWithExtraCost = extraCost>0? documentItem.UnitPriceWithExtraCost: documentItem.UnitPrice;
                item.CurrencyRate = documentItem.CurrencyRate;

                _DocumentItemRepository.Update(item);

            }

            await _DocumentItemRepository.SaveChangesAsync();

        }

        private async Task<CodeVoucherGroup> CreateCodeVoucher(Receipt receipt)
        {

            switch (receipt.DocumentStauseBaseValue)
            {
                case (int)DocumentStateEnam.Leave:
                    receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.invoiceAmountLeave;
                    break;
                case (int)DocumentStateEnam.Direct:
                    receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.invoiceAmount;
                    break;

                case (int)DocumentStateEnam.invoiceAmountLeave:
                    if (receipt.DocumentId == null)
                        receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Leave;
                    break;
                case (int)DocumentStateEnam.invoiceAmount:
                    if (receipt.DocumentId == null)
                        receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Direct;
                    break;

                default:

                    break;
            }


            CodeVoucherGroup NewCodeVoucherGroup = await _receiptCommandsService.GetNewCodeVoucherGroup(receipt);
            return NewCodeVoucherGroup;
        }

        //======================================فقط در هنگام ویرایش کرایه حمل=======================================================
        private async Task __ReceiptUpdateExtraCost(ConvertToRailsReceiptCommand request, IEnumerable<int> documentHeadIds, CancellationToken cancellationToken)
        {
            var entity = await _receiptRepository.GetAll().Where(a => documentHeadIds.Contains(a.Id)).ToListAsync();
            var Receipt = entity.FirstOrDefault();
            var documentId = await _receiptCommandsService.InsertAndUpdateDocument(Receipt);
            var documentHeads = await _receiptRepository.GetAll().Where(a => a.DocumentId == documentId).ToListAsync();

            await __UpdatePriceExtraCost(request, entity);

            //---------------------Insert Attachments----------------------------------
            await _receiptCommandsService.ModifyDocumentAttachments(request.AttachmentIds, documentId);
            await _receiptRepository.SaveChangesAsync(cancellationToken);
        }
        private async Task __UpdatePriceExtraCost(ConvertToRailsReceiptCommand request, List<Receipt> entity)
        {
            int i = 0;
            foreach (var receipt in entity)
            {
                await UpdateExtraCost(request, receipt, i);
                i++;
                await _receiptCommandsService.CalculateTotalItemPrice(receipt);


                _receiptRepository.Update(receipt);

            };

        }
        private async Task __documentItemUpdateExtraCost(ConvertToRailsReceiptCommand request)
        {
            foreach (var documentItem in request.ReceiptDocumentItems)
            {
                var item = await _DocumentItemRepository.Find(documentItem.Id);
                item.UnitPriceWithExtraCost = documentItem.UnitPriceWithExtraCost;
                _DocumentItemRepository.Update(item);

            }

            await _DocumentItemRepository.SaveChangesAsync();

        }
       
    }
}
