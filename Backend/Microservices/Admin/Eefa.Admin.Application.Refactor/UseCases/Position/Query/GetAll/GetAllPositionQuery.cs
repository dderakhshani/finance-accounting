using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllPositionQuery : Specification<Position>, IRequest<ServiceResult<PaginatedList<PositionModel>>>
{
}

public class GetAllPositionQueryHandler : IRequestHandler<GetAllPositionQuery, ServiceResult<PaginatedList<PositionModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllPositionQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<PositionModel>>> Handle(GetAllPositionQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Positions
                            .GetPaginatedProjectedListAsync<PositionModel>(request));
    }
}