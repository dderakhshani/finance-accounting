using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class DeleteUserCommand : IRequest<ServiceResult<UserModel>>
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ServiceResult<UserModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<UserModel>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User entity = await _unitOfWork.Users.GetByIdAsync(request.Id);

        _unitOfWork.Users.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<UserModel>(entity));
    }
}