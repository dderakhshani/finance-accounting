using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllBaseValueTypeQuery : Specification<BaseValueType>, IRequest<ServiceResult<PaginatedList<BaseValueTypeModel>>>
{
}

public class GetAllBaseValueTypeQueryHandler : IRequestHandler<GetAllBaseValueTypeQuery, ServiceResult<PaginatedList<BaseValueTypeModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllBaseValueTypeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<BaseValueTypeModel>>> Handle(GetAllBaseValueTypeQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.BaseValueTyps
                            .GetPaginatedProjectedListAsync<BaseValueTypeModel>(request));
    }
}