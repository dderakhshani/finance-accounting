using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdateMenuItemCommand : IRequest<ServiceResult<MenuItemModel>>, IMapFrom<MenuItem>
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public int? PermissionId { get; set; }
    public int OrderIndex { get; set; }
    public string Title { get; set; } = default!;
    public string? HelpUrl { get; set; }
    public string? ImageUrl { get; set; }
    public string? FormUrl { get; set; }
    public string? PageCaption { get; set; }
    public bool IsActive { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateMenuItemCommand, MenuItem>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateMenuItemCommandHandler : IRequestHandler<UpdateMenuItemCommand, ServiceResult<MenuItemModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMenuItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<MenuItemModel>> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
    {
        MenuItem entity = await _unitOfWork.MenuItems.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.MenuItems.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<MenuItemModel>(entity));
    }
}