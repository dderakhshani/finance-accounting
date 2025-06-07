//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoMapper;
//using MediatR;
//using Newtonsoft.Json;

//public class GetAccountReferenceBookQuery : IMapFrom<GetAccountReferenceBookQuery>, IRequest<ServiceResult>
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

//    public void Mapping(Profile profile)
//    {
//        profile.CreateMap<GetAccountReferenceBookQuery, StpAccountReferenceBookInput>()
//            .ForMember(x => x.YearIds, opt => opt.MapFrom(t => (t.YearIds != null && t.YearIds.Length != 0) ? JsonConvert.SerializeObject(t.YearIds) : null))
//            .ForMember(x => x.AccountHeadIds, opt => opt.MapFrom(t => (t.AccountHeadIds != null && t.AccountHeadIds.Length != 0) ? JsonConvert.SerializeObject(t.AccountHeadIds) : null))
//            .ForMember(x => x.ReferenceGroupIds, opt => opt.MapFrom(t => (t.ReferenceGroupIds != null && t.ReferenceGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceGroupIds) : null))
//            .ForMember(x => x.ReferenceIds, opt => opt.MapFrom(t => (t.ReferenceIds != null && t.ReferenceIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceIds) : null))
//            .IgnoreAllNonExisting();
//    }
//}

//public class GetAccountReferenceBookQueryHandler : IRequestHandler<GetAccountReferenceBookQuery, ServiceResult>
//{
//    private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
//    private readonly IApplicationUser _applicationUser;

//    private readonly IMapper _mapper;
//    public GetAccountReferenceBookQueryHandler(IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures, IApplicationUser applicationUserAccessor, IMapper mapper)
//    {
//        _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
//        _currentUserAccessor = currentUserAccessor;

//        _mapper = mapper;
//    }

//    public async Task<ServiceResult> Handle(GetAccountReferenceBookQuery request, CancellationToken cancellationToken)
//    {
//        try
//        {
//            if (request.VoucherDateTo.HasValue)
//                request.VoucherDateTo = request.VoucherDateTo.Value.AddDays(1).AddSeconds(-1);

//            var res = await _accountingUnitOfWorkProcedures
//                .StpAccountReferenceBook(_mapper.Map<StpAccountReferenceBookInput>(request),
//                    cancellationToken: cancellationToken
//                );

//            var totalDebit = res.Aggregate(new long(), (r, c) => r + c.Debit ?? 0);
//            var totalCredit = res.Aggregate(new long(), (r, c) => r + c.Credit ?? 0);

//            return ServiceResult.Success(new { totalCredit = totalCredit, totalDebit = totalDebit, remain = totalCredit - totalDebit, result = res });
//        }
//        catch (Exception e)
//        {
//            return ServiceResult.Success(null);
//        }
//    }
//}