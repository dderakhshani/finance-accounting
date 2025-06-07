using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteVouchersHeadCommand : IRequest<ServiceResult<VouchersHeadModel>>
{
    public int Id { get; set; }
}

public class DeleteVouchersHeadCommandHandler : IRequestHandler<DeleteVouchersHeadCommand, ServiceResult<VouchersHeadModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteVouchersHeadCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<VouchersHeadModel>> Handle(DeleteVouchersHeadCommand request, CancellationToken cancellationToken)
    {
        VouchersHead entity = await _unitOfWork.VouchersHeads.GetByIdAsync(request.Id);

        _unitOfWork.VouchersHeads.Delete(entity);
        _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<VouchersHeadModel>(entity));
    }
}