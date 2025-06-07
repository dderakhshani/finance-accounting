using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllCodeRowDescriptionQuery : Specification<CodeRowDescription>, IRequest<ServiceResult<PaginatedList<CodeRowDescriptionModel>>>
{
}

public class GetAllCodeRowDescriptionQueryHandler : IRequestHandler<GetAllCodeRowDescriptionQuery, ServiceResult<PaginatedList<CodeRowDescriptionModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCodeRowDescriptionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<CodeRowDescriptionModel>>> Handle(GetAllCodeRowDescriptionQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.CodeRowDescriptions
                            .GetPaginatedProjectedListAsync<CodeRowDescriptionModel>(request));
    }
}