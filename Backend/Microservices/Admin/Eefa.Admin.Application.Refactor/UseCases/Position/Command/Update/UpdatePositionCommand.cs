using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class UpdatePositionCommand : IRequest<ServiceResult<PositionModel>>, IMapFrom<Position>
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Title { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdatePositionCommand, Position>()
            .IgnoreAllNonExisting();
    }
}

public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, ServiceResult<PositionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePositionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PositionModel>> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
    {
        Position entity = await _unitOfWork.Positions.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.Positions.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PositionModel>(entity));
    }
}