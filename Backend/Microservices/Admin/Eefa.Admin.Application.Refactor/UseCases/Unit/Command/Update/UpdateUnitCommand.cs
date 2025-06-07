using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateUnitCommand : IRequest<ServiceResult<UnitModel>>, IMapFrom<Unit>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public int? ParentId { get; set; }
    public int BranchId { get; set; }
    public IList<int> PositionIds { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUnitCommand, Unit>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, ServiceResult<UnitModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUnitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<UnitModel>> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
    {
        Unit entity = await _unitOfWork.Units.GetByIdAsync(request.Id,
            x => x.Include(y => y.UnitPositions));

        entity.ParentId = request.ParentId;
        entity.Title = request.Title;
        entity.BranchId = request.BranchId;

        _unitOfWork.Units.Update(entity);

        foreach (var removedPositions in entity.UnitPositions.Select(x => x.PositionId)
            .Except(request.PositionIds))
        {
            UnitPosition deletingEntity = await _unitOfWork.UnitPositions
                                .GetAsync(x => x.PositionId == removedPositions &&
                                x.UnitId == entity.Id);

            _unitOfWork.UnitPositions.Delete(deletingEntity);
        }

        foreach (var addedPositions in request.PositionIds.Except(entity.UnitPositions.Select(x => x.PositionId)))
        {
            if (await _unitOfWork.UnitPositions.ExistsAsync(x =>
                x.UnitId == entity.Id && x.PositionId == addedPositions &&
                x.IsDeleted != true))
            {
                continue;
            }
            _unitOfWork.UnitPositions.Add(new UnitPosition()
            {
                PositionId = addedPositions,
                UnitId = entity.Id
            });
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<UnitModel>(entity));
    }
}