//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//public class AnnualLedgerReportQuery : IRequest<ServiceResult>
//{
//    public int CompanyId { get; set; }
//    public int[] YearIds { get; set; }
//    public DateTime VoucherDateFrom { get; set; }
//    public DateTime VoucherDateTo { get; set; }
//    public int?[] AccountHeadIds { get; set; }
//    public List<Condition> Conditions { get; set; }
//}
//public class AnnualLedgerReportQueryHandler : IRequestHandler<AnnualLedgerReportQuery, ServiceResult>
//{
//    private readonly IAccountingUnitOfWork _context;

//    public AnnualLedgerReportQueryHandler(IAccountingUnitOfWork context)
//    {
//        _context = context;
//    }

//    public async Task<ServiceResult> Handle(AnnualLedgerReportQuery request, CancellationToken cancellationToken)
//    {
//        var beforequery = _context.ViewVoucherDetails
//            .Where(a => a.VoucherDate < request.VoucherDateFrom && a.YearId == request.YearIds[0] && a.CompanyId == request.CompanyId);

//        var query = _context.ViewVoucherDetails.Where(a => a.VoucherDate >= request.VoucherDateFrom && a.VoucherDate < request.VoucherDateTo &&
//       a.YearId == request.YearIds[0] && a.CompanyId == request.CompanyId).GroupBy(a => new { a.Level2Code, a.Level2Title, a.Level2 });
//        var debitquery = query
//       .Select(a => new AnnualLedgerData()
//       {
//           Level2Code = a.Key.Level2Code,
//           Level2Title = a.Key.Level2Title,
//           Debit = a.Sum(f => f.Debit),
//           Credit = 0,
//           Level2 = a.Key.Level2,
//           Id = 0
//       });
//        var creditquery = query
//       .Select(a => new AnnualLedgerData()
//       {
//           Level2Code = a.Key.Level2Code,
//           Level2Title = a.Key.Level2Title,
//           Debit = 0,
//           Credit = a.Sum(f => f.Credit),
//           Level2 = a.Key.Level2,
//           Id = 0
//       });
//        if (request.AccountHeadIds?.Length > 0)
//        {
//            beforequery = beforequery.Where(x => request.AccountHeadIds.Contains(x.Level2));
//            debitquery = debitquery.Where(x => request.AccountHeadIds.Contains(x.Level2));
//            creditquery = creditquery.Where(x => request.AccountHeadIds.Contains(x.Level2));
//        }

//        var before = beforequery.GroupBy(a => true)
//            .Select(a => new
//            {
//                debit = a.Sum(f => f.Debit),
//                credit = a.Sum(f => f.Credit)
//            }).FirstOrDefault();

//        List<AnnualLedgerData> result = new();
//        result.Add(new AnnualLedgerData() { Level2Code = "0", Level2Title = "نقل از قبل", Credit = before.credit, Debit = before.debit });

//        result.AddRange(debitquery.Where(a => a.Debit != 0).OrderBy(a => a.Level2Code)
//       .WhereQueryMaker(request.Conditions).Paginate(request.Paginator()).ToList());

//        result.AddRange(creditquery.Where(a => a.Credit != 0).OrderBy(a => a.Level2Code)
//       .WhereQueryMaker(request.Conditions).Paginate(request.Paginator()).ToList());

//        var debit = result.Sum(a => a.Debit);
//        var credit = result.Sum(a => a.Credit);

//        AnnualLedgerResultModel resultModel = new(before.debit, before.credit, debit, credit, result);

//        return ServiceResult.Success(resultModel);
//    }
//}