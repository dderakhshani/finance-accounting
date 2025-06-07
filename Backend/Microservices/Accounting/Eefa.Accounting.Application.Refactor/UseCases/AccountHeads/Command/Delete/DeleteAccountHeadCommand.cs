using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteAccountHeadCommand : IRequest<ServiceResult<AccountHeadModel>>
{
    public int Id { get; set; }
    public bool ForceDeactive { get; set; } = false;

}

public class DeleteAccountHeadCommandHandler : IRequestHandler<DeleteAccountHeadCommand, ServiceResult<AccountHeadModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public DeleteAccountHeadCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AccountHeadModel>> Handle(DeleteAccountHeadCommand request, CancellationToken cancellationToken)
    {
        AccountHead accountHead = await _unitOfWork.AccountHeads.GetByIdAsync(request.Id);

        _unitOfWork.AccountHeads.Delete(accountHead);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AccountHeadModel>(request));
    }
}