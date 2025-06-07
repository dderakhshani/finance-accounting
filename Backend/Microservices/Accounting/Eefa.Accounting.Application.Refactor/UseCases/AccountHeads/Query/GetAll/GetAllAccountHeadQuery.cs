using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllAccountHeadQuery : Specification<AccountHead>, 
    IRequest<ServiceResult<PaginatedList<AccountHeadModel>>>
{
}

public class GetAllAccountHeadQueryHandler : IRequestHandler<GetAllAccountHeadQuery, ServiceResult<PaginatedList<AccountHeadModel>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAccountHeadQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<AccountHeadModel>>> Handle(GetAllAccountHeadQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork
            .AccountHeads.GetPaginatedProjectedListAsync<AccountHeadModel>(request));
    }
}