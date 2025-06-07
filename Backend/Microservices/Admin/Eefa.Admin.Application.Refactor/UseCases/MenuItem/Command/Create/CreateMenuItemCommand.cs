using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateMenuItemCommand : IRequest<ServiceResult<MenuItemModel>>, IMapFrom<CreateMenuItemCommand>
{
    public int? ParentId { get; set; }
    public int? PermissionId { get; set; }
    public int? OrderIndex { get; set; }
    public string Title { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string? HelpUrl { get; set; }
    public string? FormUrl { get; set; }
    public string? PageCaption { get; set; }
    public bool IsActive { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateMenuItemCommand, MenuItem>()
            .IgnoreAllNonExisting();
    }
}

public class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, ServiceResult<MenuItemModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMenuItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<ServiceResult<MenuItemModel>> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
    {
        MenuItem entity = _mapper.Map<MenuItem>(request);

        _unitOfWork.MenuItems.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<MenuItemModel>(entity));
    }
}