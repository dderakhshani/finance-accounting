using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetUserQuery : IRequest<ServiceResult<UserModel>>
{
    public int Id { get; set; }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ServiceResult<UserModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<UserModel>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Users
                            .GetProjectedByIdAsync<UserModel>(request.Id));
    }
}