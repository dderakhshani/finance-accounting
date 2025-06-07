using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using Eefa.Invertory.Infrastructure.Context;
using Eefa.Invertory.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Index.HPRtree;

namespace Eefa.Inventory.Application
{
    public class SinaRequestCommandQueries : ISinaRequestCommandQueries
    {

        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IInvertoryUnitOfWork _context;
        private readonly ISinaService _SinaService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRepository<Domain.Commodity> _commodityRepository;
        private readonly IRepository<CommodityCategory> _commodityCategoryRepository;
        private readonly IRepository<CommodityMeasures> _commodityMeasuresRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IProcedureCallService _procedureCallService;

        public SinaRequestCommandQueries(

              IMapper mapper,
              ISinaService SinaService
            , IInvertoryUnitOfWork context
            , ILogger<TemporaryReceiptQueries> logger
            , ICurrentUserAccessor currentUserAccessor
            , IRepository<Domain.Commodity> commodityRepository
            , IRepository<CommodityCategory> commodityCategoryRepository
            , IRepository<CommodityMeasures> commodityMeasuresRepository
            , IReceiptCommandsService receiptCommandsService
            , IProcedureCallService procedureCallService
            )
        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
            _SinaService = SinaService;
            _currentUserAccessor = currentUserAccessor;
            _commodityRepository = commodityRepository;
            _commodityCategoryRepository = commodityCategoryRepository;
            _commodityMeasuresRepository = commodityMeasuresRepository;
            _receiptCommandsService = receiptCommandsService;
            _procedureCallService = procedureCallService;
        }

        public async Task<ICollection<SinaProduct>> GetProductByMaxSinaIdList()
        {
            var LastId = await _context.Commodities.MaxAsync(a => a.SinaId);
            try
            {
                var request = await _SinaService.GetProductList(Convert.ToInt32(LastId), ConstantValues.SinaService.UrlGetProduct);
                new LogWriter("GetProductByMaxSinaIdList Start Task", "GetProductByMaxSinaIdList");
                foreach (var item in request)
                {
                    var commodity = await _context.Commodities.Where(x => x.Code.ToLower() == item.ProductCode.ToLower()).FirstOrDefaultAsync();
                    if (commodity == null)
                    {
                       
                        await _receiptCommandsService.AddProduct(item);
                        await _commodityRepository.SaveChangesAsync();
                    }

                }
                
                new LogWriter("GetProductByMaxSinaIdList End Task", "GetProductByMaxSinaIdList");
                return request;
            }
            catch (Exception ex)
            {
                if (!(ex is ValidationError))
                {
                    new LogWriter("GetProductByMaxSinaIdList Error:" + ex.Message.ToString(), "GetProductByMaxSinaIdList");
                    if (ex.InnerException != null)
                    {
                        new LogWriter("GetProductByMaxSinaIdList InnerException:" + ex.InnerException.ToString(), "GetProductByMaxSinaIdList");
                    }

                }
                else { throw new ValidationError(ex.Message); };

            }
            return null;

        }
        public async Task<ICollection<SinaProduct>> GetAllProductNeedUpdate()
        {

            var request = await _SinaService.GetProductList(-1, ConstantValues.SinaService.UrlGetProduct);

            foreach (var item in request)
            {
                var commodity = await _context.Commodities.Where(x => x.Code.ToLower() == item.ProductCode.ToLower()).FirstOrDefaultAsync();
                if (commodity == null)
                {
                    new LogWriter(item.ProductCode, "GetAllProductNeedUpdate");
                    await _receiptCommandsService.AddProduct(item);
                }
                else
                {
                    if (commodity.Code.Contains("....") && commodity.SinaId == item.Id)
                        await _receiptCommandsService.UpdateProduct(item);
                }

            }
            try
            {
                await _commodityRepository.SaveChangesAsync();
            }
            catch (Exception ex) {

                new LogWriter("GetAllProductNeedUpdate Error"+ ex.Message, "GetAllProductNeedUpdate");
            }
          
           
            return request;
        }
        public async Task InsertProduct(SinaProduct model)
        {

            Domain.Commodity commodityNew = new Domain.Commodity();

            commodityNew = await _receiptCommandsService.AddProduct(model);
            if (commodityNew == null)
            {
                throw new ValidationError("مشکل در ثبت کالا این درخواست");
            }

        }
        public async Task<ReceiptQueryModel> GetInputProductToWarehouse(string Date)
        {

            //await GetProductByMaxSinaIdList();
            var request_89 = await _SinaService.GetInputProductToWarehouse(Date, 89, ConstantValues.SinaService.GetInputProduct);
            var request_90 = await _SinaService.GetInputProductToWarehouse(Date, 90, ConstantValues.SinaService.GetInputProduct);
            if (request_89 == null && request_90 == null)
            {
                return null;
            }

            List<SinaProducingInputProduct> request = new List<SinaProducingInputProduct>();
            request.AddRange(request_89);
            request.AddRange(request_90);


            ReceiptCommodityModel itemsCommodity = new ReceiptCommodityModel();
            var receipt = new ReceiptQueryModel();
            List<ReceiptItemModel> items = new List<ReceiptItemModel>();



            receipt.DocumentDate = Convert.ToDateTime(DateTime.UtcNow.ToShortDateString()).ToUniversalTime();

            foreach (var item in request)
            {
                itemsCommodity = await GetCommodity(itemsCommodity, items, item.TadbirCode, item.TadbirName,item.ProductId, item.QuantitySum, 1, 1, "");
                item.CommodityId = itemsCommodity.Id;

            }
            var result = _mapper.Map<ReceiptQueryModel>(receipt);
            result.ItemsCount = request.Count();
            result.Items = items;

            return result;


        }
        public async Task UpdateProductProperty(string Date)
        {
            var request_89 = await _SinaService.GetInputProductToWarehouse(Date, 89, ConstantValues.SinaService.GetInputProduct);
            var request_90 = await _SinaService.GetInputProductToWarehouse(Date, 90, ConstantValues.SinaService.GetInputProduct);
            

            List<SinaProducingInputProduct> request = new List<SinaProducingInputProduct>();
            request.AddRange(request_89);
            request.AddRange(request_90);
            foreach (var item in request)
            {
                try
                {
                    await _receiptCommandsService.UpdateProductProperty(Convert.ToInt32(item.CommodityId), item);
                }
                catch (Exception ex)
                {
                    //به مشکل خورد ادامه پیدا کند 
                }
               

            }
        }
        public async Task<ReceiptQueryModel> GetProductLeaveWarehouse(string Date)
        {
            //await GetProductByMaxSinaIdList();
            //http://sina.eefaceram.com/prime/Sales/Api/ApiGetExitSaleProduct?DocumentDate=1403/02/01
            var request = await _SinaService.GetOutProductToWarehouse(Date, ConstantValues.SinaService.GetOutProduct);

            ReceiptCommodityModel itemsCommodity = new ReceiptCommodityModel();
            var receipt = new ReceiptQueryModel();
            List<ReceiptItemModel> items = new List<ReceiptItemModel>();

            if (request == null)
            {
                return null;
            }
            receipt.DocumentDate = Convert.ToDateTime(DateTime.UtcNow.ToShortDateString()).ToUniversalTime();
            foreach (var item in request)
            {
                itemsCommodity = await GetCommodity(itemsCommodity, items, item.ProductCode,"",null, item.Quantity, 1, 1, item.FactorNo);

            }


            var result = _mapper.Map<ReceiptQueryModel>(receipt);
            result.ItemsCount = request.Count();
            result.Items = items;


            return result;
        }
        public async Task<ICollection<FilesByPaymentNumber>> GetFilesByPaymentNumber(string financialOperationNumber)
        {

            var result = await _SinaService.GetFilesByPaymentNumber(ConstantValues.SinaService.GetFilesByPaymentNumber, financialOperationNumber);


            return result;
        }
        private async Task<ReceiptCommodityModel> GetCommodity(ReceiptCommodityModel itemsCommodity, List<ReceiptItemModel> items, string CommodityCode, string TadbirName,int? ProductId, double Quantity, long? UnitPrice, long? UnitBasePrice, string RequestNo)
        {
            var commodity = await _context.ViewCommodity.Where(x => x.Code.ToLower() == CommodityCode.ToLower()).FirstOrDefaultAsync();


            itemsCommodity = _mapper.Map<ViewCommodity, ReceiptCommodityModel>(commodity);


            ReceiptItemModel item = new ReceiptItemModel();
            if (commodity != null)
            {
                item.MainMeasureId = itemsCommodity.MeasureId ?? 0;
                item.Quantity = Quantity;
                item.CommodityId = Convert.ToInt32(itemsCommodity.Id);
                item.UnitPrice = (long)UnitPrice;
                item.UnitBasePrice = (long)UnitBasePrice;
                item.DocumentMeasureId = item.MainMeasureId;
                item.Commodity = itemsCommodity;
                item.RequestNo = RequestNo.ToString();

                items.Add(item);
            }
            else
            {
              var model=  new SinaProduct()
                {
                    ProductCode = CommodityCode,
                    Id= ProductId,
                    ProductName = TadbirName,
                    UnitSaleType = "Meter"
              };
                await _receiptCommandsService.AddProduct(model);
               

                return await GetCommodity(itemsCommodity, items, CommodityCode, TadbirName, ProductId, Quantity, UnitPrice, UnitBasePrice, RequestNo);



            }

            return itemsCommodity;
        }


    }






}



