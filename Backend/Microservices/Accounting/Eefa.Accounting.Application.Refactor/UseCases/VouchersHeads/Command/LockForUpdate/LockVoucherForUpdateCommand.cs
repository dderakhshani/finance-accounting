using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class LockVoucherForUpdateCommand : IRequest<ServiceResult<VouchersHeadModel>>
{
    public int VoucherId { get; set; }
}

public class LockVoucherForUpdateCommandHandler : IRequestHandler<LockVoucherForUpdateCommand, ServiceResult<VouchersHeadModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationUser _applicationUser;

    public LockVoucherForUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser)
    {
        _mapper = mapper;
        _applicationUser = applicationUser;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<VouchersHeadModel>> Handle(LockVoucherForUpdateCommand request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.VouchersHeads
            .GetProjectedByIdAsync<VouchersHeadModel>(request.VoucherId));
    }
}