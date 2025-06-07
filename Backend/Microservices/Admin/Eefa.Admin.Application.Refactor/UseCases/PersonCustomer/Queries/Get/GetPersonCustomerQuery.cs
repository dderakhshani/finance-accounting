using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetPersonCustomerQuery : IRequest<ServiceResult<PersonCustomerModel>>
{
    public int Id { get; set; }
}

public class GetPersonCustomerQueryHandler : IRequestHandler<GetPersonCustomerQuery, ServiceResult<PersonCustomerModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetPersonCustomerQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonCustomerModel>> Handle(GetPersonCustomerQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Customers
                            .GetProjectedByIdAsync<PersonCustomerModel>(request.Id));
    }
}