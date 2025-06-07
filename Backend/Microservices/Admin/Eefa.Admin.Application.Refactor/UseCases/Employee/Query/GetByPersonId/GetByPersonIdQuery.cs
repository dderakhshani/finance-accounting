using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetByPersonIdQuery : IRequest<ServiceResult<EmployeeModel>>
{
    public int Id { get; set; }
}

public class GetByPersonIdQueryHandler : IRequestHandler<GetByPersonIdQuery, ServiceResult<EmployeeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetByPersonIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<EmployeeModel>> Handle(GetByPersonIdQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Employees
               .GetProjectedAsync<EmployeeModel>(x => x.PersonId == request.Id));
    }
}