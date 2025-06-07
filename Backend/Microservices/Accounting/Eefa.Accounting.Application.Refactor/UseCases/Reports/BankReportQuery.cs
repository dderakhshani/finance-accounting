//using System.Threading.Tasks;
//using AutoMapper;
//using MediatR;
//using Newtonsoft.Json;
//using System;
//using System.Threading;
//using System.Linq;

//public class BankReportQuery : IMapFrom<BankReportQuery>, IRequest<ServiceResult>
//{
//    public int ReportType { get; set; }
//    public int Level { get; set; }
//    public int CompanyId { get; set; }
//    public int[]? YearIds { get; set; }
//    public int? VoucherStateId { get; set; }
//    public int[]? CodeVoucherGroupIds { get; set; }
//    public int? TransferId { get; set; }
//    public int[]? AccountHeadIds { get; set; }
//    public int[]? ReferenceGroupIds { get; set; }
//    public int[]? ReferenceIds { get; set; }
//    public int? ReferenceNo { get; set; } = 1;
//    public int? VoucherNoFrom { get; set; }
//    public int? VoucherNoTo { get; set; }
//    public DateTime? VoucherDateFrom { get; set; }
//    public DateTime? VoucherDateTo { get; set; }
//    public long? DebitFrom { get; set; }
//    public long? DebitTo { get; set; }
//    public long? CreditFrom { get; set; }
//    public long? CreditTo { get; set; }
//    public int? DocumentIdFrom { get; set; }
//    public int? DocumentIdTo { get; set; }
//    public int? CurrencyTypeBaseId { get; set; }

//    public string? VoucherDescription { get; set; }
//    public string? VoucherRowDescription { get; set; }
//    public bool Remain { get; set; }
//    public string? ReportTitle { get; set; }
//    public SsrsUtil.ReportFormat ReportFormat { get; set; } = SsrsUtil.ReportFormat.None;
//    public void Mapping(Profile profile)
//    {
//        profile.CreateMap<BankReportQuery, StpReportBalance6Input>()
//            .ForMember(x => x.YearIds, opt => opt.MapFrom(t => (t.YearIds != null && t.YearIds.Length != 0) ? JsonConvert.SerializeObject(t.YearIds) : null))
//            .ForMember(x => x.AccountHeadIds, opt => opt.MapFrom(t => (t.AccountHeadIds != null && t.AccountHeadIds.Length != 0) ? JsonConvert.SerializeObject(t.AccountHeadIds) : null))
//            .ForMember(x => x.ReferenceGroupIds, opt => opt.MapFrom(t => (t.ReferenceGroupIds != null && t.ReferenceGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceGroupIds) : null))
//            .ForMember(x => x.ReferenceIds, opt => opt.MapFrom(t => (t.ReferenceIds != null && t.ReferenceIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceIds) : null))
//            .ForMember(x => x.CodeVoucherGroupIds, opt => opt.MapFrom(t => (t.CodeVoucherGroupIds != null && t.CodeVoucherGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.CodeVoucherGroupIds) : null))
//            .IgnoreAllNonExisting();
//    }
//}

//public class BankReportQueryHandler : IRequestHandler<BankReportQuery, ServiceResult>
//{
//    private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
//    private readonly IApplicationUser _applicationUser;
//    private readonly IMapper _mapper;
//    private readonly IAccountingUnitOfWork _context;

//    public BankReportQueryHandler(IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures, IApplicationUser applicationUserAccessor, IMapper mapper, IAccountingUnitOfWork context)
//    {
//        _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
//        _currentUserAccessor = currentUserAccessor;

//        _mapper = mapper;
//        _context = context;
//    }

//    public async Task<ServiceResult> Handle(BankReportQuery request, CancellationToken cancellationToken)
//    {
//        var year = await _context.Years.Where(a => a.Id == _currentUserAccessor.GetYearId()).FirstOrDefaultAsync(cancellationToken);
//        var bank = await _context.AccountReferencesGroups.Where(a => a.Code == BankReportConst.BankCode).Select(a => a.Id).FirstOrDefaultAsync(cancellationToken);
//        var Balancebank = await _context.AccountHeads.Where(a => a.Code == BankReportConst.BalancebankCode).Select(a => a.Id).FirstOrDefaultAsync(cancellationToken);
//        var Facilities = await _context.AccountHeads.Where(a => a.Code == BankReportConst.FacilitieCode).Select(a => a.Id).FirstOrDefaultAsync(cancellationToken);

//        request.CompanyId = _currentUserAccessor.GetCompanyId();
//        request.YearIds = new int[1];
//        request.YearIds[0] = _currentUserAccessor.GetYearId();
//        request.ReferenceGroupIds = new int[1];
//        request.ReferenceGroupIds[0] = bank;
//        request.Level = 4;
//        request.CurrencyTypeBaseId = 28306;
//        request.AccountHeadIds = new int[1];
//        request.AccountHeadIds[0] = Balancebank;
//        /////////// bank
//        var bankinput = _mapper.Map<StpReportBalance6Input>(request);
//        bankinput.VoucherDateFrom = year.FirstDate;
//        bankinput.VoucherDateTo = year.LastDate;
//        var bankresults = await _accountingUnitOfWorkProcedures
//            .StpReportBalance6Async(bankinput,
//                cancellationToken: cancellationToken
//            );
//        bankresults = bankresults.ApplyPermission<StpReportBalance6Result, VouchersDetail>
//                               (_context, _currentUserAccessor, false, false);

//        /////////// Balancebank ///chek

//        var Balancebankinput = _mapper.Map<StpReportBalance6Input>(request);
//        if (Balancebankinput.VoucherDateTo.HasValue)
//            Balancebankinput.VoucherDateTo = Balancebankinput.VoucherDateTo.Value.AddDays(1).AddSeconds(-1);
//        var Balancebankresults = await _accountingUnitOfWorkProcedures
//            .StpReportBalance6Async(Balancebankinput,
//                cancellationToken: cancellationToken
//            );
//        Balancebankresults = Balancebankresults.ApplyPermission<StpReportBalance6Result, VouchersDetail>
//                               (_context, _currentUserAccessor, false, false);

//        /////////// Facilities

//        request.AccountHeadIds = new int[1];
//        request.AccountHeadIds[0] = Facilities;
//        var Facilitiesinput = _mapper.Map<StpReportBalance6Input>(request);
//        if (Facilitiesinput.VoucherDateTo.HasValue)
//            Facilitiesinput.VoucherDateTo = Facilitiesinput.VoucherDateTo.Value.AddDays(1).AddSeconds(-1);
//        var Facilitiesresults = await _accountingUnitOfWorkProcedures
//            .StpReportBalance6Async(Facilitiesinput,
//                cancellationToken: cancellationToken
//            );
//        Facilitiesresults = Facilitiesresults.ApplyPermission<StpReportBalance6Result, VouchersDetail>
//                               (_context, _currentUserAccessor, false, false);

//        var results = (from br in bankresults
//                       from f in Facilitiesresults.Where(a => a.Code == br.Code).DefaultIfEmpty()
//                       from bb in Balancebankresults.Where(a => a.Code == br.Code).DefaultIfEmpty()
//                       select new
//                       {
//                           br.Title,
//                           bankremain = br.Debit,
//                           Balancebankremain = bb != null ? bb.Credit : 0,
//                           Facilitiesremain = f != null ? f.RemainDebit - f.RemainCredit : 0,
//                           remain = bb != null ? br.Debit - bb.Credit : br.Debit
//                       }).ToList();

//        return ServiceResult.Success(results);
//    }
//}