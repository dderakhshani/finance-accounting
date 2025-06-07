using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetPersonPhoneQuery : IRequest<ServiceResult<PersonPhoneModel>>
{
    public int Id { get; set; }
}

public class GetPersonPhoneQueryHandler : IRequestHandler<GetPersonPhoneQuery, ServiceResult<PersonPhoneModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetPersonPhoneQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonPhoneModel>> Handle(GetPersonPhoneQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.PersonPhones
            .GetProjectedByIdAsync<PersonPhoneModel>(request.Id));
    }
}