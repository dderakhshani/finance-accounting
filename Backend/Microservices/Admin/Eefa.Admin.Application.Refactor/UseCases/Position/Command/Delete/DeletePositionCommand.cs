using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class DeletePositionCommand : IRequest<ServiceResult<PositionModel>>
{
    public int Id { get; set; }
}

public class DeletePositionCommandHandler : IRequestHandler<DeletePositionCommand, ServiceResult<PositionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public DeletePositionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PositionModel>> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
    {
        Position entity = await _unitOfWork.Positions.GetByIdAsync(request.Id);

        _unitOfWork.Positions.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PositionModel>(entity));
    }
}