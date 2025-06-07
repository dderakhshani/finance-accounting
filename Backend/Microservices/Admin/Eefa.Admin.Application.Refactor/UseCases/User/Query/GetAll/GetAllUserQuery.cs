using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllUserQuery : Specification<User>, IRequest<ServiceResult<PaginatedList<UserModel>>>
{
}

public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, ServiceResult<PaginatedList<UserModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<UserModel>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Users
                            .GetPaginatedProjectedListAsync<UserModel>(request));
    }
}