using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class GetAllHelpQuery : Specification<Help>, IRequest<ServiceResult<PaginatedList<HelpModel>>>
{
}

public class GetAllHelpQueryHandler : IRequestHandler<GetAllHelpQuery, ServiceResult<PaginatedList<HelpModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllHelpQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<HelpModel>>> Handle(GetAllHelpQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Helps
            .GetPaginatedProjectedListAsync<HelpModel>(request));
    }
}