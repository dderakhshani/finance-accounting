using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllMenuItemQuery : Specification<MenuItem>, IRequest<ServiceResult<PaginatedList<MenuItemModel>>>
{
}

public class GetAllMenuItemQueryHandler : IRequestHandler<GetAllMenuItemQuery, ServiceResult<PaginatedList<MenuItemModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllMenuItemQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<MenuItemModel>>> Handle(GetAllMenuItemQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.MenuItems
                            .GetPaginatedProjectedListAsync<MenuItemModel>(request));
    }
}