using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllPersonQuery : Specification<Person>, IRequest<ServiceResult<PaginatedList<PersonModel>>>
{
}

public class GetAllPersonQueryHandler : IRequestHandler<GetAllPersonQuery, ServiceResult<PaginatedList<PersonModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllPersonQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<PersonModel>>> Handle(GetAllPersonQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Persons
            .GetPaginatedProjectedListAsync<PersonModel>(request));
    }
}