using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetMenuItemQuery : IRequest<ServiceResult<MenuItemModel>>
{
    public int Id { get; set; }
}

public class GetMenuItemQueryHandler : IRequestHandler<GetMenuItemQuery, ServiceResult<MenuItemModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetMenuItemQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<MenuItemModel>> Handle(GetMenuItemQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.MenuItems
                            .GetProjectedByIdAsync<MenuItemModel>(request.Id));
    }
}