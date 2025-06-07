using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllCorrectionRequestsQuery : Specification<CorrectionRequest>, IRequest<ServiceResult<PaginatedList<CorrectionRequestModel>>>
{
}

public class GetAllCorrectionRequestsQueryHandler : IRequestHandler<GetAllCorrectionRequestsQuery, ServiceResult<PaginatedList<CorrectionRequestModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllCorrectionRequestsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<CorrectionRequestModel>>> Handle(GetAllCorrectionRequestsQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.CorrectionRequests
                            .GetPaginatedProjectedListAsync<CorrectionRequestModel>(request));
    }
}