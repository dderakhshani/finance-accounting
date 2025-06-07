using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAccountHeadQuery : IRequest<ServiceResult<AccountHeadModel>>
{
    public int Id { get; set; }
}

public class GetAccountHeadQueryHandler : IRequestHandler<GetAccountHeadQuery, ServiceResult<AccountHeadModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAccountHeadQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AccountHeadModel>> Handle(GetAccountHeadQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork
            .AccountHeads.GetProjectedByIdAsync<AccountHeadModel>(request.Id));
    }
}