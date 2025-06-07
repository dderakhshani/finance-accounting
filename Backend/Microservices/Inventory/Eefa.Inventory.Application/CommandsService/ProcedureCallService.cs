using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using Eefa.Inventory.Domain.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data.SqlClient;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;
using Microsoft.Office.Interop.Excel;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Eefa.Inventory.Application
{
    public class ProcedureCallService : IProcedureCallService
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly Eefa.Common.ICurrentUserAccessor _currentUserAccessor;
        public ProcedureCallService(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , Eefa.Common.ICurrentUserAccessor currentUserAccessor


            )
        {
            _mapper = mapper;
            _context = context;
            _currentUserAccessor = currentUserAccessor;

        }
        /// <summary>
        /// ترتیب پارامترهای ورودی برای پروسیجرها اهمیت دارد
        /// </summary>
        /// <param name="CommodityId"></param>
        /// <param name="DocumentItemsId">اگر  فرمول ساخت داشتیم بایستی از رویس آیدی داکیومنت قیمت بدست آید</param>
        /// <returns></returns>

        public async Task<SpDocumentItemsPriceBuy> GetAndUpdatePriceBuy(int CommodityId, int WarehouseId, int? DocumentItemsId = null)
        {
            var model = new SpDocumentItemsPriceBuyParam() { CommodityId = CommodityId, DocumentItemsId = DocumentItemsId, WarehouseId = WarehouseId };
            var parameters = model.EntityToSqlParameters();

            var PriceBuy = await _context.ExecuteSqlQueryAsync<SpDocumentItemsPriceBuy>($"EXEC [inventory].[spDocumentItemsPriceBuy] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());


            return PriceBuy.FirstOrDefault();

        }

       
        public async Task<int> ComputeAvgPrice(spComputeAvgPriceParam spComputeAvgPrice, spUpdateinventory_CaredxRepairParam CardexRepair)
        {

             await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spComputeAvgPrice] {QueryUtility.SqlParametersToQuey(spComputeAvgPrice.EntityToSqlParameters())}", spComputeAvgPrice.EntityToSqlParameters(), new CancellationToken());

             await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spUpdateinventory_CaredxRepair] {QueryUtility.SqlParametersToQuey(CardexRepair.EntityToSqlParameters())}", CardexRepair.EntityToSqlParameters(), new CancellationToken());

            return 1;

        }
        public async Task<int> UpdateWarehouseCardex(spUpdateWarehouseCardexParam model)
        {

            await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spUpdateWarehouseCardex] {QueryUtility.SqlParametersToQuey(model.EntityToSqlParameters())}", model.EntityToSqlParameters(), new CancellationToken());

           
            return 1;

        }
        public async Task<SpCalculateRemainQuantityRequest> CalculateRemainQuantityRequest(string RequestNo, int CommodityId)
        {
            var model = new SpCalculateRemainQuantityRequestParam() { RequestNo = RequestNo, CommodityId = CommodityId };
            var parameters = model.EntityToSqlParameters();

            var result = await _context.ExecuteSqlQueryAsync<SpCalculateRemainQuantityRequest>($"EXEC [inventory].[spCalculateRemainQuantityRequest] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return result.FirstOrDefault();

        }
        public async Task<SpCalculateRemainQuantityRequest> CalculateRemainQuantityRequestCommodityByPerson(string RequestNo, int CommodityId, int RequestDocumentItemId)
        {
            var model = new CalculateRemainQuantityRequestCommodityByPersontParam() { RequestNo = RequestNo, RequestDocumentItemId = RequestDocumentItemId, CommodityId = CommodityId };
            var parameters = model.EntityToSqlParameters();

            var result = await _context.ExecuteSqlQueryAsync<SpCalculateRemainQuantityRequest>($"EXEC [inventory].[spCalculateRemainQuantityRequestCommodityByPerson] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return result.FirstOrDefault();

        }
        public async Task ValidationIssuedDocuments()
        {
            await _context.ExecuteSqlQueryAsync<SpDocument>($"EXEC [inventory].spValidationIssuedDocuments", null, new CancellationToken());

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

            var model = new spReceiptGroupbyInvoiceParam()
            {
                FromInvoiceDate = FromDate,
                ToInvoiceDate = ToDate,
                VoucherHeadId = VoucherHeadId,
                DocumentIds = DocumentIds,
                CreditAccountReferenceId = CreditAccountReferenceId,
                DebitAccountReferenceId = DebitAccountReferenceId,
                CreditAccountHeadId = CreditAccountHeadId,
                CreditAccountReferenceGroupId = CreditAccountReferenceGroupId,
                DebitAccountReferenceGroupId = DebitAccountReferenceGroupId,
                DebitAccountHeadId = DebitAccountHeadId,
                PageNumber= query == null?0:query.PageIndex,
                PageRow= query == null ? 100: query.PageSize
                
            };

            var parameters = model.EntityToSqlParameters();
            List<SpReceiptGroupbyInvoice> list = new List<SpReceiptGroupbyInvoice>();

            list = await _context.ExecuteSqlQueryAsync<SpReceiptGroupbyInvoice>($"[inventory].[spReceiptGroupbyInvoice]  {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());

            if (query != null)
            {
                List<int> AccessCodeVoucherGroup = AccessCodeVoucherGroups();

                var result = list.Where(a => AccessCodeVoucherGroup.Contains((int)a.CodeVoucherGroupId)).AsQueryable().FilterQuery(query.Conditions);

               
                return new PagedList<SpReceiptGroupbyInvoice>()
                {
                    Data = (IEnumerable<SpReceiptGroupbyInvoice>)result.Paginate(query.Paginator()).ToList(),
                    TotalCount = query.PageIndex <= 1

                ? Convert.ToInt32(result.Select(a => a.RowCount).FirstOrDefault())

                : 0,
                    //TotalSum = Convert.ToInt64(result.Select(a => a.TotalSum).FirstOrDefault())
                    TotalSum = Convert.ToInt64(result.Sum(a => a.TotalProductionCost))
                };

            }
            else
            {
                var result = list.AsQueryable().ToList();
                return new PagedList<SpReceiptGroupbyInvoice>()
                {
                    Data = (IEnumerable<SpReceiptGroupbyInvoice>)result,
                    TotalCount = Convert.ToInt32(result.Select(a => a.RowCount).FirstOrDefault()),
                    //TotalSum = Convert.ToInt64(result.Select(a => a.TotalSum).FirstOrDefault())
                    TotalSum = Convert.ToInt64(result.Sum(a => a.TotalProductionCost))
                };
            }


        }
        private List<int> AccessCodeVoucherGroups()
        {
            return _context.AccessToWarehouse.Where(a => a.TableName == ConstantValues.AccessToWarehouseEnam.CodeVoucherGroups && a.UserId == _currentUserAccessor.GetId() && !a.IsDeleted).Select(a => a.WarehouseId).ToList();
        }
        //گزارش ورودی های خرید در یک صورتحساب
        public async Task<List<spReceiptInvoice>> GetAllReceiptInvoice(DateTime? FromDate, DateTime? ToDate, string InvoiceNo, int? CreditAccountReferenceId, int PageNumber, int PageRow, PaginatedQueryModel query)
        {
            var model = new spReceiptInvoiceParam() { FromDate = FromDate, ToDate = ToDate, InvoiceNo = InvoiceNo, CreditAccountReferenceId = CreditAccountReferenceId, PageNumber = PageNumber, PageRow = PageRow };

            var parameters = model.EntityToSqlParameters();
            var list = await _context.ExecuteSqlQueryAsync<spReceiptInvoice>($"[inventory].[spReceiptInvoice]  {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            if (query != null)
            {
                var result = list.AsQueryable().FilterQuery(query.Conditions).ToList();
                return result;
            }
            else
            {
                var result = list.AsQueryable().ToList();
                return result;
            }


        }
        public async Task<List<SpTejaratReportImportCommodity>> TejaratReports(string documentDate, int DocumentStatuesBaseValue)
        {

            DateTime? date = documentDate == null ? null : Convert.ToDateTime(documentDate).ToUniversalTime();
            var model = new SpTejaratReportImportCommodityParam() { DocumentDate = date, DocumentStauseBaseValue = (int)DocumentStatuesBaseValue };

            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<SpTejaratReportImportCommodity>($"EXEC [inventory].[spTejaratReportImportCommodity] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return list.ToList();

        }
        //بدست آوردن لیست انبارهای هر نوع سند
        public async Task<List<WarehousesLastLevelViewModel>> GetWarehousesLastLevelByCodeVoucherGroupId(int CodeVoucherGroupId)
        {

            var model = new SPGetWarehousesLastLevelByCodeVoucherGroupId() { CodeVoucherGroupId = CodeVoucherGroupId };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<WarehousesLastLevelViewModel>($"EXEC [inventory].spGetWarehousesLastLevelByCodeVoucherGroupId {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return list.ToList();
        }
        public async Task<List<SpGetAudit>> GetAuditById(int Id, string TableName)

        {
            var model = new SpGetAuditByIdParam() { TableName = TableName, Id = Id };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<SpGetAudit>($"EXEC [dbo].[spGetAuditById] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return list.ToList();
        }

        public async Task<List<spGetLogTable>> spGetLogTable(int Id, string TableName, string ColumnName, string ShemaName)
        {

            var model = new spGetLogTableParam() { ColumnName = ColumnName, TableName = TableName, ShemaName = ShemaName, Id = Id };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spGetLogTable>($"EXEC [inventory].[spGetLogTable] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return list.ToList();
        }
        public async Task<List<spGetConsumptionCommodityByRequesterReferenceId>> GetConsumptionCommodityByRequesterReferenceId(int RequesterReferenceId, int CommodityId, string CodeVoucherGroup)
        {
            var model = new spGetConsumptionCommodityByRequesterReferenceIdParam() { RequesterReferenceId = RequesterReferenceId, CommodityId = CommodityId, CodeVoucherGroup = CodeVoucherGroup };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spGetConsumptionCommodityByRequesterReferenceId>($"EXEC [inventory].spGetConsumptionCommodityByRequesterReferenceId {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return list.ToList();
        }
        public async Task<List<spGetMonthlyEntryToWarehouse>> GetMonthlyEntryToWarehouse(int? CommodityId, int? WarehouseId, int? YearId, int? EnterMode, int? UserId)
        {
            var model = new spGetMonthlyEntryToWarehouseParam() { CommodityId = CommodityId, WarehouseId = WarehouseId, YearId = YearId, EnterMode = EnterMode, UserId = UserId };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spGetMonthlyEntryToWarehouse>($"EXEC [inventory].spGetMonthlyEntryToWarehouse {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return list.ToList();
        }

        public async Task<List<spCommodityReports>> GetCommodityReports(DateTime FromDate, DateTime ToDate, int? CommodityId, string WarehouseId, int? UserId, int? YearId,string CommodityTitle, PaginatedQueryModel query)
        {
            var model = new spCommodityReportsParam() { FromDate = FromDate, ToDate = ToDate, CommodityId = CommodityId, WarehouseId = WarehouseId,CommodityTitle=CommodityTitle, UserId = UserId, YearId = YearId, PageNumber = query.PageIndex, PageRow = query.PageSize };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spCommodityReports>($"EXEC [inventory].spCommodityReports {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            var result = list.AsQueryable().FilterQuery(query.Conditions).ToList();
            return result.ToList();
        }
        public async Task<List<spContradictionDebit>> GetContradictionDebit(DateTime FromDate, DateTime ToDate, int? CommodityId, string AccountHeadId, int? UserId, int? YearId, PaginatedQueryModel query)
        {
            var model = new spContradictionDebitParam() { FromDate = FromDate, ToDate = ToDate, CommodityId = CommodityId, AccountHeadId = AccountHeadId, UserId = UserId, YearId = YearId, PageNumber = query.PageIndex, PageRow = query.PageSize };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spContradictionDebit>($"EXEC [inventory].[spContradictionDebit] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            var result = list.AsQueryable().FilterQuery(query.Conditions).ToList();
            return result.ToList();
        }
        public async Task<List<spCommodityReportsRial>> GetCommodityReportsRial(DateTime FromDate, DateTime ToDate, int? CommodityId, string AccountHeadId, int? UserId, int? YearId, PaginatedQueryModel query)
        {
            var model = new spCommodityReportsRialParam() { FromDate = FromDate, ToDate = ToDate, CommodityId = CommodityId, AccountHeadId = Convert.ToInt32(AccountHeadId), UserId = UserId, YearId = YearId, PageNumber = query.PageIndex, PageRow = query.PageSize };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spCommodityReportsRial>($"EXEC [inventory].[spCommodityReportsRial] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            var result = list.AsQueryable().FilterQuery(query.Conditions).ToList();
            return result.ToList();
        }
        public async Task<List<spCommodityReportsSumAll>> GetCommodityReportsSumAll(DateTime FromDate, DateTime ToDate, int? CommodityId, string WarehouseId, int? UserId, int? YearId, string CommodityTitle, PaginatedQueryModel query)
        {
            var model = new spCommodityReportsSumAllParam() { FromDate = FromDate, ToDate = ToDate, CommodityId = CommodityId,CommodityTitle= CommodityTitle, WarehouseId = WarehouseId, UserId = UserId, YearId = YearId };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spCommodityReportsSumAll>($"EXEC [inventory].spCommodityReportsSumAll {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            var result = list.AsQueryable().FilterQuery(query.Conditions).ToList();
            return result.ToList();
        }
        public async Task<List<spCommodityReceiptReports>> GetCommodityReceiptReports(DateTime FromDate, DateTime ToDate, int? CommodityId, int? WarehouseId, int? UserId, int? YearId, int? DocumentNo, int? RequestNo, string CommodityTitle, PaginatedQueryModel query)
        {
            
            
            var model = new spCommodityReceiptReportsParam() { FromDate = FromDate, ToDate = ToDate, CommodityId = CommodityId, WarehouseId = WarehouseId, UserId = UserId, YearId = YearId, DocumentNo = DocumentNo, RequestNo= RequestNo, CommodityTitle= CommodityTitle, PageNumber = query.PageIndex, PageRow = query.PageSize };
            var parameters = model.EntityToSqlParameters();
           
                var list = await _context.ExecuteSqlQueryAsync<spCommodityReceiptReports>($"EXEC [inventory].spCommodityReceiptReports {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());

                var result = list.AsQueryable().FilterQuery(query.Conditions).OrderBy(a => a.DocumentDate).ThenByDescending(a => a.Mode).ToList();
                return result.ToList();
           
        }
        public async Task<List<spCommodityReceiptReports>> GetCommodityReceiptReportsRial(spCommodityReceiptReportsRialParam model, PaginatedQueryModel query)
        {
           
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spCommodityReceiptReports>($"EXEC [inventory].spCommodityReceiptReportsRial {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());

            var result = list.AsQueryable().FilterQuery(query.Conditions).ToList();
            return result.ToList();
        }
        public async Task<int> UpdateStockQuantity(int? CommodityId, int? WarehouseId)
        {

            var model = new sUpdateStockQuantityParam() { CommodityId = CommodityId, WarehouseId = WarehouseId, };
            var parameters = model.EntityToSqlParameters();

            await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spUpdateStockQuantity] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());

            return 1;


        }
        public async Task<int> UpdateWarehouseLayoutQuantities(int? CommodityId, int? WarehouseLayoutId)
        {
            var model = new spUpdateWarehouseLayoutQuantitiesParam() { CommodityId = CommodityId, WarehouseLayoutId = WarehouseLayoutId, };
            var parameters = model.EntityToSqlParameters();

            await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spUpdateWarehouseLayoutQuantities]  {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());

            return 1;
        }
        public async Task<List<spGetDocumentItemForTadbir>> GetDocumentItemForTadbir(DateTime FromDate, DateTime ToDate, int? CodeVoucherGroupId, int? DocumentNo, int? WarehouseId, int? UserId)
        {
            var model = new spGetDocumentItemForTadbirParam() { FromDate = FromDate, ToDate = ToDate, CodeVoucherGroupId = CodeVoucherGroupId, DocumentNo = DocumentNo, WarehouseId = WarehouseId, UserId = UserId };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spGetDocumentItemForTadbir>($"EXEC [inventory].[spGetDocumentItemForTadbir] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());


            return list.ToList();
        }
        public async Task<List<spReportControlsWarehouseLayoutQuantitiesByTadbir>> GetReportControlsWarehouseLayoutQuantitiesByTadbir(int warehouseId, int YearId)
        {
            var model = new spReportControlsWarehouseLayoutQuantitiesByTadbirParam() { warehouseId = warehouseId, YearId = YearId };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spReportControlsWarehouseLayoutQuantitiesByTadbir>($"EXEC [inventory].[spReportControlsWarehouseLayoutQuantitiesByTadbir] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());


            return list.ToList();
        }
        public async Task DeleteInventory_StockFromTadbir(int warehouseId)
        {
            var model = new spDeleteInventory_StockFromTadbirParam() { warehouseId = warehouseId };
            var parameters = model.EntityToSqlParameters();

            await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spDeleteInventory.StockFromTadbir] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
        }
        public async Task<spUpdateWarehouseCommodityAvailabe2> UpdateWarehouseCommodityAvailable(int warehouseId, int? YearId,int UserId)
        {
            var model = new spUpdateWarehouseCommodityAvailabe2Param() { WarehouseId = warehouseId, YearId = YearId , UserId = UserId };
            var parameters = model.EntityToSqlParameters();

            var result = await _context.ExecuteSqlQueryAsync<spUpdateWarehouseCommodityAvailabe2>($"EXEC [dbo].[spUpdateWarehouseCommodityAvailabe2] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());


            return result.FirstOrDefault();

        }
        public async Task<List<spGetCommodity>> GetCommodity(int? warehouseId, string searchTerm, bool? IsConsumable, bool? IsAsset, PaginatedQueryModel query)
        {
            var model = new GetCommodityParam() { warehouseId = warehouseId, searchTerm = searchTerm, IsConsumable = IsConsumable, IsAsset = IsAsset };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spGetCommodity>($"EXEC inventory.GetCommodity {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());

            var result = list.AsQueryable().FilterQuery(query.Conditions).Paginate(query.Paginator()).ToList();
            return result.ToList();
        }
        public async Task<Stp_Acc_Foreign_orders> GetCostImportReceipts(int CompanyId, int YearId, int ReferenceId)
        {
            var model = new Stp_Acc_Foreign_ordersPram() { CompanyId = CompanyId, YearIds = YearId, ReferenceId = ReferenceId };
            var parameters = model.EntityToSqlParameters();

            var result = await _context.ExecuteSqlQueryAsync<Stp_Acc_Foreign_orders>($"EXEC [accounting].Stp_Acc_Foreign_orders {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());


            return result != null ? result.FirstOrDefault() : new Stp_Acc_Foreign_orders();

        }
        public async Task<PagedList<spGetWarehouseLayoutQuantities>> GetWarehouseLayoutQuantities(int? warehouseId, int? CommodityId, int? UserId, PaginatedQueryModel query)
        {
            var model = new spGetWarehouseLayoutQuantitiesParam() { WarehouseId = warehouseId, CommodityId = CommodityId, UserId = UserId };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spGetWarehouseLayoutQuantities>($"EXEC [inventory].[spGetWarehouseLayoutQuantities] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());

            if (query != null)
            {
                var result = list.AsQueryable().FilterQuery(query.Conditions);

                return new PagedList<spGetWarehouseLayoutQuantities>()
                {
                    Data = (IEnumerable<spGetWarehouseLayoutQuantities>)result.Paginate(query.Paginator()).ToList(),
                    TotalCount = query.PageIndex <= 1
                ? result.Count()

                : 0
                };

            }
            else
            {
                var result = list.AsQueryable().ToList();
                return new PagedList<spGetWarehouseLayoutQuantities>()
                {
                    Data = (IEnumerable<spGetWarehouseLayoutQuantities>)result,
                    TotalCount = query.PageIndex <= 1
                ? list.Count()

                : 0
                };


            }
        }
        public async Task<object> UpdateWarehouseCommodityPrice(int warehouseId, int YearId)
        {
            var model = new spUpdateWarehouseCommodityAvailabe2Param() { WarehouseId = warehouseId, YearId = YearId };
            var parameters = model.EntityToSqlParameters();

            return await _context.ExecuteSqlQueryAsync<object>($"EXEC [dbo].[spUpdateWarehouseCommodityPrice] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
        }

        public async Task<PagedList<Stp_FreightPays>> GetFreightPays(DateTime FromDate, DateTime ToDate, int? AccountReferenceId, PaginatedQueryModel query)
        {
            var model = new Stp_FreightPaysParam() { 
                AccountReferenceId = AccountReferenceId, 
                FromDate = FromDate, 
                ToDate = ToDate };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<Stp_FreightPays>($"EXEC accounting.Stp_FreightPays {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());


            var result = list.AsQueryable().FilterQuery(query.Conditions);

            return new PagedList<Stp_FreightPays>()
            {
                Data = (IEnumerable<Stp_FreightPays>)result.Paginate(query.Paginator()).ToList(),
                TotalCount = list.Count()
            };


        }

        public async Task<IQueryable<MakeProductPriceReportModel>> spGetTotalWeightProduct(DateTime? FromDate, DateTime? ToDate, PaginatedQueryModel query)
        {
            var model = new spGetTotalWeightProductParam() { FromDate = FromDate, ToDate = ToDate };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<MakeProductPriceReportModel>($"EXEC inventory.spGetTotalWeightProduct {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());

            var result = list.AsQueryable().FilterQuery(query.Conditions);
            return result;
        }
        public async Task<PagedList<spCommodityCost>> spGetCommodityCost(DateTime? FromDate, DateTime? ToDate, string WarehouseId, PaginatedQueryModel query)
        {
            var model = new spCommodityCostParam() { 
                FromInvoiceDate = FromDate,
                ToInvoiceDate = ToDate,
                WarehouseId = WarehouseId };
            var parameters = model.EntityToSqlParameters();

            var list = await _context.ExecuteSqlQueryAsync<spCommodityCost>($"EXEC inventory.spCommodityCost {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            var result = list.AsQueryable().FilterQuery(query.Conditions);

            return new PagedList<spCommodityCost>()
            {
                Data = (IEnumerable<spCommodityCost>)result.Paginate(query.Paginator()).ToList(),
                TotalCount = list.Count()
            };
        }
        public async Task<PagedList<spCommodityReportsWithWarehouse>> GetCommodityReportsWithWarehouse(DateTime? FromDate,
            DateTime? ToDate,
             string WarehouseId,
             string CodeVoucherGroupId,
             int DocumentStauseBaseValue,
             int? UserId,
             int? yearId,
            PaginatedQueryModel query)
        {

            var model = new spCommodityReportsWithWarehouseParam()
            {
                FromDate = FromDate,
                ToDate = ToDate,
                WarehouseId = WarehouseId,
                CodeVoucherGroupId = CodeVoucherGroupId,
                DocumentStauseBaseValue = DocumentStauseBaseValue,
                UserId = UserId,
                yearId = yearId
            };

            var parameters = model.EntityToSqlParameters();
            var list = await _context.ExecuteSqlQueryAsync<spCommodityReportsWithWarehouse>($"[inventory].[spCommodityReportsWithWarehouse]  {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());

            if (query != null)
            {
                List<int> AccessCodeVoucherGroup = AccessCodeVoucherGroups();

                var result = list.AsQueryable().FilterQuery(query.Conditions);


                return new PagedList<spCommodityReportsWithWarehouse>()
                {
                    Data = (IEnumerable<spCommodityReportsWithWarehouse>)result.Paginate(query.Paginator()).ToList(),
                    TotalCount = query.PageIndex <= 1
                ? result.Count()

                : 0
                };

            }
            else
            {
                var result = list.AsQueryable().ToList();
                return new PagedList<spCommodityReportsWithWarehouse>()
                {
                    Data = (IEnumerable<spCommodityReportsWithWarehouse>)result,
                    TotalCount = result.Count()

                };
            }
        }
        public async Task<int> UpdateAllStockQuantity()
        {
            await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spUpdateLastChangeWarhouseLayoutQuantity]", null, new CancellationToken());
            return 1;
        }
        public async Task<int> UpdateWarehouseLayout(int WarehouseId,int UserId)
        {
            var model = new spUpdateWarehouseLayoutParam() { warehouseId = WarehouseId, UserId =UserId};
            var parameters = model.EntityToSqlParameters();
            await _context.ExecuteSqlQueryAsync<object>($"EXEC [dbo].[Stp_AddNewCommodity] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            await _context.ExecuteSqlQueryAsync<object>($"EXEC inventory.MakeWarehouseLayouts {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            await _context.ExecuteSqlQueryAsync<object>($"EXEC [dbo].spUpdateWarehouseLayout {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return 1;
        }
        public async Task<int> UpdateAddNewCommodity(int UserId)
        {
           
            await _context.ExecuteSqlQueryAsync<object>($"EXEC [dbo].[Stp_AddNewCommodity]", null, new CancellationToken());
            
            return 1;
        }
        public async Task<int> RemoveCommodityFromWarehouse(int WarehouseId, int UserId)
        {
            var model = new spUpdateWarehouseLayoutParam() { warehouseId = WarehouseId, UserId = UserId };
            var parameters = model.EntityToSqlParameters();
            await _context.ExecuteSqlQueryAsync<object>($"EXEC [dbo].RemoveCommodityFromWarehouse {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return 1;
        }
        public async Task<int> ArchiveDocumentHeadsByDocumentDate(DateTime? FromDate, DateTime? ToDate,int WarehouseId, int UserId,int DocumentStatuesBaseValue)
        {
            var model = new ArchiveDocumentHeadsByDocumentDateParam() { 
                WarehouseId = WarehouseId,
                FromDate = FromDate,
                ToDate = ToDate,
                DocumentStauseBaseValue = DocumentStatuesBaseValue,
                UserId = UserId 
            };
            var parameters = model.EntityToSqlParameters();
            await _context.ExecuteSqlQueryAsync<object>($"EXEC inventory.spArchiveDocumentHeadsByDocumentDate {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return 1;
        }
       
        public async Task<int> WarehouseCheckLoseData()
        {
            await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spWarehouseCheckLoseData]", null, new CancellationToken());
            return 1;
        }
        public async Task<int> UpdateVoucherHeadAfterRepairCardex()
        {
            await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spVoucherHeadAfreRepairCardex]", null, new CancellationToken());
            return 1;
        }
        public async Task<int> WarehouseRequestExitUpdateIsDeleted()
        {
            await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spWarehouseRequestExitUpdateIsDeleted]", null, new CancellationToken());
            return 1;
        }

        public Task<SpDocumentItemsPriceBuy> ComputeAvgPrice(int CommodityId, int WarehouseId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertDocumentHeads(WarehouseIOCommodity model)
        {
            // تبدیل کلاس به JSON
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            string jsonData = JsonSerializer.Serialize(model, options);
           
            spInsertDocumentHeadsParam spInsertDocumentHeadsParam = new spInsertDocumentHeadsParam() { jsonData=jsonData};

            await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spInsertDocumentHeads] {QueryUtility.SqlParametersToQuey(spInsertDocumentHeadsParam.EntityToSqlParameters())}", spInsertDocumentHeadsParam.EntityToSqlParameters(), new CancellationToken());


            return 1;

        }
        public async Task<Receipt> InsertDocumentHeadsForIOCommodity(InsertDocumentHeadsForIOCommodity model)
        {
            // تبدیل کلاس به JSON
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            string jsonData = JsonSerializer.Serialize(model.receiptdocument, options);

            spInsertDocumentHeadsForIOCommodityParam spInsertDocumentHeadsParam = new spInsertDocumentHeadsForIOCommodityParam() { userId=model.userId,yearId= model.yearId, OwnerRoleId=model.OwnerRoleId, jsonData = jsonData };

           var receipt=  await _context.ExecuteSqlQueryAsync<Receipt>($"EXEC [inventory].[spInsertDocumentHeadsForIOCommodity] {QueryUtility.SqlParametersToQuey(spInsertDocumentHeadsParam.EntityToSqlParameters())}", spInsertDocumentHeadsParam.EntityToSqlParameters(), new CancellationToken());


            return receipt.FirstOrDefault();

        }
        public async Task<Receipt> UpdateDocumentHeadsForIOCommodity(InsertDocumentHeadsForIOCommodity model)
        {
            // تبدیل کلاس به JSON
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            string jsonData = JsonSerializer.Serialize(model.receiptdocument, options);

            spInsertDocumentHeadsForIOCommodityParam spInsertDocumentHeadsParam = new spInsertDocumentHeadsForIOCommodityParam() { userId = model.userId, yearId = model.yearId, OwnerRoleId = model.OwnerRoleId, jsonData = jsonData };

            var receipt = await _context.ExecuteSqlQueryAsync<Receipt>($"EXEC [inventory].[spUpdateDocumentHeadsForIOCommodity] {QueryUtility.SqlParametersToQuey(spInsertDocumentHeadsParam.EntityToSqlParameters())}", spInsertDocumentHeadsParam.EntityToSqlParameters(), new CancellationToken());


            return receipt.FirstOrDefault();

        }
        public async Task<Receipt>UpdateDocumentHeadsForIOCommodityMaterial(InsertDocumentHeadsForIOCommodity model)
        {
            // تبدیل کلاس به JSON
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            string jsonData = JsonSerializer.Serialize(model.receiptdocument, options);

            spInsertDocumentHeadsForIOCommodityParam spInsertDocumentHeadsParam = new spInsertDocumentHeadsForIOCommodityParam() { userId = model.userId, yearId = model.yearId, OwnerRoleId = model.OwnerRoleId, jsonData = jsonData };

            var receipt = await _context.ExecuteSqlQueryAsync<Receipt>($"EXEC [inventory].[spUpdateDocumentHeadsForIOCommodityMaterial] {QueryUtility.SqlParametersToQuey(spInsertDocumentHeadsParam.EntityToSqlParameters())}", spInsertDocumentHeadsParam.EntityToSqlParameters(), new CancellationToken());


            return receipt.FirstOrDefault();

        }
        public async Task<PagedList<spErpDarkhastJoinDocumentHeads>> GetErpDarkhastJoinDocumentHeads(spErpDarkhastJoinDocumentHeadsParam model, PaginatedQueryModel query)
        {

            var list =await _context.ExecuteSqlQueryAsync<spErpDarkhastJoinDocumentHeads>($"EXEC [inventory].[spErpDarkhastJoinDocumentHeads] {QueryUtility.SqlParametersToQuey(model.EntityToSqlParameters())}", model.EntityToSqlParameters(), new CancellationToken());

           
            var result = list.AsQueryable().FilterQuery(query.Conditions);

            return new PagedList<spErpDarkhastJoinDocumentHeads>()
            {
                Data = (IEnumerable<spErpDarkhastJoinDocumentHeads>)result.Paginate(query.Paginator()).ToList(),
                TotalCount = list.Count()
            };

        }
    }
}
