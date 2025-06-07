using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetRoleQuery : IRequest<ServiceResult<RoleModel>>
{
    public int Id { get; set; }
}

public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, ServiceResult<RoleModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetRoleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<RoleModel>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        Specification<Role> specification = new Specification<Role>();
        specification.ApplicationConditions.Add(x => x.Id == request.Id);
        specification.Includes = x => x.Include(y => y.RolePermissionRoles);

        return ServiceResult.Success(await _unitOfWork.Roles
                            .GetProjectedAsync<RoleModel>(specification));
    }
}