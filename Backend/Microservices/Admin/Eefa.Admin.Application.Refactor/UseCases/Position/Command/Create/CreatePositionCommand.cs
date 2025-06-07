using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreatePositionCommand : IRequest<ServiceResult<PositionModel>>, IMapFrom<CreatePositionCommand>
{
    public int? ParentId { get; set; }
    public string Title { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreatePositionCommand, Position>()
            .IgnoreAllNonExisting();
    }
}

public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, ServiceResult<PositionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePositionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PositionModel>> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        Position entity = _mapper.Map<Position>(request);

        _unitOfWork.Positions.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PositionModel>(entity));
    }
}