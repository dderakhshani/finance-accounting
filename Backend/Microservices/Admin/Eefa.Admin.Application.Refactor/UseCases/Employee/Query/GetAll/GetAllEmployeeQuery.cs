using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllEmployeeQuery : Specification<Employee>, IRequest<ServiceResult<PaginatedList<EmployeeModel>>>
{
}

public class GetAllEmployeeQueryHandler : IRequestHandler<GetAllEmployeeQuery, ServiceResult<PaginatedList<EmployeeModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllEmployeeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<EmployeeModel>>> Handle(GetAllEmployeeQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Employees
                            .GetPaginatedProjectedListAsync<EmployeeModel>(request));
    }
}