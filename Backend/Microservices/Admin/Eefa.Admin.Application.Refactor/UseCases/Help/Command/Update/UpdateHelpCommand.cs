using AutoMapper;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

public class UpdateHelpCommand : IRequest<ServiceResult<MinifiedHelpModel>>, IMapFrom<Help>
{
    public int Id { get; set; }
    public int MenuItemId { get; set; }
    public string Contents { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateHelpCommand, Help>()
            .ForMember(x => x.MenuId, opt => opt.MapFrom(x => x.MenuItemId));
    }
}

public class UpdateHelpCommandHandler : IRequestHandler<UpdateHelpCommand, ServiceResult<MinifiedHelpModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateHelpCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<MinifiedHelpModel>> Handle(UpdateHelpCommand request, CancellationToken cancellationToken)
    {
        Help entity = await _unitOfWork.Helps.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.Helps.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<MinifiedHelpModel>(entity));
    }
}