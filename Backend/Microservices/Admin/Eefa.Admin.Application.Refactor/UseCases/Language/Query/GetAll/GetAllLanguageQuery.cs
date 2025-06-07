using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllLanguageQuery : Specification<Language>, IRequest<ServiceResult<PaginatedList<LanguageModel>>>
{
}

public class GetAllLanguageQueryHandler : IRequestHandler<GetAllLanguageQuery, ServiceResult<PaginatedList<LanguageModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllLanguageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<LanguageModel>>> Handle(GetAllLanguageQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Languages
                            .GetPaginatedProjectedListAsync<LanguageModel>(request));
    }
}