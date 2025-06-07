using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllByCategoryUniqueNameQuery : Specification<BaseValue>, IRequest<ServiceResult<PaginatedList<BaseValueModel>>>
{
    public string BaseValueTypeUniqueName { get; set; }
}

public class GetAllByCategoryUniqueNameQueryHandler : IRequestHandler<GetAllByCategoryUniqueNameQuery, ServiceResult<PaginatedList<BaseValueModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllByCategoryUniqueNameQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<BaseValueModel>>> Handle(GetAllByCategoryUniqueNameQuery request, CancellationToken cancellationToken)
    {
        request.ApplicationConditions.Add(x => x.BaseValueType.UniqueName == request.BaseValueTypeUniqueName);

        return ServiceResult.Success(await _unitOfWork.BaseValues
            .GetPaginatedProjectedListAsync<BaseValueModel>(request));
    }
}