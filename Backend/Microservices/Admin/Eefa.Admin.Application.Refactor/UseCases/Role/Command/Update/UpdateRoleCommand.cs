using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateRoleCommand : IRequest<ServiceResult<RoleModel>>, IMapFrom<Role>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? UniqueName { get; set; }
    public string? Description { get; set; }
    public int? ParentId { get; set; }
    public IList<int> PermissionsId { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateRoleCommand, Role>()
            .IgnoreAllNonExisting();
    }
}


public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ServiceResult<RoleModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<RoleModel>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        Role entity = await _unitOfWork.Roles.GetByIdAsync(request.Id,
                            x => x.Include(y => y.RolePermissionRoles));

        entity.ParentId = request.ParentId;
        entity.UniqueName = request.UniqueName;
        entity.Title = request.Title;
        entity.Description = request.Description;

        foreach (var removedPermission in entity.RolePermissionRoles.Select(x => x.PermissionId).Except(request.PermissionsId))
        {
            var deletingEntity = await _unitOfWork.RolePermissions
                .GetAsync(x => x.PermissionId == removedPermission && x.RoleId == entity.Id);

            _unitOfWork.RolePermissions.Delete(deletingEntity);
        }

        foreach (var addedPermission in request.PermissionsId.Except(entity.RolePermissionRoles.Select(x => x.PermissionId)))
        {
            if (await _unitOfWork.RolePermissions.ExistsAsync(x =>
                x.RoleId == entity.Id && x.PermissionId == addedPermission &&
                x.IsDeleted != true))
            {
                continue;
            }

            _unitOfWork.RolePermissions.Add(new RolePermission()
            {
                RoleId = entity.Id,
                PermissionId = addedPermission
            });
        }

        _unitOfWork.Roles.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<RoleModel>(entity));
    }
}