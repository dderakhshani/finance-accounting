using MediatR;
using System.Threading.Tasks;
using AutoMapper;
using System.Threading;

public class DeleteHelpCommand : IRequest<ServiceResult<MinifiedHelpModel>>
{
    public int Id { get; set; }
}

public class DeleteHelpCommandHandler : IRequestHandler<DeleteHelpCommand, ServiceResult<MinifiedHelpModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public DeleteHelpCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResult<MinifiedHelpModel>> Handle(DeleteHelpCommand request, CancellationToken cancellationToken)
    {
        Help entity = await _unitOfWork.Helps.GetByIdAsync(request.Id);

        _unitOfWork.Helps.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<MinifiedHelpModel>(entity));
    }
}