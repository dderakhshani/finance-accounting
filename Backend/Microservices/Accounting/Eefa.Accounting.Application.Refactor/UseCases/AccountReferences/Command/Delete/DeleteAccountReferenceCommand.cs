using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteAccountReferenceCommand : IRequest<ServiceResult<AccountReferenceModel>>
{
    public int Id { get; set; }
    public bool ForceDeactive { get; set; } = false;
}

public class DeleteAccountReferenceCommandHandler : IRequestHandler<DeleteAccountReferenceCommand, ServiceResult<AccountReferenceModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteAccountReferenceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AccountReferenceModel>> Handle(DeleteAccountReferenceCommand request, CancellationToken cancellationToken)
    {
        AccountReference accountReference = await _unitOfWork.AccountReferences
                                            .GetByIdAsync(request.Id);

        _unitOfWork.AccountReferences.Delete(accountReference);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AccountReferenceModel>(accountReference));
    }
}