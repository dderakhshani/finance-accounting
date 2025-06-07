using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateHelpCommand : IRequest<ServiceResult<MinifiedHelpModel>>, IMapFrom<CreateHelpCommand>
{
    public int MenuItemId { get; set; }
    public string Contents { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateHelpCommand, Help>()
            .ForMember(x => x.MenuId, opt => opt.MapFrom(x => x.MenuItemId));
    }
}

public class CreateHelpCommandHandler : IRequestHandler<CreateHelpCommand, ServiceResult<MinifiedHelpModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateHelpCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResult<MinifiedHelpModel>> Handle(CreateHelpCommand request, CancellationToken cancellationToken)
    {
        Help entity = _mapper.Map<Help>(request);

        _unitOfWork.Helps.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<MinifiedHelpModel>(entity));
    }
}