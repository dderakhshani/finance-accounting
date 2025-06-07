using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateUnitCommand : IRequest<ServiceResult<UnitModel>>, IMapFrom<CreateUnitCommand>
{
    public string Title { get; set; } = default!;
    public int? ParentId { get; set; }
    public int BranchId { get; set; }
    public IList<int> PositionIds { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateUnitCommand, Unit>()
            .IgnoreAllNonExisting();
    }
}

public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, ServiceResult<UnitModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUnitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<UnitModel>> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        Unit entity = _mapper.Map<Unit>(request);

        _unitOfWork.Units.Add(entity);
        foreach (var possition in request.PositionIds)
        {
            _unitOfWork.UnitPositions.Add(new UnitPosition()
            { PositionId = possition, Unit = entity });
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<UnitModel>(entity));
    }
}