using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetLanguageQuery : IRequest<ServiceResult<LanguageModel>>
{
    public int Id { get; set; }
}

public class GetLanguageQueryHandler : IRequestHandler<GetLanguageQuery, ServiceResult<LanguageModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetLanguageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<LanguageModel>> Handle(GetLanguageQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Languages
                            .GetProjectedByIdAsync<LanguageModel>(request.Id));
    }
}