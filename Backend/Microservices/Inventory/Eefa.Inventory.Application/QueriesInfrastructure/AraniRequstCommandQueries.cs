using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using Eefa.Invertory.Infrastructure.Services.AdminApi;
using Eefa.Invertory.Infrastructure.Services.Arani;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Eefa.Inventory.Domain.Common.ConstantValues;

namespace Eefa.Inventory.Application
{
    public class AraniRequstCommandQueries : IAraniRequstCommandQueries
    {

        private readonly IMapper _mapper;
        private readonly IAraniService _araniService;
        private readonly IAdminApiService _adminApiService;
        private readonly IRepository<CodeVoucherGroup> _codeVoucherGroupRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMeasureUnitQueries _measureUnitQueries;
        private readonly ILogger _logger;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Domain.Commodity> _commodityRepository;
        private readonly IRepository<CommodityCategory> _commodityCategoryRepository;
        private readonly IRepository<CommodityMeasures> _commodityMeasuresRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptQueries _receiptQueries;
        private readonly IRepository<AccountReference> _accountReferenceRepository;
        private readonly IRepository<AccountReferencesRelReferencesGroup> _accountReferenceRelReferencesRepository;
        private readonly IRepository<BaseValue> _baseValueRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<Employees> _employeesRepository;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IProcedureCallService _procedureCallService;

        public AraniRequstCommandQueries(

              IMapper mapper,
              IAraniService araniService
            , IRepository<CodeVoucherGroup> codeVoucherGroupRepository
            , ICurrentUserAccessor currentUserAccessor
            , IInvertoryUnitOfWork context
            , IReceiptQueries receiptQueries
            , IMeasureUnitQueries measureUnitQueries
            , IAdminApiService adminApiService
            , ILogger<TemporaryReceiptQueries> logger
            , IRepository<Person> personRepository
            , IRepository<AccountReference> accountReferenceRepository
            , IRepository<Domain.Commodity> commodityRepository
            , IRepository<CommodityCategory> commodityCategoryRepository
            , IRepository<CommodityMeasures> commodityMeasuresRepository
            , IRepository<AccountReferencesRelReferencesGroup> accountReferenceRelReferencesRepository
            , IReceiptRepository receiptRepository
            , IRepository<BaseValue> baseValueRepository
            , IRepository<Document> documentRepository
            , IRepository<DocumentItem> documentItemRepository
            , IRepository<Employees> employeesRepository
            , IReceiptCommandsService receiptCommandsService
            , IProcedureCallService procedureCallService
            )
        {
            _mapper = mapper;
            _araniService = araniService;
            _codeVoucherGroupRepository = codeVoucherGroupRepository;
            _currentUserAccessor = currentUserAccessor;
            _context = context;
            _receiptQueries = receiptQueries;
            _measureUnitQueries = measureUnitQueries;
            _adminApiService = adminApiService;
            _logger = logger;
            _personRepository = personRepository;
            _accountReferenceRepository = accountReferenceRepository;
            _commodityRepository = commodityRepository;
            _commodityCategoryRepository = commodityCategoryRepository;
            _commodityMeasuresRepository = commodityMeasuresRepository;
            _accountReferenceRelReferencesRepository = accountReferenceRelReferencesRepository;
            _receiptRepository = receiptRepository;
            _baseValueRepository = baseValueRepository;
            _documentItemRepository = documentItemRepository;
            _documentRepository = documentRepository;
            _employeesRepository = employeesRepository;
            _receiptCommandsService = receiptCommandsService;
            _procedureCallService = procedureCallService;
        }


        //------------------ گرفتن درخواست خرید ورود در انبار ---------------------------
        public async Task<ReceiptQueryModel> GetPurchaseRequestById(int requestId)
        {


            ReceiptCommodityModel itemsCommodity = new ReceiptCommodityModel();
            var receipt = new ReceiptQueryModel();
            List<ReceiptItemModel> items = new List<ReceiptItemModel>();


            var request = (await _araniService.GetRequestById(requestId, ConstantValues.AraniService.UrlPurchase)).FirstOrDefault();


            if (request == null)
            {
                return null;
            }
            receipt.RequestDate = request.ZamaneMoredeNiaz;
            receipt.IsManual = false;
            receipt.YearId = _currentUserAccessor.GetYearId();
            receipt.DocumentDate = ConstBaseValue.DocumnetDateUtc;
            receipt.DocumentDescription = request.Sharh;

            receipt.TotalItemPrice = Convert.ToInt64(request.GheymateKol);
            receipt.TotalProductionCost = Convert.ToInt64(request.GheymateKol);

           

            itemsCommodity = await GetCommodity(requestId, itemsCommodity, items, request.Code, request.AnbarGhataAt_Kala_Id_Name, request.MeghdareDarkhasti, (long)request.GheymateVahed, (long)request.GheymateKol, request.Moshakhasat, request.MeghdareDarkhasti);


            var result = _mapper.Map<ReceiptQueryModel>(receipt);
            result.Items = items;
            result.RequesterReferenceTitle = request.DarkhastKonande_Name;
            result.FollowUpReferenceTitle = request.PeygiriKonande_Name;
            //---------------------جستجو درخواست کننده از جدول پرسنل-----------------------
            if (request.DarkhastKonande_Code != null)
            {
                result.RequesterReferenceId = await _receiptCommandsService.GetEmployee(request.DarkhastKonande_Code);
                if (result.RequesterReferenceId == null)
                {
                    //throw new ValidationError("اطلاعات درخواست کننده به درستی در سیستم ثبت نشده است ، لطفا با واحد حسابداری تهران تماس بگیرید.");
                    //var person = await _receiptCommandsService.AddNewPerson(request.DarkhastKonande_Name, request.DarkhastKonande_Code, request.DarkhastKonande_Code);
                    //if (person != null)
                    //{
                    //    await _receiptCommandsService.AddNewEmployees(person.Id, request.DarkhastKonande_Code);
                    //    result.RequesterReferenceId = person.AccountReferenceId;
                    //}

                }
            }
            //---------------------جستجو پیگیری کننده از جدول پرسنل-----------------------
            if (request.PeygiriKonande_Code != null)
            {
                result.FollowUpReferenceId = await _receiptCommandsService.GetEmployee(request.PeygiriKonande_Code);
                if (result.FollowUpReferenceId == null)
                {

                    //throw new ValidationError("اطلاعات پیگیری کننده به درستی در سیستم ثبت نشده است ، لطفا با واحد حسابداری تهران تماس بگیرید.");

                    //var person = await _receiptCommandsService.AddNewPerson(request.PeygiriKonande_Name, request.PeygiriKonande_Code, request.PeygiriKonande_Code);
                    //if (person != null)
                    //{
                    //    await _receiptCommandsService.AddNewEmployees(person.Id, request.PeygiriKonande_Code);
                    //    result.FollowUpReferenceId = person.AccountReferenceId;
                    //}

                }
            }

            receipt.RequestNo = requestId.ToString();


            return result;
        }

        private async Task<ReceiptCommodityModel> GetCommodity(int requestId,
            ReceiptCommodityModel itemsCommodity,
            List<ReceiptItemModel> items,
            string CommodityCode,
            string CommodityName,
            double Quantity,
            long? UnitPrice,
            long? UnitBasePrice,
            string Description,
            double QuantityChose

            )
        {
            var commodity = await _context.ViewCommodity.Where(x => x.Code.ToLower() == CommodityCode.ToLower()).FirstOrDefaultAsync();
            //----------------------------------اگر کالا وجود نداشت--------------------------
            Domain.Commodity commodityNew = new Domain.Commodity();
            if (commodity == null)
            {
                commodityNew = await _receiptCommandsService.AddCommodity(CommodityCode, CommodityName, CommodityCode);
                if (commodityNew == null)
                {
                    throw new ValidationError("مشکل در ثبت کالا این درخواست");
                }
            }
            if (commodity != null)
            {
                itemsCommodity = _mapper.Map<ViewCommodity, ReceiptCommodityModel>(commodity);
            }
            else
            {
                itemsCommodity = _mapper.Map<Domain.Commodity, ReceiptCommodityModel>(commodityNew);
            }

            ReceiptItemModel item = new ReceiptItemModel()
            {
                MainMeasureId = itemsCommodity.MeasureId ?? 0,
                Quantity = Quantity,
                CommodityId = Convert.ToInt32(itemsCommodity.Id),
                UnitPrice = (long)UnitPrice,
                UnitBasePrice = (long)UnitBasePrice,
                DocumentMeasureId = itemsCommodity.MeasureId ?? 0,
                Commodity = itemsCommodity,
                RequestNo = requestId.ToString(),
                QuantityChose = QuantityChose,
                Description = Description,
            };


            items.Add(item);
            return itemsCommodity;
        }

        //-----------------درخواست ورود کالاهای خروجی----------------------------------- 
        public async Task<ReceiptQueryModel> GetReturnCommodityWarehouse(int requestId)
        {
            ReceiptCommodityModel itemsCommodity = new ReceiptCommodityModel();
            var receipt = new ReceiptQueryModel();
            List<ReceiptItemModel> items = new List<ReceiptItemModel>();

            AraniReturnCommodityWarehouseModel request = (await _araniService.GetReturnCommodityWarehouse(requestId, ConstantValues.AraniService.UrlReturnCommodity));

            if (request == null || request.items.Length == 0)
            {
                return null;
            }
            var Header = request.result.FirstOrDefault();


            receipt.RequestDate = Header.Tarikh_Sabt;
            receipt.IsManual = false;
            receipt.YearId = _currentUserAccessor.GetYearId();
            receipt.DocumentDate = ConstBaseValue.DocumnetDateUtc;
            receipt.RequestNo = requestId.ToString();
            receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Temp;
            foreach (var item in request.items)
            {
                itemsCommodity = await GetCommodity(requestId, itemsCommodity, items, item.Kala_Code, item.Kala_Name, item.Megdar_Khorooj, 0, 0, "", item.Megdar_Khorooj);
            }


            var result = _mapper.Map<ReceiptQueryModel>(receipt);
            result.Items = items;
            result.RequesterReferenceTitle = Header.DarkhastKonandeh;


            //---------------------جستجو درخواست کننده از جدول پرسنل-----------------------
            result.RequesterReferenceId = await _receiptCommandsService.GetEmployee(Header.CodePersoneli);
            if (result.RequesterReferenceId == null)
            {
                throw new ValidationError("اطلاعات درخواست کننده به درستی در سیستم ثبت نشده است ، لطفا با واحد حسابداری تهران تماس بگیرید.");
                //var person = await _receiptCommandsService.AddNewPerson(Header.DarkhastKonandeh, Header.CodePersoneli, Header.CodePersoneli);
                //if (person != null)
                //{
                //    await _receiptCommandsService.AddNewEmployees(person.Id, Header.CodePersoneli);
                //    result.RequesterReferenceId = person.AccountReferenceId;
                //}

            }
            return result;


        }

        //------------------گرفتن درخواست خروج از انبار----------------------------------
        public async Task<RequestCommodityWarehouseModel> leavingWarehouseRequestById(int requestId, int warehouseId)
        {
            await _procedureCallService.WarehouseRequestExitUpdateIsDeleted();

            var request = (await _araniService.GetRequestCommodityWarehouse(requestId.ToString(), ConstantValues.AraniService.UrlRequestCommodity));

            if (request == null)
            {
                throw new ValidationError("شماره درخواست خروج اشتباه می باشد");
            }
            if (request.result.Length == 0)
            {
                throw new ValidationError("اطلاعات پایه درخواست خروج وجود ندارد");
            }
            if (request.items.Length == 0)
            {
                throw new ValidationError("برای این درخواست خروج اقلام کالایی تعریف نشده است");
            }

            RequestCommodityWarehouseModel model = new RequestCommodityWarehouseModel();

            model.Request = new RequestResult();
            model.items = new List<RequestItemCommodity>();




            var res = request.result.FirstOrDefault();
            model.Request.Force = res.Fori;
            model.Request.StatusTitle = res.Vaziat_Name;
            model.Request.StatusId = res.Vaziat;
            model.Request.RequestDate = res.Tarikh;
            model.Request.RequestDate_Jalali = res.Tarikh_Jalali;
            model.Request.RequesterTitle = res.DarkhastKonandeh;
            model.Request.warehouseCourierTitle = res.PeykeAnbar_Name;
            model.Request.RequestNo = requestId.ToString();
           


            foreach (var item in request.items)
            {
                item.CodeKootah = item.CodeKootah ?? item.Code;
                //اگر کالا تعریف نشده بود----------------------------------------
                if (!await _context.Commodities.Where(x => x.CompactCode.ToLower() == item.CodeKootah.ToLower()).AnyAsync())
                {

                    await _receiptCommandsService.AddCommodity(item.Code, item.KalaCodeName, item.CodeKootah);
                }

                var commodity = (from com in _context.ViewCommodity.Where(a => a.CompactCode == item.CodeKootah).ToList()
                                 select new
                                 {
                                     com.Id,
                                     com.MeasureId,
                                     com.Code,
                                     com.Title,
                                     com.MeasureTitle,
                                     com.CompactCode
                                 }).ToList();

                var commodityCode = commodity.Select(a => a.CompactCode).ToList();

                var listlayout = _context.WarehouseLayoutsCommoditiesView.Where(a => commodityCode.Contains(a.CommodityCompactCode.ToLower()) && (a.WarehouseId == warehouseId || warehouseId == null) && (AccessWarehouse().Contains((int)a.WarehouseId))).ToList();

                var IdList = listlayout.Select(a => a.Id);
                var warehouseRequestExit = _context.WarehouseRequestExit.Where(a=>a.RequestId== requestId  && !a.IsDeleted ).ToList();

               var WarehouseHistories=  _context.WarehouseHistoriesDocumentItemView.Where(a => commodityCode.Contains(a.CompactCode.ToLower()) && (a.WarehouseId == warehouseId || warehouseId == null) && a.YearId == _currentUserAccessor.GetYearId()).ToList();


                var Items = (from reqt in request.items.Where(a => a.Id == item.Id)
                             join com in commodity
                             on reqt.CodeKootah.ToLower() equals com.CompactCode.ToLower()
                             join wr in warehouseRequestExit on reqt.Id equals wr.RequestItemId into lwr

                             from wr in lwr.DefaultIfEmpty()


                             select new RequestItemCommodity()
                             {
                                 Layouts = listlayout.Where(a => a.CommodityCompactCode == com.CompactCode).Select(a => new WarehouseLayoutsCommoditiesModel()
                                 {
                                     AllowOutput = a.AllowOutput,
                                     CommodityId = a.CommodityId,
                                     CommodityCode = a.CommodityCode,
                                     CommodityTitle = a.CommodityTitle,
                                     WarehouseId = a.WarehouseId,
                                     Quantity = WarehouseHistories.Where(b=>b.WarehouseLayoutId==a.WarehouseLayoutId && a.CommodityCode==b.CommodityCode).Sum(a=>a.Quantity*a.Mode),
                                     WarehouseLayoutTitle = a.WarehouseLayoutTitle,
                                     WarehouseLayoutCapacity = a.WarehouseLayoutCapacity,
                                     WarehouseTitle = a.WarehouseTitle,
                                     QuantityOutput = warehouseRequestExit.Where(b=>b.RequestItemId== reqt.Id && b.WarehouseLayoutQuantityId==a.Id).Sum(a => a.Quantity),
                                     Id = a.Id,
                                 }).ToList(),

                                 CommodityCode = com.CompactCode,
                                 //CommodityId = com.Id,
                                 CommodityName = com.Title,
                                 Quantity = reqt.Tedad,
                                 QuantityTotal = reqt.Tedad,
                                 QuantityExit = warehouseRequestExit.Where(a => a.RequestItemId == reqt.Id).Sum(a => a.Quantity),
                                 MeasureTitle = com.MeasureTitle,
                                 MeasureId = com.MeasureId,
                                 Description = reqt.TozihateAnbar  ,
                                 PlaceUse = reqt.MahaleEstefade_Name,
                                 PlaceUseDetail = reqt.MahaleEstefadeDetail_Name,
                                 Daghi = reqt.Daghi,
                                 DescriptionSupervisor = reqt.SharheSarparast,
                                 ReturnDaghi = reqt.BargashteDaghi,
                                 RequestItemId = reqt.Id,
                                 LayoutTitle = reqt.Mogheiat,
                                 
                                 Inventory = WarehouseHistories.Sum(a => a.Quantity * a.Mode),


                             }
                              ); ;
                var ItemCommodity = Items.GroupBy(x => x.CommodityCode).Select(y => y.First()).ToList();

                

                if (!ItemCommodity.Any())
                {
                    throw new ValidationError("برای این درخواست خروج هیچ کالایی در انبار موجودی ندارد");
                }

                model.items.AddRange(ItemCommodity.ToList());
            }

            var commodityType = request.items.Where(a => !a.Code.StartsWith("S004105")).ToList();
            model.Request.DocumentNo = await _receiptCommandsService.AddReceiptForExitReceiptArani(model.Request, res.DarkhastKonandeh_Code, warehouseId, commodityType.Count() > 0? ConstantValues.CodeVoucherGroupValues.RemoveCommodityWarhouse: ConstantValues.CodeVoucherGroupValues.RemoveConsumptionWarhouse);

            return model;


        }
        private List<int> AccessWarehouse()
        {
            return _context.AccessToWarehouse.Where(a => a.TableName == ConstantValues.AccessToWarehouseEnam.Warehouses && a.UserId == _currentUserAccessor.GetId() && !a.IsDeleted).Select(a => a.WarehouseId).ToList();
        }
        public async Task<PagedList<spErpDarkhastJoinDocumentHeads>> GetErpDarkhastJoinDocumentHeads(
            DateTime? FromDate,
            DateTime? ToDate,
            string RequestNo,
            PaginatedQueryModel query)
        {
            
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            spErpDarkhastJoinDocumentHeadsParam model = new spErpDarkhastJoinDocumentHeadsParam() { fromDate = from, toDate = to, RequestNo = RequestNo };
            var list = await _procedureCallService.GetErpDarkhastJoinDocumentHeads(model, query);
            return list;

        }
    }






}



