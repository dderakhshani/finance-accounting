using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Aggregates;
using Eefa.Inventory.Domain.Common;
using Eefa.Invertory.Infrastructure.Context;
using Eefa.Invertory.Infrastructure.Repositories;
using Eefa.Invertory.Infrastructure.Services.AdminApi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Eefa.Inventory.Application
{
    public class ReceiptQueries : IReceiptQueries
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IRepositoryManager _repositoryManager;
        public ReceiptQueries(
              IMapper mapper
            , IAdminApiService araniService
            , IInvertoryUnitOfWork context
            , ICurrentUserAccessor currentUserAccessor
            , IProcedureCallService procedureCallService
            , IRepositoryManager repositoryManager

            )
        {
            _mapper = mapper;
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _procedureCallService = procedureCallService;
            _repositoryManager = repositoryManager;
        }
        public async Task<ReceiptQueryModel> GetPlacementById(int id, int? WarehouseId)
        {

            var view = await _context.ReceiptWithCommodityView.Where(a => a.Id == id).FirstOrDefaultAsync();
            var result = _mapper.Map<ReceiptQueryModel>(view);
            if (WarehouseId != null)
            {
                result.WarehouseId = WarehouseId;
            }
            //----------------- documentItem list---------------------------------------
            var itemsModel = await (from documentItem in _context.DocumentItems.Where(a => a.DocumentHeadId == id)
                                    join measure in _context.MeasureUnits
                                            on documentItem.DocumentMeasureId equals measure.Id
                                    select new ReceiptItemModel
                                    {
                                        Id = documentItem.Id,
                                        DocumentHeadId = documentItem.DocumentHeadId,
                                        DocumentItemId = documentItem.Id,
                                        CommodityId = documentItem.CommodityId,
                                        Quantity = documentItem.Quantity,
                                        DocumentMeasureId = documentItem.DocumentMeasureId,
                                        DocumentMeasureTitle = measure.Title,
                                        MeasureUnitConversionId = documentItem.MeasureUnitConversionId,
                                        MainMeasureId = documentItem.MainMeasureId,
                                        ConversionRatio = (double)documentItem.ConversionRatio,
                                        WarehouseHistories = new List<WarehouseHistoryModel>(),
                                        WarehouseLayoutQuantity = new WarehouseLayoutQuantityModel(),
                                        Commodity = new ReceiptCommodityModel()
                                    }
                               ).ToListAsync();
            if (itemsModel.Count() > 0)
                result.Items = itemsModel;
            else
            {
                throw new ValidationError("اقلامی در این سند وجود ندارد");
            }

            //---------------------------لیست محصولات--------------------------------------
            var commodityIds = result.Items.Select(x => x.CommodityId);

            var commodityList = await (from comidity in _context.ViewCommodity

                                       where commodityIds.Contains(comidity.Id)
                                       select new ReceiptCommodityModel()
                                       {
                                           Id = comidity.Id,
                                           Title = comidity.Title,
                                           TadbirCode = comidity.Code,
                                           MeasureId = comidity.MeasureId,
                                           MeasureTitle = comidity.MeasureTitle
                                       }).ToListAsync();
            if (!commodityList.Any())
            {
                throw new ValidationError("کالایی در این سند وجود ندارد");
            }
            //----------------------------لیست محل های نگهداری در انبار---------------------
            var commodityWarehouseList = await (from quantity in _context.WarehouseLayoutQuantities.Where(a => !a.IsDeleted)
                                                join layout in _context.WarehouseLayouts.Where(a => a.WarehouseId == result.WarehouseId && !a.IsDeleted)
                                                on quantity.WarehouseLayoutId equals layout.Id
                                                where commodityIds.Contains(quantity.CommodityId)
                                                select new WarehouseLayoutQuantityModel
                                                {
                                                    Id = quantity.Id,
                                                    WarehouseLayoutId = layout.Id,
                                                    QuantityTotal = layout.Capacity,
                                                    Quantity = quantity.Quantity,
                                                    //QuantityNeed = (double)contityByConversionFactor,
                                                    WarehouseLayoutTitle = layout.Title,
                                                    CommodityId = quantity.CommodityId

                                                }).ToListAsync();
            //------------------------------سابقه نگهداری در انبار--------------------------
            var historyModels = (from his in _context.WarehouseHistories.Where(a => a.Mode == 1)
                                 join layout in _context.WarehouseLayouts.Where(a => a.WarehouseId == result.WarehouseId && !a.IsDeleted) on his.WarehouseLayoutId equals layout.Id

                                 join document in _context.DocumentItems on his.DocumentItemId equals document.Id
                                 where document.DocumentHeadId == id
                                 select new WarehouseHistoryModel
                                 {
                                     Commodityld = his.Commodityld,
                                     WarehouseLayoutId = layout.Id,
                                     Quantity = his.Quantity,
                                     Mode = his.Mode,
                                     DocumentItemId = document.Id,
                                     WarehouseLayoutTitle = layout.Title,
                                     ModeTitle = his.Mode == 1 ? "ورودی" : "خروجی"

                                 }).ToList();

            //-------------------------------------------------------------------------------
            foreach (var item in result.Items)
            {
                var commodity = commodityList.Where(x => x.Id == item.CommodityId).FirstOrDefault();
                result.Items.Where(a => a.CommodityId == item.CommodityId).FirstOrDefault().Commodity = commodity;

                var history = historyModels.Where(a => a.DocumentItemId == item.Id).ToList();
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().WarehouseHistories.AddRange(history);


                var commodityWarehouse = commodityWarehouseList.Where(x => x.CommodityId == item.CommodityId).ToList();

                //---------- محاسبه موجودی کالا----------------------------------------------


                var QuantityUsed = item.WarehouseHistories.Sum(a => a.Quantity * a.Mode);
                var QuantityNeed = item.Quantity - QuantityUsed;


                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().QuantityUsed = (double)QuantityUsed;
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().QuantityChose = QuantityNeed;


                commodityWarehouse.ForEach(a => a.QuantityNeed = (double)QuantityNeed);

                //=============================================================================
                //----------یافتن کالاهای مشابه موجود در انبار---------------------------------

                var _commodityWarehouse = commodityWarehouse.Where(a => a.QuantityAvailable >= a.QuantityNeed).FirstOrDefault();
                if (_commodityWarehouse != null)
                {
                    result.Items.Where(a => a.Id == item.Id).FirstOrDefault().WarehouseLayoutQuantity = _commodityWarehouse;
                }
            }

            return result;
        }

        public async Task<ReceiptQueryModel> GetByDocumentIdByVoucherHeadId(int VoucherHeadId)
        {

            var DocumentId = await _context.DocumentHeads.Where(a => a.VoucherHeadId == VoucherHeadId).Select(a => a.DocumentId).FirstOrDefaultAsync();
            return await GetByDocumentId(Convert.ToInt32(DocumentId));
        }
        public async Task<ReceiptQueryModel> GetByDocumentId(int DocumentId)
        {

            var ListId = await _context.DocumentHeads.Where(a => a.DocumentId == DocumentId).Select(a => a.Id).ToListAsync();
            return await GetByListId(ListId);
        }
        public async Task<ReceiptQueryModel> GetById(int id)
        {



            var view = await _context.DocumentHeadGetIdView.Where(a => a.Id == id).FirstOrDefaultAsync();


            var result = _mapper.Map<ReceiptQueryModel>(view);

            var items = await _context.DocumentItems.Where(a => a.DocumentHeadId == id && !a.IsDeleted).ToListAsync();


            var itemsModel = _mapper.Map<List<ReceiptItemModel>>(items);

            if (itemsModel.Count() > 0)
                result.Items = itemsModel;
            else
            {
                throw new ValidationError("اقلامی در این سند وجود ندارد");
            }

            if (!String.IsNullOrEmpty(result.Tags))
            {
                result.TagClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(result.Tags);
                result.TagArray = result.TagClass.Select(a => a.Key).ToArray();
            }
            result.VatPercentage = Convert.ToInt32(VatPercentage());
            var commodityIds = result.Items.Select(x => x.CommodityId);
            var commodityList = await (from comidity in _context.ViewCommodity

                                       where commodityIds.Contains(comidity.Id)
                                       select new ReceiptCommodityModel()
                                       {
                                           Id = comidity.Id,
                                           Title = comidity.Title,
                                           TadbirCode = comidity.Code,
                                           CompactCode = comidity.Code,
                                           Code = comidity.Code,
                                           MeasureId = comidity.MeasureId,
                                           MeasureTitle = comidity.MeasureTitle,


                                       }).ToListAsync();

            if (!commodityList.Any())
            {
                throw new ValidationError("کالایی در این سند وجود ندارد");
            }

            List<WarehouseLayoutsCommoditiesModel> layoutListModel;
            List<WarehouseHistoriesDocumentView> layoutList = await _context.WarehouseHistoriesDocumentView.Where(a => a.DocumentHeadId == result.Id).ToListAsync();
            layoutListModel = _mapper.Map<List<WarehouseLayoutsCommoditiesModel>>(layoutList);


            foreach (var item in itemsModel)
            {
                var commodity = commodityList.Where(x => x.Id == item.CommodityId).FirstOrDefault();
                var Item = result.Items.Where(a => a.Id == item.Id).FirstOrDefault();
                Item.Commodity = commodity;
                Item.DocumentNo = result.DocumentNo;
                Item.RequestNo = result.RequestNo;
                var WarehouseLayouts = layoutListModel.Where(x => x.DocumentItemId == item.Id).ToList();
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().Layouts = WarehouseLayouts;

                //درصورتی که در انبار مواد اولیه جابه جایی یا رسید محصول زده شده باشد در صفحه ریالی نتوان تعداد آن را ویرایش کرد.
                Item.HasPermissionEditQuantity = result.ViewId == ConstantValues.CodeVoucherGroupValues.ViewIdRemoveAddWarehouse ? false : true;

            }
            ;


            //await GetCorrectionRequestModel(result);
            return result;
        }

        private async Task GetCorrectionRequestModel(ReceiptQueryModel result)
        {
            var CorrectionRequestModel = (from type in _context.BaseValueTypes
                                          join bas in _context.BaseValues on type.Id equals bas.BaseValueTypeId
                                          join cor in _context.CorrectionRequest on bas.Value equals cor.Status.ToString()
                                          join user in _context.User on cor.CreatedById equals user.Id

                                          where type.UniqueName == "Change_request_status" && cor.DocumentId == result.DocumentId

                                          select new CorrectionRequestModel()
                                          {
                                              Id = cor.Id,
                                              StatusTitle = bas.Title,
                                              VerifierDescription = cor.VerifierDescription,
                                              RequesterDescription = cor.RequesterDescription,
                                              CreatedAt = cor.CreatedAt,
                                              ModifiedAt = cor.ModifiedAt,
                                              Username = user.Username

                                          });
            result.CorrectionRequest = await CorrectionRequestModel.ToListAsync();
        }

        public async Task<ReceiptQueryModel> GetByInvoiceNo(string InvoiceNo, string Date, int? CreditAccountReferenceId)
        {
            DateTime? date = Date == null ? null : Convert.ToDateTime(Date).ToUniversalTime();
            DateTime? toDate = Date == null ? null : Convert.ToDateTime(Date).AddDays(1).ToUniversalTime();
            var result = new ReceiptQueryModel();
            var receipt = await _context.DocumentHeadGetIdView.Where(a => a.InvoiceNo == InvoiceNo
            && (a.InvoiceDate >= date && a.InvoiceDate < toDate || date == null)
            && (a.CreditAccountReferenceId == CreditAccountReferenceId || CreditAccountReferenceId == null)
            ).ToListAsync();

            if (receipt.Where(a => a.InvoiceNo != null).Any())
            {
                result = _mapper.Map<ReceiptQueryModel>(receipt.Where(a => a.InvoiceNo != null).FirstOrDefault());
                result.TotalItemPrice = receipt.Sum(a => a.TotalItemPrice);
                result.VatDutiesTax = receipt.Sum(a => a.VatDutiesTax);
                result.TotalProductionCost = receipt.Sum(a => a.TotalProductionCost);
                result.ExtraCost = receipt.Sum(a => a.ExtraCost);
            }
            else
            {
                result = _mapper.Map<ReceiptQueryModel>(receipt.FirstOrDefault());
            }

            var ListId = receipt.Select(a => a.Id).ToList();

            var items = await _context.DocumentItems.Where(a => ListId.Contains(a.DocumentHeadId) && !a.IsDeleted).ToListAsync();

            var itemsModel = (from i in items
                              join r in receipt on i.DocumentHeadId equals r.Id
                              select new ReceiptItemModel()
                              {
                                  Id = i.Id,
                                  DocumentHeadId = i.DocumentHeadId,
                                  CommodityId = i.CommodityId,
                                  DocumentNo = r.DocumentNo,
                                  RequestNo = r.RequestNo,
                                  Quantity = i.Quantity,
                                  Description = i.Description,
                                  UnitBasePrice = i.UnitBasePrice,
                                  ProductionCost = i.ProductionCost,
                                  CurrencyPrice = i.CurrencyPrice,
                                  UnitPrice = i.UnitPrice,
                                  IsImportPurchase = r.IsImportPurchase,

                              }

                         ).ToList();

            if (itemsModel.Count() > 0)
                result.Items = itemsModel;
            else
            {
                throw new ValidationError("اقلامی در این سند وجود ندارد");
            }
            var commodityIds = result.Items.Select(x => x.CommodityId);



            var commodityList = await (from comidity in _context.ViewCommodity

                                       where commodityIds.Contains(comidity.Id)
                                       select new ReceiptCommodityModel()
                                       {
                                           Id = comidity.Id,
                                           Title = comidity.Title,
                                           TadbirCode = comidity.Code,
                                           CompactCode = comidity.Code,
                                           Code = comidity.Code,
                                           MeasureId = comidity.MeasureId,
                                           MeasureTitle = comidity.MeasureTitle

                                       }).ToListAsync();
            if (!commodityList.Any())
            {
                throw new ValidationError("کالایی در این سند وجود ندارد");
            }


            foreach (var item in itemsModel)
            {
                var commodity = commodityList.Where(x => x.Id == item.CommodityId).FirstOrDefault();
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().Commodity = commodity;


            }


            await GetCorrectionRequestModel(result);
            return result;
        }
        public async Task<ReceiptQueryModel> GetByListId(List<int> ListId)
        {
            if (ListId.Count() == 0)
            {
                throw new ValidationError("لیستی برای انتخاب وجود ندارد");
            }
            var result = new ReceiptQueryModel();

            var receipts = await _context.DocumentHeadGetIdView.Where(a => ListId.Contains(a.Id)).ToListAsync();

            result = _mapper.Map<ReceiptQueryModel>(receipts.FirstOrDefault());
            FindDifferentGroupValue(result, receipts);
            FindDifferentAccountHeadId(result, receipts);

            result.IsFreightChargePaid = receipts.Where(a => a.IsFreightChargePaid == true).Count() > 0;


            var Document = await _context.Document.Where(a => a.Id == result.DocumentId).FirstOrDefaultAsync();

            if (Document != null)
            {
                result.DocumentDescription = Document.DocumentDescription;
                result.FinancialOperationNumber = Document.FinancialOperationNumber;
            }

            result.TotalItemPrice = receipts.Sum(a => a.TotalItemPrice);
            result.VatDutiesTax = receipts.Sum(a => a.VatDutiesTax);
            result.TotalProductionCost = receipts.Sum(a => a.TotalProductionCost);

            result.VatPercentage = Convert.ToInt32(VatPercentage());

            var items = await _context.DocumentItems.Where(a => ListId.Contains(a.DocumentHeadId) && !a.IsDeleted).ToListAsync();

            var itemsModel = (from i in items
                              join r in receipts on i.DocumentHeadId equals r.Id
                              select new ReceiptItemModel()
                              {
                                  Id = i.Id,
                                  DocumentHeadId = i.DocumentHeadId,
                                  CommodityId = i.CommodityId,
                                  DocumentNo = r.DocumentNo,
                                  RequestNo = r.RequestNo,
                                  Quantity = i.Quantity,
                                  Description = i.Description,
                                  UnitBasePrice = i.UnitBasePrice,
                                  ProductionCost = i.ProductionCost,
                                  CurrencyPrice = i.CurrencyPrice,
                                  CurrencyRate = i.CurrencyRate,
                                  UnitPrice = i.UnitPrice,
                                  IsImportPurchase = r.IsImportPurchase,
                                  CurrencyBaseId = i.CurrencyBaseId,
                                  UnitPriceWithExtraCost = i.UnitPriceWithExtraCost,

                                  //درصورتی که در انبار مواد اولیه جابه جایی یا رسید محصول زده شده باشد در صفحه ریالی نتوان تعداد آن را ویرایش کرد.
                                  HasPermissionEditQuantity = r.ViewId == ConstantValues.CodeVoucherGroupValues.ViewIdRemoveAddWarehouse ? false : true
                              }



                         ).ToList();

            if (itemsModel.Count() > 0)
                result.Items = itemsModel.OrderBy(d => ListId.IndexOf(Convert.ToInt32(d.Id))).OrderBy(a => a.DocumentItemId).ToList();
            else
            {
                throw new ValidationError("اقلامی در این سند وجود ندارد");
            }

            if (!String.IsNullOrEmpty(result.Tags))
            {
                result.TagClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(result.Tags);
                result.TagArray = result.TagClass.Select(a => a.Key).ToArray();
            }

            var commodityIds = result.Items.Select(x => x.CommodityId);



            var commodityList = await (from comidity in _context.ViewCommodity

                                       where commodityIds.Contains(comidity.Id)
                                       select new ReceiptCommodityModel()
                                       {
                                           Id = comidity.Id,
                                           Title = comidity.Title,
                                           TadbirCode = comidity.Code,
                                           CompactCode = comidity.Code,
                                           Code = comidity.Code,
                                           MeasureId = comidity.MeasureId,
                                           MeasureTitle = comidity.MeasureTitle

                                       }).ToListAsync();
            if (!commodityList.Any())
            {
                throw new ValidationError("کالایی در این سند وجود ندارد");
            }


            foreach (var item in itemsModel)
            {
                var commodity = commodityList.Where(x => x.Id == item.CommodityId).FirstOrDefault();
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().Commodity = commodity;


            }
            await GetCorrectionRequestModel(result);
            return result;
        }

        private static void FindDifferentGroupValue(ReceiptQueryModel result, List<DocumentHeadGetIdView> receipt)
        {
            var DebitAccountReferenceId = receipt.GroupBy(s => s.DebitAccountReferenceId)
                .Select(g => new { Id = g.Key, Instructors = g.Distinct().ToList() })
                .ToList();
            var CreditAccountReferenceId = receipt.GroupBy(s => s.CreditAccountReferenceId)
                .Select(g => new { Id = g.Key, Instructors = g.Distinct().ToList() })
                .ToList();


            if (DebitAccountReferenceId.Count() > 1)
            {
                result.DebitAccountReferenceId = -1;
                result.DebitAccountReferenceGroupId = -1;
            }
            if (CreditAccountReferenceId.Count() > 1)
            {
                result.CreditAccountReferenceId = -1;
                result.CreditAccountReferenceGroupId = -1;
            }
        }
        private static void FindDifferentAccountHeadId(ReceiptQueryModel result, List<DocumentHeadGetIdView> receipt)
        {
            var CreditAccountHeadId = receipt.GroupBy(s => s.CreditAccountHeadId)
                .Select(g => new { Id = g.Key, Instructors = g.Distinct().ToList() })
                .ToList();
            var DebitAccountHeadId = receipt.GroupBy(s => s.DebitAccountHeadId)
                .Select(g => new { Id = g.Key, Instructors = g.Distinct().ToList() })
                .ToList();


            if (CreditAccountHeadId.Count() > 1)
            {
                result.CreditAccountHeadId = -1;

            }
            if (DebitAccountHeadId.Count() > 1)
            {
                result.DebitAccountHeadId = -1;

            }
        }

        public async Task<PagedList<ReceiptQueryModel>> GetAll(int? DocumentStatuesBaseValue, bool? IsImportPurchase, string FromDate, string ToDate, PaginatedQueryModel query)
        {

            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();

            

            List<int> AllowAccessToWareHouse = AccessWarehouse();

            var entities = _context.ReceiptView.Where(a => (a.DocumentStauseBaseValue == DocumentStatuesBaseValue || DocumentStatuesBaseValue == 0)
                                                   && (a.IsImportPurchase == IsImportPurchase || IsImportPurchase == null)
                                                   && (a.DocumentDate >= from || from == null)
                                                   && (a.DocumentDate <= to || to == null)
                                                   && (AllowAccessToWareHouse.Contains((int)a.WarehouseId))

           )
                        .FilterQuery(query.Conditions).OrderByDescending(a => a.DocumentDate).OrderByDescending(a => a.DocumentNo)
                        .ProjectTo<ReceiptQueryModel>(_mapper.ConfigurationProvider)

                        .OrderByMultipleColumns(query.OrderByProperty);

            var TotalCount = await entities.CountAsync();

            var list = (List<ReceiptQueryModel>)await entities.Paginate(query.Paginator()).ToListAsync();

            list.ForEach(a =>
            {
                if (!String.IsNullOrEmpty(a.Tags))
                {
                    a.TagClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(a.Tags);
                }
            });
            return new PagedList<ReceiptQueryModel>()
            {
                Data = (IEnumerable<ReceiptQueryModel>)list,
                TotalCount = query.PageIndex <= 1
                ? TotalCount

                : 0
            };
        }
        /// <summary>
        /// لیست سندها به صورت کالا به کالا
        /// </summary>
        /// <param name="DocumentStatuesBaseValue"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public async Task<PagedList<ReceiptQueryModel>> GetAllReceiptItemsCommodity(int? DocumentStatuesBaseValue, string FromDate, string ToDate, PaginatedQueryModel query)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            List<int> AllowAccessToWareHouse = AccessWarehouse();
            var entities = _context.ReceiptItemsView.Where(a => ((a.DocumentStauseBaseValue == DocumentStatuesBaseValue
                                                                || (DocumentStatuesBaseValue == 0
                                                                  && a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveReceipt
                                                                  && a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveBuy
                                                                  && a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveRequest))  //(int)DocumentState.Leave
                                                       && (a.DocumentDate >= from || from == null)
                                                       && (a.DocumentDate <= to || to == null)
                                                       && (AllowAccessToWareHouse.Contains((int)a.WarehouseId))
       ))
                    .FilterQuery(query.Conditions).OrderBy(a => a.DocumentDate).OrderByDescending(a => a.DocumentNo)
                    .ProjectTo<ReceiptQueryModel>(_mapper.ConfigurationProvider)
                    .OrderByMultipleColumns(query.OrderByProperty);

            var TotalCount = await entities.CountAsync();
            var list = (List<ReceiptQueryModel>)await entities.Paginate(query.Paginator()).ToListAsync();


            list.ForEach(a =>
            {


                if (!String.IsNullOrEmpty(a.Tags))
                {
                    a.TagClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(a.Tags);

                }

            });
            return new PagedList<ReceiptQueryModel>()
            {
                Data = (IEnumerable<ReceiptQueryModel>)list,
                TotalCount = query.PageIndex <= 1 ? TotalCount : 0
            };

        }
        public async Task<PagedList<WarehouseHistoriesDocumentViewModel>> GetAllReceiptItems(string FromDate, string ToDate, PaginatedQueryModel query)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            var entities = _context.WarehouseHistoriesDocumentView.Where(a => (a.InvoiceDate >= from || from == null)
                                                                         && (a.InvoiceDate <= to || to == null))

                  .FilterQuery(query.Conditions)
                  .ProjectTo<WarehouseHistoriesDocumentViewModel>(_mapper.ConfigurationProvider)
                  .OrderByMultipleColumns(query.OrderByProperty);


            var TotalCount = await entities.CountAsync();
            var list = (List<WarehouseHistoriesDocumentViewModel>)await entities.Paginate(query.Paginator()).ToListAsync();
            var TotalSum = await entities.SumAsync(a => ((a.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmountLeave || a.DocumentStauseBaseValue == (int)DocumentStateEnam.registrationAccountingLeave) ? -1 : 1) * a.ItemUnitPrice * a.Quantity);

            return new PagedList<WarehouseHistoriesDocumentViewModel>()
            {
                Data = (IEnumerable<WarehouseHistoriesDocumentViewModel>)list,
                TotalCount = query.PageIndex <= 1 ? TotalCount : 0,
                TotalSum= Convert.ToDecimal(TotalSum),
            };

        }
        public async Task<PagedList<ReceiptQueryModel>> GetAllAmountReceipt(string FromDate, string ToDate, PaginatedQueryModel query)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            List<int> AllowAccessToWareHouse = AccessWarehouse();
            var entities = _context.ReceiptWithCommodityView.Where(a => (a.DocumentDate >= from || from == null)
                                                         && (a.DocumentDate <= to || to == null)
                                                         && (AllowAccessToWareHouse.Contains((int)a.WarehouseId))

           )
                        .FilterQuery(query.Conditions)
                        .ProjectTo<ReceiptQueryModel>(_mapper.ConfigurationProvider)
                        .OrderByMultipleColumns(query.OrderByProperty);

            var TotalCount = await entities.CountAsync();

            var list = (List<ReceiptQueryModel>)await entities.Paginate(query.Paginator()).ToListAsync();

            list.ForEach(a =>
            {


                if (!String.IsNullOrEmpty(a.Tags))
                {
                    a.TagClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(a.Tags);

                }

            });
            return new PagedList<ReceiptQueryModel>()
            {
                Data = (IEnumerable<ReceiptQueryModel>)list,
                TotalCount = query.PageIndex <= 1
                ? TotalCount
                : 0
            };
        }
        public async Task<PagedList<ReceiptQueryModel>> GetByViewId(int ViewId, string FromDate, string ToDate, PaginatedQueryModel query)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            List<int> AllowAccessToWareHouse = AccessWarehouse();
            var entities = _context.ReceiptWithCommodityView.Where(a => a.ViewId == ViewId
                                                   && (a.DocumentDate >= from || from == null)
                                                   && (a.DocumentDate <= to || to == null)
                                                   //سندهایی که براساس فرمول ساخت خروج آن ایجاد شده است آورده نشود و سند های متقابل ورودی آن آورده شود
                                                   && (a.DocumentStauseBaseValue != (int)DocumentStateEnam.Transfer && a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveReceipt)
                                                   && (AllowAccessToWareHouse.Contains((int)a.WarehouseId)))
                        .FilterQuery(query.Conditions).OrderByDescending(a => a.DocumentDate).OrderByDescending(a => a.DocumentNo)
                        .ProjectTo<ReceiptQueryModel>(_mapper.ConfigurationProvider)
                        .OrderByMultipleColumns(query.OrderByProperty);

            var TotalCount = await entities.CountAsync();

            var list = (List<ReceiptQueryModel>)await entities.Paginate(query.Paginator()).ToListAsync();

            list.ForEach(a =>
            {


                if (!String.IsNullOrEmpty(a.Tags))
                {
                    a.TagClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(a.Tags);

                }

            });
            return new PagedList<ReceiptQueryModel>()
            {
                Data = (IEnumerable<ReceiptQueryModel>)list,
                TotalCount = query.PageIndex <= 1
                    ? TotalCount : 0


            };
        }
        /// <summary>
        /// درخواست دریافت کالا
        /// </summary>
        /// <param name="DocumentStatuesBaseValue"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<ReceiptQueryModel>> GetByDocumentStatuesBaseValue(int DocumentStatuesBaseValue, string FromDate, string ToDate, PaginatedQueryModel query)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            //---------------------------------------

            List<int> AllowAccessToWareHouse = AccessWarehouse();
            var entities = _context.ReceiptView.Where(a => (a.DocumentStauseBaseValue == DocumentStatuesBaseValue || DocumentStatuesBaseValue == 0)
                                                   && (a.DocumentDate >= from || from == null)
                                                   && (a.DocumentDate <= to || to == null)
                                                   && ((AllowAccessToWareHouse.Contains((int)a.WarehouseId)) || DocumentStatuesBaseValue == 0)
                                                   && a.ParentId == null  //سندهایی که براساس فرمول ساخت خروج آن ایجاد شده است آورده نشود و سند های متقابل ورودی آن آورده شود
           )
                        .FilterQuery(query.Conditions).OrderByDescending(a => a.DocumentDate).OrderByDescending(a => a.DocumentNo)
                        .ProjectTo<ReceiptQueryModel>(_mapper.ConfigurationProvider)
                        .OrderByMultipleColumns(query.OrderByProperty);

            var TotalCount = await entities.CountAsync();

            var list = (List<ReceiptQueryModel>)await entities.Paginate(query.Paginator()).ToListAsync();

            list.ForEach(a =>
            {
                if (!String.IsNullOrEmpty(a.Tags))
                {
                    a.TagClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(a.Tags);

                }

            });
            return new PagedList<ReceiptQueryModel>()
            {
                Data = (IEnumerable<ReceiptQueryModel>)list,
                TotalCount = query.PageIndex <= 1
                    ? TotalCount

                    : 0
            };
        }
        public async Task<ReceiptQueryModel> GetByDocumentNoAndCodeVoucherGroupId(int DocumentNo, int codeVoucherGroupId)
        {
            var YearId = _currentUserAccessor.GetYearId();
            if (codeVoucherGroupId > 0)
            {
                codeVoucherGroupId = GetNewCodeVoucherGroupId(codeVoucherGroupId, (int)DocumentStateEnam.requestReceive);
            }
            //اگر درخواست خرید در سیستم دانا تعریف شده بود 

            var view = await _context.ReceiptWithCommodityView.Where(a => a.DocumentNo == DocumentNo  && (a.CodeVoucherGroupId == codeVoucherGroupId || codeVoucherGroupId == 0)).OrderByDescending(a=>a.DocumentDate).FirstOrDefaultAsync();

            //var view = await _context.ReceiptWithCommodityView.Where(a => a.DocumentNo == DocumentNo && a.YearId == YearId && (a.CodeVoucherGroupId == codeVoucherGroupId || codeVoucherGroupId == 0)).FirstOrDefaultAsync();

            var result = _mapper.Map<ReceiptQueryModel>(view);
            if (result == null)
            {
                throw new ValidationError("شماره درخواست یافت نشد");
            }
            await ReceiptQueryGet(result);
            return result;
        }
        public async Task<ReceiptQueryModel> GetByDocumentNoAndDocumentStauseBaseValue(int DocumentNo, int DocumentStauseBaseValue)
        {
            var YearId = _currentUserAccessor.GetYearId();


            var view = await _context.ReceiptWithCommodityView.Where(a => a.DocumentNo == DocumentNo && a.YearId == YearId && (a.DocumentStauseBaseValue == DocumentStauseBaseValue || DocumentStauseBaseValue == 0)).FirstOrDefaultAsync();

            var result = _mapper.Map<ReceiptQueryModel>(view);
            if (result == null)
            {
                throw new ValidationError("شماره درخواست یافت نشد");
            }
            await ReceiptQueryGet(result);
            return result;
        }
        public int GetNewCodeVoucherGroupId(int CodeVoucherGroupId, int DocumentStatuesBaseValue)
        {
            var codeVoucherGroup = _context.CodeVoucherGroups.Where(a => a.Id == CodeVoucherGroupId).FirstOrDefault();

            if (codeVoucherGroup == null)
            {
                throw new ValidationError("برای تغییر وضعیت هیچ نوع سندی در حسابداری تعریف نشده است");
            }
            var codeVoucherGroupCode = codeVoucherGroup.Code;
            var NewCode = codeVoucherGroupCode.Substring(0, 2) + DocumentStatuesBaseValue.ToString();

            CodeVoucherGroupId = _context.CodeVoucherGroups.Where(a => a.Code == NewCode).Select(a => a.Id).FirstOrDefault();
            return CodeVoucherGroupId;
        }
        private async Task ReceiptQueryGet(ReceiptQueryModel result)
        {
            if (result == null)
            {
                return;
            }
            var codeVoucherGroupUniqueName = _context.CodeVoucherGroups.Where(a => a.Id == result.CodeVoucherGroupId).Select(a => a.UniqueName).FirstOrDefault();

            if (!String.IsNullOrEmpty(result.Tags))
            {
                result.TagClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(result.Tags);
                result.TagArray = result.TagClass.Select(a => a.Key).ToArray();
            }
            var items = await _context.DocumentItems.Where(a => a.DocumentHeadId == result.Id && !a.IsDeleted).ToListAsync();

            if (!items.Any())
            {
                throw new ValidationError("برای این سند هیچ اقلامی وجود ندارد");
            }

            var itemsModel = _mapper.Map<List<ReceiptItemModel>>(items);

            result.Items = itemsModel;

            var commodityIds = result.Items.Select(x => x.CommodityId);

            List<WarehouseLayoutsCommoditiesModel> layoutListModel;
            //--------------------محل قرارگرفتن کالا در انبار-
            if (result.DocumentStauseBaseValue == (int)DocumentStateEnam.requestBuy || result.DocumentStauseBaseValue == (int)DocumentStateEnam.requestReceive)
            {
                List<WarehouseLayoutsCommoditiesView> layoutList = await _context.WarehouseLayoutsCommoditiesView.Where(a => commodityIds.Contains(Convert.ToInt32(a.CommodityId)) && a.WarehouseId == result.WarehouseId).ToListAsync();
                layoutListModel = _mapper.Map<List<WarehouseLayoutsCommoditiesModel>>(layoutList);
                foreach(var s in layoutListModel)               
                {

                    s.Quantity = await _context.WarehouseHistoriesDocumentItemView.Where(a => a.Commodityld == s.CommodityId && a.WarehouseLayoutId == s.WarehouseLayoutId && a.YearId==_currentUserAccessor.GetYearId() ).SumAsync(a => a.Quantity*a.Mode);
                };
                
                result.InvoiceNo = (await _context.DocumentHeadView.Where(a => a.RequestNo == result.DocumentNo.ToString() && a.WarehouseId == result.WarehouseId && a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveReceipt).Select(a => a.DocumentNo).FirstOrDefaultAsync()).ToString();

            }
            else
            {
                List<WarehouseHistoriesDocumentView> layoutList = await _context.WarehouseHistoriesDocumentView.Where(a => a.DocumentHeadId == result.Id).ToListAsync();
                layoutListModel = _mapper.Map<List<WarehouseLayoutsCommoditiesModel>>(layoutList);
            }




            //-------------------اگر قرار است کالا اموال خارج شود
            List<AssetsSerialModel> Assetslist = new List<AssetsSerialModel>();
            if (codeVoucherGroupUniqueName == ConstantValues.CodeVoucherGroupValues.RequesReceiveAssets)
            {
                Assetslist = _context.AssetsNotAssignedView.Where(a => commodityIds.Contains(Convert.ToInt32(a.CommodityId))).Select(a =>
                    new AssetsSerialModel()
                    {
                        Id = a.Id,
                        CommodityId = Convert.ToInt32(a.CommodityId),
                        Serial = a.AssetSerial,
                        Title = a.AssetSerial,

                    }).ToList();

            }
            //--------------------مشخصات کالا-----------------
            var commodityList = (from comidity in layoutListModel

                                 select new ReceiptCommodityModel()
                                 {
                                     Id = comidity.CommodityId,
                                     Title = comidity.CommodityTitle,
                                     TadbirCode = comidity.CommodityTadbirCode,
                                     CompactCode = comidity.CommodityCompactCode,
                                     Code = comidity.CommodityCode,
                                     MeasureId = comidity.MeasureId,
                                     MeasureTitle = comidity.MeasureTitle,



                                 }).Distinct().ToList();


            foreach (var item in itemsModel)
            {
                var commodity = commodityList.Where(x => x.Id == item.CommodityId).FirstOrDefault();
                //-----------اگر کالا در لیست در محل های انبار موجود نبود
                if (commodity == null)
                {
                    commodity = _mapper.Map<ReceiptCommodityModel>(_context.ViewCommodity.Where(x => x.Id == item.CommodityId).FirstOrDefault());
                }
                var WarehouseLayouts = layoutListModel.Where(x => x.CommodityId == item.CommodityId || x.DocumentItemId == item.Id).ToList();

                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().Layouts = WarehouseLayouts;
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().Commodity = commodity;
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().DocumentNo = result.DocumentNo;
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().RequestNo = result.RequestNo;
                //تعداد قابل انتخاب 
                //Quantity-QuantityUsed=RemainQuantity
                //Q-C=R => -C=R-Q => C=-(R-C)
                var Q = result.Items.Where(a => a.Id == item.Id).FirstOrDefault().Quantity;
                var R = result.Items.Where(a => a.Id == item.Id).FirstOrDefault().RemainQuantity;
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().QuantityUsed = -1 * (R - Q);

                if (codeVoucherGroupUniqueName == ConstantValues.CodeVoucherGroupValues.RequesReceiveAssets)
                {
                    result.Items.Where(a => a.Id == item.Id).FirstOrDefault().AssetsSerials = Assetslist.Where(a => a.CommodityId == item.CommodityId).ToList();
                }
                //اگر درخواست کالا مصرفی باشد ، کیل مصرف را نشان میدهد.
                if (codeVoucherGroupUniqueName == ConstantValues.CodeVoucherGroupValues.RequestReceiveConsumption || codeVoucherGroupUniqueName == ConstantValues.CodeVoucherGroupValues.RequestBuyConsumption)
                {
                    result.Items.Where(a => a.Id == item.Id).FirstOrDefault().ConsumptionCommodity = await _procedureCallService.GetConsumptionCommodityByRequesterReferenceId(Convert.ToInt32(result.RequesterReferenceId), item.CommodityId, ConstantValues.CodeVoucherGroupValues.RemoveConsumptionWarhouse);
                }

            }
        }
        public async Task<PagedList<ReceiptItemModel>> GetALLDocumentItemsBom(int DocumentItemsId)
        {

            var list =  (from comidity in _context.ViewCommodity
                              join bom in _context.DocumentItemsBom.Where(a => a.DocumentItemsId == DocumentItemsId && !a.IsDeleted)
                              on comidity.Id equals bom.CommodityId
                              select new ReceiptItemModel()
                              {
                                  Id = bom.Id,
                                  CommodityTitle = comidity.Title,
                                  CommodityCode = comidity.Code,
                                  MainMeasureId = Convert.ToInt32(comidity.MeasureId),
                                  MeasureTitle = comidity.MeasureTitle,
                                  UnitPrice = bom.UnitPrice,
                                  ProductionCost = bom.ProductionCost,
                                  Quantity = bom.Quantity,
                                  DocumentItemId = bom.DocumentItemsId

                              });


            return new PagedList<ReceiptItemModel>()
            {
                Data = (IEnumerable<ReceiptItemModel>)await list.ToListAsync(),
                TotalCount = 0

            };

        }
        public async Task<ReceiptItemModel> GetDocumentItemsBomById(int Id)
        {
            var list = await (from comidity in _context.ViewCommodity
                              join i in _context.DocumentItemsBom on comidity.Id equals i.CommodityId
                              where i.Id == Id
                              select new ReceiptItemModel()
                              {
                                  Id = i.Id,
                                  CommodityTitle = comidity.Title,
                                  CommodityCode = comidity.Code,
                                  MainMeasureId = Convert.ToInt32(comidity.MeasureId),
                                  MeasureTitle = comidity.MeasureTitle,
                                  UnitPrice = i.UnitBasePrice,
                                  ProductionCost = i.ProductionCost,
                                  Quantity = i.Quantity,

                              }).FirstOrDefaultAsync();

            return list;

        }
        public async Task<PagedList<SpReceiptGroupbyInvoice>> GetAllReceiptGroupInvoice(DateTime? FromDate,
            DateTime? ToDate,
            int? VoucherHeadId,
            string DocumentIds,
             int? CreditAccountReferenceId,
             int? DebitAccountReferenceId,
             int? CreditAccountHeadId,
             int? CreditAccountReferenceGroupId,
             int? DebitAccountReferenceGroupId,
             int? DebitAccountHeadId,
            PaginatedQueryModel query)
        {
            
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();

            var list = await _procedureCallService.GetAllReceiptGroupInvoice(from, to, VoucherHeadId, DocumentIds, CreditAccountReferenceId, DebitAccountReferenceId, CreditAccountHeadId, CreditAccountReferenceGroupId, DebitAccountReferenceGroupId, DebitAccountHeadId, query);
            return list;

        }
        private int GetAccountingCode(int VoucherHeadId, int DocumentStatuesBaseValue, int? AccountingCodeVoucherGroup)
        {
            if (VoucherHeadId > 0 && DocumentStatuesBaseValue == 53)
            {
                return AccountingCodeVoucherGroup ?? 43; // Return the group or 43 if null
            }
            else if (VoucherHeadId > 0 && DocumentStatuesBaseValue == 54)
            {
                return 44; // Return 44 for this condition
            }
            else
            {
                // Return DocumentStatusBaseValue for any other case
                return DocumentStatuesBaseValue;
            }


        }


        public async Task<int[]> GetDocumentAttachmentIdByDocumentIdId(int DocumentHeadId)
        {
            var DocumentId = await _context.DocumentHeads.Where(a => a.Id == DocumentHeadId).Select(a => a.DocumentId).FirstOrDefaultAsync();
            var Ids = await _context.DocumentAttachment.Where(a => a.DocumentId == DocumentId).Select(a => a.AttachmentId).ToArrayAsync();
            return Ids;
        }
        public async Task<CorrectionRequest> GetCorrectionRequestById(int DocumentId, int status)
        {
            var DocumentHead = await _context.DocumentHeads.Where(a => a.DocumentId == DocumentId).FirstOrDefaultAsync();
            var CorrectionRequest = await _context.CorrectionRequest.Where(a => a.DocumentId == DocumentId && (a.Status == status)).FirstOrDefaultAsync();
            return CorrectionRequest;
        }
        public async Task<CorrectionRequest> GetCorrectionRequestById(int Id)
        {
            var CorrectionRequest = await _context.CorrectionRequest.Where(a => a.Id == Id).FirstOrDefaultAsync();
            return CorrectionRequest;
        }
        /// <summary>
        /// فهرست جامع
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<ReceiptQueryModel>> GetComprehensiveList(string FromDate, string ToDate, PaginatedQueryModel query)
        {


            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();

            List<int> AllowAccessToWareHouse = AccessWarehouse();
            var entities = _context.ReceiptView.Where(a => (a.DocumentDate >= from || from == null)
                                                      && (a.DocumentDate <= to || to == null)
                                                      && (AllowAccessToWareHouse.Contains((int)a.WarehouseId))
                                                      && (a.DocumentStauseBaseValue != (int)DocumentStateEnam.Transfer)
                                                      )

                        .FilterQuery(query.Conditions)//.OrderByDescending(a => a.DocumentDate).OrderByDescending(a => a.DocumentNo)
                        .ProjectTo<ReceiptQueryModel>(_mapper.ConfigurationProvider)
                        .OrderByMultipleColumns(query.OrderByProperty);

            var TotalCount = await entities.CountAsync();
            var list = (List<ReceiptQueryModel>)await (from ent in entities
                                                       join user in _context.User on ent.CreatedById equals user.Id
                                                       join person in _context.Persons on user.PersonId equals person.Id
                                                       join year in _context.Years on ent.YearId equals year.Id
                                                       select new ReceiptQueryModel()
                                                       {
                                                           Id = ent.Id,
                                                           DocumentDate = ent.DocumentDate,
                                                           DocumentNo = ent.DocumentNo,
                                                           DocumentDescription = ent.DocumentDescription,
                                                           DocumentId = ent.DocumentId,
                                                           DocumentStauseBaseValueTitle = ent.DocumentStauseBaseValueTitle,
                                                           DocumentStateBaseTitle = ent.DocumentStateBaseTitle,
                                                           DocumentStauseBaseValue = ent.DocumentStauseBaseValue,
                                                           CreditReferenceTitle = ent.CreditReferenceTitle,
                                                           DebitReferenceTitle = ent.DebitReferenceTitle,
                                                           CodeVoucherGroupTitle = ent.CodeVoucherGroupTitle,
                                                           Quantity = ent.Quantity,
                                                           CommodityCode = ent.CommodityCode,
                                                           CommodityTitle = ent.CommodityTitle,
                                                           RequestDate = ent.RequestDate,
                                                           RequestNo = ent.RequestNo,
                                                           InvoiceNo = ent.InvoiceNo,
                                                           YearId = year.YearName,
                                                           Username = person.FirstName + " " + person.LastName,
                                                           VoucherHeadId = ent.VoucherHeadId,
                                                           VoucherNo = ent.VoucherNo,
                                                           CodeVoucherGroupId = ent.CodeVoucherGroupId,
                                                           WarehouseTitle = ent.WarehouseTitle,
                                                           WarehouseId = ent.WarehouseId,
                                                           MeasureTitle = ent.MeasureTitle,
                                                           SerialNumber=ent.SerialNumber,


                                                       }).Paginate(query.Paginator()).ToListAsync();





            return new PagedList<ReceiptQueryModel>()
            {
                Data = (IEnumerable<ReceiptQueryModel>)list,
                TotalCount = query.PageIndex <= 1
                ? TotalCount : 0
            };
        }
        public async Task<PagedList<ReceiptQueryModel>> GetAllReceiptRequestNO(string RequestNO, PaginatedQueryModel query)
        {

            List<int> AllowAccessToWareHouse = AccessWarehouse();
            var entities = _context.ReceiptItemsView.Where(a => a.RequestNo == RequestNO && (AllowAccessToWareHouse.Contains((int)a.WarehouseId)))

           .FilterQuery(query.Conditions)
                        .ProjectTo<ReceiptQueryModel>(_mapper.ConfigurationProvider)
                        .OrderByMultipleColumns(query.OrderByProperty);

            var TotalCount = await entities.CountAsync();

            var list = (List<ReceiptQueryModel>)await entities.Paginate(query.Paginator()).ToListAsync();

            return new PagedList<ReceiptQueryModel>()
            {
                Data = (IEnumerable<ReceiptQueryModel>)list,
                TotalCount = query.PageIndex <= 1
                ? TotalCount

                : 0
            };
        }
        private List<int> AccessWarehouse()
        {
            return _context.AccessToWarehouse.Where(a => a.TableName == ConstantValues.AccessToWarehouseEnam.Warehouses && a.UserId == _currentUserAccessor.GetId() && !a.IsDeleted).Select(a => a.WarehouseId).ToList();
        }
        //بدست آوردن درصد مالیات ارزش افزوده-------------------------------------
        private string VatPercentage()
        {
            return _context.BaseValues.Where(a => a.UniqueName.ToLower() == ConstantValues.ConstBaseValue.vatDutiesTax.ToLower()).Select(a => a.Value).FirstOrDefault();
        }
        public async Task<long> GetCostImportReceipts(int ReferenceId)
        {
            var CompanyId = _currentUserAccessor.GetCompanyId();
            var YearId = _currentUserAccessor.GetYearId();

            var result = await _procedureCallService.GetCostImportReceipts(CompanyId, YearId, ReferenceId);

            return result != null ? Convert.ToInt64(result.TotalDebit) : 0;
        }
        public async Task<DocumentItem> GetStartReceiptsItem(int CommodityId, int WarehouseId)
        {

            var YearId = _currentUserAccessor.GetYearId();

            var receipt = await _context.DocumentHeads.Where(a => a.WarehouseId == WarehouseId && a.YearId == _currentUserAccessor.GetYearId() && (a.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmountStart || a.DocumentStauseBaseValue == (int)DocumentStateEnam.registrationAccountingStart)).FirstOrDefaultAsync();
            if (receipt == null)
            {
                throw new ValidationError("سند افتتاحیه این سال مالی برای این انبار ثبت نشده است.");
            }


            var item = await _context.DocumentItems.Where(a => a.CommodityId == CommodityId && a.DocumentHeadId == receipt.Id).FirstOrDefaultAsync();



            return item;
        }

        public Task<CorrectionRequest> GetCorrectionRequestById(int DocumentId, int status, int CodeVoucherGroupId)
        {
            throw new NotImplementedException();
        }
        public async Task<PagedList<WarehouseRequestExitViewModel>> GetWarehouseRequestExitView(string FromDate, string ToDate, PaginatedQueryModel query)
        {
            DateTime from = Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime to = Convert.ToDateTime(ToDate).ToUniversalTime();
            var entities = _context.WarehouseRequestExitView.Where(a => a.DocumentDate >= from && a.DocumentDate <= to)
                            .ProjectTo<WarehouseRequestExitViewModel>(_mapper.ConfigurationProvider)
                            .FilterQuery(query.Conditions)
                            .OrderByMultipleColumns(query.OrderByProperty);

            return new PagedList<WarehouseRequestExitViewModel>()
            {

                Data = await entities.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            };
        }
        public async Task<PagedList<View_Sina_FinancialOperationNumberModel>> GeView_Sina_FinancialOperationNumber(PaginatedQueryModel query)
        {

            var entities = _context.View_Sina_FinancialOperationNumber.ProjectTo<View_Sina_FinancialOperationNumberModel>(_mapper.ConfigurationProvider).FilterQuery(query.Conditions)
                            .OrderByMultipleColumns(query.OrderByProperty);

            return new PagedList<View_Sina_FinancialOperationNumberModel>()
            {

                Data = await entities.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            };
        }
    }
}
