using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetEmployeeQuery : IRequest<ServiceResult<EmployeeModel>>
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int PersonId { get; set; }
}

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, ServiceResult<EmployeeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetEmployeeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<EmployeeModel>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            return ServiceResult.Success(await _unitOfWork.Employees
                                .GetProjectedByIdAsync<EmployeeModel>(request.Id));
        }
        else
        {
            return ServiceResult.Success(await _unitOfWork.Employees
                   .GetProjectedAsync<EmployeeModel>(x => x.PersonId == request.PersonId ||
                                                          x.Id == request.EmployeeId));
        }
    }
}