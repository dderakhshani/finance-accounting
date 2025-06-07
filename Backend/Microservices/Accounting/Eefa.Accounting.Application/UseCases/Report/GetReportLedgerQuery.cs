using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Eefa.Accounting.Application.UseCases.Report.Model;
using Eefa.Accounting.Data.Databases.Sp;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PersianDate.Standard;
using ServiceStack.Script;

namespace Eefa.Accounting.Application.UseCases.Report
{
    public class GetReportLedgerQuery : Pagination, IMapFrom<GetReportLedgerQuery>, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public bool UseEf { get; set; }
        public List<Condition> Conditions { get; set; }
        public int ReportType { get; set; }
        public int Level { get; set; }
        public int CompanyId { get; set; }
        public int[] YearIds { get; set; }
        public int? VoucherStateId { get; set; }
        public int[] CodeVoucherGroupIds { get; set; }
        public int? TransferId { get; set; }
        public int[] AccountHeadIds { get; set; }
        public int[] ReferenceGroupIds { get; set; }
        public int[] ReferenceIds { get; set; }
        public int[] ChequeSheetIds { get; set; }
        public int? ReferenceNo { get; set; } = 1;
        public int? VoucherNoFrom { get; set; }
        public int? VoucherNoTo { get; set; }
        public DateTime? VoucherDateFrom { get; set; }
        public DateTime? VoucherDateTo { get; set; }
        public double? DebitFrom { get; set; }
        public double? DebitTo { get; set; }
        public double? CreditFrom { get; set; }
        public double? CreditTo { get; set; }
        public int? DocumentIdFrom { get; set; }
        public int? DocumentIdTo { get; set; }
        public string VoucherDescription { get; set; }
        public string VoucherRowDescription { get; set; }
        public bool Remain { get; set; }
        public string ReportTitle { get; set; }
        public int? CurrencyTypeBaseId { get; set; }
        public SsrsUtil.ReportFormat ReportFormat { get; set; } = SsrsUtil.ReportFormat.None;
        public bool IsPrint { get; set; }
        public bool ForcePrint { get; set; }
        public int PrintType { get; set; }//0=rial,1=dollar,2=rial&dollar
        public bool UsePagination { get; set; } = false;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetReportLedgerQuery, StpReportLedgerInput>()
                .ForMember(x => x.YearIds, opt => opt.MapFrom(t => (t.YearIds != null && t.YearIds.Length != 0) ? JsonConvert.SerializeObject(t.YearIds) : null))
                .ForMember(x => x.AccountHeadIds, opt => opt.MapFrom(t => (t.AccountHeadIds != null && t.AccountHeadIds.Length != 0) ? JsonConvert.SerializeObject(t.AccountHeadIds) : null))
                .ForMember(x => x.ReferenceGroupIds, opt => opt.MapFrom(t => (t.ReferenceGroupIds != null && t.ReferenceGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceGroupIds) : null))
                .ForMember(x => x.ReferenceIds, opt => opt.MapFrom(t => (t.ReferenceIds != null && t.ReferenceIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceIds) : null))
                .ForMember(x => x.CodeVoucherGroupIds, opt => opt.MapFrom(t => (t.CodeVoucherGroupIds != null && t.CodeVoucherGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.CodeVoucherGroupIds) : null))
                .IgnoreAllNonExisting();
        }
    }

    public class GetReportLedgerQueryHandler : IRequestHandler<GetReportLedgerQuery, ServiceResult>
    {
        private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
        private readonly ICurrentUserAccessor _currentUser;


        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IAccountingUnitOfWork _context;
        private readonly DanaAccountingUnitOfWork _danaContext;

        public GetReportLedgerQueryHandler(IRepository repository, IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures, ICurrentUserAccessor currentUserAccessor, IMapper mapper, IAccountingUnitOfWork context, DanaAccountingUnitOfWork danaContext)
        {
            _repository = repository;
            _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
            _currentUser = currentUserAccessor;
            _mapper = mapper;
            _context = context;
            _danaContext = danaContext;
        }

        public async Task<ServiceResult> Handle(GetReportLedgerQuery request, CancellationToken cancellationToken)
        {

            if (request.UseEf)
            {
                IQueryable<Data.Entities.VouchersDetail> voucherDetailQuery = null;


                request.VoucherDateTo = request.VoucherDateTo?.AddDays(1).AddMilliseconds(-1);
                if (_currentUser.GetYearId() == 3)
                {
                    voucherDetailQuery = _danaContext.VouchersDetails.AsQueryable().ApplyPermission(_danaContext, _currentUser, false, true);
                    request.YearIds = await _danaContext.Years.Where(x => x.FirstDate <= request.VoucherDateTo && x.LastDate >= request.VoucherDateFrom && x.UserYears.Any(x => x.UserId == _currentUser.GetId())).Select(a => a.Id).ToArrayAsync();
                }
                else
                {
                    voucherDetailQuery = _repository.GetQuery<Data.Entities.VouchersDetail>().ApplyPermission(_context, _currentUser, false, true);
                    request.YearIds = await _repository.GetQuery<Data.Entities.Year>().Where(x => x.FirstDate <= request.VoucherDateTo && x.LastDate >= request.VoucherDateFrom && x.UserYears.Any(x => x.UserId == _currentUser.GetId())).Select(a => a.Id).ToArrayAsync();
                }

                if (request.YearIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.YearIds.Contains(x.Voucher.YearId));
                if (request.CompanyId != 0) voucherDetailQuery = voucherDetailQuery.Where(x => x.Voucher.CompanyId == request.CompanyId);

                if (request.ChequeSheetIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.ChequeSheetIds.ToList().Contains((int)x.ChequeSheetId));

                if (request.AccountHeadIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.AccountHeadIds.Contains(x.AccountHeadId));
                if (request.ReferenceIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.ReferenceIds.ToList().Contains(x.ReferenceId1 ?? -1));
                if (request.ReferenceGroupIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.ReferenceGroupIds.ToList().Contains(x.AccountReferencesGroupId ?? -1));


                if (request.CodeVoucherGroupIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.CodeVoucherGroupIds.ToList().Contains(x.Voucher.CodeVoucherGroupId));
                if (request.VoucherRowDescription != null) voucherDetailQuery = voucherDetailQuery.Where(x => x.VoucherRowDescription.Contains(request.VoucherRowDescription));

                if (request.CurrencyTypeBaseId != null && request.CurrencyTypeBaseId != 0)
                {
                    voucherDetailQuery = voucherDetailQuery.Where(a => a.CurrencyTypeBaseId == request.CurrencyTypeBaseId);
                }
                IQueryable<LedgerReportModel> entities = voucherDetailQuery
                    .Include(x => x.Voucher)
                    .ProjectTo<LedgerReportModel>(_mapper.ConfigurationProvider)
                    .WhereQueryMaker(request.Conditions);

                if (string.IsNullOrEmpty(request.OrderByProperty))
                {
                    entities = entities.OrderBy(a => a.VoucherNo).ThenBy(a => a.RowIndex);
                }
                else
                {
                    entities = entities.OrderByMultipleColumns(request.OrderByProperty + ",VoucherNo,RowIndex");
                }

                var totalCount = (request.VoucherDateFrom != null && request.VoucherDateTo != null) ? await entities.Where(x => x.VoucherDate >= request.VoucherDateFrom && x.VoucherDate <= request.VoucherDateTo).CountAsync() : await entities.CountAsync(cancellationToken);

                if ((request.IsPrint && totalCount > 1000) || request.ForcePrint)
                {
                    if (request.VoucherDateFrom != null && request.VoucherDateTo != null) entities = entities.Where(x => x.VoucherDate >= request.VoucherDateFrom && x.VoucherDate <= request.VoucherDateTo);

                    var excelData = await GetExcelData(entities, request);
                    string base64String;


                    using (var wb = new XLWorkbook())
                    {
                        wb.RightToLeft = true;
                        wb.Author = _currentUser.GetUsername();
                        wb.RowHeight = 22.5;

                        // Create the worksheet
                        var sheetName = "دفتر تفصیلی مطابق ردیفهای سند";

                        if (request.PrintType == 3)
                        {
                            sheetName = "دفاتر الکترونیکی";
                        }


                        var sheet = wb.AddWorksheet(sheetName);


                        // Define colors
                        var headerColor = XLColor.FromHtml("#6D72ED");
                        var alternateRowColor = XLColor.FromHtml("#F2F2FE");

                        // Add headers and apply style
                        for (int col = 0; col < excelData.Columns.Count; col++)
                        {
                            var headerCell = sheet.Cell(1, col + 1);
                            headerCell.Value = excelData.Columns[col].ColumnName;
                            headerCell.Style.Font.Bold = true; // Make headers bold
                            headerCell.Style.Fill.BackgroundColor = headerColor; // Set header background color
                            headerCell.Style.Font.FontColor = XLColor.White; // Optionally set header font color to white
                            headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Center-align header text
                        }

                        // Add data and apply alternating row color
                        for (int row = 0; row < excelData.Rows.Count; row++)
                        {
                            for (int col = 0; col < excelData.Columns.Count; col++)
                            {
                                var cell = sheet.Cell(row + 2, col + 1); // Data starts from the second row
                                var value = excelData.Rows[row][col];

                                // Set cell value based on its type
                                if (value is int || value is long)
                                {
                                    cell.Value = Convert.ToInt64(value); // Assign numeric value as an integer
                                    cell.Style.NumberFormat.SetFormat("#,##0"); // No decimal places
                                }
                                else if (value is decimal || value is double)
                                {
                                    double numericValue = Convert.ToDouble(value);
                                    cell.Value = numericValue; // Assign numeric value
                                    if (Math.Floor(numericValue) == numericValue) // Check if the number is an integer
                                    {
                                        cell.Style.NumberFormat.SetFormat("#,##0"); // No decimal places
                                    }
                                    else
                                    {
                                        cell.Style.NumberFormat.SetFormat("#,##0.00"); // Two decimal places
                                    }
                                }

                                else if (value is DateTime)
                                {
                                    cell.Value = (DateTime)value;
                                    cell.Style.DateFormat.Format = "yyyy-mm-dd"; // Set date format
                                }
                                else
                                {
                                    cell.Value = value.ToString();
                                }

                                // Apply alternating row color
                                if (row % 2 == 1) // Apply color to every other row
                                {
                                    cell.Style.Fill.BackgroundColor = alternateRowColor;
                                }

                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Center-align header text

                            }
                        }
                        double maxWidth = 50; // Set the maximum width (in characters)
                        foreach (var column in sheet.ColumnsUsed())
                        {
                            column.AdjustToContents();
                            if (column.Width > maxWidth)
                            {
                                column.Width = maxWidth;
                            }
                        }

                        using (var ms = new MemoryStream())
                        {
                            wb.SaveAs(ms);
                            base64String = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                    return ServiceResult.Success(new
                    {
                        Data = base64String
                    });
                }

                List<LedgerReportModel> data = new();
                int size = (request.PageIndex - 1) * request.PageSize;

                var beforeDebit = (long)(await entities.Where(x => x.VoucherDate >= request.VoucherDateFrom && x.VoucherDate <= request.VoucherDateTo).Take(size).SumAsync(a => a.Debit) + ((request.VoucherDateFrom != null && request.VoucherDateTo != null) ? await entities.Where(x => x.VoucherDate < request.VoucherDateFrom).SumAsync(y => y.Debit) : 0) ?? 0);
                var beforeCredit = (long)(await entities.Where(x => x.VoucherDate >= request.VoucherDateFrom && x.VoucherDate <= request.VoucherDateTo).Take(size).SumAsync(a => a.Credit) + ((request.VoucherDateFrom != null && request.VoucherDateTo != null) ? await entities.Where(x => x.VoucherDate < request.VoucherDateFrom).SumAsync(y => y.Credit) : 0) ?? 0);

                var beforeCurrencyDebit = (double)(await entities.Where(x => x.VoucherDate >= request.VoucherDateFrom && x.VoucherDate <= request.VoucherDateTo).Take(size).SumAsync(a => a.CurrencyDebit) + ((request.VoucherDateFrom != null && request.VoucherDateTo != null) ? await entities.Where(x => x.VoucherDate < request.VoucherDateFrom).SumAsync(y => y.CurrencyDebit) : 0) ?? 0);
                var beforeCurrencyCredit = (double)(await entities.Where(x => x.VoucherDate >= request.VoucherDateFrom && x.VoucherDate <= request.VoucherDateTo).Take(size).SumAsync(a => a.CurrencyCredit) + ((request.VoucherDateFrom != null && request.VoucherDateTo != null) ? await entities.Where(x => x.VoucherDate < request.VoucherDateFrom).SumAsync(y => y.CurrencyCredit) : 0) ?? 0);

                if (request.VoucherDateFrom != null && request.VoucherDateTo != null) entities = entities.Where(x => x.VoucherDate >= request.VoucherDateFrom && x.VoucherDate <= request.VoucherDateTo);

                var result = await entities.Paginate(request.Paginator()).ToListAsync(cancellationToken);
                data.AddRange(result);

                if (data.Count > 0 && (beforeDebit > 0 || beforeCredit > 0))
                {
                    LedgerReportModel before = new LedgerReportModel();
                    before.AccountHeadCode = "";
                    before.Credit = beforeCredit;
                    before.Debit = beforeDebit;
                    before.CurrencyCredit = beforeCurrencyCredit;
                    before.CurrencyDebit = beforeCurrencyDebit;
                    before.Id = 0;
                    before.Remaining = (double)(beforeDebit - beforeCredit);
                    before.Title = "";
                    before.VoucherRowDescription = "مانده از قبل";
                    before.VoucherNo = null;
                    if (beforeDebit > 0 || beforeCredit > 0)
                        data.Insert(0, before);
                }


                long remaining = 0;
                long allDebit = 0;
                long allCredit = 0;

                double currencyRemaining = 0;
                double allCurrencyDebit = 0;
                double allCurrencyCredit = 0;

                if (beforeDebit > 0 || beforeCredit > 0)
                {
                    remaining = beforeDebit - beforeCredit;
                    allDebit += beforeDebit;
                    allCredit += beforeCredit;

                    currencyRemaining = beforeCurrencyDebit - beforeCurrencyCredit;
                    allCurrencyDebit += beforeCurrencyDebit;
                    allCurrencyCredit += beforeCurrencyCredit;
                }
                foreach (var entity in data.Where(x => x.Id > 0))
                {
                    remaining = (long)entity.Debit - (long)entity.Credit + remaining;
                    allDebit += (long)entity.Debit;
                    allCredit += (long)entity.Credit;
                    entity.Remaining = remaining;

                    currencyRemaining = (double)entity.CurrencyDebit - (double)entity.CurrencyCredit + currencyRemaining;
                    allCurrencyDebit += (double)entity.CurrencyDebit;
                    allCurrencyCredit += (double)entity.CurrencyCredit;
                    entity.CurrencyRemain = currencyRemaining;
                }


                return ServiceResult.Success(new
                {
                    Data = data,
                    TotalDebit = await entities.SumAsync(x => x.Debit),
                    TotalCredit = await entities.SumAsync(x => x.Credit),
                    TotalCurrencyCredit = await entities.SumAsync(x => x.Credit > 0 ? x.CurrencyAmount : 0),
                    TotalCurrencyDebit = await entities.SumAsync(x => x.Debit > 0 ? x.CurrencyAmount : 0),
                    CurrencyRemain = currencyRemaining,
                    TotalCount = totalCount,
                    Remaining = remaining
                });
            }
            else
            {
                var input = _mapper.Map<StpReportLedgerInput>(request);

                input.CompanyId = _currentUser.GetCompanyId();

                if (input.VoucherDateTo.HasValue)
                    input.VoucherDateTo = input.VoucherDateTo.Value.AddDays(1).AddSeconds(-1);
                var res = await _accountingUnitOfWorkProcedures
                     .StpReportLedgerAsync(
                         input,
                         cancellationToken: cancellationToken
                     );

                var totalDebit = res.Aggregate(new double(), (r, c) => r + c.Debit ?? 0);
                var totalCredit = res.Aggregate(new double(), (r, c) => r + c.Credit ?? 0);


                return ServiceResult.Success(new { totalCredit = totalCredit, totalDebit = totalDebit, remain = totalCredit - totalDebit, result = res });
            }



        }
        private async Task<DataTable> GetExcelData(IQueryable<LedgerReportModel> input, GetReportLedgerQuery request)
        {
            if (request.PrintType == 0)
            {
                return await GetExcelDataRial(input, request);
            }
            else if (request.PrintType == 1)
            {
                return GetExcelDataDollar(input, request);
            }

            else if (request.PrintType == 2)
            {

                return GetExcelDataRialDollar(input, request);
            }
            else if (request.PrintType == 3)
            {
                return await GetExcelElectronicLedgers(input, request);
            }
            else
            {
                return GetExcelDataRialDollar(input, request);
            }
        }
        private async Task<DataTable> GetExcelElectronicLedgers(IQueryable<LedgerReportModel> input, GetReportLedgerQuery request)
        {
            var legerreports = await input.ToListAsync();
            DataTable data = new DataTable();

            data.TableName = "دفاتر الکترونیکی";
            data.Columns.Add("کد کل حساب", typeof(string));
            data.Columns.Add("عنوان حساب کل", typeof(string));
            data.Columns.Add("کد حساب معین", typeof(string));
            data.Columns.Add("عنوان حساب معین", typeof(string));
            if (request.ReferenceIds.Length == 0)
            {
                data.Columns.Add("کد حساب تفصیلی", typeof(string));
                data.Columns.Add("عنوان حساب تفصیل", typeof(string));
            }

            data.Columns.Add("گردش بدهکار (میلیون ریال)", typeof(double));
            data.Columns.Add("گردش بستانکار (میلیون ریال)", typeof(double));
            data.Columns.Add("تاریخ گردش حساب", typeof(string));

            if (legerreports.Count > 0)
            {
                double remaining = 0;
                double allDebit = 0;
                double allCredit = 0;

                if (request.ReferenceIds.Length == 0)
                {
                    for (int i = 0; i < legerreports.Count; i++)
                    {
                        LedgerReportModel LedgerReportModel = Calculate(i);

                        data.Rows.Add(LedgerReportModel.AccountHeadCode, LedgerReportModel.Title, LedgerReportModel.AccountHeadLevel2Code, LedgerReportModel.AccountHeadLevel2Title,
                         LedgerReportModel.ReferenceCode_1, LedgerReportModel.ReferenceName_1,
                         LedgerReportModel.Debit / 1000000, LedgerReportModel.Credit / 1000000, LedgerReportModel.VoucherDate.ToFa());
                    }
                }
                else
                {
                    for (int i = 0; i < legerreports.Count; i++)
                    {
                        var LedgerReportModel = Calculate(i);

                        data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate.ToFa(), LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
                            LedgerReportModel.VoucherRowDescription, LedgerReportModel.Debit, LedgerReportModel.Credit, remaining);
                    }
                }
                LedgerReportModel Calculate(int i)
                {
                    LedgerReportModel LedgerReportModel = legerreports[i];
                    remaining = (double)LedgerReportModel.Debit - (double)LedgerReportModel.Credit + remaining;
                    allDebit += (double)LedgerReportModel.Debit;
                    allCredit += (double)LedgerReportModel.Credit;
                    return LedgerReportModel;
                }


            }
            return data;
        }
        private async Task<DataTable> GetExcelDataRial(IQueryable<LedgerReportModel> input, GetReportLedgerQuery request)
        {
            var legerreports = await input.ToListAsync();
            DataTable data = new DataTable();

            data.TableName = "دفتر تفصیلی مطابق ردیفهای سند";
            data.Columns.Add("سند", typeof(string));
            data.Columns.Add("تاریخ", typeof(string));
            data.Columns.Add("کد حساب", typeof(string));
            data.Columns.Add("عنوان حساب", typeof(string));
            if (request.ReferenceIds.Length == 0)
            {
                data.Columns.Add("کد تفصیل", typeof(string));
                data.Columns.Add("عنوان تفصیل", typeof(string));
            }
            data.Columns.Add("شرح", typeof(string));
            data.Columns.Add("بدهکار", typeof(double));
            data.Columns.Add("بستانکار", typeof(double));
            data.Columns.Add("مانده", typeof(double));

            if (legerreports.Count > 0)
            {
                double remaining = 0;
                double allDebit = 0;
                double allCredit = 0;

                if (request.ReferenceIds.Length == 0)
                {
                    for (int i = 0; i < legerreports.Count; i++)
                    {
                        LedgerReportModel LedgerReportModel = Calculate(i);

                        data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate.ToFa(), LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
                         LedgerReportModel.ReferenceCode_1, LedgerReportModel.ReferenceName_1, LedgerReportModel.VoucherRowDescription,
                         LedgerReportModel.Debit, LedgerReportModel.Credit, remaining);
                    }
                }
                else
                {
                    for (int i = 0; i < legerreports.Count; i++)
                    {
                        var LedgerReportModel = Calculate(i);

                        data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate.ToFa(), LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
                            LedgerReportModel.VoucherRowDescription, LedgerReportModel.Debit, LedgerReportModel.Credit, remaining);
                    }
                }
                LedgerReportModel Calculate(int i)
                {
                    LedgerReportModel LedgerReportModel = legerreports[i];
                    remaining = (double)LedgerReportModel.Debit - (double)LedgerReportModel.Credit + remaining;
                    allDebit += (double)LedgerReportModel.Debit;
                    allCredit += (double)LedgerReportModel.Credit;
                    return LedgerReportModel;
                }


            }
            return data;
        }
        private DataTable GetExcelDataDollar(IQueryable<LedgerReportModel> input, GetReportLedgerQuery request)
        {
            var legerreports = input.Where(a => a.CurrencyAmount != null & a.CurrencyAmount != 0).ToList();
            DataTable data = new DataTable();
            data.TableName = "دفتر تفصیلی مطابق ردیفهای سند";
            data.Columns.Add("سند", typeof(string));
            data.Columns.Add("تاریخ", typeof(string));
            data.Columns.Add("کد حساب", typeof(string));
            data.Columns.Add("عنوان حساب", typeof(string));
            if (request.ReferenceIds.Length == 0)
            {
                data.Columns.Add("کد تفصیل", typeof(string));
                data.Columns.Add("عنوان تفصیل", typeof(string));
            }
            data.Columns.Add("شرح", typeof(string));
            data.Columns.Add("بدهکار", typeof(double));
            data.Columns.Add("بستانکار", typeof(double));
            data.Columns.Add("مانده", typeof(double));
            data.Columns.Add("نوع ارز", typeof(string));
            data.Columns.Add("نرخ تبدیل", typeof(double));

            if (legerreports.Count > 0)
            {
                bool showSum = false;
                double remaining = 0;
                double allDebit = 0;
                double allCredit = 0;
                if (request.ReferenceIds.Length == 0)
                {
                    for (int i = 0; i < legerreports.Count; i++)
                    {
                        LedgerReportModel LedgerReportModel = Calculate(i);

                        data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate.ToFa(), LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
                         LedgerReportModel.ReferenceCode_1, LedgerReportModel.ReferenceName_1, LedgerReportModel.VoucherRowDescription,
                       LedgerReportModel.Debit != 0 ? LedgerReportModel.CurrencyAmount : 0, LedgerReportModel.Credit != 0 ? LedgerReportModel.CurrencyAmount : 0,
                       remaining, LedgerReportModel.CurrencyTypeBaseTitle, LedgerReportModel.CurrencyFee);

                        if (showSum)
                        {
                            data.Rows.Add("", "", "", "", "", "", "جمع کل" + $"({LedgerReportModel.CurrencyTypeBaseTitle})", allDebit, allCredit, remaining);
                            remaining = 0;
                            allDebit = 0;
                            allCredit = 0;
                            showSum = false;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < legerreports.Count; i++)
                    {
                        LedgerReportModel LedgerReportModel = Calculate(i);

                        data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate.ToFa(), LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
                            LedgerReportModel.VoucherRowDescription,
                       LedgerReportModel.Debit != 0 ? LedgerReportModel.CurrencyAmount : 0, LedgerReportModel.Credit != 0 ? LedgerReportModel.CurrencyAmount : 0,
                       remaining, LedgerReportModel.CurrencyTypeBaseTitle, LedgerReportModel.CurrencyFee);

                        if (showSum)
                        {
                            data.Rows.Add("", "", "", "", "جمع کل" + $"({LedgerReportModel.CurrencyTypeBaseTitle})", allDebit, allCredit, remaining);
                            remaining = 0;
                            allDebit = 0;
                            allCredit = 0;
                            showSum = false;
                        }
                    }
                }



                LedgerReportModel Calculate(int i)
                {
                    if (i + 1 < legerreports.Count)
                    {
                        if (legerreports[i + 1].CurrencyTypeBaseTitle != legerreports[i].CurrencyTypeBaseTitle)
                        {
                            showSum = true;
                        }
                    }
                    else
                    {
                        showSum = true;
                    }
                    LedgerReportModel LedgerReportModel = legerreports[i];
                    if (LedgerReportModel.Debit != 0)
                    {
                        remaining = LedgerReportModel.CurrencyAmount != null ? (double)LedgerReportModel.CurrencyAmount - (double)LedgerReportModel.Credit + remaining : 0 - (double)LedgerReportModel.Credit + remaining;
                        allDebit += LedgerReportModel.CurrencyAmount != null ? (double)LedgerReportModel.CurrencyAmount : 0;

                    }
                    else if (LedgerReportModel.Credit != 0)
                    {
                        remaining = LedgerReportModel.CurrencyAmount != null ? (double)LedgerReportModel.Debit - (double)LedgerReportModel.CurrencyAmount + remaining : (double)LedgerReportModel.Debit - 0 + remaining;
                        allCredit += LedgerReportModel.CurrencyAmount != null ? (double)LedgerReportModel.CurrencyAmount : 0;
                    }
                    return LedgerReportModel;
                }
            }
            return data;
        }

        private DataTable GetExcelDataRialDollar(IQueryable<LedgerReportModel> input, GetReportLedgerQuery request)
        {
            var legerreports = input.Where(a => a.CurrencyAmount != null & a.CurrencyAmount != 0).ToList();
            DataTable data = new DataTable();
            data.TableName = "دفتر تفصیلی مطابق ردیفهای سند";
            data.Columns.Add("سند", typeof(string));
            data.Columns.Add("تاریخ", typeof(string));
            data.Columns.Add("کد حساب", typeof(string));
            data.Columns.Add("عنوان حساب", typeof(string));
            if (request.ReferenceIds.Length == 0)
            {
                data.Columns.Add("کد تفصیل", typeof(string));
                data.Columns.Add("عنوان تفصیل", typeof(string));
            }
            data.Columns.Add("شرح", typeof(string));
            data.Columns.Add("بدهکار ریالی", typeof(double));
            data.Columns.Add("بستانکار ریالی", typeof(double));
            data.Columns.Add("مانده ریالی", typeof(double));

            data.Columns.Add("بدهکار ارزی", typeof(double));
            data.Columns.Add("بستانکار ارزی", typeof(double));
            data.Columns.Add("مانده ارزی", typeof(double));
            data.Columns.Add("نوع ارز", typeof(string));
            data.Columns.Add("نرخ تبدیل", typeof(double));

            if (legerreports.Count > 0)
            {
                double remaining = 0;
                double dollarRemaining = 0;
                double allDebit = 0;
                double allCredit = 0;
                double allDollarDebit = 0;
                double allDollarCredit = 0;
                bool showSum = false;
                double debitDollar = 0;
                double creditDollar = 0;

                if (request.ReferenceIds.Length == 0)
                {
                    for (int i = 0; i < legerreports.Count; i++)
                    {
                        LedgerReportModel LedgerReportModel = Calculate(i);

                        data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate.ToFa(), LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
                         LedgerReportModel.ReferenceCode_1, LedgerReportModel.ReferenceName_1, LedgerReportModel.VoucherRowDescription,
                       LedgerReportModel.Debit, LedgerReportModel.Credit, remaining,
                       debitDollar, creditDollar, dollarRemaining, LedgerReportModel.CurrencyTypeBaseTitle, LedgerReportModel.CurrencyFee);

                        if (showSum)
                        {
                            data.Rows.Add("", "", "", "", "", "", "جمع کل" + $"({LedgerReportModel.CurrencyTypeBaseTitle})", allDebit, allCredit, remaining,
                                allDollarDebit, allDollarCredit, dollarRemaining);
                            remaining = 0;
                            dollarRemaining = 0;
                            allDebit = 0;
                            allCredit = 0;
                            allDollarDebit = 0;
                            allDollarCredit = 0;
                            showSum = false;
                        }
                    }
                }

                else
                {
                    for (int i = 0; i < legerreports.Count; i++)
                    {
                        LedgerReportModel LedgerReportModel = Calculate(i);
                        data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate.ToFa(), LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
                            LedgerReportModel.VoucherRowDescription,
                       LedgerReportModel.Debit, LedgerReportModel.Credit, remaining,
                       debitDollar, creditDollar, dollarRemaining, LedgerReportModel.CurrencyTypeBaseTitle, LedgerReportModel.CurrencyFee);

                        if (showSum)
                        {
                            data.Rows.Add("", "", "", "", "جمع کل" + $"({LedgerReportModel.CurrencyTypeBaseTitle})", allDebit, allCredit, remaining,
                                allDollarDebit, allDollarCredit, dollarRemaining);
                            remaining = 0;
                            dollarRemaining = 0;
                            allDebit = 0;
                            allCredit = 0;
                            allDollarDebit = 0;
                            allDollarCredit = 0;
                            showSum = false;
                        }
                    }
                }


                LedgerReportModel Calculate(int i)
                {
                    LedgerReportModel LedgerReportModel = legerreports[i];

                    if (i + 1 < legerreports.Count)
                    {
                        if (legerreports[i + 1].CurrencyTypeBaseTitle != legerreports[i].CurrencyTypeBaseTitle)
                        {
                            showSum = true;
                        }
                    }
                    else
                    {
                        showSum = true;
                    }
                    remaining = (double)LedgerReportModel.Debit - (double)LedgerReportModel.Credit + remaining;
                    allDebit += (double)LedgerReportModel.Debit;
                    allCredit += (double)LedgerReportModel.Credit;

                    debitDollar = 0;
                    creditDollar = 0;
                    if (LedgerReportModel.Credit > 0)
                    {
                        debitDollar = 0;
                        creditDollar = (double)LedgerReportModel.CurrencyAmount;
                    }
                    else
                    {
                        debitDollar = (double)LedgerReportModel.CurrencyAmount;
                        creditDollar = 0;
                    }
                    allDollarCredit += creditDollar;
                    allDollarDebit += debitDollar;
                    dollarRemaining = debitDollar - creditDollar + dollarRemaining;
                    return LedgerReportModel;
                }

            }
            return data;
        }
    }
}