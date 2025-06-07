//using MediatR;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoMapper;

//public class AccountReferenceCodeBookRemainQuery : IMapFrom<AccountReferenceCodeBookRemainQuery>, IRequest<ServiceResult>
//{
//    public string AccountHeadCode { get; set; }
//    public int AccountReferenceId { get; set; }
//    public string? ReportTitle { get; set; }

//    public void Mapping(Profile profile)
//    {
//        profile.CreateMap<AccountReferenceCodeBookRemainQuery, StpAccountReferenceBookInput>()
//            .IgnoreAllNonExisting();
//    }
//}

//public class AccountReferenceCodeBookRemainQueryHandler : IRequestHandler<AccountReferenceCodeBookRemainQuery, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    public AccountReferenceCodeBookRemainQueryHandler(IUnitOfWork _unitOfWork)
//    {
//        this._repository = _repository;
//    }

//    public async Task<ServiceResult> Handle(AccountReferenceCodeBookRemainQuery request, CancellationToken cancellationToken)
//    {
//        var currentYear = await _unitOfWorkFind<Year>(x => x.ConditionExpression(x => x.IsCurrentYear == true)).FirstOrDefaultAsync();

//        var accountHead = await _unitOfWorkFind<AccountHead>(x => x.ConditionExpression(x => x.Code == request.AccountHeadCode)).FirstOrDefaultAsync();

//        var sum = await _unitOfWorkGetAll<VouchersDetail>(x => x.ConditionExpression(x => x.AccountHeadId == accountHead.Id && x.ReferenceId1 == request.AccountReferenceId)).SumAsync(x => x.Credit - x.Debit);

//        return ServiceResult.Success(sum);
//    }
//}