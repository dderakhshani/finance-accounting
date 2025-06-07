using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllRoleQuery : Specification<Role>, IRequest<ServiceResult<PaginatedList<RoleModel>>>
{
}

public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, ServiceResult<PaginatedList<RoleModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllRoleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<RoleModel>>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Roles
                            .GetPaginatedProjectedListAsync<RoleModel>(request));
    }
}