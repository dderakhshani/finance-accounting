using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateRoleCommand : IRequest<ServiceResult<RoleModel>>, IMapFrom<CreateRoleCommand>
{
    public string Title { get; set; } = default!;
    public string? UniqueName { get; set; }
    public string? Description { get; set; }
    public int? ParentId { get; set; }
    public IList<int> PermissionsId { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateRoleCommand, Role>()
            .IgnoreAllNonExisting();
    }
}

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ServiceResult<RoleModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<RoleModel>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        Role role = _mapper.Map<Role>(request);

        _unitOfWork.Roles.Add(role);
        foreach (var permission in request.PermissionsId)
        {
            _unitOfWork.RolePermissions.Add(new RolePermission() { PermissionId = permission, Role = role });
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<RoleModel>(role));
    }
}