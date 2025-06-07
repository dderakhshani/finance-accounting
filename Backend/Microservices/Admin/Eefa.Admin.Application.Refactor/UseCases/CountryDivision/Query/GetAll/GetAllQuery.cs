using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllQuery : Specification<CountryDivision>, IRequest<ServiceResult<PaginatedList<CountryDivision>>>
{
}

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, ServiceResult<PaginatedList<CountryDivision>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<CountryDivision>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.CountryDivisions
                            .GetPaginatedListAsync(request));
    }
}