using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using MediatR;

public class GetAllUserByRoleQuery : Specification<User>, IRequest<ServiceResult<PaginatedList<UserModel>>>
{
    public int RoleId { get; set; }
}

public class GetAllUserByRoleQueryHandler : IRequestHandler<GetAllUserByRoleQuery, ServiceResult<PaginatedList<UserModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllUserByRoleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<UserModel>>> Handle(GetAllUserByRoleQuery request, CancellationToken cancellationToken)
    {
        request.Where(x => x.UserRoleUsers.Any(y => y.RoleId == request.RoleId));
        var users = await _unitOfWork.Users.GetPaginatedProjectedListAsync<UserModel>(request);
        return ServiceResult.Success(users);
    }
}