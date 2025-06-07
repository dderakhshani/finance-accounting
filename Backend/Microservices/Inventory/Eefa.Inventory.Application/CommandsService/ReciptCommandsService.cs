using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Invertory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;
using System;
using System.Collections.Generic;
using Eefa.Inventory.Domain.Common;
using System.Threading;
using Eefa.Common;
using Eefa.Common.Exceptions;
using Eefa.Invertory.Infrastructure.Services.Arani;

using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using Eefa.Invertory.Infrastructure.Services.AdminApi;
using static Eefa.Invertory.Infrastructure.Services.AdminApi.AdminApiService;
using static Eefa.Inventory.Domain.Common.ConstantValues;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Diagnostics;
using NetTopologySuite.Index.HPRtree;

namespace Eefa.Inventory.Application
{
    public class ReceiptCommandsService : IReceiptCommandsService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IAraniService _araniService;
        private readonly IReceiptQueries _receiptQueries;
        private readonly IAssetsRepository _assetsRepository;
        private readonly IAdminApiService _adminApiService;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IRepository<Employees> _employeesRepository;
        private readonly IRepository<BaseValue> _baseValueRepository;
        private readonly IRepository<VouchersDetail> _vouchersDetailRepository;
        private readonly IRepository<CorrectionRequest> _correctionRequest;
        private readonly IRepository<Domain.Commodity> _commodityRepository;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<Domain.Attachment> _repositoryAttachment;
        private readonly IRepository<DocumentItemsBom> _documentItemsBomRepository;
        private readonly IRepository<AccountReference> _accountReferenceRepository;
        private readonly IRepository<WarehouseHistory> _warehouseHistoryRepository;
        private readonly IRepository<WarehouseStocks> _warehouseStocksRepository;
        private readonly IRepository<CommodityMeasures> _commodityMeasuresRepository;
        private readonly IRepository<DocumentHeadExtend> _documentHeadExtendRepository;
        private readonly IRepository<CommodityPropertyValue> _commodityPropertyValue;
        private readonly IRepository<Domain.AssetAttachments> _repositoryAssetAttachments;
        private readonly IRepository<Domain.DocumentAttachment> _repositoryDocumentAttachments;
        private readonly IRepository<Domain.inventory_StockFromTadbir> _repositoryinventory_StockFromTadbir;
        private readonly IRepository<AccountReferencesRelReferencesGroup> _accountReferenceRelReferencesRepository;

        public ReceiptCommandsService(
              IMapper mapper
            , IAraniService araniService
            , IInvertoryUnitOfWork context
            , IConfiguration configuration
            , IReceiptRepository receiptRepository
            , IAdminApiService adminApiService
            , IReceiptQueries receiptQueries
            , IAssetsRepository assetsRepository
            , IRepository<Person> personRepository
            , IRepository<Document> documentRepository
            , IRepository<BaseValue> baseValueRepository
            , ICurrentUserAccessor currentUserAccessor
            , IRepository<Employees> employeesRepository
            , IProcedureCallService procedureCallService
            , IRepository<CorrectionRequest> correctionRequest
            , IRepository<DocumentItem> documentItemRepository
            , IRepository<Domain.Commodity> commodityRepository
            , IRepository<Domain.Attachment> repositoryAttachment
            , IRepository<WarehouseStocks> warehouseStocksRepository
            , IRepository<AccountReference> accountReferenceRepository
            , IRepository<CommodityMeasures> commodityMeasuresRepository
            , IRepository<DocumentItemsBom> documentItemsBomRepository
            , IRepository<WarehouseHistory> warehouseHistoryRepository
            , IRepository<VouchersDetail> vouchersDetailRepository
            , IRepository<DocumentHeadExtend> documentHeadExtendRepository
            , IRepository<CommodityPropertyValue> commodityPropertyValue
            , IRepository<Domain.AssetAttachments> repositoryAssetAttachments
            , IRepository<Domain.DocumentAttachment> repositoryDocumentAttachments
            , IRepository<Domain.inventory_StockFromTadbir> repositoryinventory_StockFromTadbir
            , IRepository<AccountReferencesRelReferencesGroup> accountReferenceRelReferencesRepository

            )
        {
            _mapper = mapper;
            _context = context;
            _araniService = araniService;
            _configuration = configuration;
            _personRepository = personRepository;
            _assetsRepository = assetsRepository;
            _adminApiService = adminApiService;
            _receiptQueries = receiptQueries;
            _documentRepository = documentRepository;
            _baseValueRepository = baseValueRepository;
            _currentUserAccessor = currentUserAccessor;
            _receiptRepository = receiptRepository;
            _correctionRequest = correctionRequest;
            _employeesRepository = employeesRepository;
            _commodityRepository = commodityRepository;
            _procedureCallService = procedureCallService;
            _documentItemRepository = documentItemRepository;
            _repositoryAttachment = repositoryAttachment;
            _warehouseStocksRepository = warehouseStocksRepository;
            _vouchersDetailRepository = vouchersDetailRepository;
            _commodityPropertyValue = commodityPropertyValue;
            _warehouseHistoryRepository = warehouseHistoryRepository;
            _documentItemsBomRepository = documentItemsBomRepository;
            _repositoryAssetAttachments = repositoryAssetAttachments;
            _accountReferenceRepository = accountReferenceRepository;
            _commodityMeasuresRepository = commodityMeasuresRepository;
            _documentHeadExtendRepository = documentHeadExtendRepository;
            _repositoryDocumentAttachments = repositoryDocumentAttachments;
            _repositoryinventory_StockFromTadbir = repositoryinventory_StockFromTadbir;
            _accountReferenceRelReferencesRepository = accountReferenceRelReferencesRepository;

        }


        public async Task<int> InsertAndUpdateDocument(Domain.Receipt receipt)
        {
            Document document = new Document();
            if (receipt.DocumentId == null)
            {
                return await InsertDocuments(receipt, document);
            }
            else
            {

                return await UpdateDocuments(receipt, document);
            }

        }
        private async Task<int> UpdateDocuments(Receipt receipt, Document document)
        {
            document = await _context.Document.Where(a => a.Id == receipt.DocumentId).FirstOrDefaultAsync();
            document.FinancialOperationNumber = receipt.FinancialOperationNumber;
            document.DocumentDescription = receipt.DocumentDescription;
            document.DocumentDate = Convert.ToDateTime(receipt.InvoiceDate);
            _documentRepository.Update(document);
            await _documentRepository.SaveChangesAsync();

            return Convert.ToInt32(receipt.DocumentId);
        }
        private async Task<int> InsertDocuments(Receipt receipt, Document document)
        {
            document.DocumentNo = await MaxDocumentNo() + 1;
            document.DocumentDate = Convert.ToDateTime(receipt.InvoiceDate);
            document.ReferenceId = Convert.ToInt32(receipt.CreditAccountReferenceId);
            document.DocumentTypeBaseId = Convert.ToInt32(receipt.DocumentStateBaseId);
            document.DocumentId = receipt.Id;
            document.FinancialOperationNumber = receipt.FinancialOperationNumber;
            document.DocumentDescription = receipt.DocumentDescription;

            _documentRepository.Insert(document);
            await _documentRepository.SaveChangesAsync();

            return document.Id;
        }

        public async Task<int> lastDocumentNo(Receipt receipt, CancellationToken cancellationToken)
        {

            if (receipt.DocumentSerial.Contains(((int)DocumentStateEnam.requestReceive).ToString()))
            {
                return await _context.DocumentHeads.Where(x => x.DocumentStauseBaseValue == (int)DocumentStateEnam.requestReceive).MaxAsync(x => x.DocumentNo) ?? 0;
            }
            if (receipt.DocumentSerial.Contains(((int)DocumentStateEnam.requestBuy).ToString()))
            {
                return await _context.DocumentHeads.Where(x => x.DocumentStauseBaseValue == (int)DocumentStateEnam.requestBuy).MaxAsync(x => x.DocumentNo) ?? 0;
            }
            else
            {
                return (await _context.DocumentHeads
                                 .Where(x => x.DocumentSerial == receipt.DocumentSerial)
                                 .Select(x => x.DocumentNo).MaxAsync(cancellationToken: cancellationToken)) ?? 0;
            }

        }
        public async Task<int> MaxDocumentNo()
        {
            return await _context.Document.Select(x => x.DocumentNo).MaxAsync();

        }
        public async Task<bool> IsDuplicateDocumentNo(Receipt receipt, CancellationToken cancellationToken)
        {

            var Count = _context.DocumentHeads.Where(a => a.DocumentNo == receipt.DocumentNo
                                                            && a.DocumentSerial == receipt.DocumentSerial
                                                            && a.Id != receipt.Id
                                                            && a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveBuy
                                                            && a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveReceipt
                                                            && a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveRequest
                                                            && a.DocumentStauseBaseValue != (int)DocumentStateEnam.Transfer
                                                            && !a.IsDeleted
                                                            );


            return await Count.CountAsync() > 0 ? true : false;
        }
        public void ReceiptBaseDataInsert(DateTime DocumentDate, Receipt receipt)
        {
            receipt.YearId = _currentUserAccessor.GetYearId();
            receipt.TotalWeight = default;
            receipt.TotalQuantity = default;
            receipt.DocumentDiscount = default;
            receipt.DiscountPercent = default;
            receipt.DocumentStateBaseId = ConstantValues.ConstBaseValue.NotChecked;
            receipt.PaymentTypeBaseId = 1;
            receipt.InvoiceDate = DocumentDate;
            receipt.DocumentDate = DocumentDate;
        }
        public void GenerateInvoiceNumber(string LeaveType, Receipt receipt, CodeVoucherGroup codeVoucherGroup)
        {
            receipt.InvoiceNo = LeaveType + "-" + receipt.DocumentNo.ToString() + "-" + codeVoucherGroup.Code;
        }
        public string GenerateInvoiceNumber(string LeaveType, CodeVoucherGroup codeVoucherGroup)
        {
            return LeaveType  + "-" + codeVoucherGroup.Code;
        }
        public async Task SerialFormula(Receipt receipt, string code, CancellationToken cancellationToken)
        {

            var serialFormula = (await _baseValueRepository
                            .GetAll(x =>
                                x.ConditionExpression(c =>
                                    c.UniqueName == ConstantValues.ConstBaseValue.UtilityDocument)
                                    )
                            .FirstOrDefaultAsync(
                                cancellationToken:
                                cancellationToken)).Value;


            foreach (var s in serialFormula.Split("-"))
            {
                switch (s)
                {
                    case "[YearId]":
                        receipt.DocumentSerial += _currentUserAccessor.GetYearId() + "-";
                        break;
                    case "[BranchId]":
                        receipt.DocumentSerial += _currentUserAccessor.GetBranchId() + "-";
                        break;
                }
            }

            if (receipt.DocumentSerial.EndsWith("-"))
            {
                receipt.DocumentSerial =
                    receipt.DocumentSerial.Remove(receipt.DocumentSerial.Length - 1, 1);
            }
            if (code.Substring(2, 2) == ((int)DocumentStateEnam.requestBuy).ToString())
            {
                receipt.DocumentSerial = receipt.DocumentSerial + "-" + (code.Substring(2, 2));
            }
            if (code.Substring(2, 2) == ((int)DocumentStateEnam.requestReceive).ToString())
            {
                receipt.DocumentSerial = receipt.DocumentSerial + "-" + (code.Substring(2, 2));
            }

        }
        public async Task<CodeVoucherGroup> GetNewCodeVoucherGroup(Receipt receipt)
        {
            var codeVoucherGroup = await _context.CodeVoucherGroups.Where(a => a.Id == receipt.CodeVoucherGroupId).FirstOrDefaultAsync();

            if (codeVoucherGroup == null)
            {
                throw new ValidationError("برای تغییر وضعیت هیچ نوع سندی در حسابداری تعریف نشده است");
            }


            var codeVoucherGroupCode = codeVoucherGroup.Code;
            var NewCode = codeVoucherGroupCode.Substring(0, 2) + receipt.DocumentStauseBaseValue.ToString();

            var NewCodeVoucherGroup = await _context.CodeVoucherGroups.Where(a => a.Code == NewCode).FirstOrDefaultAsync();
            return NewCodeVoucherGroup;


        }

        public async Task InsertDocumentHeadExtend(int? RequesterReferenceId, int? FollowUpReferenceId, Domain.Receipt receipt)
        {
            DocumentHeadExtend documentHeadExtend = new DocumentHeadExtend()
            {
                DocumentHeadId = receipt.Id,
                RequesterReferenceId = RequesterReferenceId > 0 ? RequesterReferenceId : null,
                FollowUpReferenceId = FollowUpReferenceId > 0 ? FollowUpReferenceId : null,
                CorroborantReferenceId = RequesterReferenceId,
            };

            if (RequesterReferenceId > 0 || FollowUpReferenceId > 0)
            {
                _documentHeadExtendRepository.Insert(documentHeadExtend);
                await _documentHeadExtendRepository.SaveChangesAsync();

            }


        }
        public async Task InsertAssets(AssetsModel Assets, int MainMeasureId, int CommodityId, Domain.Receipt receipt)
        {

            if (Assets != null)
                foreach (var serial in Assets.AssetsSerials)
                {
                    var assets = new Assets()
                    {
                        AssetGroupId = Assets.AssetGroupId,
                        WarehouseId = receipt.WarehouseId,
                        DepreciationTypeBaseId = Assets.DepreciationTypeBaseId,
                        DocumentDate = receipt.DocumentDate,
                        MeasureId = MainMeasureId,
                        CommodityId = CommodityId,
                        IsActive = true,
                        DocumentHeadId = receipt.Id,
                        AssetSerial = serial.Serial,
                        DocumentItemId = Assets.DocumentItemId,
                    };

                    _assetsRepository.Insert(assets);
                }
            await _assetsRepository.SaveChangesAsync();

        }
        //--------------------------------آیا تامین کننده خارجی است-----------------------------------------------------
        public async Task<Receipt> UpdateImportPurchaseReceipt(Receipt receipt)
        {


            var referenceGroupList = await _context.AccountReferenceView.Where(r => r.Id == receipt.CreditAccountReferenceId && r.AccountReferenceGroupId == receipt.CreditAccountReferenceGroupId).ToListAsync();
            var countImport = referenceGroupList.Where(a => a.AccountReferencesGroupsCode == ConstantValues.AccountReferenceGroup.ProviderCode || a.AccountReferencesGroupsCode == ConstantValues.AccountReferenceGroup.ExternalProvider).ToList();

            receipt.IsImportPurchase = countImport.Count() > 0 ? receipt.IsImportPurchase = true : receipt.IsImportPurchase = false;

            return receipt;
        }
        //----------------------------------خروج انبار سیستم آرانی
        public async Task<int> AddReceiptForExitReceiptArani(RequestResult Request, string DarkhastKonandehCode, int WarehouseId, string codeVoucherGroupType)
        {
            var codeVoucherGroup = await _context.CodeVoucherGroups.Where(t => t.UniqueName == codeVoucherGroupType).FirstOrDefaultAsync();
            if (codeVoucherGroup == null)
            {
                throw new ValidationError("برای این درخواست کد حسابی وجود ندارد");
            }
            var WarehouseExit = await _context.WarehouseRequestExitView.Where(a => a.RequestNo == Request.RequestNo.ToString() && a.WarehouseId == WarehouseId).FirstOrDefaultAsync();
            var receipt = new Receipt();
            if (WarehouseExit != null)
            {
                var receiptView = await _context.DocumentHeads.Where(a => a.Id == WarehouseExit.Id
                                                                    && a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveReceipt
                                                                    ).FirstOrDefaultAsync();
                //-------------------------اگر قبلا این سند ثبت شده باشد دوباره ثبت نشود----------------------------
                if (receiptView != null)
                {
                    Request.DocumentId = receiptView.DocumentId;

                    return Convert.ToInt32(receiptView.DocumentNo);
                }
                else
                {
                    var _receiptView = await _context.DocumentHeads.Where(a => a.RequestNo == Request.RequestNo.ToString() && a.WarehouseId == WarehouseId && (a.DocumentStauseBaseValue == (int)DocumentStateEnam.Leave || a.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmountLeave)).FirstOrDefaultAsync();

                    if (_receiptView != null)
                    {


                        Request.DocumentId = _receiptView.DocumentId;
                        return Convert.ToInt32(_receiptView.DocumentNo);
                    }
                }
                Request.NewSearch = receiptView != null && await _context.DocumentItems.Where(a => a.DocumentHeadId == receiptView.Id).CountAsync() > 0 ? false : true;
            }
            else
            {
                var _receiptView = await _context.DocumentHeads.Where(a => a.RequestNo == Request.RequestNo.ToString() && a.WarehouseId == WarehouseId && (a.DocumentStauseBaseValue == (int)DocumentStateEnam.Leave || a.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmountLeave)).FirstOrDefaultAsync();

                if (_receiptView != null)
                {

                    Request.DocumentId = _receiptView.DocumentId;
                    Request.NewSearch = await _context.DocumentItems.Where(a => a.DocumentHeadId == _receiptView.Id).CountAsync() > 0 ? false : true;
                    return Convert.ToInt32(_receiptView.DocumentNo);
                }
            }

            var result = (await _araniService.GetRequestCommodityWarehouse(Request.RequestNo, ConstantValues.AraniService.UrlRequestCommodity));
            var head = result.result.FirstOrDefault();


            if (result == null)
            {
                throw new ValidationError("شماره درخواست خرید اشتباه می باشد");
            }
            //--------------------------------------------------------------------------

            receipt.WarehouseId = Convert.ToInt32(WarehouseId);
            receipt.ExpireDate = head.Tarikh;

            ReceiptBaseDataInsert(ConstBaseValue.DocumnetDateUtc, receipt);

            receipt.RequestNo = Request.RequestNo.ToString();
            receipt.CodeVoucherGroupId = codeVoucherGroup.Id;
            receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Leave;

            receipt.CommandDescription = $"Command:AddReceiptForExitReceipt -درخواست خروج سیستم آرانی-codeVoucherGroup.id={receipt.CodeVoucherGroupId.ToString()}";

            await SerialFormula(receipt, codeVoucherGroup.Code, new CancellationToken());

            int lastNo = await lastDocumentNo(receipt, new CancellationToken());


            receipt.DocumentNo = lastNo + 1;

            GenerateInvoiceNumber(ConstantValues.WarehouseInvoiceNoEnam.LeavePar, receipt, codeVoucherGroup);

            _receiptRepository.Insert(receipt);
            if (await _receiptRepository.SaveChangesAsync() > 0)
            {
                int? RequesterReferenceId = await GetEmployee(DarkhastKonandehCode);
                await InsertDocumentHeadExtend(RequesterReferenceId, null, receipt);

                receipt.DocumentId = await InsertAndUpdateDocument(receipt);
                await _receiptRepository.SaveChangesAsync();

            }
            Request.NewSearch = true;
            return Convert.ToInt32(receipt.DocumentNo);
        }

        //------------------------------------------------------------
        public Receipt ConvertTagArray(string Tag, Receipt a)
        {
            if (String.IsNullOrEmpty(Tag))
            {
                return a;
            }
           
            a.Tags = ConvertTagArray(Tag);
            return a;
        }
        public string ConvertTagArray(string Tag)
        {
            string tag = "";
            if (String.IsNullOrEmpty(Tag))
            {
                return tag;
            }
            string[] TagArray = Tag.Split(",");


            tag = "[";
            foreach (var item in TagArray)
            {
                if (item != "")
                    tag = tag + "{\"Key\":\"" + item + "\"},";
            }
            if (TagArray.Length > 0)
            {
                tag.Remove(tag.Length - 1, 1);
            }
            tag = tag + "]";
            return tag;
        }
        public string AppendNewTagToReceipt(string tag, Receipt receipt)
        {
            var tagList = new List<string>();
            if (!String.IsNullOrEmpty(receipt.Tags))
            {
                var tags = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(receipt.Tags);
                tagList = tags.Select(x => x.Key).ToList();
            }
            tagList.Add(tag);
            var tagValues = _context.BaseValues.Where(x => x.BaseValueTypeId == ConstantValues.ConstBaseValue.InventoryTagSystem).ToList();
            var tagItems = new List<TagClass>();
            foreach (var item in tagList)
            {
                tagItems.Add(new TagClass
                {
                    Key = item,
                    Value = tagValues.FirstOrDefault(x => x.Title == item).Id
                });
            }
            var tagJson = Newtonsoft.Json.JsonConvert.SerializeObject(tagItems);
            return tagJson.ToString();
        }
        public string RemoveTagFromReceipt(string tag, Receipt receipt)
        {
            if (String.IsNullOrEmpty(receipt.Tags))
                return string.Empty;

            var tags = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(receipt.Tags);
            var tagList = tags.Where(x => x.Key != tag).ToList();
            if (tagList.Count > 0)
                return Newtonsoft.Json.JsonConvert.SerializeObject(tagList).ToString();
            else
                return string.Empty;
        }
        public DateTime? SetExpireDate(DateTime? ExpireDate, DateTime LastDate)
        {

            //اگر تاریخ پایان انتخاب نشده باشد  یا تاریخ پایان بیشتر از پایان سال باشد ، تاریخ به صورت پیش فرض می خورد
            if (ExpireDate == default || ExpireDate > LastDate)
            {
                ExpireDate = LastDate;
            }
            return ExpireDate;
        }
        public async Task<DocumentItemsBom> AddDocumentItemsBom(int WarehouseLayoutsId, int WarehouseId, int currency, double Quantity, int CommodityId, int documentItemId, BomValueView bomValueView)
        {
            DocumentItemsBom BomItem = new DocumentItemsBom()
            {
                ParentCommodityId = CommodityId,
                DocumentItemsId = documentItemId,
                CommodityId = bomValueView.UsedCommodityId,
                Quantity = Math.Round(bomValueView.Value * Quantity, 2, MidpointRounding.AwayFromZero),
                MainMeasureId = bomValueView.MeasureId,
                BomValueHeaderId = bomValueView.BomValueHeaderId,
                WarehouseLayoutsId = WarehouseLayoutsId,
                Weight = 0,
                UnitBasePrice = currency,
            };



            await GetPriceBuyBom(bomValueView.UsedCommodityId, WarehouseId, BomItem.Quantity, BomItem);

            _documentItemsBomRepository.Insert(BomItem);
            return BomItem;
        }
        //اینجا فقط قیمت خود کالا در این فرمول ساخت بدست می آید.
        public async Task GetPriceBuyBom(int CommodityId, int WarehouseId, double Quantity, DocumentItemsBom documentItemsBom)
        {
            var PriceBuyItems = await _procedureCallService.GetAndUpdatePriceBuy(CommodityId, WarehouseId, null);
            documentItemsBom.UnitPrice = PriceBuyItems != null ? PriceBuyItems.AveragePurchasePrice : 0;
            documentItemsBom.ProductionCost = PriceBuyItems != null ? Convert.ToInt64(PriceBuyItems.AveragePurchasePrice * Quantity) : 0;

        }
        public async Task GetPriceBuyItems(int CommodityId, int WarehouseId, int? DocumentItemsId, double Quantity, DocumentItem documentItem)
        {
            //براساس ورودی آیتم می فهمد آیا فرمول ساخت داشته است یا خیر
            var PriceBuyItems = await _procedureCallService.GetAndUpdatePriceBuy(CommodityId, WarehouseId, DocumentItemsId > 0 ? DocumentItemsId : null);

            documentItem.UnitPrice = PriceBuyItems != null ? PriceBuyItems.AveragePurchasePrice : 0;
            documentItem.UnitPriceWithExtraCost = documentItem.UnitPrice;
            documentItem.ProductionCost = PriceBuyItems != null ? Convert.ToInt64(PriceBuyItems.AveragePurchasePrice * Quantity) : 0;


        }
        //محاسبه حدود قیمت 
        public async Task GetPriceEstimateItems(int CommodityId, int WarehouseId, DocumentItem documentItem)
        {
            var PriceBuyItems = await (from doc in _context.DocumentHeads
                                       join items in _context.DocumentItems on doc.Id equals items.DocumentHeadId
                                       where (doc.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmount || doc.DocumentStauseBaseValue == (int)DocumentStateEnam.registrationAccounting)
                                             && doc.ViewId != 22
                                             && items.UnitPrice > 1
                                             && items.CommodityId == CommodityId
                                             && doc.WarehouseId == WarehouseId
                                       select new { UnitPrice = items.UnitPrice, DocumentDate = doc.DocumentDate }).OrderByDescending(a => a.DocumentDate).FirstOrDefaultAsync();


            documentItem.UnitBasePrice = PriceBuyItems != null ? Convert.ToInt64(PriceBuyItems.UnitPrice) : 0;

        }
        class warehouses
        {
            public Domain.Receipt receipt1 { get; set; }
            public int CommodityId { get; set; }
            public int? warehouseId { get; set; }
            public int? DocumentNo { get; set; }
        }
        List<warehouses> warehousesRepair { get; set; }
        public async Task<double> ComputeAvgPrice(int CommodityId, Domain.Receipt receipt)
        {
            warehousesRepair = new List<warehouses>();
            var stocks = await _context.WarehouseHistoriesDocumentItemView.Where(a => a.Commodityld == CommodityId && a.DocumentDate >= receipt.DocumentDate && a.DocumentNo >= receipt.DocumentNo && a.YearId == receipt.YearId).OrderBy(a => a.DocumentDate).ThenBy(a => -1 * a.Mode).ToListAsync();
            var FirstQuantityStocks = await _context.WarehouseHistoriesDocumentItemView.Where(a => a.Commodityld == CommodityId && a.DocumentNo < receipt.DocumentNo && a.YearId == receipt.YearId).OrderBy(a => a.DocumentDate).ThenBy(a => -1 * a.Mode).ToListAsync();


            double AVG = await RepairWarehouseStock(CommodityId, receipt, receipt.DocumentNo, stocks, FirstQuantityStocks);
            await _warehouseHistoryRepository.SaveChangesAsync();


            //تابع بازگشتی برای بروز رسانی همه انبارهای درگیر با این سند 
            foreach (var item in warehousesRepair)
            {
                new LogWriter("foreach ComputeAvgPrice " + item.warehouseId.ToString() + " commodityId " + item.ToString() + " DocumentNo " + item.DocumentNo.ToString(), "ComputeAvgPrice");
                await RepairWarehouseStock(item.CommodityId, item.receipt1, item.DocumentNo, stocks, FirstQuantityStocks);


            };

            await UpdateVoucherHeadAfterRepairCardex();
            new LogWriter("End ComputeAvgPrice CommodityId:" + CommodityId.ToString() + " WarehouseId :" + receipt.WarehouseId.ToString(), "ComputeAvgPrice");
            return AVG;

        }

        private async Task<double> RepairWarehouseStock(int CommodityId, Receipt receipt, int? documentNo, List<WarehouseHistoriesDocumentItemView> stocks, List<WarehouseHistoriesDocumentItemView> FirstQuantityStocks)
        {
            double AVG = 0;


            try
            {
                var StartReceipt = await _context.DocumentHeads.Where(a => a.WarehouseId == receipt.WarehouseId && a.YearId == _currentUserAccessor.GetYearId() && (a.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmountStart || a.DocumentStauseBaseValue == (int)DocumentStateEnam.registrationAccountingStart)).FirstOrDefaultAsync();

                var _first = FirstQuantityStocks.Where(a => a.WarehouseId == receipt.WarehouseId && a.DocumentDate > StartReceipt.DocumentDate).OrderBy(a => a.DocumentDate).ThenBy(a => -1 * a.Mode).ThenBy(a => a.DocumentNo).ToList();
                var first = _first.LastOrDefault();
                var FirstQuantity = Convert.ToDouble(_first.Sum(a => a.Quantity * a.Mode));

                var history = stocks.Where(a => a.WarehouseId == receipt.WarehouseId && (a.DocumentStauseBaseValue != (int)DocumentStateEnam.invoiceAmountStart && a.DocumentStauseBaseValue != (int)DocumentStateEnam.registrationAccountingStart) && a.DocumentDate > StartReceipt.DocumentDate).OrderBy(a => a.DocumentDate).ThenBy(a => -1 * a.Mode).ToList();
                //حذف مرجوعی های خرید از لیست
                history = history.Where(a => a.CodeVoucherGroupId != 2381 && a.CodeVoucherGroupId != 2382 && a.CodeVoucherGroupId != 2383).ToList();
                //سند افتتاحیه
                var DocumentItemId = _context.DocumentItems.Where(a => a.DocumentHeadId == StartReceipt.Id && a.CommodityId == CommodityId).Select(a => a.Id).FirstOrDefault();

                var StartReceiptsItem = await _context.WarehouseHistoriesDocumentItemView.Where(a => a.DocumentItemId == DocumentItemId).FirstOrDefaultAsync();
                if (StartReceiptsItem == null)
                {
                    StartReceiptsItem = new WarehouseHistoriesDocumentItemView();
                    StartReceiptsItem.AVGPrice = 0;
                    StartReceiptsItem.Quantity = 0;
                    StartReceiptsItem.Mode = 0;

                }
                //در حالتی که از سند افتتاحیه اولبن رکرد را بخوانیم
                first = first ?? StartReceiptsItem;

                FirstQuantity = FirstQuantity + (StartReceiptsItem.Quantity * StartReceiptsItem.Mode);


                if (first != null)
                {
                    AVG = Convert.ToDouble(first.AVGPrice);

                }

                var i = 0;

                foreach (var Item in history)
                {
                    try
                    {

                        List<int> validIds = new List<int> { 2384, 2385, 2386, 2387, 2376, 2377, 2378, 2404 }; //برگشتی ها از خط تولید;
                        var row2 = new WarehouseHistoriesDocumentItemView();
                        row2.AVGPrice = 0;
                        if (i == 0 && first != null)
                        {
                            row2 = first;

                        }
                        else if (history.Count() > 1 && i > 0)
                        {
                            row2 = history[i - 1];
                        }
                        if (Item.Mode == 1 && !validIds.Contains(Item.CodeVoucherGroupId))
                        {

                            var total = (Math.Abs(Item.Quantity) * Convert.ToDouble(Item.UnitPriceWithExtraCost)) + (Convert.ToDouble(row2.AVGPrice) * FirstQuantity);

                            var totalNow = (FirstQuantity + Item.Quantity) == 0 ? 1 : (FirstQuantity + Item.Quantity);

                            AVG = Math.Abs(Convert.ToDouble(total / totalNow));

                        }
                        i++;

                        //if (AVG != Item.AVGPrice || Item.UnitPriceWithExtraCost != AVG )
                        //{
                        var wh = await _warehouseHistoryRepository.Find(Item.Id);

                        wh.AVGPrice = Convert.ToDouble(AVG);
                        Item.AVGPrice = Convert.ToDouble(AVG);
                        var documentItem = await _documentItemRepository.Find(Convert.ToInt32(Item.DocumentItemId));
                        var documentHead = await _receiptRepository.Find(documentItem.DocumentHeadId);
                        var old_ProductionCostItem = documentItem.ProductionCost;
                        var new_ProductionCostItem = (Item.AVGPrice * Math.Abs(documentItem.Quantity));


                        if (Item.Mode == -1 || validIds.Contains(Item.CodeVoucherGroupId))
                        {
                            if (documentItem.BomValueHeaderId != null)//رسید محصول باشد
                            {
                                new_ProductionCostItem = await ProductReceipt(AVG, Item, documentItem, new_ProductionCostItem);
                            }
                            else if (documentHead.ParentId > 0)//انتقال 
                            {
                                var DI_Transfer = await _documentItemRepository.GetAll().Where(a => a.CommodityId == Item.Commodityld && a.DocumentHeadId == documentHead.ParentId).FirstOrDefaultAsync();
                                var DH_Transfer = await _receiptRepository.Find(Convert.ToInt32(documentHead.ParentId));

                                if (DI_Transfer != null)//رسید محصول نباشد
                                {
                                    await NotProductReceipt(AVG, Item, DI_Transfer, DH_Transfer);
                                }
                            }
                            else
                            {
                                documentItem.UnitPriceWithExtraCost = Convert.ToDouble(AVG);
                                documentItem.UnitPrice = Convert.ToDouble(AVG);
                                documentItem.ProductionCost = Convert.ToDouble(new_ProductionCostItem);
                            }


                            if (documentHead.VoucherHeadId > 0)//ویرایش سند حسابداری
                            {
                                await UpdaterVoucherAmount(receipt, documentHead, old_ProductionCostItem, new_ProductionCostItem);

                            }

                            documentHead.TotalProductionCost = documentHead.TotalProductionCost - old_ProductionCostItem + new_ProductionCostItem;
                            documentHead.TotalItemPrice = Convert.ToDouble(documentHead.TotalItemPrice - old_ProductionCostItem + new_ProductionCostItem);
                            _documentItemRepository.Update(documentItem);
                            _receiptRepository.Update(documentHead);

                        }

                        _warehouseHistoryRepository.Update(wh);
                        //}
                        FirstQuantity += (Item.Mode * Item.Quantity);
                        await _receiptRepository.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        new LogWriter("End ComputeAvgPrice Exception :" + Item.CommodityCode.ToString() + " DocumentNo :" + Item.DocumentNo.ToString() + " Exception :" + ex.Message, "ComputeAvgPrice_Error");
                        if (ex.InnerException != null)
                        {
                            new LogWriter("End ComputeAvgPrice Exception :" + Item.CommodityCode.ToString() + " DocumentNo :" + Item.DocumentNo.ToString() + " InnerException :" + ex.InnerException, "ComputeAvgPrice_Error");
                        }
                    }



                }

                var stock = await _warehouseStocksRepository.GetAll().Where(a => a.WarehousId == receipt.WarehouseId && a.CommodityId == CommodityId).FirstOrDefaultAsync();
                if (stock != null)
                {
                    stock.Price = AVG;
                    _warehouseStocksRepository.Update(stock);
                    await _warehouseStocksRepository.SaveChangesAsync();

                }


            }
            catch (Exception ex)
            {
                new LogWriter("End ComputeAvgPrice Exception :" + CommodityId.ToString() + " DocumentNo :" + receipt.DocumentNo.ToString() + " Exception :" + ex.Message, "ComputeAvgPrice_Error");
                if (ex.InnerException != null)
                {
                    new LogWriter("End ComputeAvgPrice Exception :" + CommodityId.ToString() + " DocumentNo :" + receipt.DocumentNo.ToString() + " InnerException :" + ex.InnerException, "ComputeAvgPrice_Error");
                }
            }


            return AVG;
        }

        private async Task NotProductReceipt(double AVG, WarehouseHistoriesDocumentItemView Item, DocumentItem DocumentItem_Transfer, Receipt DocumentHead_Transfer)
        {
            var oldTotalProductionCost = (DocumentItem_Transfer.UnitPrice * Math.Abs(DocumentItem_Transfer.Quantity));
            var newTotalProductionCost = (Item.AVGPrice * Math.Abs(DocumentItem_Transfer.Quantity));


            DocumentHead_Transfer.TotalProductionCost = Convert.ToDouble(DocumentHead_Transfer.TotalProductionCost - oldTotalProductionCost + newTotalProductionCost);
            DocumentHead_Transfer.TotalItemPrice = Convert.ToDouble(DocumentHead_Transfer.TotalItemPrice - oldTotalProductionCost + newTotalProductionCost);

            DocumentItem_Transfer.UnitPriceWithExtraCost = Convert.ToDouble(AVG);
            DocumentItem_Transfer.UnitPrice = Convert.ToDouble(AVG);
            DocumentItem_Transfer.ProductionCost = Convert.ToDouble(newTotalProductionCost);

            _documentItemRepository.Update(DocumentItem_Transfer);
            _receiptRepository.Update(DocumentHead_Transfer);

            var WH_Transfer = await _warehouseHistoryRepository.GetAll().Where(a => a.Commodityld == Item.Commodityld && (a.DocumentHeadId == DocumentHead_Transfer.Id)).FirstOrDefaultAsync();

            if (WH_Transfer != null)
            {
                WH_Transfer.AVGPrice = Convert.ToDouble(AVG);
                _warehouseHistoryRepository.Update(WH_Transfer);
            }

            if (warehousesRepair.Where(a => a.warehouseId == WH_Transfer.WarehousesId && a.CommodityId == Item.Commodityld).Count() == 0)
            {
                warehousesRepair.Add(new() { receipt1 = DocumentHead_Transfer, CommodityId = Item.Commodityld, DocumentNo = Item.DocumentNo, warehouseId = WH_Transfer.WarehousesId });

            }
            new LogWriter("NotProductReceipt :" + Item.CommodityTitle + " warehouseid" + WH_Transfer.WarehousesId.ToString(), "ComputeAvgPrice");

        }

        private async Task<double?> ProductReceipt(double AVG, WarehouseHistoriesDocumentItemView Item, DocumentItem documentItem, double? new_ProductionCostItem)
        {
            var bomItems = await _documentItemsBomRepository.GetAll().Where(a => a.DocumentItemsId == documentItem.Id).ToListAsync();
            var bomItem = bomItems.Where(a => a.CommodityId == Item.Commodityld).ToList();
            bomItem.ForEach(b =>
            {
                var old_bom = (b.UnitPrice * Math.Abs(b.Quantity));
                var ne_bom = (AVG * Math.Abs(b.Quantity));

                b.UnitPrice = AVG;
                b.ProductionCost = ne_bom;

                _documentItemsBomRepository.Update(b);
            });


            documentItem.UnitPrice = bomItems.Sum(a => a.ProductionCost) / Math.Abs(documentItem.Quantity);
            documentItem.UnitPriceWithExtraCost = documentItem.UnitPrice;
            documentItem.ProductionCost = documentItem.UnitPrice * Math.Abs(documentItem.Quantity);

            new_ProductionCostItem = documentItem.ProductionCost;

            new LogWriter("ProductReceipt :" + Item.CommodityTitle, "ComputeAvgPrice_Error");
            return new_ProductionCostItem;
        }

        private async Task UpdaterVoucherAmount(Receipt receipt, Receipt documentHead, double old_ProductionCostItem, double? new_ProductionCostItem)
        {
            var voucher = await _vouchersDetailRepository.GetAll().Where(a => a.DocumentId == documentHead.DocumentId && a.VoucherId == documentHead.VoucherHeadId).ToListAsync();
            if (voucher == null)
            {
                return;
            }

            voucher.ForEach(a =>
            {
                a.Debit = a.Debit > 0 ? Convert.ToInt64(a.Debit - old_ProductionCostItem + new_ProductionCostItem) : a.Debit;
                a.Credit = a.Credit > 0 ? Convert.ToInt64(a.Credit - old_ProductionCostItem + new_ProductionCostItem) : a.Credit;

                new LogWriter($"Edit  voucher=============== documentHead.VoucherHeadId : {documentHead.VoucherHeadId.ToString()} WarehouseId :{receipt.WarehouseId.ToString()}", "UpdaterVoucherAmount");

                _vouchersDetailRepository.Update(a);
            });


        }

        public async Task CalculateTotalItemPrice(Receipt Receipt)
        {
            var Items = await _context.DocumentItems.Where(a => a.DocumentHeadId == Receipt.Id && !a.IsDeleted).ToListAsync();
            var VatPercentage = Receipt.VatPercentage;
            var VatDutiesTax = Receipt.VatDutiesTax;
            var ExtraCost = Receipt.IsImportPurchase == false ? Receipt.ExtraCost : 0;

            Receipt.TotalProductionCost = Items.Sum(a => a.ProductionCost);
            if (VatPercentage >= 0)
            {
                Receipt.VatDutiesTax = (Convert.ToInt64(Receipt.TotalProductionCost) * Convert.ToInt32(VatPercentage)) / 100;
            }

            Receipt.TotalItemPrice = Receipt.IsFreightChargePaid == true ?
                                    Convert.ToDouble(Receipt.TotalProductionCost) + Convert.ToInt64(Receipt.VatDutiesTax) + Convert.ToInt64(ExtraCost) :
                                    Convert.ToDouble(Receipt.TotalProductionCost) + Convert.ToInt64(Receipt.VatDutiesTax);

        }
        public async Task<int?> GetEmployee(string Code)
        {

            var AccountReference = (from epp in _context.Employees
                                    join per in _context.Persons
                                    on epp.PersonId equals per.Id
                                    where (epp.EmployeeCode == Code || Code == $"000{Code}" || Code == $"00{Code}" || Code == $"0{Code}")
                                    select per.AccountReferenceId);
            var AccountReferenceId = await AccountReference.FirstOrDefaultAsync();

            return AccountReferenceId;
        }
        public async Task<Person> AddNewPerson(string Name, string NationalNumber, string Mobile)
        {
            Person person = new Person();
            var AccountReference = await AddNewAccountReference(Name, ConstantValues.AraniService.Prefix + NationalNumber);
            if (AccountReference == null)
            {
                throw new ValidationError("مشکل در ثبت درخواست کننده جدید");
            }
            if (await AddNewAccountRelReferencesGroup(AccountReference.Id) > 0)
            {

                person.FirstName = " ";
                person.LastName = Name;
                person.GenderBaseId = ConstantValues.ConstBaseValue.MenId;
                person.TaxIncluded = false;
                person.LegalBaseId = ConstantValues.ConstBaseValue.legalBaseId;
                person.GovernmentalBaseId = ConstantValues.ConstBaseValue.governmentalBaseId;
                person.NationalNumber = ConstantValues.AraniService.Prefix + NationalNumber;
                person.AccountReferenceId = AccountReference.Id;
                person.BirthPlaceCountryDivisionId = 92344;
                person.BirthDate = DateTime.Now;
                person.IdentityNumber = "";
                person.FatherName = "";
                string phoneNumber = "[{\"Operator\":\"\",\"Number\":\" \",\"Type\":\"\"},{\"Operator\":\"\",\"Number\":\"" + Mobile + "\",\"Type\":\"mobile\"}]\"";
                person.MobileJson = phoneNumber;
                _personRepository.Insert(person);
                await _personRepository.SaveChangesAsync();
            }

            return person;
        }
        private async Task<int> AddNewAccountRelReferencesGroup(int AccountReferenceId)
        {
            AccountReferencesRelReferencesGroup accountReferencesRelReferencesGroup = new AccountReferencesRelReferencesGroup()
            {
                ReferenceId = AccountReferenceId,
                ReferenceGroupId = ConstantValues.AccountReferenceGroup.accountReferenceGroupId_personnel,
            };

            _accountReferenceRelReferencesRepository.Insert(accountReferencesRelReferencesGroup);
            return await _accountReferenceRelReferencesRepository.SaveChangesAsync();

        }
        private async Task<AccountReference> AddNewAccountReference(string Name, string Code)
        {

            AccountReference AccountReference = new AccountReference() { Code = Code, Title = Name, IsActive = true };

            _accountReferenceRepository.Insert(AccountReference);

            return await _accountReferenceRepository.SaveChangesAsync() > 0 ? AccountReference : null;
        }
        public async Task<Employees> AddNewEmployees(int PersonId, string Code)
        {

            Employees employees = new Employees()
            {
                EmployeeCode = Code,
                Floating = false,
                EmploymentDate = DateTime.Now,
                PersonId = PersonId,
                UnitPositionId = 119 //این کد غلط است فقط چون سیستم آقای ملک پور با یک آیدی پر شود از این کد استفاده شده است و کد درست برایش نداریم
            };
            _employeesRepository.Insert(employees);

            return await _employeesRepository.SaveChangesAsync() > 0 ? employees : null;
        }
        public async Task ModifyDocumentAttachments(List<int> attachmentIds, int DocumentId)
        {

            var AssetAttachments = await _repositoryDocumentAttachments.GetAll().Where(a => a.DocumentId == DocumentId).ToListAsync();
            // LIST OF DELETEDS
            var AssetAttachmentsDeleted = AssetAttachments.Where(a => attachmentIds.Contains(a.AttachmentId)).ToList();

            if (attachmentIds != null && attachmentIds.Any())
            {

                foreach (var attachmentI in attachmentIds)
                {

                    var AssetAttachment = AssetAttachments.Where(a => a.AttachmentId == attachmentI).FirstOrDefault();

                    //UPDATE
                    var Attachment = await _repositoryAttachment.GetAll().Where(a => a.Id == attachmentI).FirstOrDefaultAsync();

                    //INSERT NEW
                    if (Attachment != null && AssetAttachment == null)
                    {
                        Attachment.IsUsed = true;
                        var attach = new Domain.DocumentAttachment()
                        {
                            AttachmentId = attachmentI,
                            DocumentId = Convert.ToInt32(DocumentId),

                        };
                        _repositoryDocumentAttachments.Insert(attach);
                    }

                }
                foreach (var Asset in AssetAttachmentsDeleted)
                {
                    _repositoryDocumentAttachments.Delete(Asset);
                }
                await _repositoryDocumentAttachments.SaveChangesAsync();

            }
        }
        public async Task ModifyAttachmentAssets(List<AttachmentAssetsRequest> attachmentAssets, int PersonsDebitedCommoditiesId)
        {
            var AssetAttachments = await _repositoryAssetAttachments.GetAll().Where(a => a.PersonsDebitedCommoditiesId == PersonsDebitedCommoditiesId).ToListAsync();
            // LIST OF DELETEDS
            var AssetAttachmentsDeleted = AssetAttachments.Where(a => attachmentAssets.Select(b => b.AttachmentId).Contains(a.AttachmentId)).ToList();

            if (attachmentAssets.Any())
            {

                foreach (var Asset in attachmentAssets)
                {

                    var AssetAttachment = AssetAttachments.Where(a => a.AttachmentId == Asset.AttachmentId).FirstOrDefault();

                    //UPDATE
                    var Attachment = await _repositoryAttachment.GetAll().Where(a => a.Id == Asset.AttachmentId).FirstOrDefaultAsync();

                    //INSERT NEW
                    if (Attachment != null && AssetAttachment == null)
                    {
                        Attachment.IsUsed = true;
                        var attach = new Domain.AssetAttachments()
                        {
                            AssetId = Convert.ToInt32(Asset.AssetsId),
                            PersonsDebitedCommoditiesId = PersonsDebitedCommoditiesId,
                            AttachmentId = Asset.AttachmentId,
                        };
                        _repositoryAssetAttachments.Insert(attach);
                    }

                }
                foreach (var Asset in AssetAttachmentsDeleted)
                {
                    _repositoryAssetAttachments.Delete(Asset);
                }
                await _repositoryAssetAttachments.SaveChangesAsync();

            }
        }


        public async Task<Domain.Commodity> AddCommodity(string Code, string Name, string CompactCode)
        {

            var commodity = await _commodityRepository.GetAll().Where(a => a.Code == Code).FirstOrDefaultAsync();
            if (commodity == null)
            {
                return await InsertCommodity(Code, Name, CompactCode);

            }
            commodity.CompactCode = CompactCode;
            _commodityRepository.Update(commodity);
            await _commodityRepository.SaveChangesAsync();
            return commodity;
        }

        public async Task<Domain.Commodity> InsertCommodity(string Code, string Name, string CompactCode)
        {

            CommodityCategory commodityCategory = await GetCategory(Code);
            if (commodityCategory != null)
            {
                if (Code.Length == 24)
                {
                    //S00411100000900000003537
                    //S004-111-000-009-0000000-3537
                    //compactCode=1110093537
                    var right4 = Code.Substring(20, 4);
                    var center3 = Code.Substring(10, 3);
                    var left3 = Code.Substring(4, 3);
                    CompactCode = left3 + center3 + right4;
                }

            };

            Domain.Commodity Commodity = new Domain.Commodity()
            {
                Title = Name,
                Code = Code,
                TadbirCode = Code,
                CompactCode = CompactCode,
                CommodityCategoryId = commodityCategory.Id,
                PricingTypeBaseId = ConstantValues.ConstBaseValue.BasedOnWeightedAverage,
                MeasureId = commodityCategory.MeasureId == null ? 1 : commodityCategory.MeasureId,
                MinimumQuantity = 0,
                MaximumQuantity = 50,
                LevelCode = "",
                YearId = _currentUserAccessor.GetYearId(),
            };
            _commodityRepository.Insert(Commodity);
            await _commodityRepository.SaveChangesAsync();

            CommodityMeasures me = new CommodityMeasures() { CommodityId = Commodity.Id, MeasureId = commodityCategory.MeasureId, OrderIndex = commodityCategory.OrderIndex };



            _commodityMeasuresRepository.Insert(me);
            await _commodityMeasuresRepository.SaveChangesAsync();




            return Commodity;
        }


        private async Task<CommodityCategory> GetCategory(string Code)
        {
            var commodityCategory = await _context.CommodityCategories.Where(a => Code.Length >= 13 && a.Code == Code.Substring(0, 13)).FirstOrDefaultAsync();

            commodityCategory = commodityCategory ?? await _context.CommodityCategories.Where(a => Code.Length >= 7 && a.Code == Code.Substring(0, 7)).FirstOrDefaultAsync();
            commodityCategory = commodityCategory ?? await _context.CommodityCategories.Where(a => Code.Length >= 4 && a.Code == Code.Substring(0, 4)).FirstOrDefaultAsync();
            commodityCategory = commodityCategory ?? await _context.CommodityCategories.Where(a => Code.Length >= 2 && a.Code == Code.Substring(0, 2)).FirstOrDefaultAsync();

            //اگر هیچ گروهی یافت نشد در گروه انبار قطعات یدکی قرار گیرد
            commodityCategory = commodityCategory ?? await _context.CommodityCategories.Where(a => a.Code == "S0").FirstOrDefaultAsync();


            return commodityCategory;
        }

        public async Task<Domain.Commodity> AddProduct(SinaProduct model)
        {
            Domain.Commodity Commodity = new Domain.Commodity();

            var measure = await _context.MeasureUnits.Where(a => a.UniqueName == model.UnitSaleType).FirstOrDefaultAsync();

            CommodityCategory commodityCategory = await GetCategory(model.ProductCode);

            if (commodityCategory != null && measure != null)
            {
                Commodity.Title = model.ProductName;
                Commodity.SinaId = model.Id;
                Commodity.Code = model.ProductCode;
                Commodity.TadbirCode = model.ProductCode;
                Commodity.CompactCode = model.ProductCode;
                Commodity.CommodityCategoryId = commodityCategory.Id;
                Commodity.PricingTypeBaseId = ConstantValues.ConstBaseValue.BasedOnWeightedAverage;
                Commodity.MeasureId = measure.Id;
                Commodity.MinimumQuantity = 0;
                Commodity.MaximumQuantity = 50;
                Commodity.LevelCode = "";
                Commodity.YearId = _currentUserAccessor.GetYearId();

                _commodityRepository.Insert(Commodity);
                await _commodityRepository.SaveChangesAsync();

            }
            return Commodity;
        }
        public async Task<Domain.Commodity> UpdateProduct(SinaProduct model)
        {
            Domain.Commodity Commodity = await _commodityRepository.GetAll().Where(a => a.SinaId == model.Id).FirstOrDefaultAsync();
            Commodity.Title = model.ProductName;
            Commodity.Code = model.ProductCode;
            Commodity.TadbirCode = model.ProductCode;
            Commodity.CompactCode = model.ProductCode;
            _commodityRepository.Update(Commodity);

            return Commodity;
        }

        public async Task<Domain.Commodity> UpdateProductProperty(int CommodityId, SinaProducingInputProduct inputProduct)
        {

            Domain.Commodity Commodity = _commodityRepository.GetAll().Where(a => a.Code.ToLower() == inputProduct.TadbirCode.ToLower()).FirstOrDefault();
            if (Commodity == null)
            {
                return null;
            }
            await AddProperty(Commodity.Id, inputProduct.SizeName, "size");
            await AddProperty(Commodity.Id, inputProduct.Thickness, "Thickness");
            await AddProperty(Commodity.Id, inputProduct.BoxCountTile.ToString(), "BoxCountTile");
            await AddProperty(Commodity.Id, inputProduct.PalletBoxCount.ToString(), "PalletBoxCount");
            await AddProperty(Commodity.Id, inputProduct.FactorNumber.ToString(), "FactorNumber");
            await AddProperty(Commodity.Id, inputProduct.Wieght.ToString(), "Weight");
            await AddProperty(Commodity.Id, inputProduct.TileBrandName.ToString(), "TileBrand");
            await AddProperty(Commodity.Id, inputProduct.PolishName.ToString(), "PolishName");
            await AddProperty(Commodity.Id, inputProduct.Grade.ToString(), "Grade");

            Commodity.Title = inputProduct.TadbirName;

            _commodityRepository.Update(Commodity);
            await _commodityRepository.SaveChangesAsync();

            return Commodity;
        }

        private async Task AddProperty(int CommodityId, string value, string UniqueName)
        {
            var ValuePropertyItemId = await _context.CommodityCategoryPropertyItems.Where(a => a.CategoryProperty.UniqueName.ToLower() == UniqueName.ToLower() && !a.CategoryProperty.IsDeleted && a.Title.ToLower() == value.ToLower() && !a.IsDeleted).FirstOrDefaultAsync();
            var CategoryPropertyId = await _context.CommodityCategoryProperties.Where(a => a.UniqueName.ToLower() == UniqueName.ToLower() && !a.IsDeleted).FirstOrDefaultAsync();
            var _propertyValue = await _commodityPropertyValue.GetAll().Where(a => a.CategoryProperty.UniqueName.ToLower() == UniqueName.ToLower() && a.CommodityId == CommodityId && !a.IsDeleted).FirstOrDefaultAsync();
            if (_propertyValue == null && CategoryPropertyId != null)
            {
                CommodityPropertyValue propertyValue = new CommodityPropertyValue();

                propertyValue.Value = value;
                propertyValue.ValuePropertyItemId = ValuePropertyItemId != null ? ValuePropertyItemId.Id : null;
                propertyValue.CategoryPropertyId = CategoryPropertyId.Id;
                propertyValue.CommodityId = CommodityId;

                propertyValue.IsDeleted = false;

                _commodityPropertyValue.Insert(propertyValue);

            }
        }

        public async Task<int> UploadInventoryTadbir(IFormFile file, string warehouseId)
        {
            await _procedureCallService.DeleteInventory_StockFromTadbir(Convert.ToInt32(warehouseId));

            return await ReadInventoryFile(file, warehouseId);
        }
        public async Task<object> UpdateWarehouseCommodityPrice(int warehouseId, int yearId)
        {

            return await _procedureCallService.UpdateWarehouseCommodityPrice(warehouseId, yearId);
        }

        private async Task<int> ReadInventoryFile(IFormFile file, string warehouseId)
        {
            var lines = await ReadLines(file);
            var headerLine = lines[0];
            var commodityLines = lines.Except(new List<string>() { headerLine });

            int i = 0;
            string sep = "\t";
            foreach (var line in lines)
            {
                if (i != 0)
                {
                    try
                    {
                        var props = line.Split(sep).Select(x => x.Replace("\r", "")).ToArray();
                        if (props[0] != null && props[0] != "")
                        {
                            var importedCommodity = new inventory_StockFromTadbir();
                            importedCommodity.TadbirCode = props[0];
                            importedCommodity.warehouseId = Convert.ToInt32(warehouseId);
                            importedCommodity.Quantity = Convert.ToDouble(props[1]);
                            if (props.Length > 2)
                            {
                                if (props[2] != "" && Convert.ToDouble(props[2]) > 0)
                                {
                                    importedCommodity.Price = Convert.ToDouble(props[2]);
                                }

                            }
                            if (props.Length > 3)
                            {
                                importedCommodity.LocationName = props[3];
                            }
                            if (props.Length > 4)
                            {
                                importedCommodity.CommodityName = props[4];
                            }
                            _repositoryinventory_StockFromTadbir.Insert(importedCommodity);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ValidationError(" فرم فایل ورودی صحیح نمی باشد " + ex.Message.ToString());
                    }

                }
                i++;
            }
            return await _repositoryinventory_StockFromTadbir.SaveChangesAsync();
        }

        public async Task<List<string>> ReadLines(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }
            return result.ToString().Split("\n").ToList();
        }

        public async Task<spUpdateWarehouseCommodityAvailabe2> UpdateWarehouseCommodityAvailable(int warehouseId, int yearId)
        {
            return await _procedureCallService.UpdateWarehouseCommodityAvailable(warehouseId, yearId, _currentUserAccessor.GetId());
        }
        public async Task<ResultModel> UpdateVoucher(ConvertToRailsReceiptCommand model)
        {
            var receipts = await _receiptQueries.GetAllReceiptGroupInvoice(null, null, model.VoucherHeadId, model.DocumentId.ToString(), null, null, null, null, null, null, null);

            UpdateAutoVoucher autoVoucher = new UpdateAutoVoucher();
            List<Voucher> inputs = new List<Voucher>();
            DateTime date = DateTime.Now;
            string persianDateString = date.ToString("yyyy/MM/dd", new CultureInfo("fa-IR"));

            foreach (var a in receipts.Data)
            {
                var obj = new Voucher
                {

                    FinancialOperationNumber = a.FinancialOperationNumber.ToString(),
                    Tag = a.tags.ToString(),
                    InvoiceNo = a.InvoiceNo,
                    DocumentDate = a.InvoiceDate.ToString(),

                    CodeVoucherGroupId = a.DocumentStauseBaseValue.ToString(),
                    CodeVoucherGroupTitle = a.CodeVoucherGroupTitle,

                    DebitAccountHeadId = a.DebitAccountHeadId.ToString(),
                    DebitAccountReferencesGroupId = a.DebitAccountReferenceGroupId.ToString(),
                    DebitAccountReferenceId = a.DebitAccountReferenceId.ToString(),

                    CreditAccountHeadId = a.CreditAccountHeadId.ToString(),
                    CreditAccountReferencesGroupId = a.CreditAccountReferenceGroupId.ToString(),
                    CreditAccountReferenceId = a.CreditAccountReferenceId.ToString(),

                    ToTalPriceMinusVat = a.TotalProductionCost.ToString(),
                    VatDutiesTax = a.VatDutiesTax.ToString(),
                    PriceMinusDiscountPlusTax = a.TotalItemPrice.ToString(),

                    TotalQuantity = "0",
                    TotalWeight = "0",
                    ExtraCost = a.ExtraCost > 0 && a.IsImportPurchase == false ? a.ExtraCost.ToString() : "0",
                    DocumentId = a.DocumentId.ToString(),
                    DocumentNo = a.DocumentNo.ToString(),
                    DocumentIds = a.DocumentIds,
                    VoucherRowDescription = a.CodeVoucherGroupTitle.Contains("خرید") ? a.CreditReferenceTitle + " طی شماره رسید :" + string.Join("-", a.InvoiceNo.Split('-').Distinct().ToArray()) + "-" + a.DocumentDescription + " اصلاحیه تاریخ " + persianDateString
                    : a.CodeVoucherGroupTitle + " طی شماره حواله " + string.Join("-", a.InvoiceNo.Split('-').Distinct().ToArray()) + "-" + a.DocumentDescription,

                    DebitAccountHeadTitle = a.DebitAccountHeadTitle,
                    CreditAccountHeadTitle = a.CreditAccountHeadTitle,

                    ExtraCostAccountHeadId = a.ExtraCostAccountHeadId.ToString(),
                    ExtraCostAccountReferenceGroupId = a.ExtraCostAccountReferenceGroupId.ToString(),
                    ExtraCostAccountReferenceId = a.ExtraCostAccountReferenceId.ToString(),
                    ExtraCostAccountHeadTitle = a.ExtraCostAccountHeadTitle != null ? a.ExtraCostAccountHeadTitle.ToString() : "",

                    CurrencyFee = a.CurrencyRate.ToString(),
                    CurrencyTypeBaseId = a.CurrencyBaseId.ToString(),
                    CurrencyAmount = a.CurrencyPrice.ToString(),
                    VoucherHeadId = a.VoucherHeadId.ToString(),
                    IsFreightChargePaid = a.IsFreightChargePaid == true ? "1" : "0",
                    TotalItemPrice = a.TotalItemPrice.ToString()

                };

                inputs.Add(obj);
            };
            autoVoucher.DataList = inputs;

            autoVoucher.VoucherHeadId = Convert.ToInt32(model.VoucherHeadId);
            var AccessToken = await _currentUserAccessor.GetAccessToken();
            var AccountingUpdateApiUrl = _configuration.GetSection("AccountingApiUrl").GetSection("AutoVoucher").Value;
            new LogWriter("befor CallApiAutoVoucher2 ===================================== DocumentId" + model.DocumentId.ToString());
            var result = await _adminApiService.CallApiAutoVoucher2(autoVoucher, AccessToken, AccountingUpdateApiUrl);
            await _procedureCallService.ValidationIssuedDocuments();

            return result;
        }

        public async Task<int> WarehouseCheckLoseData()
        {
            try
            {
                await _procedureCallService.WarehouseCheckLoseData();
            }
            catch (Exception ex) { }

            return 0;
        }
        public async Task<int> UpdateVoucherHeadAfterRepairCardex()
        {
            try
            {
                await _procedureCallService.UpdateVoucherHeadAfterRepairCardex();
            }
            catch (Exception ex) { }

            return 0;
        }

        public async Task<int> UpdateWarehouseLayout(int WarehouseId)
        {

            await _procedureCallService.UpdateWarehouseLayout(WarehouseId, _currentUserAccessor.GetId());


            return 0;
        }
        public async Task<int> RemoveCommodityFromWarehouse(int WarehouseId)
        {

            await _procedureCallService.RemoveCommodityFromWarehouse(WarehouseId, _currentUserAccessor.GetId());


            return 0;
        }
        public async Task<int> ArchiveDocumentHeadsByDocumentDate(DateTime? FromDate, DateTime? ToDate, int WarehouseId, int DocumentStatuesBaseValue)
        {

            await _procedureCallService.ArchiveDocumentHeadsByDocumentDate(FromDate, ToDate, WarehouseId, _currentUserAccessor.GetId(), DocumentStatuesBaseValue);


            return 0;
        }
        public async Task<int> UpdateAddNewCommodity()
        {

            await _procedureCallService.UpdateAddNewCommodity(_currentUserAccessor.GetId());


            return 0;
        }
        public async Task<bool> IsValidAccountHeadRelationByReferenceGroup(int? AccountHeadId, int? ReferenceGroupId)
        {
            if (ReferenceGroupId == null)
                return true;
            return true;
            //return await _context.AccountHeadRelReferenceGroup.Where(a=>a.AccountHeadId== AccountHeadId && a.ReferenceGroupId== ReferenceGroupId).AnyAsync();
        }
    }
}
