using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetByBranchIdQuery : IRequest<ServiceResult<UnitModel>>
{
    public int BranchId { get; set; }
}

public class GetByBranchIdQueryHandler : IRequestHandler<GetByBranchIdQuery, ServiceResult<UnitModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetByBranchIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<UnitModel>> Handle(GetByBranchIdQuery request, CancellationToken cancellationToken)
    {
        Specification<Unit> specification = new Specification<Unit>();
        specification.ApplicationConditions.Add(x => x.BranchId == request.BranchId);
        specification.Includes = x => x.Include(y => y.UnitPositions);

        return ServiceResult.Success(await _unitOfWork.Units
                                     .GetProjectedAsync<UnitModel>(specification));
    }
}