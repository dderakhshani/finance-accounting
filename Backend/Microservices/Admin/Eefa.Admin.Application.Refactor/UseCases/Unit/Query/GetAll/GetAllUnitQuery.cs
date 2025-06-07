using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllUnitQuery : Specification<Unit>, IRequest<ServiceResult<PaginatedList<UnitModel>>>
{
}

public class GetAllUnitQueryHandler : IRequestHandler<GetAllUnitQuery, ServiceResult<PaginatedList<UnitModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllUnitQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<UnitModel>>> Handle(GetAllUnitQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Units
                            .GetPaginatedProjectedListAsync<UnitModel>(request));
    }
}