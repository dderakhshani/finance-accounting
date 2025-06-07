using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetBaseValueQuery : IRequest<ServiceResult<BaseValueModel>>
{
    public int Id { get; set; }
}

public class GetBaseValueQueryHandler : IRequestHandler<GetBaseValueQuery, ServiceResult<BaseValueModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetBaseValueQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<BaseValueModel>> Handle(GetBaseValueQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.BaseValues
                        .GetProjectedByIdAsync<BaseValueModel>(request.Id));
    }
}