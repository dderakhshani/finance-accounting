using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteLanguageCommand : IRequest<ServiceResult<LanguageModel>>
{
    public int Id { get; set; }
}

public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, ServiceResult<LanguageModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteLanguageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<LanguageModel>> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
    {
        Language entity = await _unitOfWork.Languages.GetByIdAsync(request.Id);

        _unitOfWork.Languages.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<LanguageModel>(entity));
    }
}