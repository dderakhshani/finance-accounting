using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllBaseValueQuery : Specification<BaseValue>, IRequest<ServiceResult<PaginatedList<BaseValueModel>>>
{
}

public class GetAllBaseValueQueryHandler : IRequestHandler<GetAllBaseValueQuery, ServiceResult<PaginatedList<BaseValueModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllBaseValueQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<BaseValueModel>>> Handle(GetAllBaseValueQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.BaseValues
                    .GetPaginatedProjectedListAsync<BaseValueModel>(request));
    }
}