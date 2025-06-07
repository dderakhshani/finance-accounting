using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Exceptions;
using Eefa.Purchase.Application.Models;
using Eefa.Purchase.Application.Models.Receipt;
using Eefa.Purchase.Application.Queries.Abstraction;
using Eefa.Purchase.Domain.Aggregates.InvoiceAggregate;
using Eefa.Purchase.Domain.Common;
using Eefa.Purchase.Infrastructure.Context;
using Eefa.Purchase.Infrastructure.Services.AdminApi;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Purchase.Application.Queries
{
    public class InvoiceQueries : IInvoiceQueries
    {
        private readonly IMapper _mapper;
        private readonly PurchaseContext _contex;
        private readonly IRepository<Invoice> _InvoiceRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IBaseValueQueries _baseValueQueries;

        public InvoiceQueries(
              IMapper mapper
            , IRepository<Invoice> InvoiceRepository
            , PurchaseContext contex
            , IAdminApiService araniService
            , IBaseValueQueries baseValueQueries
            , ICurrentUserAccessor currentUserAccessor
            )
        {
            _mapper = mapper;
            _contex = contex;
            _baseValueQueries = baseValueQueries;
            _InvoiceRepository = InvoiceRepository;
            _currentUserAccessor = currentUserAccessor;
        }


        public async Task<InvoiceQueryModel> GetById(int id)
        {
            var view = await _contex.InvoiceView.Where(a => a.Id == id).FirstOrDefaultAsync();

            var result = _mapper.Map<InvoiceQueryModel>(view);

            var items = await _contex.DocumentItems.Where(a => a.DocumentHeadId == id && !a.IsDeleted).ToListAsync();


            var itemsModel = _mapper.Map<List<InvoiceItemModel>>(items);

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
            var commodityList = await (from comidity in _contex.ViewCommodity
                                       where commodityIds.Contains(comidity.Id)
                                       select new InvoiceCommodityModel()
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
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().DocumentNo = result.DocumentNo;
                result.Items.Where(a => a.Id == item.Id).FirstOrDefault().RequestNo = result.RequestNo;

            }

            return result;
        }
        public async Task<InvoiceQueryModel> GetByListId(List<int> ListId)
        {
            if (ListId.Count() == 0)
            {
                throw new ValidationError("لیستی برای انتخاب وجود ندارد");
            }
            var result = new InvoiceQueryModel();
            var Invoice = await _contex.InvoiceView.Where(a => ListId.Contains(a.Id)).ToListAsync();

            if (Invoice.Where(a => a.InvoiceNo != null).Any())
            {
                result = _mapper.Map<InvoiceQueryModel>(Invoice.Where(a => a.InvoiceNo != null).FirstOrDefault());
                result.TotalItemPrice = Invoice.Sum(a => a.TotalItemPrice);
                result.VatDutiesTax = Invoice.Sum(a => a.VatDutiesTax);
                result.TotalProductionCost = Invoice.Sum(a => a.TotalProductionCost);
            }
            else
            {
                result = _mapper.Map<InvoiceQueryModel>(Invoice.FirstOrDefault());
            }

            result.VatPercentage = Convert.ToInt32(VatPercentage());

            var items = await _contex.DocumentItems.Where(a => ListId.Contains(a.DocumentHeadId) && !a.IsDeleted).ToListAsync();

            var itemsModel = (from i in items
                              join r in Invoice on i.DocumentHeadId equals r.Id
                              select new InvoiceItemModel()
                              {
                                  Id = i.Id,
                                  DocumentHeadId = i.DocumentHeadId,
                                  CommodityId = i.CommodityId,
                                  DocumentNo = r.DocumentNo,
                                  RequestNo = r.RequestNo,
                                  Quantity = i.Quantity,
                                  Description = i.Description,
                                  UnitBasePrice = i.UnitBasePrice,
                                  CurrencyPrice = i.CurrencyPrice,
                                  UnitPrice = i.UnitPrice,

                              }

                         ).ToList();

            if (itemsModel.Count() > 0)
                result.Items = itemsModel;
            else
            {
                throw new ValidationError("کالایی در این سند وجود ندارد");
            }

            if (!String.IsNullOrEmpty(result.Tags))
            {
                result.TagClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(result.Tags);
                result.TagArray = result.TagClass.Select(a => a.Key).ToArray();
            }




            var commodityIds = result.Items.Select(x => x.CommodityId);



            var commodityList = await (from comidity in _contex.ViewCommodity

                                       where commodityIds.Contains(comidity.Id)
                                       select new InvoiceCommodityModel()
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

            return result;
        }

        //بدست آوردن درصد مالیات ارزش افزوده-------------------------------------
        private string VatPercentage()
        {
            return _contex.BaseValues.Where(a => a.UniqueName.ToLower() == ConstantValues.BaseValue.vatDutiesTax.ToLower()).Select(a => a.Value).FirstOrDefault();
        }

        public async Task<PagedList<InvoiceALLStatusModel>> GetInvoiceALLVoucherGroup()
        {

            var codeVoucherGroup = await _contex.CodeVoucherGroups.Where(a => a.Code.Substring(0, 2) == ConstantValues.CodeVoucherGroup.Code)
            .ProjectTo<InvoiceALLStatusModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

            var result = new PagedList<InvoiceALLStatusModel>()
            {
                Data = codeVoucherGroup,
                TotalCount = 0
            };
            return result;
        }

        public async Task<PagedList<InvoiceQueryModel>> GetAll(int codeVoucherGroupId, bool? IsImportPurchase, DateTime? FromDate, DateTime? ToDate, PaginatedQueryModel query)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            new LogWriter("GetAll from" + from.ToString());
            new LogWriter("GetAll to" + to.ToString());

            var entitis = _contex.InvoiceView.Where(a => a.CodeVoucherGroupId == codeVoucherGroupId
                                                   && (a.IsImportPurchase == IsImportPurchase || IsImportPurchase == null)
                                                   && (a.DocumentDate >= from || from == null)
                                                   && (a.DocumentDate <= to || to == null)
           )
                        .FilterQuery(query.Conditions).OrderByDescending(a => a.DocumentDate).OrderByDescending(a => a.DocumentNo)
                        .ProjectTo<InvoiceQueryModel>(_mapper.ConfigurationProvider)
                        .Paginate(query.Paginator())
                        .OrderByMultipleColumns(query.OrderByProperty);

            var list = (List<InvoiceQueryModel>)await entitis.ToListAsync();
                     
                     
            list.ForEach(a =>
            {


                if (!String.IsNullOrEmpty(a.Tags))
                {
                    a.TagClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagClass>>(a.Tags);

                }

            });
            return new PagedList<InvoiceQueryModel>()
            {
                Data = (IEnumerable<InvoiceQueryModel>)list,
                TotalCount = query.PageIndex <= 1
                    ? await _contex.InvoiceView.Where(a => a.CodeVoucherGroupId == codeVoucherGroupId).CountAsync()
                        
                    : 0
            };
        }
        //--------آیدی درخواست خرید کالا--------------------------
        public async Task<PagedList<InvoiceQueryModel>> GetInvoiceActiveRequestNo(int documentHeadId, int ReferenceId, string FromDate, string ToDate)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate);
            DateTime? to = ToDate == null ? DateTime.Now.ToUniversalTime() : Convert.ToDateTime(ToDate).ToUniversalTime();



            var documentItems =await _contex.DocumentItems.Where(a => a.DocumentHeadId == documentHeadId).ToListAsync();
            var commodityIdLis = documentItems.Select(a => a.CommodityId).ToList();

            List<InvoiceQueryModel> result =await Query(ReferenceId, to, commodityIdLis);

            return new PagedList<InvoiceQueryModel>()
            {
                Data = result,
                TotalCount = 0

            };

        }

    

        //در حالتی که از سیستم آرانی درخواست های خرید خوانده شود و در سیستم ایفا ثبت نشده باشد.
        //از کد کالا برای پیدا کردن فاکتورهای مرتبط با آن استفاده می کنیم.
        public async Task<PagedList<InvoiceQueryModel>> GetInvoiceActiveCommodityId(int CommodityId, int ReferenceId, string FromDate, string ToDate)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();

          
            List<int> commodityIdLis = new List<int>() { CommodityId };

            List<InvoiceQueryModel> result =await Query(ReferenceId, to, commodityIdLis);

            
            return new PagedList<InvoiceQueryModel>()
            {
                Data = result,
                TotalCount = 0

            };
        }

        private async Task<List<InvoiceQueryModel>> Query(int ReferenceId, DateTime? to, List<int> commodityIdLis)
        {
           
           
                var CodeVoucherGroupList = await _contex.CodeVoucherGroups.Where(a => a.UniqueName == ConstantValues.CodeVoucherGroup.PreInvoice ||
                                                                                       a.UniqueName == ConstantValues.CodeVoucherGroup.ContractVoucherGroup)
                                                                           .Select(a => a.Id).ToListAsync();
            
            
            if (!CodeVoucherGroupList.Any())
            {
                throw new ValidationError("کد نوع سند موجود نیست");
            }

            var ReceiptItemsView = _contex.ReceiptItemsView.Where(a => commodityIdLis.Contains(a.CommodityId)
                                                                     && CodeVoucherGroupList.Contains(a.CodeVoucherGroupId)
                                                                     && a.ExpireDate >= to
                                                                     && (ReferenceId == 0 || ReferenceId.Equals(a.CreditAccountReferenceId)
                                                                     ));
            var DocumentHeadIdList =await ReceiptItemsView.Distinct().Select(a => a.Id).ToListAsync();

            List<InvoiceItemModel> Items = _mapper.Map<List<InvoiceItemModel>>(ReceiptItemsView);

            var invoiceView = await _contex.InvoiceView.Where(a => DocumentHeadIdList.Contains(a.Id)).OrderByDescending(a => a.DocumentDate).ToListAsync();
            var result = _mapper.Map<List<InvoiceQueryModel>>(invoiceView).Select(invoice => new InvoiceQueryModel()
            {

                Id = invoice.Id,
                CodeVoucherGroupId = invoice.CodeVoucherGroupId,
                CodeVoucherGroupTitle = invoice.CodeVoucherGroupTitle,
                CreditAccountReferenceId = invoice.CreditAccountReferenceId,
                CreditAccountReferenceGroupId = invoice.CreditAccountReferenceGroupId,
                CreditReferenceTitle = invoice.CreditReferenceTitle,
                DocumentNo = invoice.DocumentNo,
                DocumentDate = invoice.DocumentDate,
                ExpireDate = invoice.ExpireDate,
                InvoiceNo = invoice.InvoiceNo,
                WarehouseId = invoice.WarehouseId,
                WarehouseTitle = invoice.WarehouseTitle,
                VatDutiesTax = invoice.VatDutiesTax,
            }).ToList();

            result.ForEach(a =>
            {
                a.Items = Items.Where(a => a.Id == a.Id).ToList();
            });
            return result;
            
        }
    }
}
