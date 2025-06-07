using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteMenuItemCommand : IRequest<ServiceResult<MenuItemModel>>
{
    public int Id { get; set; }
}

public class DeleteMenuItemCommandHandler : IRequestHandler<DeleteMenuItemCommand, ServiceResult<MenuItemModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteMenuItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<MenuItemModel>> Handle(DeleteMenuItemCommand request, CancellationToken cancellationToken)
    {
        MenuItem entity = await _unitOfWork.MenuItems.GetByIdAsync(request.Id);

        _unitOfWork.MenuItems.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<MenuItemModel>(entity));
    }
}