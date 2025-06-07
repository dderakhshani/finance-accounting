using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteCodeRowDescriptionCommand : IRequest<ServiceResult<CodeRowDescriptionModel>>
{
    public int Id { get; set; }
}

public class DeleteCodeRowDescriptionCommandHandler : IRequestHandler<DeleteCodeRowDescriptionCommand, ServiceResult<CodeRowDescriptionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteCodeRowDescriptionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<CodeRowDescriptionModel>> Handle(DeleteCodeRowDescriptionCommand request, CancellationToken cancellationToken)
    {
        CodeRowDescription entity = await _unitOfWork.CodeRowDescriptions.GetByIdAsync(request.Id);

        _unitOfWork.CodeRowDescriptions.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<CodeRowDescriptionModel>(entity));
    }
}