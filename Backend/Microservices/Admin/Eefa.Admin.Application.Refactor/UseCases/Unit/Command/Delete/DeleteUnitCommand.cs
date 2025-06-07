using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class DeleteUnitCommand : IRequest<ServiceResult<UnitModel>>
{
    public int Id { get; set; }
}

public class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand, ServiceResult<UnitModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteUnitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<UnitModel>> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
    {
        Unit entity = await _unitOfWork.Units.GetByIdAsync(request.Id,
            x => x.Include(y => y.UnitPositions));

        foreach (var unitPosition in entity.UnitPositions)
        {
            _unitOfWork.UnitPositions.Delete(unitPosition);
        }
        _unitOfWork.Units.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<UnitModel>(entity));
    }
}