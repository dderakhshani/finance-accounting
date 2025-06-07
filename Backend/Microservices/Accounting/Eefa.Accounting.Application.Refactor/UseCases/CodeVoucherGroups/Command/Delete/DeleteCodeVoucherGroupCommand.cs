using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteCodeVoucherGroupCommand : IRequest<ServiceResult<CodeVoucherGroupModel>>
{
    public int Id { get; set; }
}

public class DeleteCodeVoucherGroupCommandHandler : IRequestHandler<DeleteCodeVoucherGroupCommand, ServiceResult<CodeVoucherGroupModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteCodeVoucherGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<CodeVoucherGroupModel>> Handle(DeleteCodeVoucherGroupCommand request, CancellationToken cancellationToken)
    {
        CodeVoucherGroup entity = await _unitOfWork.CodeVoucherGroups.GetByIdAsync(request.Id);

        _unitOfWork.CodeVoucherGroups.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<CodeVoucherGroupModel>(entity));
    }
}