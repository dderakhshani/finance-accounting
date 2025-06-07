using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class GetAllAccountReferenceByGroupIdQuery : Specification<AccountReference>, IRequest<ServiceResult<PaginatedList<AccountReferenceModel>>>
{
    public int GroupId { get; set; }
}

public class GetAllAccountReferenceByGroupIdQueryHandler : IRequestHandler<GetAllAccountReferenceByGroupIdQuery, ServiceResult<PaginatedList<AccountReferenceModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllAccountReferenceByGroupIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<AccountReferenceModel>>> Handle(GetAllAccountReferenceByGroupIdQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.AccountReferences
                                    .GetPaginatedProjectedListAsync<AccountReferenceModel>(request));
        //var entitis = (from r in _repository
        //            .GetAll<AccountReference>(x =>
        //                x.ConditionExpression(t => t.IsActive))
        //               join rg in _unitOfWorkGetQuery<AccountReferencesRelReferencesGroup>()
        //                   on r.Id equals rg.ReferenceId
        //               where rg.ReferenceGroupId == request.GroupId
        //               select r
        //    ).ProjectTo<AccountReferenceModel>(_mapper.ConfigurationProvider)
        //    .WhereQueryMaker(request.Conditions)
        //    .OrderByMultipleColumns(request.OrderByProperty);

        //return ServiceResult.Success(new PagedList()
        //{
        //    Data = await entitis
        //        .Paginate(request.Paginator())
        //        .ToListAsync(cancellationToken),
        //    TotalCount = request.PageIndex <= 1
        //        ? await entitis
        //            .CountAsync(cancellationToken)
        //        : 0
        //});
    }
}