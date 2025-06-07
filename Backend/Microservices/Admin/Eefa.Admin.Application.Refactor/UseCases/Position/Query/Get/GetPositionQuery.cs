using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetPositionQuery : IRequest<ServiceResult<PositionModel>>
{
    public int Id { get; set; }
}

public class GetPositionQueryHandler : IRequestHandler<GetPositionQuery, ServiceResult<PositionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetPositionQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PositionModel>> Handle(GetPositionQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Positions
            .GetProjectedByIdAsync<PositionModel>(request.Id));
    }
}