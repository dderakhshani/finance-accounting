using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteVouchersDetailCommand : IRequest<ServiceResult<VouchersDetailModel>>
{
    public int Id { get; set; }
    /// <summary>
    /// بدهکار
    /// </summary>
    public long Debit { get; set; } = default!;

    /// <summary>
    /// اعتبار
    /// </summary>
    public long Credit { get; set; } = default!;
}

public class DeleteVouchersDetailCommandHandler : IRequestHandler<DeleteVouchersDetailCommand, ServiceResult<VouchersDetailModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteVouchersDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<VouchersDetailModel>> Handle(DeleteVouchersDetailCommand request, CancellationToken cancellationToken)
    {
        VouchersDetail entity = await _unitOfWork.VouchersDetails.GetByIdAsync(request.Id);

        _unitOfWork.VouchersDetails.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<VouchersDetailModel>(entity));
    }
}