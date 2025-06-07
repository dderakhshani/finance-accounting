using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllBranchQuery : Specification<Branch>, IRequest<ServiceResult<PaginatedList<BranchModel>>>
{
}

public class GetAllBranchQueryHandler : IRequestHandler<GetAllBranchQuery, ServiceResult<PaginatedList<BranchModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllBranchQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<BranchModel>>> Handle(GetAllBranchQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Branchs
                            .GetPaginatedProjectedListAsync<BranchModel>(request));
    }
}