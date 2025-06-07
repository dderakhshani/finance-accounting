using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;
using System;
using System.Collections.Generic;
using Eefa.Common;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain.Common;
using Eefa.Common.Exceptions;

namespace Eefa.Inventory.Application
{
    public class ReportsQueries : IReportsQueries
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IReceiptQueries _ReceiptQueries;
        public ReportsQueries(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , ICurrentUserAccessor currentUserAccessor
            , IProcedureCallService procedureCallService
            , IReceiptCommandsService receiptCommandsService
            , IReceiptQueries ReceiptQueries
            )
        {
            _mapper = mapper;
            _context = context;
            _procedureCallService = procedureCallService;
            _currentUserAccessor = currentUserAccessor;
            _receiptCommandsService = receiptCommandsService;
            _ReceiptQueries = ReceiptQueries;

        }


        public async Task<List<spGetMonthlyEntryToWarehouse>> GetMonthlyEntryToWarehouse(int? CommodityId, int? WarehouseId, int? YearId, int? EnterMode)
        {

            var list = await _procedureCallService.GetMonthlyEntryToWarehouse(CommodityId, WarehouseId, YearId, EnterMode, _currentUserAccessor.GetId());
            return list.ToList();

        }
        public async Task<List<spCommodityReports>> GetCommodityReports(string FromDate, string ToDate, int? CommodityId, string WarehouseId, string CommodityTitle, PaginatedQueryModel query)
        {
            DateTime from = Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime to = Convert.ToDateTime(ToDate).ToUniversalTime();


            var list = await _procedureCallService.GetCommodityReports(from, to, CommodityId, WarehouseId, _currentUserAccessor.GetId(), _currentUserAccessor.GetYearId(), CommodityTitle, query);
            return list.ToList();

        }
        public async Task<List<spCommodityReportsRial>> GetCommodityReportsRial(string FromDate, string ToDate, int? CommodityId, string AccountHeadId, PaginatedQueryModel query)
        {
            DateTime from = Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime to = Convert.ToDateTime(ToDate).ToUniversalTime();


            var list = await _procedureCallService.GetCommodityReportsRial(from, to, CommodityId, AccountHeadId, _currentUserAccessor.GetId(), _currentUserAccessor.GetYearId(), query);
            return list.ToList();

        }
        public async Task<List<spContradictionDebit>> GetContradictionDebit(string FromDate, string ToDate, int? CommodityId, string AccountHeadId, PaginatedQueryModel query)
        {
            DateTime from = Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime to = Convert.ToDateTime(ToDate).ToUniversalTime();


            var list = await _procedureCallService.GetContradictionDebit(from, to, CommodityId, AccountHeadId, _currentUserAccessor.GetId(), _currentUserAccessor.GetYearId(), query);
            return list.ToList();

        }

        public async Task<List<spCommodityReportsSumAll>> GetCommodityReportsSumAll(string FromDate, string ToDate, int? CommodityId, string WarehouseId, string CommodityTitle, PaginatedQueryModel query)
        {
            DateTime from = Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime to = Convert.ToDateTime(ToDate).ToUniversalTime();


            var list = await _procedureCallService.GetCommodityReportsSumAll(from, to, CommodityId, WarehouseId, _currentUserAccessor.GetId(), _currentUserAccessor.GetYearId(), CommodityTitle, query);
            return list.ToList();

        }
        public async Task<List<spCommodityReceiptReports>> GetCommodityReceiptReports(DateTime FromDate, DateTime ToDate, int? CommodityId, int? WarehouseId, int? DocumentNo, int? RequestNo,string CommodityTitle, PaginatedQueryModel query)
        {
            DateTime from = Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime to = Convert.ToDateTime(ToDate).ToUniversalTime();


            var list = await _procedureCallService.GetCommodityReceiptReports(from, to, CommodityId, WarehouseId, _currentUserAccessor.GetId(), _currentUserAccessor.GetYearId(), DocumentNo,RequestNo,CommodityTitle , query);
            return list.ToList();

        }
        public async Task<List<spCommodityReceiptReports>> GetCommodityReceiptReportsRial(string FromDate, string ToDate, int? CommodityId, int? AccountHeadId, int? WarehouseId, int? DocumentNo, PaginatedQueryModel query)
        {
            DateTime from = Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime to = Convert.ToDateTime(ToDate).ToUniversalTime();
            var model = new spCommodityReceiptReportsRialParam() { FromDate = from, ToDate = to, CommodityId = CommodityId, AccountHeadId = AccountHeadId, WarehouseId= WarehouseId, UserId = _currentUserAccessor.GetId(), YearId = _currentUserAccessor.GetYearId(), DocumentNo = DocumentNo, PageNumber = query.PageIndex, PageRow = query.PageSize };

            var list = await _procedureCallService.GetCommodityReceiptReportsRial(model,  query);
            return list.ToList();

        }
        public async Task<List<spReceiptInvoice>> GetAllInvoice(string FromDate, string ToDate, string InvoiceNo, int? CreditAccountReferenceId, PaginatedQueryModel query)
        {

            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();


            var list = await _procedureCallService.GetAllReceiptInvoice(from, to, InvoiceNo, CreditAccountReferenceId, query.PageIndex, query.PageSize, query);

            return list;
        }

        //---------------------------------------------------------------------------------------------------
        //-----------------------------------------Tadbir----------------------------------------------------
        public async Task<List<spGetDocumentItemForTadbir>> GetDocumentItemForTadbir(string FromDate, string ToDate, int? CodeVoucherGroupId, int? DocumentNo, int? WarehouseId)
        {
            DateTime from = Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime to = Convert.ToDateTime(ToDate).ToUniversalTime();

            var list = await _procedureCallService.GetDocumentItemForTadbir(from, to, CodeVoucherGroupId, DocumentNo, WarehouseId, _currentUserAccessor.GetId());
            return list.ToList();

        }

        public async Task<List<spReportControlsWarehouseLayoutQuantitiesByTadbir>> UploadInventoryTadbir(IFormFile file, string warehouseId, string yearId)
        {
            await _receiptCommandsService.UploadInventoryTadbir(file, warehouseId);

            var list = await _procedureCallService.GetReportControlsWarehouseLayoutQuantitiesByTadbir(Convert.ToInt32(warehouseId), Convert.ToInt32(yearId));
            return list.ToList();

        }
        public async Task<List<spReportControlsWarehouseLayoutQuantitiesByTadbir>> GetReportControlsWarehouseLayoutQuantitiesByTadbir(int warehouseId, int yearId)
        {
            var list = await _procedureCallService.GetReportControlsWarehouseLayoutQuantitiesByTadbir(warehouseId, yearId);
            return list.ToList();

        }
        public async Task<List<SpTejaratReportImportCommodity>> TejaratReports(string documentDate, int DocumentStatuesBaseValue)
        {

            var list = await _procedureCallService.TejaratReports(documentDate, DocumentStatuesBaseValue);
            return list.ToList();

        }
        //کرایه حمل
        public async Task<PagedList<Stp_FreightPays>> GetFreightPays(string FromDate, string ToDate, int? AccountReferenceId, PaginatedQueryModel query)
        {
            DateTime from = Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime to = Convert.ToDateTime(ToDate).ToUniversalTime();


            var list = await _procedureCallService.GetFreightPays(from, to, AccountReferenceId, query);
            return list;

        }
        public async Task<PagedList<spCommodityCost>> GetCommodityCost(string FromDate, string ToDate, string warehouseId, PaginatedQueryModel query)
        {
            DateTime from = Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime to = Convert.ToDateTime(ToDate).ToUniversalTime();


            var list = await _procedureCallService.spGetCommodityCost(from, to, warehouseId, query);
            return list;

        }
        public async Task<PagedList<MakeProductPrice>> GetMakeProductPriceReport(string FromDate, string ToDate, PaginatedQueryModel query)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            List<MakeProductPriceReportModel> list = new List<MakeProductPriceReportModel>();



            var result = await _procedureCallService.spGetTotalWeightProduct(from, to, query);
            var TotalCount = result.Count();
            list = result.Paginate(query.Paginator()).ToList();

            MakeProductPrice makeProduct = new MakeProductPrice();


            var AccountHeadRawMaterial = await _context.AccountHead.Where(a => a.Code == "80263" && !a.IsDeleted).Select(a => a.Id).FirstOrDefaultAsync();
            var Salary = _context.AccountHead.Where(a => a.Code.StartsWith("801") && !a.IsDeleted);
            var AccountHeadSalary = await Salary.Select(a => a.Id).ToListAsync();
            var Overload = _context.AccountHead.Where(a => (a.Code.StartsWith("802") || a.Code.StartsWith("803") || a.Code.StartsWith("805")) && (a.Code != "80256" && a.Code != "80257" && a.Code != "80263") && !a.IsDeleted);
            var AccountHeadOverload = await Overload.Select(a => a.Id).ToListAsync();

            makeProduct.TotalRawMaterial = await _context.VouchersDetail.Where(a => a.AccountHeadId == AccountHeadRawMaterial && a.AccountReferencesGroupId == 5 && (a.VoucherDate >= from || from == null) && (a.VoucherDate <= to || to == null) && !a.IsDeleted).SumAsync(a => a.Debit);
            makeProduct.TotalSalary = await _context.VouchersDetail.Where(a => AccountHeadSalary.Contains(a.AccountHeadId) && (a.AccountReferencesGroupId == 5 || a.AccountReferencesGroupId == 38) && (a.VoucherDate >= from || from == null) && (a.VoucherDate <= to || to == null) && !a.IsDeleted).SumAsync(a => a.Debit);
            makeProduct.TotalOverload = await _context.VouchersDetail.Where(a => AccountHeadOverload.Contains(a.AccountHeadId) && a.AccountReferencesGroupId == 5 && (a.VoucherDate >= from || from == null) && (a.VoucherDate <= to || to == null) && !a.IsDeleted).SumAsync(a => a.Debit);

            makeProduct.TotalWeight = Convert.ToDouble(list.Sum(a => a.Weight));
            makeProduct.TotalMeterage = Convert.ToDouble(list.Sum(a => a.Meterage));
            makeProduct.SumALL = makeProduct.TotalSalary + makeProduct.TotalRawMaterial + makeProduct.TotalOverload;

            var voucher = await _context.VouchersHead.Where(a => a.CodeVoucherGroupId == 2447 && !a.IsDeleted).OrderByDescending(a => a.VoucherDate).FirstOrDefaultAsync();

            if (voucher != null)
            {
                makeProduct.LastDate = voucher.VoucherDate;
                makeProduct.VoucherNO = voucher.VoucherNo;
                makeProduct.VoucherId = voucher.Id;
            }

            if (makeProduct.LastDate != null)
            {
                var TotalDays = Math.Ceiling((Convert.ToDateTime(makeProduct.LastDate).AddDays(1) - Convert.ToDateTime(from)).TotalDays);

                makeProduct.AllowAssumeDocument = TotalDays == 1 ? true : false;
            }
            else
            {
                makeProduct.AllowAssumeDocument = true;
            }


            if (voucher != null)
            {
                //Id=3238 =>Code=80160
                makeProduct.DocumentControls160 = await _context.VouchersDetail.Where(a => a.AccountHeadId == 3238 && a.VoucherId == voucher.Id && !a.IsDeleted).SumAsync(a => a.Credit);

                //Id=3316 =>Code=80296

                makeProduct.DocumentControls296 = await _context.VouchersDetail.Where(a => a.AccountHeadId == 3316 && a.VoucherId == voucher.Id && !a.IsDeleted).SumAsync(a => a.Credit);

                //Id=3240 =>Code=80295
                makeProduct.DocumentControls295 = await _context.VouchersDetail.Where(a => a.AccountHeadId == 3240 && a.VoucherId == voucher.Id && !a.IsDeleted).SumAsync(a => a.Credit);

            }

            foreach (var item in list)
            {
                item.RawMaterial = Convert.ToDouble(item.Weight * (makeProduct.TotalRawMaterial / makeProduct.TotalWeight));
                item.Overload = Convert.ToDouble(item.Weight * (makeProduct.TotalOverload / makeProduct.TotalWeight));
                item.Salary = Convert.ToDouble(item.Weight * (makeProduct.TotalSalary / makeProduct.TotalWeight));
                item.Total = item.RawMaterial + item.Overload + item.Salary;
            }
            makeProduct.MakeProductPriceReport = list;
            List<MakeProductPrice> pro = new List<MakeProductPrice>();
            pro.Add(makeProduct);

            return new PagedList<MakeProductPrice>()
            {
                Data = (IEnumerable<MakeProductPrice>)pro,
                TotalCount = query.PageIndex <= 1
                ? TotalCount

                : 0
            };

        }
        public async Task<PagedList<spCommodityReportsWithWarehouse>> GetCommodityReportsWithWarehouse(DateTime? FromDate,
           DateTime? ToDate,
          string WarehouseId,
           string CodeVoucherGroupId,
            int DocumentStauseBaseValue,

           PaginatedQueryModel query)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            var UserId = _currentUserAccessor.GetId();
            var YearId = _currentUserAccessor.GetYearId();
            var list = await _procedureCallService.GetCommodityReportsWithWarehouse(from, to, WarehouseId, CodeVoucherGroupId, DocumentStauseBaseValue,
                UserId,
                YearId,
                 query);
            return list;

        }
        public async Task<PagedList<ReceiptGroupbyInvoice>> GetAllReceiptGroupInvoice(DateTime? FromDate,
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

            DateTime? fromInvoiceDate = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? toInvoiceDate = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();

            List<string> resultDocument = String.IsNullOrEmpty(DocumentIds) ? null : DocumentIds.Split(',').ToList();
            var Q = (from d in _context.DocumentHeadView

                     where new[] { 43, 44, 53, 54 }.Contains(d.DocumentStauseBaseValue)
                           && (d.VoucherHeadId == VoucherHeadId  || VoucherHeadId == null)
                           && d.CreditAccountHeadId != null
                           && d.DebitAccountHeadId != null
                           && (d.DocumentDate >= fromInvoiceDate || fromInvoiceDate == null)
                           && (d.DocumentDate <= toInvoiceDate || toInvoiceDate == null)
                           && (string.IsNullOrEmpty(DocumentIds) || resultDocument.Contains(d.DocumentId.ToString()))
                           && (CreditAccountReferenceId == null || CreditAccountReferenceId == d.CreditAccountReferenceId)
                           && (DebitAccountReferenceId == null || DebitAccountReferenceId == d.DebitAccountReferenceId)
                           && (CreditAccountHeadId == null || CreditAccountHeadId == d.CreditAccountHeadId)
                           && (CreditAccountReferenceGroupId == null || CreditAccountReferenceGroupId == d.CreditAccountReferenceGroupId)
                           && (DebitAccountReferenceGroupId == null || DebitAccountReferenceGroupId == d.DebitAccountReferenceGroupId)
                           && (DebitAccountHeadId == null || DebitAccountHeadId == d.DebitAccountHeadId)
                           && d.IsPlacementComplete == true
                           && d.IsDocumentIssuance == true
                           && (d.TotalProductionCost ?? 0) > 0
                     select d).FilterQuery(query.Conditions);

            var TotalCount = await Q.GroupBy(a=>a.DocumentId).CountAsync();
            var TotalSum_ = await Q.SumAsync(d => d.TotalItemPrice);
            List<DocumentHeadView> DocumentHeadView = await Q.OrderByMultipleColumns(query.OrderByProperty).Paginate(query.Paginator()).ToListAsync();

            List<int> CodeVoucherGroups = _context.CodeVoucherGroups.Where(a=>a.ViewId==122).Select(a=>a.Id).ToList();
            CodeVoucherGroups.Add(2365);
            CodeVoucherGroups.Add(2370);

            var result = (from d in DocumentHeadView


                          group d by new
                          {
                              d.CodeVoucherGroupId,
                              d.CodeVoucherGroupTitle,
                              d.CreditAccountReferenceId,
                              d.CreditReferenceTitle,
                              d.DebitAccountReferenceId,
                              d.DebitReferenceTitle,
                              d.YearId,
                              DocumentDate = d.VoucherHeadId > 0 && d.AccountingCodeVoucherGroup == 53 ? d.InvoiceDate : d.DocumentDate,
                              d.CreditAccountHeadId,
                              d.CreditAccountReferenceGroupId,
                              d.DebitAccountHeadId,
                              d.DebitAccountReferenceGroupId,
                              d.DocumentStauseBaseValue,
                              d.VoucherHeadId,
                              d.FinancialOperationNumber,
                              d.DebitAccountHeadTitle,
                              d.CreditAccountHeadTitle,
                              d.DocumentId,
                              d.IsImportPurchase,
                              d.AccountingCodeVoucherGroup,

                          } into g
                          where g.Sum(d => d.TotalItemPrice) > 0
                          orderby g.Key.DocumentId
                          select new ReceiptGroupbyInvoice
                          {
                              DocumentNo = g.Max(a => a.DocumentNo),
                              FinancialOperationNumber = g.Key.FinancialOperationNumber,
                              tags = "",
                              InvoiceDate = g.Key.DocumentDate,

                              CodeVoucherGroupId = g.Key.CodeVoucherGroupId,
                              CodeVoucherGroupTitle = g.Key.CodeVoucherGroupTitle,
                              DocumentStauseBaseValue = g.Key.DocumentStauseBaseValue,
                              DebitAccountHeadId = g.Key.DebitAccountHeadId,

                              DebitAccountReferenceId = CodeVoucherGroups.Contains(Convert.ToInt32(g.Key.CodeVoucherGroupId)) ? null:g.Key.DebitAccountReferenceId,
                              DebitAccountReferenceGroupId = CodeVoucherGroups.Contains(Convert.ToInt32(g.Key.CodeVoucherGroupId)) ? null : g.Key.DebitAccountReferenceGroupId,


                              CreditReferenceTitle = g.Key.CreditReferenceTitle,
                              CreditAccountReferenceId = CodeVoucherGroups.Contains(Convert.ToInt32(g.Key.CodeVoucherGroupId)) ? null : g.Key.CreditAccountReferenceId,
                              CreditAccountReferenceGroupId = CodeVoucherGroups.Contains(Convert.ToInt32(g.Key.CodeVoucherGroupId)) ? null : g.Key.CreditAccountReferenceGroupId,

                              TotalItemPrice = Convert.ToInt64(g.Sum(d => d.TotalProductionCost)),
                              TotalProductionCost = Convert.ToInt64(g.Sum(d => d.TotalItemPrice)),
                              VatDutiesTax = Convert.ToInt64(g.Sum(d => d.VatDutiesTax)),

                              DocumentId = g.Key.DocumentId,

                              ExtraCost = g.Sum(d => d.ExtraCost),

                              ExtraCostAccountHeadId = g.Max(a => a.ExtraCostAccountHeadId),
                              ExtraCostAccountReferenceGroupId = g.Max(a => a.ExtraCostAccountReferenceGroupId),
                              ExtraCostAccountReferenceId = g.Max(a => a.ExtraCostAccountReferenceId),
                              
                              IsImportPurchase = g.Key.IsImportPurchase,
                              VoucherHeadId = g.Key.VoucherHeadId,
                              
                              
                              VoucherNo = g.Max(a => a.VoucherNo),
                              DebitAccountHeadTitle = g.Key.DebitAccountHeadTitle,
                              CreditAccountHeadTitle = g.Key.CreditAccountHeadTitle,
                              DebitReferenceTitle = g.Key.DebitReferenceTitle,
                              CreditAccountHeadId = g.Key.CreditAccountHeadId,
                              YearId = g.Key.YearId,
                              Id = g.Key.DocumentId,


                          }); ;

            var list = result.ToList();

          
            var DocumentItems = await (from di in _context.DocumentItems
                                       join dh in _context.DocumentHeads on di.DocumentHeadId equals dh.Id
                                       where !di.IsDeleted && !dh.IsDeleted && (dh.DocumentStauseBaseValue < 50 && dh.ExtraCost > 0) && list.Select(a => a.DocumentId).Contains(dh.DocumentId) 

                                       group di by dh.DocumentId into g
                                       select new TemDocumentItem
                                       {
                                           CurrencyPrice = g.Sum(x => x.CurrencyPrice * x.Quantity), // Assuming Quantity is a property in DocumentItem
                                           CurrencyBaseId = g.Max(x => x.CurrencyBaseId),
                                           CurrencyRate = g.Max(x => x.CurrencyRate ?? 1),
                                           DocumentId = g.Key.Value,

                                       }).ToListAsync();


            list.ForEach(b =>
            {
                List<DocumentHeadView> HeadView = DocumentHeadView.Where(a => a.DocumentId == b.DocumentId && b.DocumentId > 1).ToList();
                var dd = HeadView.Select(doc => new { DocumentNo = doc.DocumentNo, Id = doc.Id }).ToList();
                DocumentHeadView d = HeadView.FirstOrDefault();
                b.InvoiceNo = string.Join("-", dd.Select(doc => doc.DocumentNo).ToList());
                b.DocumentDescription = string.Join("-", _context.Document.Where(a => a.DocumentId == b.DocumentId).Select(doc => doc.DocumentDescription).FirstOrDefault());
                b.DocumentIds = string.Join(", ", dd.Select(doc => doc.Id).ToList());
                b.CurrencyPrice = 0;
                b.CurrencyBaseId = 28306;
                b.CurrencyRate = 0;
                b.ExtraCostAccountHeadTitle = "";
                b.IsAddCost = 0;
                
                if (b.DocumentStauseBaseValue < 50 && b.ExtraCost > 0)
                {

                    b.CurrencyPrice = b.IsImportPurchase == true ? DocumentItems.Where(a => a.DocumentId == b.DocumentId).Max(b => b.CurrencyPrice)+d.ExtraCostCurrency : 0;
                    b.CurrencyBaseId = DocumentItems.Where(a => a.DocumentId == b.DocumentId).Max(a => a.CurrencyBaseId);
                    b.CurrencyRate = b.IsImportPurchase == true ? DocumentItems.Where(a => a.DocumentId == b.DocumentId).Max(b => b.CurrencyRate) : 0;
                    b.ExtraCostAccountHeadTitle = _context.AccountHead.Where(a => a.Id == d.ExtraCostAccountHeadId).Select(a => a.Title).FirstOrDefault();
                    b.IsAddCost = d.IsFreightChargePaid == true ? 1 : 0;
                }
            });

            return new PagedList<ReceiptGroupbyInvoice>()
            {
                Data = (IEnumerable<ReceiptGroupbyInvoice>)list,
                TotalCount = query.PageIndex <= 1
                ? TotalCount : 0,
                TotalSum = Convert.ToDecimal(TotalSum_)

            };



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

    }

}
