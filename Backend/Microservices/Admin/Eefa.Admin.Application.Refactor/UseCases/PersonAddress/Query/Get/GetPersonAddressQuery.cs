using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetPersonAddressQuery : IRequest<ServiceResult<PersonAddressModel>>
{
    public int Id { get; set; }
}

public class GetPersonAddressQueryHandler : IRequestHandler<GetPersonAddressQuery, ServiceResult<PersonAddressModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetPersonAddressQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonAddressModel>> Handle(GetPersonAddressQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.PersonsAddress
                            .GetProjectedByIdAsync<PersonAddressModel>(request.Id));
    }
}