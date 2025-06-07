using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class DeleteAccountReferencesGroupCommand : IRequest<ServiceResult<AccountReferencesGroupModel>>
{
    public int Id { get; set; }
}

public class DeleteReferencesGroupCommandHandler : IRequestHandler<DeleteAccountReferencesGroupCommand, ServiceResult<AccountReferencesGroupModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteReferencesGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AccountReferencesGroupModel>> Handle(DeleteAccountReferencesGroupCommand request, CancellationToken cancellationToken)
    {
        AccountReferencesGroup entity = await _unitOfWork.AccountReferencesGroups.GetByIdAsync(request.Id);

        _unitOfWork.AccountReferencesGroups.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AccountReferencesGroupModel>(entity));
    }
}