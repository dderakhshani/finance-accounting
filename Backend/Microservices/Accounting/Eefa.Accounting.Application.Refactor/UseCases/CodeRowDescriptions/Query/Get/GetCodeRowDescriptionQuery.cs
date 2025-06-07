using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetCodeRowDescriptionQuery : IRequest<ServiceResult<CodeRowDescriptionModel>>
{
    public int Id { get; set; }
}

public class GetCodeRowDescriptionQueryHandler : IRequestHandler<GetCodeRowDescriptionQuery, ServiceResult<CodeRowDescriptionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCodeRowDescriptionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<CodeRowDescriptionModel>> Handle(GetCodeRowDescriptionQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.CodeRowDescriptions
                            .GetProjectedByIdAsync<CodeRowDescriptionModel>(request));
    }
}