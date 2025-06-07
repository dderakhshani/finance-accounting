using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetPermissionQuery : IRequest<ServiceResult<PermissionModel>>
{
    public int Id { get; set; }
}

public class GetPermissionQueryHandler : IRequestHandler<GetPermissionQuery, ServiceResult<PermissionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetPermissionQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PermissionModel>> Handle(GetPermissionQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Permissions
                            .GetProjectedByIdAsync<PermissionModel>(request.Id));
    }
}