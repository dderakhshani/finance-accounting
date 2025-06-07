using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllPermissionQuery : Specification<Permission>, IRequest<ServiceResult<PaginatedList<PermissionModel>>>
{
}

public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, ServiceResult<PaginatedList<PermissionModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllPermissionQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<PermissionModel>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Permissions
                            .GetPaginatedProjectedListAsync<PermissionModel>(request));
    }
}