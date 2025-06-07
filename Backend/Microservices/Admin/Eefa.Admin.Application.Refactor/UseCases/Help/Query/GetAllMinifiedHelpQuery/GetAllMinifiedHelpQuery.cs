using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllMinifiedHelpQuery : Specification<Help>, IRequest<ServiceResult<PaginatedList<MinifiedHelpModel>>>
{
}

public class GetAllMinifiedHelpQueryHandler : IRequestHandler<GetAllMinifiedHelpQuery, ServiceResult<PaginatedList<MinifiedHelpModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllMinifiedHelpQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<MinifiedHelpModel>>> Handle(GetAllMinifiedHelpQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Helps
            .GetPaginatedProjectedListAsync<MinifiedHelpModel>(request));
    }
}