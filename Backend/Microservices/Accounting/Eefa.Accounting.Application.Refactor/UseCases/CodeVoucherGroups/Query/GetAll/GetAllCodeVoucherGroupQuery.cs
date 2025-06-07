using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllCodeVoucherGroupQuery : Specification<CodeVoucherGroup>, IRequest<ServiceResult<PaginatedList<CodeVoucherGroupModel>>>
{
}

public class GetAllCodeVoucherGroupQueryHandler : IRequestHandler<GetAllCodeVoucherGroupQuery, ServiceResult<PaginatedList<CodeVoucherGroupModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    //private readonly IMapper _mapper;
    //private readonly IAccountingUnitOfWork _context;
    //private readonly IApplicationUser _applicationUser;

    public GetAllCodeVoucherGroupQueryHandler(IUnitOfWork unitOfWork/*, IMapper mapper, IAccountingUnitOfWork context, IApplicationUser applicationUserAccessor*/)
    {
        _unitOfWork = unitOfWork;
        //_mapper = mapper;
        //_context = context;
        //_currentUserAccessor = currentUserAccessor;
    }

    public async Task<ServiceResult<PaginatedList<CodeVoucherGroupModel>>> Handle(GetAllCodeVoucherGroupQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.CodeVoucherGroups
                            .GetPaginatedProjectedListAsync<CodeVoucherGroupModel>(request));

        //var entitis = _repository
        //    .GetAll<CodeVoucherGroup>().ApplyPermission<CodeVoucherGroup>(_context, _currentUserAccessor, false, false)
        //    .ProjectTo<CodeVoucherGroupModel>(_mapper.ConfigurationProvider)
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