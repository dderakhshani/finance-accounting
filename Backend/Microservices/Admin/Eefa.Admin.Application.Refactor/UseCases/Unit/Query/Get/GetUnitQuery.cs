using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetUnitQuery : IRequest<ServiceResult<UnitModel>>
{
    public int Id { get; set; }
}

public class GetUnitQueryHandler : IRequestHandler<GetUnitQuery, ServiceResult<UnitModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetUnitQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<UnitModel>> Handle(GetUnitQuery request, CancellationToken cancellationToken)
    {
        Specification<Unit> specification = new Specification<Unit>();
        specification.ApplicationConditions.Add(x => x.Id == request.Id);
        specification.Includes = x => x.Include(y => y.UnitPositions);

        return ServiceResult.Success(await _unitOfWork.Units
                                     .GetProjectedAsync<UnitModel>(specification));
    }
}