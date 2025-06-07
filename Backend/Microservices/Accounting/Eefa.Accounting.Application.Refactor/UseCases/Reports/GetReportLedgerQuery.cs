//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using MediatR;
//using Newtonsoft.Json;

//public class GetReportLedgerQuery : IMapFrom<GetReportLedgerQuery>, IRequest<ServiceResult>
//{
//    public bool UseEf { get; set; }
//    public List<Condition> Conditions { get; set; }
//    public int ReportType { get; set; }
//    public int Level { get; set; }
//    public int CompanyId { get; set; }
//    public int[] YearIds { get; set; }
//    public int? VoucherStateId { get; set; }
//    public int[] CodeVoucherGroupIds { get; set; }
//    public int? TransferId { get; set; }
//    public int[] AccountHeadIds { get; set; }
//    public int[] ReferenceGroupIds { get; set; }
//    public int[] ReferenceIds { get; set; }
//    public int[] ChequeSheetIds { get; set; }
//    public int? ReferenceNo { get; set; } = 1;
//    public int? VoucherNoFrom { get; set; }
//    public int? VoucherNoTo { get; set; }
//    public DateTime? VoucherDateFrom { get; set; }
//    public DateTime? VoucherDateTo { get; set; }
//    public double? DebitFrom { get; set; }
//    public double? DebitTo { get; set; }
//    public double? CreditFrom { get; set; }
//    public double? CreditTo { get; set; }
//    public int? DocumentIdFrom { get; set; }
//    public int? DocumentIdTo { get; set; }
//    public string VoucherDescription { get; set; }
//    public string VoucherRowDescription { get; set; }
//    public bool Remain { get; set; }
//    public string ReportTitle { get; set; }
//    public int? CurrencyTypeBaseId { get; set; }
//    public SsrsUtil.ReportFormat ReportFormat { get; set; } = SsrsUtil.ReportFormat.None;
//    public bool IsPrint { get; set; }
//    public bool ForcePrint { get; set; }
//    public int PrintType { get; set; }//0=rial,1=dollar,2=rial&dollar
//    public void Mapping(Profile profile)
//    {
//        profile.CreateMap<GetReportLedgerQuery, StpReportLedgerInput>()
//            .ForMember(x => x.YearIds, opt => opt.MapFrom(t => (t.YearIds != null && t.YearIds.Length != 0) ? JsonConvert.SerializeObject(t.YearIds) : null))
//            .ForMember(x => x.AccountHeadIds, opt => opt.MapFrom(t => (t.AccountHeadIds != null && t.AccountHeadIds.Length != 0) ? JsonConvert.SerializeObject(t.AccountHeadIds) : null))
//            .ForMember(x => x.ReferenceGroupIds, opt => opt.MapFrom(t => (t.ReferenceGroupIds != null && t.ReferenceGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceGroupIds) : null))
//            .ForMember(x => x.ReferenceIds, opt => opt.MapFrom(t => (t.ReferenceIds != null && t.ReferenceIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceIds) : null))
//            .ForMember(x => x.CodeVoucherGroupIds, opt => opt.MapFrom(t => (t.CodeVoucherGroupIds != null && t.CodeVoucherGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.CodeVoucherGroupIds) : null))
//            .IgnoreAllNonExisting();
//    }
//}

//public class GetReportLedgerQueryHandler : IRequestHandler<GetReportLedgerQuery, ServiceResult>
//{
//    private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
//    private readonly IApplicationUser _applicationUser;
//    private readonly IMapper _mapper;
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IAccountingUnitOfWork _context;
//    private readonly DanaAccountingUnitOfWork _danaContext;

//    public GetReportLedgerQueryHandler(IUnitOfWork unitOfWork, IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures, IApplicationUser applicationUserAccessor, IMapper mapper, IAccountingUnitOfWork context, DanaAccountingUnitOfWork danaContext)
//    {
//        _unitOfWork= unitOfWork;
//        _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
//        _currentUserAccessor = currentUserAccessor;
//        _mapper = mapper;
//        _context = context;
//        _danaContext = danaContext;
//    }

//    public async Task<ServiceResult> Handle(GetReportLedgerQuery request, CancellationToken cancellationToken)
//    {
//        if (request.UseEf)
//        {
//            request.VoucherDateTo = request.VoucherDateTo?.AddDays(1).AddMilliseconds(-1);
//            //var lksss = _context.AccountReferencesGroups.AsQueryable().ApplyPermission(_context, _currentUserAccessor, false, true);
//            IQueryable<VouchersDetail> voucherDetailQuery = null;

//            // Remove For Chocolate Factory Project
//            if (request.YearIds.Contains(3))
//            {
//                voucherDetailQuery = _danaContext.VouchersDetails.AsQueryable().ApplyPermission(_danaContext, _currentUserAccessor, false, false);
//            }
//            else
//            {
//                voucherDetailQuery = _unitOfWorkGetQuery<VouchersDetail>().ApplyPermission(_context, _currentUserAccessor, false, false);
//            }
//            //var voucherDetailQuery = _unitOfWorkGetAll<VouchersDetail>();

//            if (request.VoucherDateFrom != null && request.VoucherDateTo != null) voucherDetailQuery = voucherDetailQuery.Where(x => x.Voucher.VoucherDate >= request.VoucherDateFrom && x.Voucher.VoucherDate <= request.VoucherDateTo);
//            if (request.YearIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.YearIds.Contains(x.Voucher.YearId));
//            if (request.CompanyId != 0) voucherDetailQuery = voucherDetailQuery.Where(x => x.Voucher.CompanyId == request.CompanyId);

//            if (request.ChequeSheetIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.ChequeSheetIds.ToList().Contains((int)x.ChequeSheetId));

//            if (request.AccountHeadIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.AccountHeadIds.Contains(x.AccountHeadId));
//            if (request.ReferenceIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.ReferenceIds.ToList().Contains(x.ReferenceId1 ?? -1));
//            if (request.ReferenceGroupIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.ReferenceGroupIds.ToList().Contains(x.AccountReferencesGroupId ?? -1));


//            if (request.CodeVoucherGroupIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.CodeVoucherGroupIds.ToList().Contains(x.Voucher.CodeVoucherGroupId));
//            if (request.VoucherRowDescription != null) voucherDetailQuery = voucherDetailQuery.Where(x => x.VoucherRowDescription.Contains(request.VoucherRowDescription));

//            if (request.CurrencyTypeBaseId != null && request.CurrencyTypeBaseId != 0)
//            {
//                voucherDetailQuery = voucherDetailQuery.Where(a => a.CurrencyTypeBaseId == request.CurrencyTypeBaseId);
//            }

//            IOrderedQueryable<LedgerReportModel> entities = voucherDetailQuery
//                .ProjectTo<LedgerReportModel>(_mapper.ConfigurationProvider)
//                .WhereQueryMaker(request.Conditions)
//                .OrderByMultipleColumns(request.OrderByProperty);
//            var TotalCount = await entities.CountAsync(cancellationToken);

//            if ((request.IsPrint && TotalCount > 1000) || request.ForcePrint)
//            {

//                var exeldata = await GetExcelData(entities, request);
//                string base64String;

//                using (var wb = new XLWorkbook())
//                {
//                    wb.RightToLeft = true;
//                    wb.Author = _currentUserAccessor.GetUsername();
//                    wb.RowHeight = 20;

//                    var sheet = wb.AddWorksheet(exeldata, "دفتر تفصیلی مطابق ردیفهای سند");
//                    using (var ms = new MemoryStream())
//                    {
//                        wb.SaveAs(ms);
//                        base64String = Convert.ToBase64String(ms.ToArray());
//                    }
//                }
//                return ServiceResult.Success(new
//                {
//                    Data = base64String
//                });
//            }
//            List<LedgerReportModel> data = await entities.Paginate(request.Paginator()).ToListAsync(cancellationToken);

//            long remaining = 0;
//            long allDebit = 0;
//            long allCredit = 0;
//            foreach (var entity in data)
//            {
//                remaining = (long)entity.Debit - (long)entity.Credit + remaining;
//                allDebit += (long)entity.Debit;
//                allCredit += (long)entity.Credit;
//                entity.Remaining = remaining;
//            }

//            return ServiceResult.Success(new
//            {
//                Data = data,
//                TotalDebit = await entities.SumAsync(x => x.Debit),
//                TotalCredit = await entities.SumAsync(x => x.Credit),
//                TotalCount = TotalCount,
//                Remaining = remaining
//            });
//        }
//        else
//        {
//            var input = _mapper.Map<StpReportLedgerInput>(request);

//            input.CompanyId = _currentUserAccessor.GetCompanyId();

//            if (input.VoucherDateTo.HasValue)
//                input.VoucherDateTo = input.VoucherDateTo.Value.AddDays(1).AddSeconds(-1);
//            var res = await _accountingUnitOfWorkProcedures
//                 .StpReportLedgerAsync(
//                     input,
//                     cancellationToken: cancellationToken
//                 );

//            var totalDebit = res.Aggregate(new double(), (r, c) => r + c.Debit ?? 0);
//            var totalCredit = res.Aggregate(new double(), (r, c) => r + c.Credit ?? 0);


//            return ServiceResult.Success(new { totalCredit = totalCredit, totalDebit = totalDebit, remain = totalCredit - totalDebit, result = res });
//        }
//    }
//    private async Task<DataTable> GetExcelData(IOrderedQueryable<LedgerReportModel> input, GetReportLedgerQuery request)
//    {
//        if (request.PrintType == 0)
//        {
//            return await GetExcelDataRial(input, request);
//        }
//        else if (request.PrintType == 1)
//        {
//            return GetExcelDataDollar(input, request);
//        }
//        else
//        {
//            return GetExcelDataRialDollar(input, request);
//        }
//    }

//    private async Task<DataTable> GetExcelDataRial(IOrderedQueryable<LedgerReportModel> input, GetReportLedgerQuery request)
//    {
//        var legerreports = await input.ToListAsync();
//        DataTable data = new DataTable();

//        data.TableName = "دفتر تفصیلی مطابق ردیفهای سند";
//        data.Columns.Add("سند", typeof(string));
//        data.Columns.Add("تاریخ", typeof(string));
//        data.Columns.Add("کد حساب", typeof(string));
//        data.Columns.Add("عنوان حساب", typeof(string));
//        if (request.ReferenceIds.Length == 0)
//        {
//            data.Columns.Add("کد تفصیل", typeof(string));
//            data.Columns.Add("عنوان تفصیل", typeof(string));
//        }
//        data.Columns.Add("شرح", typeof(string));
//        data.Columns.Add("بدهکار", typeof(string));
//        data.Columns.Add("بستانکار", typeof(string));
//        data.Columns.Add("مانده", typeof(string));

//        if (legerreports.Count > 0)
//        {
//            double remaining = 0;
//            double allDebit = 0;
//            double allCredit = 0;

//            if (request.ReferenceIds.Length == 0)
//            {
//                for (int i = 0; i < legerreports.Count; i++)
//                {
//                    LedgerReportModel LedgerReportModel = Calculate(i);

//                    data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate.ToFa(), LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
//                     LedgerReportModel.ReferenceCode_1, LedgerReportModel.ReferenceName_1, LedgerReportModel.VoucherRowDescription,
//                     LedgerReportModel.Debit?.ToString("n"), LedgerReportModel.Credit?.ToString("n"), remaining.ToString("n"));
//                }
//            }
//            else
//            {
//                for (int i = 0; i < legerreports.Count; i++)
//                {
//                    var LedgerReportModel = Calculate(i);

//                    data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate, LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
//                        LedgerReportModel.VoucherRowDescription, LedgerReportModel.Debit?.ToString("n"), LedgerReportModel.Credit?.ToString("n"), remaining.ToString("n"));
//                }
//            }
//            LedgerReportModel Calculate(int i)
//            {
//                LedgerReportModel LedgerReportModel = legerreports[i];
//                remaining = (double)LedgerReportModel.Debit - (double)LedgerReportModel.Credit + remaining;
//                allDebit += (double)LedgerReportModel.Debit;
//                allCredit += (double)LedgerReportModel.Credit;
//                return LedgerReportModel;
//            }

//            if (request.ReferenceIds.Length == 0)
//            {
//                data.Rows.Add("", "", "", "", "", "", "جمع کل", allDebit.ToString("n"), allCredit.ToString("n"), remaining.ToString("n"));
//                data.Rows.Add("", "", "", "", "", "", "", "", "تاریخ چاپ: " + DateTime.Now.ToFa("F"), "کاربر چاپ کننده: " + _currentUserAccessor.GetUsername());
//            }
//            else
//            {
//                data.Rows.Add("", "", "", "", "جمع کل", allDebit.ToString("n"), allCredit.ToString("n"), remaining.ToString("n"));
//                data.Rows.Add("", "", "", "", "", "", "تاریخ چاپ: " + DateTime.Now.ToFa("F"), "کاربر چاپ کننده: " + _currentUserAccessor.GetUsername());
//            }
//        }
//        return data;
//    }
//    private DataTable GetExcelDataDollar(IOrderedQueryable<LedgerReportModel> input, GetReportLedgerQuery request)
//    {
//        var legerreports = input.Where(a => a.CurrencyAmount != null & a.CurrencyAmount != 0).ToList();
//        DataTable data = new DataTable();
//        data.TableName = "دفتر تفصیلی مطابق ردیفهای سند";
//        data.Columns.Add("سند", typeof(string));
//        data.Columns.Add("تاریخ", typeof(string));
//        data.Columns.Add("کد حساب", typeof(string));
//        data.Columns.Add("عنوان حساب", typeof(string));
//        if (request.ReferenceIds.Length == 0)
//        {
//            data.Columns.Add("کد تفصیل", typeof(string));
//            data.Columns.Add("عنوان تفصیل", typeof(string));
//        }
//        data.Columns.Add("شرح", typeof(string));
//        data.Columns.Add("بدهکار", typeof(string));
//        data.Columns.Add("بستانکار", typeof(string));
//        data.Columns.Add("مانده", typeof(string));
//        data.Columns.Add("نوع ارز", typeof(string));
//        data.Columns.Add("نرخ تبدیل", typeof(string));

//        if (legerreports.Count > 0)
//        {
//            bool showSum = false;
//            double remaining = 0;
//            double allDebit = 0;
//            double allCredit = 0;
//            if (request.ReferenceIds.Length == 0)
//            {
//                for (int i = 0; i < legerreports.Count; i++)
//                {
//                    LedgerReportModel LedgerReportModel = Calculate(i);

//                    data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate.ToFa(), LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
//                     LedgerReportModel.ReferenceCode_1, LedgerReportModel.ReferenceName_1, LedgerReportModel.VoucherRowDescription,
//                   LedgerReportModel.Debit != 0 ? LedgerReportModel.CurrencyAmount?.ToString("n") : 0, LedgerReportModel.Credit != 0 ? LedgerReportModel.CurrencyAmount?.ToString("n") : 0,
//                   remaining.ToString("n"), LedgerReportModel.CurrencyTypeBaseTitle, LedgerReportModel.CurrencyFee);

//                    if (showSum)
//                    {
//                        data.Rows.Add("", "", "", "", "", "", "جمع کل" + $"({LedgerReportModel.CurrencyTypeBaseTitle})", allDebit.ToString("n"), allCredit.ToString("n"), remaining.ToString("n"));
//                        remaining = 0;
//                        allDebit = 0;
//                        allCredit = 0;
//                        showSum = false;
//                    }
//                }
//            }
//            else
//            {
//                for (int i = 0; i < legerreports.Count; i++)
//                {
//                    LedgerReportModel LedgerReportModel = Calculate(i);

//                    data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate, LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
//                        LedgerReportModel.VoucherRowDescription,
//                   LedgerReportModel.Debit != 0 ? LedgerReportModel.CurrencyAmount?.ToString("n") : 0, LedgerReportModel.Credit != 0 ? LedgerReportModel.CurrencyAmount?.ToString("n") : 0,
//                   remaining.ToString("n"), LedgerReportModel.CurrencyTypeBaseTitle, LedgerReportModel.CurrencyFee);

//                    if (showSum)
//                    {
//                        data.Rows.Add("", "", "", "", "جمع کل" + $"({LedgerReportModel.CurrencyTypeBaseTitle})", allDebit.ToString("n"), allCredit.ToString("n"), remaining.ToString("n"));
//                        remaining = 0;
//                        allDebit = 0;
//                        allCredit = 0;
//                        showSum = false;
//                    }
//                }
//            }

//            if (request.ReferenceIds.Length == 0)
//            {
//                data.Rows.Add("", "", "", "", "", "", "", "", "تاریخ چاپ: " + DateTime.Now.ToFa("F"), "کاربر چاپ کننده: " + _currentUserAccessor.GetUsername());
//            }
//            else
//            {
//                data.Rows.Add("", "", "", "", "", "", "تاریخ چاپ: " + DateTime.Now.ToFa("F"), "کاربر چاپ کننده: " + _currentUserAccessor.GetUsername());
//            }

//            LedgerReportModel Calculate(int i)
//            {
//                if (i + 1 < legerreports.Count)
//                {
//                    if (legerreports[i + 1].CurrencyTypeBaseTitle != legerreports[i].CurrencyTypeBaseTitle)
//                    {
//                        showSum = true;
//                    }
//                }
//                else
//                {
//                    showSum = true;
//                }
//                LedgerReportModel LedgerReportModel = legerreports[i];
//                if (LedgerReportModel.Debit != 0)
//                {
//                    remaining = LedgerReportModel.CurrencyAmount != null ? (double)LedgerReportModel.CurrencyAmount : 0 - (double)LedgerReportModel.Credit + remaining;
//                    allDebit += LedgerReportModel.CurrencyAmount != null ? (double)LedgerReportModel.CurrencyAmount : 0;
//                }
//                else if (LedgerReportModel.Credit != 0)
//                {
//                    remaining = (double)LedgerReportModel.Debit - LedgerReportModel.CurrencyAmount != null ? (double)LedgerReportModel.CurrencyAmount : 0 + remaining;
//                    allCredit += LedgerReportModel.CurrencyAmount != null ? (double)LedgerReportModel.CurrencyAmount : 0;
//                }
//                return LedgerReportModel;
//            }
//        }
//        return data;
//    }

//    private DataTable GetExcelDataRialDollar(IOrderedQueryable<LedgerReportModel> input, GetReportLedgerQuery request)
//    {
//        var legerreports = input.Where(a => a.CurrencyAmount != null & a.CurrencyAmount != 0).ToList();
//        DataTable data = new DataTable();
//        data.TableName = "دفتر تفصیلی مطابق ردیفهای سند";
//        data.Columns.Add("سند", typeof(string));
//        data.Columns.Add("تاریخ", typeof(string));
//        data.Columns.Add("کد حساب", typeof(string));
//        data.Columns.Add("عنوان حساب", typeof(string));
//        if (request.ReferenceIds.Length == 0)
//        {
//            data.Columns.Add("کد تفصیل", typeof(string));
//            data.Columns.Add("عنوان تفصیل", typeof(string));
//        }
//        data.Columns.Add("شرح", typeof(string));
//        data.Columns.Add("بدهکار ریالی", typeof(string));
//        data.Columns.Add("بستانکار ریالی", typeof(string));
//        data.Columns.Add("مانده ریالی", typeof(string));

//        data.Columns.Add("بدهکار ارزی", typeof(string));
//        data.Columns.Add("بستانکار ارزی", typeof(string));
//        data.Columns.Add("مانده ارزی", typeof(string));
//        data.Columns.Add("نوع ارز", typeof(string));
//        data.Columns.Add("نرخ تبدیل", typeof(string));

//        if (legerreports.Count > 0)
//        {
//            double remaining = 0;
//            double dollarRemaining = 0;
//            double allDebit = 0;
//            double allCredit = 0;
//            double allDollarDebit = 0;
//            double allDollarCredit = 0;
//            bool showSum = false;
//            double debitDollar = 0;
//            double creditDollar = 0;

//            if (request.ReferenceIds.Length == 0)
//            {
//                for (int i = 0; i < legerreports.Count; i++)
//                {
//                    LedgerReportModel LedgerReportModel = Calculate(i);

//                    data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate.ToFa(), LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
//                     LedgerReportModel.ReferenceCode_1, LedgerReportModel.ReferenceName_1, LedgerReportModel.VoucherRowDescription,
//                   LedgerReportModel.Debit?.ToString("n"), LedgerReportModel.Credit?.ToString("n"), remaining.ToString("n"),
//                   debitDollar.ToString("n"), creditDollar.ToString("n"), dollarRemaining.ToString("n"), LedgerReportModel.CurrencyTypeBaseTitle, LedgerReportModel.CurrencyFee);

//                    if (showSum)
//                    {
//                        data.Rows.Add("", "", "", "", "", "", "جمع کل" + $"({LedgerReportModel.CurrencyTypeBaseTitle})", allDebit.ToString("n"), allCredit.ToString("n"), remaining.ToString("n"),
//                            allDollarDebit.ToString("n"), allDollarCredit.ToString("n"), dollarRemaining.ToString("n"));
//                        remaining = 0;
//                        dollarRemaining = 0;
//                        allDebit = 0;
//                        allCredit = 0;
//                        allDollarDebit = 0;
//                        allDollarCredit = 0;
//                        showSum = false;
//                    }
//                }
//            }

//            else
//            {
//                for (int i = 0; i < legerreports.Count; i++)
//                {
//                    LedgerReportModel LedgerReportModel = Calculate(i);
//                    data.Rows.Add(LedgerReportModel.VoucherNo, LedgerReportModel.VoucherDate, LedgerReportModel.AccountHeadCode, LedgerReportModel.Title,
//                        LedgerReportModel.VoucherRowDescription,
//                   LedgerReportModel.Debit?.ToString("n"), LedgerReportModel.Credit?.ToString("n"), remaining.ToString("n"),
//                   debitDollar.ToString("n"), creditDollar.ToString("n"), dollarRemaining.ToString("n"), LedgerReportModel.CurrencyTypeBaseTitle, LedgerReportModel.CurrencyFee);

//                    if (showSum)
//                    {
//                        data.Rows.Add("", "", "", "", "جمع کل" + $"({LedgerReportModel.CurrencyTypeBaseTitle})", allDebit.ToString("n"), allCredit.ToString("n"), remaining.ToString("n"),
//                            allDollarDebit.ToString("n"), allDollarCredit.ToString("n"), dollarRemaining.ToString("n"));
//                        remaining = 0;
//                        dollarRemaining = 0;
//                        allDebit = 0;
//                        allCredit = 0;
//                        allDollarDebit = 0;
//                        allDollarCredit = 0;
//                        showSum = false;
//                    }
//                }
//            }

//            LedgerReportModel Calculate(int i)
//            {
//                LedgerReportModel LedgerReportModel = legerreports[i];

//                if (i + 1 < legerreports.Count)
//                {
//                    if (legerreports[i + 1].CurrencyTypeBaseTitle != legerreports[i].CurrencyTypeBaseTitle)
//                    {
//                        showSum = true;
//                    }
//                }
//                else
//                {
//                    showSum = true;
//                }
//                remaining = (double)LedgerReportModel.Debit - (double)LedgerReportModel.Credit + remaining;
//                allDebit += (double)LedgerReportModel.Debit;
//                allCredit += (double)LedgerReportModel.Credit;

//                debitDollar = 0;
//                creditDollar = 0;
//                if (LedgerReportModel.Credit > 0)
//                {
//                    debitDollar = 0;
//                    creditDollar = (double)LedgerReportModel.CurrencyAmount;
//                }
//                else
//                {
//                    debitDollar = (double)LedgerReportModel.CurrencyAmount;
//                    creditDollar = 0;
//                }
//                allDollarCredit += creditDollar;
//                allDollarDebit += debitDollar;
//                dollarRemaining = debitDollar - creditDollar + dollarRemaining;
//                return LedgerReportModel;
//            }

//            if (request.ReferenceIds.Length == 0)
//            {
//                data.Rows.Add("", "", "", "", "", "", "", "", "تاریخ چاپ: " + DateTime.Now.ToFa("F"), "کاربر چاپ کننده: " + _currentUserAccessor.GetUsername());

//            }
//            else
//            {
//                data.Rows.Add("", "", "", "", "", "", "تاریخ چاپ: " + DateTime.Now.ToFa("F"), "کاربر چاپ کننده: " + _currentUserAccessor.GetUsername());
//            }
//        }
//        return data;
//    }
//}