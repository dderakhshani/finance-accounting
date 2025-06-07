//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoMapper;
//using MediatR;

//public class StpReportReference2AccountHeadQuery : IMapFrom<StpReportReference2AccountHeadQuery>, IRequest<ServiceResult>
//{
//    public int ReportType { get; set; }
//    public int Level { get; set; }
//    public int CompanyId { get; set; }
//    public int[]? YearIds { get; set; }
//    public int? VoucherStateId { get; set; }
//    public int? CodeVoucherGroupId { get; set; }
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
//    public string? VoucherDescription { get; set; }
//    public string? VoucherRowDescription { get; set; }
//    public bool Remain { get; set; }
//    public string? ReportTitle { get; set; }
//    public SsrsUtil.ReportFormat ReportFormat { get; set; } = SsrsUtil.ReportFormat.None;

//    public void Mapping(Profile profile)
//    {
//        profile.CreateMap<StpReportReference2AccountHeadQuery, StpReportReference2AccountHeadInput>()
//            .IgnoreAllNonExisting();
//    }
//}
//public class StpReportReference2AccountHeadQueryHandler : IRequestHandler<StpReportReference2AccountHeadQuery, ServiceResult>
//{
//    private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
//    private readonly IApplicationUser _applicationUser;
//    private readonly IMapper _mapper;
//    public StpReportReference2AccountHeadQueryHandler(IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures, IApplicationUser applicationUserAccessor, IMapper mapper)
//    {
//        _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
//        _currentUserAccessor = currentUserAccessor;
//        _mapper = mapper;
//    }

//    public async Task<ServiceResult> Handle(StpReportReference2AccountHeadQuery request, CancellationToken cancellationToken)
//    {
//        try
//        {
//            var input = _mapper.Map<StpReportReference2AccountHeadInput>(request);
//            input.CompanyId = _currentUserAccessor.GetCompanyId();
//            if (input.VoucherDateTo.HasValue)
//                input.VoucherDateTo = input.VoucherDateTo.Value.AddDays(1).AddSeconds(-1);
//            return ServiceResult.Success(await _accountingUnitOfWorkProcedures
//            .StpReportReference2AccountHeadAsync(input,
//                cancellationToken: cancellationToken
//            ));
//        }
//        catch
//        {
//            return ServiceResult.Success(null);
//        }
//    }
//}