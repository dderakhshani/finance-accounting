using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetHelpQuery : IRequest<ServiceResult<HelpModel>>
{
    public int Id { get; set; }
}

public class GetHelpQueryHandler : IRequestHandler<GetHelpQuery, ServiceResult<HelpModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetHelpQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<HelpModel>> Handle(GetHelpQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Helps
                            .GetProjectedByIdAsync<HelpModel>(request.Id));
    }
}