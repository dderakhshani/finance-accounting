//using MediatR;
//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Data;
//using AutoMapper;

//public class ProfitAndLossReportQuery : IRequest<ServiceResult>
//{
//    public int CompanyId { get; set; }
//    public int[] YearIds { get; set; }
//    public DateTime? VoucherDateFrom { get; set; }
//    public DateTime? VoucherDateTo { get; set; }
//}
//public class ProfitAndLossReportQueryHandler : IRequestHandler<ProfitAndLossReportQuery, ServiceResult>
//{
//    private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
//    private readonly IApplicationUser _applicationUser;
//    private readonly IMapper _mapper;
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IAccountingUnitOfWork _context;
//    public ProfitAndLossReportQueryHandler(IUnitOfWork unitOfWork, IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures, IApplicationUser applicationUserAccessor, IMapper mapper, IAccountingUnitOfWork context)
//    {
//        _unitOfWork= unitOfWork;
//        _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
//        _currentUserAccessor = currentUserAccessor;
//        _mapper = mapper;
//        _context = context;
//    }
//    public async Task<ServiceResult> Handle(ProfitAndLossReportQuery request, CancellationToken cancellationToken)
//    {
//        request.VoucherDateTo = request.VoucherDateTo?.AddDays(1).AddMilliseconds(-1);
//        var voucherDetailQuery = _context.VouchersDetails.ApplyPermission(_context, _currentUserAccessor, false, false);

//        if (request.VoucherDateFrom != null && request.VoucherDateTo != null) voucherDetailQuery = voucherDetailQuery.Where(x => x.Voucher.VoucherDate >= request.VoucherDateFrom && x.Voucher.VoucherDate <= request.VoucherDateTo);
//        if (request.YearIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.YearIds.Contains(x.Voucher.YearId));
//        if (request.CompanyId != 0) voucherDetailQuery = voucherDetailQuery.Where(x => x.Voucher.CompanyId == request.CompanyId);

//        var level3 = await voucherDetailQuery.Join(_context.AccountHeads, vd => vd.AccountHeadId, ah => ah.Id, (vd, ah) => new { vd, ah })
//            .Where(a => a.ah.Code == "6101" ||
//        a.ah.Code == "6102" || a.ah.Code == "6103").GroupBy(a => new { a.ah.Title, a.ah.Code })
//            .Select(a => new Level3
//            {
//                Level3Code = a.Key.Code,
//                Leve3Name = a.Key.Title,
//                Price = a.Sum(f => f.vd.Credit) - a.Sum(f => f.vd.Debit),
//            }).ToListAsync(cancellationToken);
//        ProfitAndLossReportModel result = new();
//        result.level3s.AddRange(level3);

//        result.SaleSum = result.level3s.Where(a => a.Level3Code == "6101" || a.Level3Code == "6102").Sum(a => a.Price);

//        result.TotalOperatingIncome = result.SaleSum +
//            (result.level3s.Where(a => a.Level3Code == "6103").FirstOrDefault() == null ? 0 : result.level3s.Where(a => a.Level3Code == "6103").FirstOrDefault().Price);

//        var level2 = await voucherDetailQuery.Join(_context.AccountHeads, vd => vd.Level2, ah => ah.Id, (vd, ah) => new { vd, ah })
//            .Where(a => a.ah.Code == "71" || a.ah.Code == "80" || a.ah.Code == "81" || a.ah.Code == "82" || a.ah.Code == "62" ||
//        a.ah.Code == "83" || a.ah.Code == "63").GroupBy(a => new { a.ah.Title, a.ah.Code }).
//        Select(a => new Level2
//        {
//            Level2Code = a.Key.Code,
//            Leve2Name = a.Key.Title,
//            Price = a.Sum(f => f.vd.Credit) - a.Sum(f => f.vd.Debit),
//        }).ToListAsync(cancellationToken);
//        result.level2s.AddRange(level2);

//        result.GrossProfit = (result.level2s.Where(a => a.Level2Code == "71").FirstOrDefault() == null ? 0 : result.level2s.Where(a => a.Level2Code == "71").FirstOrDefault().Price)
//            + result.TotalOperatingIncome;
//        result.TotalOperatingCosts = result.level2s.Where(a => a.Level2Code == "80" || a.Level2Code == "81" || a.Level2Code == "82" || a.Level2Code == "62")
//            .Sum(a => a.Price);

//        result.OperatingProfitAndLoss = result.TotalOperatingCosts + result.GrossProfit;
//        result.GrossProfitBeforeTax = result.OperatingProfitAndLoss + result.level2s.Where(a => a.Level2Code == "83" || a.Level2Code == "63").Sum(a => a.Price);
//        result.IncomeTaxEexpense = 0;
//        result.NetProfit = result.IncomeTaxEexpense + result.GrossProfitBeforeTax;
//        result.SalesMargin = result.GrossProfit / result.TotalOperatingIncome;
//        result.OperatingPprofitMargin = result.OperatingProfitAndLoss / result.TotalOperatingIncome;
//        result.ProfitMarigin = result.NetProfit / result.TotalOperatingIncome;
//        return ServiceResult.Success(result);
//    }
//}