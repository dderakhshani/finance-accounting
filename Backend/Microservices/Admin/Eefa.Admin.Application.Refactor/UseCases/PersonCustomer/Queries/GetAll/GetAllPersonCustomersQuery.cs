using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllPersonCustomersQuery : Specification<Customer>, IRequest<ServiceResult<PaginatedList<PersonCustomerModel>>>
{
}

public class GetAllPersonCustomersQueryHandler : IRequestHandler<GetAllPersonCustomersQuery, ServiceResult<PaginatedList<PersonCustomerModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllPersonCustomersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ServiceResult<PaginatedList<PersonCustomerModel>>> Handle(GetAllPersonCustomersQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Customers
                            .GetPaginatedProjectedListAsync<PersonCustomerModel>(request));
    }
}