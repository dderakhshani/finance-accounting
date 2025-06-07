using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class DeleteRoleCommand : IRequest<ServiceResult<RoleModel>>
{
    public int Id { get; set; }
}

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, ServiceResult<RoleModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<RoleModel>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        Role entity = await _unitOfWork.Roles.GetByIdAsync(request.Id,
                            x => x.Include(y => y.RolePermissionRoles));

        foreach (var rolePermission in entity.RolePermissionRoles)
        {
            _unitOfWork.RolePermissions.Delete(rolePermission);
        }
        _unitOfWork.Roles.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<RoleModel>(entity));
    }
}