//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoMapper;
//using MediatR;
//using Newtonsoft.Json;

//public class GetReportBalance6Query : IMapFrom<GetReportBalance6Query>, IRequest<ServiceResult>
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
//    public string? VoucherDescription { get; set; }
//    public string? VoucherRowDescription { get; set; }
//    public bool Remain { get; set; }
//    public string? ReportTitle { get; set; }
//    public SsrsUtil.ReportFormat ReportFormat { get; set; } = SsrsUtil.ReportFormat.None;

//    public void Mapping(Profile profile)
//    {
//        profile.CreateMap<GetReportBalance6Query, StpReportBalance6Input>()
//            .ForMember(x => x.YearIds, opt => opt.MapFrom(t => (t.YearIds != null && t.YearIds.Length != 0) ? JsonConvert.SerializeObject(t.YearIds) : null))
//            .ForMember(x => x.AccountHeadIds, opt => opt.MapFrom(t => (t.AccountHeadIds != null && t.AccountHeadIds.Length != 0) ? JsonConvert.SerializeObject(t.AccountHeadIds) : null))
//            .ForMember(x => x.ReferenceGroupIds, opt => opt.MapFrom(t => (t.ReferenceGroupIds != null && t.ReferenceGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceGroupIds) : null))
//            .ForMember(x => x.ReferenceIds, opt => opt.MapFrom(t => (t.ReferenceIds != null && t.ReferenceIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceIds) : null))
//            .ForMember(x => x.CodeVoucherGroupIds, opt => opt.MapFrom(t => (t.CodeVoucherGroupIds != null && t.CodeVoucherGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.CodeVoucherGroupIds) : null))
//            .IgnoreAllNonExisting();
//    }
//}

//public class GetReportBalance6QueryHandler : IRequestHandler<GetReportBalance6Query, ServiceResult>
//{
//    private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
//    private readonly IApplicationUser _applicationUser;
//    private readonly IMapper _mapper;
//    private readonly IUnitOfWork _unitOfWork;
//    public GetReportBalance6QueryHandler(IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures, IApplicationUser applicationUserAccessor, IMapper mapper, IUnitOfWork unitOfWork)
//    {
//        _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
//        _currentUserAccessor = currentUserAccessor;
//        _mapper = mapper;
//        _unitOfWork= unitOfWork;
//    }

//    public async Task<ServiceResult> Handle(GetReportBalance6Query request, CancellationToken cancellationToken)
//    {
//        try
//        {
//            var input = _mapper.Map<StpReportBalance6Input>(request);
//            input.CompanyId = _currentUserAccessor.GetCompanyId();
//            if (input.VoucherDateTo.HasValue)
//                input.VoucherDateTo = input.VoucherDateTo.Value.AddDays(1).AddSeconds(-1);
//            return ServiceResult.Success(await _accountingUnitOfWorkProcedures
//                .StpReportBalance6Async(input,
//                    cancellationToken: cancellationToken
//                ));
//        }
//        catch
//        {
//            return ServiceResult.Success(null);
//        }
//    }
//}