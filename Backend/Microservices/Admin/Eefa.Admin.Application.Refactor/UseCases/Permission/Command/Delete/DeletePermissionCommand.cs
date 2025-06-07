using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class DeletePermissionCommand : IRequest<ServiceResult<PermissionModel>>
{
    public int Id { get; set; }
}

public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, ServiceResult<PermissionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public DeletePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PermissionModel>> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
    {
        Permission entity = await _unitOfWork.Permissions.GetByIdAsync(request.Id);

        _unitOfWork.Permissions.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PermissionModel>(entity));
    }
}