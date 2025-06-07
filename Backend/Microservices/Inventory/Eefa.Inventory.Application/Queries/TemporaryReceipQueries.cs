using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain.Common;
using Eefa.Invertory.Infrastructure.Services.AdminApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Eefa.Inventory.Domain;
using Eefa.Common.Data.Query;
using AutoMapper.QueryableExtensions;
using Eefa.Inventory.Domain.Enum;

namespace Eefa.Inventory.Application
{
    public class TemporaryReceiptQueries : ITemporaryReceipQueries
    {

        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptQueries _receiptQueries;
        private readonly IAdminApiService _adminApiService;
        private readonly ICommodityQueries _commodityQueries;
        private readonly IMeasureUnitQueries _measureUnitQueries;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly ISinaRequestCommandQueries _araniRequstCommandQueries;
        private readonly IRepository<CodeVoucherGroup> _codeVoucherGroupRepository;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;

        public TemporaryReceiptQueries(

              IMapper mapper
            , IInvertoryUnitOfWork context
            , IReceiptQueries receiptQueries
            , IAdminApiService adminApiService
            , ICommodityQueries commodityQueries
            , ILogger<TemporaryReceiptQueries> logger
            , IMeasureUnitQueries measureUnitQueries
            , ICurrentUserAccessor currentUserAccessor
            , IProcedureCallService procedureCallService
            , IReceiptCommandsService receiptCommandsService
            , ISinaRequestCommandQueries araniRequstCommandQueries
            , IRepository<CodeVoucherGroup> codeVoucherGroupRepository
            , IWarehouseLayoutCommandsService warehouseLayoutCommandsService
            )

        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
            _receiptQueries = receiptQueries;
            _adminApiService = adminApiService;
            _commodityQueries = commodityQueries;
            _measureUnitQueries = measureUnitQueries;
            _currentUserAccessor = currentUserAccessor;
            _procedureCallService = procedureCallService;
            _receiptCommandsService = receiptCommandsService;
            _araniRequstCommandQueries = araniRequstCommandQueries;
            _codeVoucherGroupRepository = codeVoucherGroupRepository;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;


        }

        public async Task<ReceiptQueryModel> GetById(int id)
        {
            var view = await _context.ReceiptView.Where(a => a.Id == id).FirstOrDefaultAsync();
            var code = await _context.CodeVoucherGroups.Where(a => a.Id == view.CodeVoucherGroupId).Select(a => a.UniqueName).FirstOrDefaultAsync();
            var result = _mapper.Map<ReceiptQueryModel>(view);
            await ReceiptQueryGet(result, code);
            return result;
        }
        //در حالت برگشت کالا به انبار
        public async Task<ReceiptQueryModel> GetByDocumentNo(int DocumentNo)
        {

            var view = await _context.ReceiptView.Where(a => a.DocumentNo == DocumentNo
                                                                && a.DocumentStauseBaseValue != (int)DocumentStateEnam.Leave
                                                                && a.DocumentStauseBaseValue != (int)DocumentStateEnam.invoiceAmountLeave
                                                                && a.DocumentStauseBaseValue != (int)DocumentStateEnam.registrationAccountingLeave
                                                                && a.DocumentStauseBaseValue != (int)DocumentStateEnam.requestReceive
            ).FirstOrDefaultAsync();

            ReceiptQueryModel result = await GetQuery(DocumentNo, view);

            //----------------------------------------------------------------------
            if (result == null)
            {
                return new ReceiptQueryModel();
            }

            int? DebitAccountHeadId = result.DebitAccountHeadId;
            int? DebitAccountReferenceId = result.DebitAccountReferenceId;
            int? DebitAccountReferenceGroupId = result.DebitAccountReferenceGroupId;
            int? CreditAccountHeadId = result.CreditAccountHeadId;
            int? CreditAccountReferenceId = result.CreditAccountReferenceId;
            int? CreditAccountReferenceGroupId = result.CreditAccountReferenceGroupId;

            //اطلاعات قبلی از روی ورودی کالاست الان می خواهیم خروج کالا را بزنیم---------
            result.DebitAccountHeadId = CreditAccountHeadId;
            result.DebitAccountReferenceId = CreditAccountReferenceId;
            result.DebitAccountReferenceGroupId = CreditAccountReferenceGroupId;
            result.CreditAccountHeadId = DebitAccountHeadId;
            result.CreditAccountReferenceId = DebitAccountReferenceId;
            result.CreditAccountReferenceGroupId = DebitAccountReferenceGroupId;

            return result;


        }
        //اگر درخواست خرید در سیستم دانا تعریف نشده بود 
        public async Task<ReceiptQueryModel> GetByDocumentNo(int DocumentNo, int warehouseId)
        {

            var view = await _context.ReceiptView.Where(a => a.DocumentNo == DocumentNo
                                                        && (a.WarehouseId == warehouseId || warehouseId == 0)
                                                        && a.DocumentStauseBaseValue == (int)DocumentStateEnam.requestBuy)
                .FirstOrDefaultAsync();

            ReceiptQueryModel result = await GetQuery(DocumentNo, view);

            return result;


        }
        private async Task<ReceiptQueryModel> GetQuery(int DocumentNo, ReceiptView view)
        {
            var result = _mapper.Map<ReceiptQueryModel>(view);
            if (result != null)
            {
                Receipt receipt = new Receipt();
                receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Temp;
                receipt.CodeVoucherGroupId = Convert.ToInt32(result.CodeVoucherGroupId);

                CodeVoucherGroup NewCodeVoucherGroup = await _receiptCommandsService.GetNewCodeVoucherGroup(receipt);
                result.CodeVoucherGroupId = NewCodeVoucherGroup.Id;

                result.RequestNo = DocumentNo.ToString();
                await ReceiptQueryGet(result, NewCodeVoucherGroup.Code);

                result.DocumentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToUniversalTime();

            }

            return result;
        }
        public async Task<PagedList<ReceiptQueryModel>> GetAll(int? DocumentStatuesBaseValue, string FromDate, string ToDate, PaginatedQueryModel query)
        {

            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();


            List<int> AllowAccessToWareHouse = AccessWarehouse();

            var entities = _context.ReceiptView.Where(a => (a.DocumentStauseBaseValue == DocumentStatuesBaseValue || DocumentStatuesBaseValue == 0)

                                                   && (a.DocumentDate >= from || from == null)
                                                   && (a.DocumentDate <= to || to == null)
                                                   && (AllowAccessToWareHouse.Contains((int)a.WarehouseId))

           )
                        .FilterQuery(query.Conditions).OrderByDescending(a => a.DocumentDate).OrderByDescending(a => a.DocumentNo)
                        .ProjectTo<ReceiptQueryModel>(_mapper.ConfigurationProvider)
                        .OrderByMultipleColumns(query.OrderByProperty);

            var TotalCount = await entities.CountAsync();

            var list = (List<ReceiptQueryModel>)await entities.Paginate(query.Paginator()).ToListAsync();
            foreach (var a in list)
            {
                var items = await _context.DocumentItems.Where(b => b.DocumentHeadId == a.Id).ToListAsync();
                a.IsAllowedInputOrOutputCommodity = true;
                foreach (var item in items)
                {
                    var layout = await _warehouseLayoutCommandsService.FindLayout(Convert.ToInt32(a.WarehouseId), item.CommodityId);
                    if(layout.Status != null)
                    if (layout.Status != WarehouseLayoutStatus.InputOnly && layout.Status != WarehouseLayoutStatus.Free)
                    {
                        a.IsAllowedInputOrOutputCommodity = false; break;
                    }
                }
            };

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
        private async Task ReceiptQueryGet(ReceiptQueryModel result, string CodeVoucher)
        {
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
                throw new ValidationError("برای این سند هیچ کالایی وجود ندارد");
            }
            result.Items.ForEach(a =>
            {
                a.QuantityChose = _commodityQueries.GetQuantityCommodity(Convert.ToInt32(result.WarehouseId), Convert.ToInt32(a.CommodityId));

            });

            foreach (var item in itemsModel)
            {
                var commodity = commodityList.Where(x => x.Id == item.CommodityId).FirstOrDefault();

                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().Commodity = commodity;
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().DocumentNo = result.DocumentNo;
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().RequestNo = result.RequestNo;
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().AssetsSerialsCount = _context.Assets.Where(a => a.DocumentHeadId == item.DocumentHeadId && a.CommodityId == item.CommodityId).Count();
                //اگر درخواست کالا مصرفی باشد ، کیل مصرف را نشان میدهد.
                if (CodeVoucher == ConstantValues.CodeVoucherGroupValues.RequestReceiveConsumption || CodeVoucher == ConstantValues.CodeVoucherGroupValues.RequestBuyConsumption)
                {
                    var ConsumptionCommodity = await _procedureCallService.GetConsumptionCommodityByRequesterReferenceId(Convert.ToInt32(result.RequesterReferenceId), item.CommodityId, ConstantValues.CodeVoucherGroupValues.RemoveConsumptionWarhouse);
                    result.Items.Where(a => a.Id == item.Id).FirstOrDefault().ConsumptionCommodity = ConsumptionCommodity;
                    item.CommodityQuota = ConsumptionCommodity.Count() > 0 ? ConsumptionCommodity.Select(a => a.CommodityQuota).FirstOrDefault() : 0;
                    item.CommodityQuotaUsed = ConsumptionCommodity.Count() > 0 ? ConsumptionCommodity.Sum(a => a.Quantity) : 0;
                }
                if (CodeVoucher == ConstantValues.CodeVoucherGroupValues.ChangeMaterialWarhouse)
                {
                    var fromWarehouseId = await _context.DocumentHeads.Where(b => b.ParentId == result.Id).Select(b => b.WarehouseId).FirstOrDefaultAsync();
                    if (fromWarehouseId != null)
                    {
                        item.SecondaryQuantity = _commodityQueries.GetQuantityCommodity(Convert.ToInt32(fromWarehouseId), Convert.ToInt32(item.CommodityId));
                    }

                }

            }
        }

        //--------------------فیلتر با درخواست دهنده-------------------------------------
        public async Task<ReceiptQueryModel> GetByRequesterReferenceId(int RequesterReferenceId, string FromDate, string ToDate)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();



            var receipt = await _context.ReceiptView.Where(a => a.RequesterReferenceId == RequesterReferenceId
                                                            && a.DocumentStauseBaseValue == (int)DocumentStateEnam.Temp
                                                            && (a.DocumentDate >= from || from == null)
                                                            && (a.DocumentDate <= to || to == null)
                                                            ).ToListAsync();

            var result = _mapper.Map<ReceiptQueryModel>(receipt.FirstOrDefault());

            var ListId = receipt.Select(a => a.Id).ToList();
            var items = await _context.DocumentItems.Where(a => ListId.Contains(a.DocumentHeadId) && !a.IsDeleted).ToListAsync();
            if (!items.Any())
            {
                throw new ValidationError("برای این سند هیچ اقلامی وجود ندارد");
            }

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
                                  CommoditySerial = i.CommoditySerial,
                              }

                         ).ToList();
            if (itemsModel.Count() == 0)
                return result;
            result.Items = itemsModel;

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
                throw new ValidationError("برای این سند هیچ کالایی وجود ندارد");
            }

            foreach (var item in itemsModel)
            {
                var commodity = commodityList.Where(x => x.Id == item.CommodityId).FirstOrDefault();

                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().Commodity = commodity;
            }

            return result;
        }

    }






}



